using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace DynamoDBApp.Shared.Models
{
    [DynamoDBTable("Music")]
    public class MusicModel
    {
        public string Artist { get; set; }
        public string SongTitle { get; set; }
        public string AlbumTitle { get; set; }
        public string Genre { get; set; }
        public List<TourInfo> TourDates { get; set; }
    }

    public class TourInfo
    {
        public string City { get; set; }
        public DateTime TourDate { get; set; }
    }
}

