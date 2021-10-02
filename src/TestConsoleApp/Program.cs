using Newtonsoft.Json;
using System;
using TestConsoleApp.Models;
using TestConsoleApp.Repo;

namespace TestConsoleApp
{
    //Tutorial: https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/CRUDHighLevelExample1.html

    class Program
    {
        static void Main(string[] args)
        {
            DynamoDBRepo repo = new DynamoDBRepo();

            Console.WriteLine("Starting!");

            MusicModel record1 = new MusicModel()
            {
                Artist = "Bob Marley",
                SongTitle = "No Woman No Cry",
                AlbumTitle = "Rastafa Album",
                Genre = "Reggae",
                TourDates = new System.Collections.Generic.List<TourInfo>()
                {
                    new TourInfo()
                    {
                        City = "Auckland",
                        TourDate = DateTime.Now.AddDays(100)
                    }
                }
            };

            try
            {
                bool addRecord = repo.WriteData(record1);
                MusicModel addedRecord = repo.GetDataByArtist(record1.Artist, record1.SongTitle);
                ObjectToString(addedRecord);

                repo.DeleteData(record1.Artist, record1.SongTitle);
                addedRecord = repo.GetDataByArtist(record1.Artist, record1.SongTitle);

                if(addedRecord == null)
                {
                    Console.WriteLine("Null");
                }
                else
                {
                    ObjectToString(addedRecord);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private static string ObjectToString(MusicModel record)
        {
            string retval = "";

            retval = JsonConvert.SerializeObject(record);
            Console.WriteLine(retval);

            return retval;
        }

    }
}
