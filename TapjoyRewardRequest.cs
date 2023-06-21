using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using IronsourceReward.Modal;

namespace Peterworks.Function;

public static class TapjoyRewardRequest
{
    [FunctionName("TapjoyRewardRequest")]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req, 
        [CosmosDB(
            databaseName: "RewardStatus",
            containerName: "TapjoyRewardDb",
            Connection = "CosmosDBConnection")] IAsyncCollector<dynamic> documents,
        ILogger log)
    {
        string sunid = req.Query["snuid"];
        long currendcy = Convert.ToInt64(req.Query["currency"]);
        string mac_address = req.Query["mac_address"];
        string id = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
            
        TapjoyRewardModal item = new TapjoyRewardModal()
        { Snuid = sunid, Currency = currendcy, MacAddress = mac_address, id = id };

        await documents.AddAsync(item);
        
        return new OkObjectResult("{\"Message\" :\"Tapjoy Reward Complete\"}");
        
    }
}