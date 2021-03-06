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



            #region GET & DELETE APIs

            Dictionary<string, string> requestTemplate = new Dictionary<string, string>
            {
                ["application/json"] = "{ \"statusCode\": \"200\" }"
            };

            LambdaIntegration getApiIntegration = new LambdaIntegration(props.GetMusicLambdaHandler, new LambdaIntegrationOptions
            {
                RequestTemplates = requestTemplate
            });

            var musicGetMethod = musicResource.AddMethod("GET", getApiIntegration, GetDeleteMethodOptions("getMusic-Validator", musicApi));

            LambdaIntegration deleteApiIntegration = new LambdaIntegration(props.DeleteMusicLambdaHandler, new LambdaIntegrationOptions
            {
                RequestTemplates = requestTemplate
            });

            musicResource.AddMethod("DELETE", deleteApiIntegration, GetDeleteMethodOptions("deleteMusic-Validator", musicApi));

            #endregion

            #region POST + PUT APIs

            var postApiIntegration = new LambdaIntegration(props.AddMusicLambdaHandler, new LambdaIntegrationOptions
            {
                RequestTemplates = new Dictionary<string, string>
                {
                    ["application/json"] = "{ \"statusCode\": \"200\" }"
                }
            });

            Model musicModel = new Model(this, "POST-Validator-CDK", new ModelProps
            {
                RestApi = musicApi,
                ContentType = "application/json",
                Description = "Validation for POST body",
                ModelName = "MusicModel",
                Schema = new JsonSchema()
                {
                    Type = JsonSchemaType.OBJECT,
                    Required = new string[] { "Artist", "SongTitle", "AlbumTitle", "Genre" },
                    Properties = new Dictionary<string, IJsonSchema>()
                    {
                        ["Artist"] = new JsonSchema() { Type = "string" },
                        ["SongTitle"] = new JsonSchema() { Type = "string" },
                        ["AlbumTitle"] = new JsonSchema() { Type = "string" },
                        ["Genre"] = new JsonSchema() { Type = "string" }
                    }
                }
            });

            musicResource.AddMethod("POST", postApiIntegration, new MethodOptions 
            { 
                RequestValidator = new RequestValidator(this, "postMusic-Validator", new RequestValidatorProps 
                { 
                    RestApi = musicApi,
                    RequestValidatorName = "postMusic-Validator",
                    ValidateRequestBody = true
                }),
                RequestModels = new Dictionary<string, IModel>() 
                { 
                    ["application/json"] = musicModel
                }
            });

            musicResource.AddMethod("PUT", postApiIntegration, new MethodOptions
            {
                RequestValidator = new RequestValidator(this, "putMusic-Validator", new RequestValidatorProps
                {
                    RestApi = musicApi,
                    RequestValidatorName = "putMusic-Validator",
                    ValidateRequestBody = true
                }),
                RequestModels = new Dictionary<string, IModel>()
                {
                    ["application/json"] = musicModel
                }
            });

            #endregion

            new CfnOutput(this, "API Gateway API:", new CfnOutputProps() { Value = musicApi.Url });
            string urlPrefix = musicApi.Url.Remove(musicApi.Url.Length - 1);
            new CfnOutput(this, "Music Lambda API Gateway Link:", new CfnOutputProps() { Value = urlPrefix + musicGetMethod.Resource.Path });
        }

        private MethodOptions GetDeleteMethodOptions(string validatorName, RestApi restApi)
        {
            MethodOptions retval = new MethodOptions
            {
                RequestValidator = new RequestValidator(this, validatorName, new RequestValidatorProps
                {
                    RestApi = restApi,
                    RequestValidatorName = validatorName,
                    ValidateRequestParameters = true
                }),
                RequestParameters = new Dictionary<string, bool>()
                {
                    { "method.request.querystring.artist", true },
                    { "method.request.querystring.songTitle", true }
                }
            };

            return retval;
        }
    }
}
