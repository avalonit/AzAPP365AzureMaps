using System;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

using Microsoft.Extensions.Configuration;

namespace varprime.app365
{
    public static class DownloadItemImage
    {
        [FunctionName("DownloadItemImage")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = null)]
            HttpRequest req,
            ILogger log,
            ExecutionContext context)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            // Load configuration
            var config = new ConfigurationBuilder()
                .SetBasePath(context.FunctionAppDirectory)
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
            var bcConfig = new ConnectorConfig(config);
            var storageHelper = new StorageHelper(bcConfig);

            var itemId = req.Query["itemId"].ToString();
            var isImageDownloaded = false;

            if (!string.IsNullOrEmpty(itemId))
            {

                var baseUrl = bcConfig.WebServiceMainImageUrl;
                var url = String.Format(baseUrl, itemId, bcConfig.WebServiceCompany);

                var authData = string.Format("{0}:{1}", bcConfig.WebServiceUser, bcConfig.WebServicePassword);
                var authHeaderValue = Convert.ToBase64String(Encoding.UTF8.GetBytes(authData));

                log.LogInformation(bcConfig.WebServiceUser);
                log.LogInformation(bcConfig.WebServicePassword);
                log.LogInformation(url);

                byte[] result = null;
                var buffer = new byte[4096];

                var wr = (HttpWebRequest)WebRequest.Create(url);
                wr.Headers.Add("Authorization", "Basic " + authHeaderValue);
                //wr.AllowAutoRedirect = true;

                var photoStream = (await storageHelper.Download(itemId));

                if (photoStream != null)
                    return new FileContentResult(photoStream.ToArray(), "image/jpg");

                try
                {
                    using (var response = (HttpWebResponse)wr.GetResponse())
                    {
                        log.LogInformation(response.StatusCode.ToString());
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            using (var responseStream = response.GetResponseStream())
                            {
                                using (var memoryStream = new MemoryStream())
                                {
                                    int count = 0;
                                    do
                                    {
                                        count = responseStream.Read(buffer, 0, buffer.Length);
                                        memoryStream.Write(buffer, 0, count);

                                    } while (count != 0);

                                    result = memoryStream.ToArray();

                                }
                            }
                            isImageDownloaded = true;
                        }
                        else
                            result = null;
                    }
                }
                catch (Exception ex1)
                {
                }


                if (!isImageDownloaded)
                {
                    using (var client = new WebClient())
                    {
                        result = client.DownloadData("https://www.allianceplast.com/wp-content/uploads/2017/11/no-image.png");
                    }
                }

                using (var outputStream = new MemoryStream())
                {
                    var white = new Rgba32(255, 255, 255);

                    using (var image = Image.Load(result))
                    {
                        image.Mutate(x => x.BackgroundColor(Rgba32.ParseHex("FFFFFF")));
                        image.Mutate(x => x
                            .Resize(new ResizeOptions
                            {
                                Mode = ResizeMode.Pad,
                                Size = new Size(350, 270),
                            }).BackgroundColor(white));

                        image.SaveAsJpeg(outputStream);
                    }

                    outputStream.Seek(0, SeekOrigin.Begin);

                    if (isImageDownloaded)
                    {
                        await storageHelper.Upload(outputStream, itemId);
                    }


                    return new FileContentResult(outputStream.ToArray(), "image/jpg");
                }
                //return new FileContentResult(data, "image/jpeg");
            }
            else
            {
                return new BadRequestResult();
            }

        }

    }
}
