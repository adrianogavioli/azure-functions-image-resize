using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;

namespace Frattina.Functions
{
    public static class ImageResizeToDefault
    {
        [FunctionName("ImageResizeToDefault")]
        public static void Run([BlobTrigger("uploads/{name}", Connection = "AzureWebJobsStorage")]Stream myBlob, string name, 
            [Blob("imagens/{name}", FileAccess.Write)] Stream outputBlob, ILogger log)
        {
            log.LogInformation($"Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");

            using (var image = Image.Load(myBlob))
            {
                var resizeOptions = new ResizeOptions
                {
                    Size = new Size(720, 720),
                    Compand = true,
                    Mode = ResizeMode.Crop
                };

                image.Mutate(ctx => ctx.Resize(resizeOptions));

                image.Save(outputBlob, new JpegEncoder { Quality = 90 });
            }
        }
    }
}
