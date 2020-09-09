using System;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;

using Microsoft.Extensions.Configuration;

namespace varprime.app365
{
    public static class DownloadItemAltImages
    {
        [FunctionName("DownloadItemAltImages")]
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

            var Id = req.Query["Id"].ToString();

            if (!string.IsNullOrEmpty(Id))
            {
                var photoStream = (await storageHelper.Download(Id));

                if (photoStream != null)
                    return new FileContentResult(photoStream.ToArray(), "image/jpg");


                using (WebClient client = new WebClient())
                {
                    var baseUrl = bcConfig.WebServiceAltImageUrl;
                    var url = String.Format(baseUrl, Id, bcConfig.WebServiceCompany);
                    var uri = new Uri(url);

                    var authData = string.Format("{0}:{1}", bcConfig.WebServiceUser, bcConfig.WebServicePassword);
                    var authHeaderValue = Convert.ToBase64String(Encoding.UTF8.GetBytes(authData));

                    client.Headers[HttpRequestHeader.Authorization] = "Basic " + authHeaderValue;

                    log.LogInformation(url);

                    var response = client.DownloadString(uri).ToString();
                    var result = JsonConvert.DeserializeObject<APP365DEDocumentAttachmentsBase64>(response.ToString());
                    var bytes = System.Convert.FromBase64String(result.PictureJson);

                    var outputStream = new MemoryStream();
                    var white = new Rgba32(255, 255, 255);
                    IImageDecoder decoder = new JpegDecoder();

                    if (result.FileExtension.ToLower().Equals("png"))
                        decoder = new PngDecoder();

                    using (var image = Image.Load(bytes, decoder))
                    {
                        image.Mutate(x => x.BackgroundColor(Rgba32.ParseHex("FFFFFF")));
                        image.Mutate(x => x
                            .Resize(new ResizeOptions
                            {
                                Mode = ResizeMode.Pad,
                                Size = new Size(300, 600),
                            }).BackgroundColor(white));

                        image.SaveAsJpeg(outputStream);
                    }

                    outputStream.Seek(0, SeekOrigin.Begin);

                    await storageHelper.Upload(outputStream, Id);

                    return new FileContentResult(outputStream.ToArray(), "image/jpg");
                    //return new FileContentResult(data, "image/jpeg");
                }
            }
            else
            {
                return new BadRequestResult();
            }

        }

        public class APP365DEDocumentAttachmentsBase64
        {
            public String ID { get; set; }
            public String No { get; set; }
            public String PictureJson { get; set; }
            public String FileType { get; set; }
            public String FileExtension { get; set; }

        }

    }
}
