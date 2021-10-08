using Amazon.CDK;
using Amazon.CDK.AWS.Lambda;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cdkscaffold
{
    public class LambdaPostMusicApiStack : Stack
    {
        public Function MusicPostApiLambdaHandler { get; set; }

        internal LambdaPostMusicApiStack(Construct scope, string id, LambdaApiStackProps props) : base(scope, id)
        {
            MusicPostApiLambdaHandler = new Function(this, "CDKMusicPostApi", new FunctionProps
            {
                Runtime = Runtime.DOTNET_CORE_3_1,
                Code = Code.FromAsset("src//DynamoDBApp.PostApi//bin//Release//netcoreapp3.1"),
                Handler = "DynamoDBApp.PostApi::DynamoDBApp.PostApi.Function::FunctionHandler",
                FunctionName = "MusicPostAPI",
                Timeout = Duration.Seconds(30),
                MemorySize = 5000
            });

            props.MusicTable.GrantFullAccess(MusicPostApiLambdaHandler);
        }
    }
}
