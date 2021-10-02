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

namespace DynamoDBApp.GetApi
{
    public class Function
    {
        
        public APIGatewayProxyResponse FunctionHandler(APIGatewayProxyRequest item, ILambdaContext context)
        {
            APIGatewayProxyResponse retval = new APIGatewayProxyResponse();
            DynamoDBRepo<MusicModel> repo = new DynamoDBRepo<MusicModel>();

            LambdaLogger.Log("In handler");

            if (item.HttpMethod.ToUpperInvariant() == "GET")
            {
                retval = new APIGatewayProxyResponse
                {
                    Headers = new Dictionary<string, string> { { "Accept", "application/json" } }
                };

                try
                {
                    string artist = item.QueryStringParameters["artist"];
                    string songTitle = item.QueryStringParameters["songTitle"];

                    LambdaLogger.Log("Artist parameter value: " + artist);
                    LambdaLogger.Log("SongTitle parameter value: " + songTitle);

                    MusicModel record = repo.GetRecord(artist, songTitle);

                    if(record != null)
                    {
                        retval.StatusCode = (int)HttpStatusCode.OK;
                        retval.Body = JsonConvert.SerializeObject(record);
                    }
                    else
                    {
                        retval.StatusCode = (int)HttpStatusCode.NotFound;
                        retval.Body = "Record not found";
                    }
                }
                catch(Exception ex)
                {
                    retval.StatusCode = (int)HttpStatusCode.InternalServerError;
                    LambdaLogger.Log("Order GET Handler error: " + ex.ToString());
                }
            }


            return retval;
        }
    }
}
