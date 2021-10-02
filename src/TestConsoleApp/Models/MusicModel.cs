using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestConsoleApp.Models
{
    [DynamoDBTable("Music")]
    public class MusicModel
    {
 
       // [DynamoDBHashKey]
        public string Artist { get; set; }

        public string SongTitle { get; set; }

       // [DynamoDBGlobalSecondaryIndexHashKey]
        public string AlbumTitle { get; set; }

      //  [DynamoDBGlobalSecondaryIndexRangeKey]
        public string Genre { get; set; }
        public List<TourInfo> TourDates { get; set; }
    }

    public class TourInfo
    {
        public string City { get; set; }
        public DateTime TourDate { get; set; }
    }
}
