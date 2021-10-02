using DynamoDBApp.Shared.Models;
using DynamoDBApp.Shared.Repos.Concrete;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace TestConsoleApp
{
    //Tutorial: https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/CRUDHighLevelExample1.html

    class Program
    {
        private static DynamoDBRepo<MusicModel> _repo = new DynamoDBRepo<MusicModel>();

        static void Main(string[] args)
        {
            

            Console.WriteLine("Starting!");

            try
            {
                List<MusicModel> records = DummyData();

                InsertRecords(records);
                GetInsertedRecords(records);
                UpdateRecords(records);
                DeleteRecords(records);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            Console.WriteLine("Finished");
        }

        private static void DeleteRecords(List<MusicModel> records)
        {
            foreach(MusicModel item in records)
            {
                bool deleted = _repo.DeleteRecord(item.Artist, item.SongTitle);
                MusicModel record = _repo.GetRecord(item.Artist, item.SongTitle);

                if(record == null)
                {
                    Console.WriteLine("Record Artist: " + item.Artist + " SongTitle: " + item.SongTitle + " expected delete status: " + deleted.ToString() + " doesn't exist");
                }
                else
                {
                    Console.WriteLine("Record Artist: " + item.Artist + " SongTitle: " + item.SongTitle + " expected delete status: " + deleted.ToString() + " exists");
                }
            }
        }

        private static void UpdateRecords(List<MusicModel> records)
        {
            foreach(MusicModel item in records)
            {
                foreach(TourInfo info in item.TourDates)
                {
                    info.City = info.City + " A";
                    info.TourDate = info.TourDate.AddDays(10);
                }

                try
                {
                    bool updated = _repo.UpdateRecord(item);
                    Console.WriteLine(ObjectToString(_repo.GetRecord(item.Artist, item.SongTitle)));

                }
                catch(Exception ex)
                {
                    Console.WriteLine("Update error: " + ex.ToString());
                }

                
            }
        }

        private static void GetInsertedRecords(List<MusicModel> records)
        {
            foreach(MusicModel item in records)
            {
                try
                {
                    MusicModel record = _repo.GetRecord(item.Artist, item.SongTitle);

                    if(record == null)
                    {
                        Console.WriteLine("No record for Artist: " + item.Artist + " and SongTitle: " + item.SongTitle);
                    }
                    else
                    {
                        Console.WriteLine(ObjectToString(item));
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Get error: " + ex.ToString());
                }

            }
        }

        private static void InsertRecords(List<MusicModel> records)
        {
            foreach(MusicModel item in records)
            {
                try
                {
                    bool inserted = _repo.InserRecord(item);
                    Console.WriteLine("Artist: " + item.Artist + " record insert result: " + inserted.ToString());
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Insert error: " + ex.ToString());
                }
            }
        }

        private static string ObjectToString(MusicModel record)
        {
            string retval = "";

            retval = JsonConvert.SerializeObject(record);
            Console.WriteLine(retval);

            return retval;
        }

        private static List<MusicModel> DummyData()
        {
            List<MusicModel> retval = new List<MusicModel>()
            {

                new MusicModel()
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
                },
                new MusicModel()
                {
                    Artist = "War",
                    SongTitle = "Low Rider",
                    AlbumTitle = "Low Rider",
                    Genre = "Funk",
                    TourDates = new System.Collections.Generic.List<TourInfo>()
                    {
                        new TourInfo()
                        {
                            City = "Wellington",
                            TourDate = DateTime.Now.AddDays(110)
                        }
                    }
                },
                new MusicModel()
                {
                    Artist = "Temptations",
                    SongTitle = "War",
                    AlbumTitle = "Psychedelic Shack",
                    Genre = "Soul",
                    TourDates = new System.Collections.Generic.List<TourInfo>()
                    {
                        new TourInfo()
                        {
                            City = "Hamilton",
                            TourDate = DateTime.Now.AddDays(105)
                        },
                        new TourInfo()
                        {
                            City = "Sydney",
                            TourDate = DateTime.Now.AddDays(20)
                        }
                    }
                }
            };

            return retval;
        }
    }
}

