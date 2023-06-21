using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Cosmos;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using IronsourceReward.Modal;

namespace Peterworks.Function
{
    public static class RewardRequest
    {
        [FunctionName("RewardRequest")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get","post", Route = null)] HttpRequest req,
            [CosmosDB(
                databaseName: "RewardStatus",
                containerName: "RewardDb",
                Connection = "CosmosDBConnection")] IAsyncCollector<dynamic> documents,
            ILogger log)
                    
        {

            string appUserId = req.Query["appUserId"];
            long rewards = Convert.ToInt64(req.Query["rewards"]);
            string eventId = req.Query["eventId"];
            string itemName = req.Query["itemName"];
            string applicationKey = req.Query["appKey"];
            string placementName = req.Query["placementName"];
            string adProvider = req.Query["adProvider"];
            string deliveryType = req.Query["deliveryType"];
            string rewardTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
            
            RewardModal item = new RewardModal()
            { UserId = appUserId, Rewards = rewards, EventId = eventId, ItemName = itemName,
                id = rewardTime, AdProvider = adProvider, ApplicationKey = applicationKey, PlacementName = placementName, DeliveryType = deliveryType
            };

        await documents.AddAsync(item);
        
        return new OkObjectResult(eventId + ":OK");

        }
        
    }
}
