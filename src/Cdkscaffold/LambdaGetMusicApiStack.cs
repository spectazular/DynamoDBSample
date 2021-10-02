using Amazon.CDK;
using Amazon.CDK.AWS.Lambda;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cdkscaffold
{
    public class LambdaGetMusicApiStack : Stack
    {
        public Function MusicGetApiLambdaHandler { get; set; }

        internal LambdaGetMusicApiStack(Construct scope, string id, LambdaApiStackProps props) : base(scope, id)
        {
            MusicGetApiLambdaHandler = new Function(this, "CDKMusicGetApi", new FunctionProps
            {
                Runtime = Runtime.DOTNET_CORE_3_1,
                Code = Code.FromAsset("src//DynamoDBApp.GetApi//bin//Debug//netcoreapp3.1"),
                Handler = "DynamoDBApp.GetApi::DynamoDBApp.GetApi.Function::FunctionHandler",
                FunctionName = "MusicGetAPI",
                Timeout = Duration.Seconds(30)
            });

            props.MusicTable.GrantFullAccess(MusicGetApiLambdaHandler);
        }
    }
}
