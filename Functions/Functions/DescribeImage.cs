using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Funcs;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.WindowsAzure.Storage.Table;
using Services.Implementation;

namespace Functions.Functions
{
    public static class DescribeImage
    {
        [FunctionName("DescribeImage")]
        public static async Task Run(
            [BlobTrigger("images/{name}")] Stream trigger,
            [Table("images")] CloudTable cloudTable,
            string name, 
            TraceWriter log)
        {
            log.Info("Start");

            using (HttpContent content = new StreamContent(trigger))
            {
                var parameters = "describe" +
                                 "?maxCandidates=1";

                var response = await CognitiveServicesHttpClient.PostVisionRequest(content, parameters);

                if (response.IsSuccessStatusCode)
                {
                    var responseBytes = await response.Content.ReadAsStringAsync();

                    await cloudTable.Update(name, responseBytes, (image, text) =>
                    {
                        image.Description = text;
                    });
                }
            }

            log.Info("Finish");
        }
    }
}