using Amazon.CDK;
using Amazon.CDK.AWS.Lambda;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cdkscaffold
{
    class LambdaDeleteMusicApiStack : Stack
    {
        public Function MusicDeleteApiLambdaHandler { get; set; }

        internal LambdaDeleteMusicApiStack(Construct scope, string id, LambdaApiStackProps props) : base(scope, id)
        {
            MusicDeleteApiLambdaHandler = new Function(this, "CDKDeleteGetApi", new FunctionProps
            {
                Runtime = Runtime.DOTNET_CORE_3_1,
                Code = Code.FromAsset("src//DynamoDBApp.DeleteApi//bin//Release//netcoreapp3.1"),
                Handler = "DynamoDBApp.DeleteApi::DynamoDBApp.DeleteApi.Function::FunctionHandler",
                FunctionName = "MusicDeleteAPI",
                Timeout = Duration.Seconds(30)
            });

            props.MusicTable.GrantFullAccess(MusicDeleteApiLambdaHandler);
        }
    }
}
