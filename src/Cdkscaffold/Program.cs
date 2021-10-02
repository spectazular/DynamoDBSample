using Amazon.CDK;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cdkscaffold
{
    sealed class Program
    {
        public static void Main(string[] args)
        {
            var app = new App();
            DynamoDBStack ddbStack = new DynamoDBStack(app, "DynamoDBStack");

            LambdaGetMusicApiStack musicGetApiStack = new LambdaGetMusicApiStack(app, "GetMusicLambdaStack", new LambdaApiStackProps 
            { 
                MusicTable = ddbStack.MusicTable
            });

            new ApiGatewayStack(app, "MusicApiGatewayStack",new ApiGatewayStackProps 
            { 
                GetMusicLambdaHandler = musicGetApiStack.MusicGetApiLambdaHandler
            });


            app.Synth();
        }
    }
}
