using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using DynamoDBApp.Shared.Models;
using DynamoDBApp.Shared.Repos.Concrete;
using Newtonsoft.Json;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace DynamoDBApp.PostApi
{
    public class Function
    {
        
        public APIGatewayProxyResponse FunctionHandler(APIGatewayProxyRequest item, ILambdaContext context)
        {
            APIGatewayProxyResponse retval = new APIGatewayProxyResponse();
            DynamoDBRepo<MusicModel> repo = new DynamoDBRepo<MusicModel>();

            LambdaLogger.Log("In handler");

            if (item.HttpMethod.ToUpperInvariant() == "POST")
            {
                retval = new APIGatewayProxyResponse
                {
                    Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
                };

                try
                {
                    MusicModel musicItem = JsonConvert.DeserializeObject<MusicModel>(item.Body);
                    LambdaLogger.Log("Music object: " + item.Body);

                    bool insertStatus = repo.InserRecord(musicItem);

                    if (insertStatus == true)
                    {
                        retval.StatusCode = (int)HttpStatusCode.OK;
                        retval.Body = JsonConvert.SerializeObject(musicItem);
                    }
                    else
                    {
                        retval.StatusCode = (int)HttpStatusCode.InternalServerError;
                    }
                }
                catch (Exception ex)
                {
                    retval.StatusCode = (int)HttpStatusCode.InternalServerError;
                    LambdaLogger.Log("POST Handler POST error: " + ex.ToString());
                }

            }

            return retval;
        }
    }
}
