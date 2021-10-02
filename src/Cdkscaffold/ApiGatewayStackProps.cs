using System;
using System.Collections.Generic;
using System.Text;
using Amazon.CDK.AWS.Lambda;

namespace Cdkscaffold
{
    public class ApiGatewayStackProps
    {
        public Function GetMusicLambdaHandler { get; set; }
        public Function AddMusicLambdaHandler { get; set; }
    }
}
