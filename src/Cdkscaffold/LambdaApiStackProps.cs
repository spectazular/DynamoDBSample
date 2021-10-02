using Amazon.CDK.AWS.DynamoDB;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cdkscaffold
{
    class LambdaApiStackProps
    {
        public Table MusicTable { get; set; }
    }
}
