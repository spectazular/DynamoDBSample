using Amazon.CDK;
using Amazon.CDK.AWS.DynamoDB;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cdkscaffold
{
    public class DynamoDBStack : Stack
    {
         public Table MusicTable { get; set; }

        internal DynamoDBStack(Construct scope, string id, IStackProps props = null) : base(scope, id, props)
        {
            MusicTable = new Table(this, "MusicTable", new TableProps
            {
                TableName = "Music",
                PartitionKey = new Amazon.CDK.AWS.DynamoDB.Attribute
                {
                    Name = "Artist",
                    Type = AttributeType.STRING
                },
                SortKey = new Amazon.CDK.AWS.DynamoDB.Attribute
                {
                    Name = "SongTitle",
                    Type = AttributeType.STRING
                },
                
                RemovalPolicy = RemovalPolicy.DESTROY,
                WriteCapacity = 10,
                ReadCapacity = 10
            });

            MusicTable.AddGlobalSecondaryIndex(new GlobalSecondaryIndexProps
            {
                IndexName = "AlbumGenre",
                PartitionKey = new Amazon.CDK.AWS.DynamoDB.Attribute
                {
                    Name = "AlbumTitle",
                    Type = AttributeType.STRING
                },
                SortKey = new Amazon.CDK.AWS.DynamoDB.Attribute
                {
                    Name = "Genre",
                    Type = AttributeType.STRING
                },
            });
        }
    }
}
