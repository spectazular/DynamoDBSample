using Amazon.CDK;
using Amazon.CDK.AWS.Cognito;


namespace Cdkscaffold
{
    public class CognitoStack : Stack
    {
        //TODO: Finish Cognito Athorizer stack: https://dev.to/ara225/adding-a-cognito-authorizer-to-api-gateway-with-the-aws-cdk-1ja2
        public CognitoStack(Construct scope, string id, IStackProps props = null) : base(scope, id, props)
        {
            UserPool userPool = new UserPool(this, "CDKCognitoStack", new UserPoolProps
            {
                UserPoolName = "Music-API-UserPool",
                SelfSignUpEnabled = true,
                SignInAliases = new SignInAliases { Email = true },
                AutoVerify = new AutoVerifiedAttrs { Email = true },
                StandardAttributes = new StandardAttributes 
                { 
                    GivenName = new StandardAttribute { Required = true, Mutable = true },
                    FamilyName = new StandardAttribute { Required = true, Mutable = true }
                },
                PasswordPolicy = new PasswordPolicy 
                { 
                    MinLength = 6,
                    RequireLowercase = true,
                    RequireDigits = true,
                    RequireUppercase = true,
                    RequireSymbols = true
                },
                AccountRecovery = AccountRecovery.EMAIL_ONLY,
                RemovalPolicy = RemovalPolicy.DESTROY
            });
        }
    }
}
