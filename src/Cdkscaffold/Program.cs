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

            LambdaApiStackProps lambdaStackProps = new LambdaApiStackProps()
            { 
                MusicTable = ddbStack.MusicTable
            };


            LambdaGetMusicApiStack musicGetApiStack = new LambdaGetMusicApiStack(app, "GetMusicLambdaStack", lambdaStackProps);

            LambdaPostMusicApiStack musicPostApiStack = new LambdaPostMusicApiStack(app, "PostMusicLambdaStack", lambdaStackProps);

            LambdaDeleteMusicApiStack musicDeleteApiStack = new LambdaDeleteMusicApiStack(app, "DeleteMusicLambdaStack", lambdaStackProps);

            new ApiGatewayStack(app, "MusicApiGatewayStack",new ApiGatewayStackProps 
            { 
                GetMusicLambdaHandler = musicGetApiStack.MusicGetApiLambdaHandler,
                AddMusicLambdaHandler = musicPostApiStack.MusicPostApiLambdaHandler,
                DeleteMusicLambdaHandler = musicDeleteApiStack.MusicDeleteApiLambdaHandler
            });

            app.Synth();
        }
    }
}
