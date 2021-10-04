using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using DynamoDBApp.Shared.Models;
using DynamoDBApp.Shared.Repos.Concrete;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace DynamoDBApp.DeleteApi
{
    public class Function
    {
        
        /// <summary>
        /// A simple function that takes a string and does a ToUpper
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public APIGatewayProxyResponse FunctionHandler(APIGatewayProxyRequest item, ILambdaContext context)
        {
            APIGatewayProxyResponse retval = new APIGatewayProxyResponse();
            DynamoDBRepo<MusicModel> repo = new DynamoDBRepo<MusicModel>();

            LambdaLogger.Log("In handler");

            if (item.HttpMethod.ToUpperInvariant() == "DELETE")
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

                    bool deleted = repo.DeleteRecord(artist, songTitle);

                    if (deleted ==true)
                    {
                        retval.StatusCode = (int)HttpStatusCode.OK;
                    }
                    else
                    {
                        retval.StatusCode = (int)HttpStatusCode.NotFound;
                        retval.Body = "Record not found";
                    }
                }
                catch (Exception ex)
                {
                    retval.StatusCode = (int)HttpStatusCode.InternalServerError;
                    LambdaLogger.Log("Music Delete Handler error: " + ex.ToString());
                }
            }


            return retval;
        }
    }
}
