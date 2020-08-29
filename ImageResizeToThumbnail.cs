using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;

namespace Frattina.Functions
{
    public static class ImageResizeToThumbnail
    {
        [FunctionName("ImageResizeToThumbnail")]
        public static void Run([BlobTrigger("imagens/{name}", Connection = "AzureWebJobsStorage")]Stream myBlob, string name, 
            [Blob("thumbnails/{name}", FileAccess.Write)] Stream outputBlob, ILogger log)
        {
            log.LogInformation($"Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");

            using (var image = Image.Load(myBlob))
            {
                var resizeOptions = new ResizeOptions
                {
                    Size = new Size(240, 240),
                    Compand = true,
                    Mode = ResizeMode.Crop
                };

                image.Mutate(ctx => ctx.Resize(resizeOptions));

                image.Save(outputBlob, new JpegEncoder { Quality = 80 });
            }
        }
    }
}
