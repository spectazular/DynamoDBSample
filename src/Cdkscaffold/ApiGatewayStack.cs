using Amazon.CDK;
using Amazon.CDK.AWS.APIGateway;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cdkscaffold
{
    public class ApiGatewayStack : Stack
    {
        internal ApiGatewayStack(Construct scope, string id, ApiGatewayStackProps props = null) : base(scope, id)
        {
            RestApi musicApi = new RestApi(this, "CDKMusicApi", new RestApiProps
            {
                RestApiName = "Music-Api",
                Description = "This is our Music Api",
                Deploy = true
            });

            Deployment deployment = new Deployment(this, "Music Api deployment", new DeploymentProps
            {
                Api = musicApi
            });

            Amazon.CDK.AWS.APIGateway.Stage stage = new Amazon.CDK.AWS.APIGateway.Stage(this, "stage name", new Amazon.CDK.AWS.APIGateway.StageProps
            {
                Deployment = deployment,
                StageName = "Prod"
            });

            musicApi.DeploymentStage = stage;
            Amazon.CDK.AWS.APIGateway.Resource musicResource = musicApi.Root.AddResource("music");

            LambdaIntegration getApiIntegration = new LambdaIntegration(props.GetMusicLambdaHandler, new LambdaIntegrationOptions
            {
                RequestTemplates = new Dictionary<string, string>
                {
                    ["application/json"] = "{ \"statusCode\": \"200\" }"
                }
            });

            var musicGetMethod = musicResource.AddMethod("GET", getApiIntegration, new MethodOptions
            {
                RequestParameters = new Dictionary<string, bool>()
                {
                    { "method.request.querystring.artist", true },
                    { "method.request.querystring.songTitle", true }
                },
                RequestValidatorOptions = new RequestValidatorOptions
                {
                    RequestValidatorName = "getMusic-Validator",
                    ValidateRequestParameters = true,
                    ValidateRequestBody = false
                }
            });

            new CfnOutput(this, "API Gateway API:", new CfnOutputProps() { Value = musicApi.Url });
            string urlPrefix = musicApi.Url.Remove(musicApi.Url.Length - 1);
            new CfnOutput(this, "Order Lambda Get:", new CfnOutputProps() { Value = urlPrefix + musicGetMethod.Resource.Path });
        }
    }
}
