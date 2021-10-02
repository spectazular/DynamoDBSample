using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestConsoleApp.Models;

namespace TestConsoleApp.Repo
{
    public class DynamoDBRepo
    {
        private DynamoDBContext _context;

        public DynamoDBRepo()
        {
            AmazonDynamoDBClient client = new AmazonDynamoDBClient();
            _context = new DynamoDBContext(client);
        }

        public MusicModel GetDataByArtist(string artist, string songTitle)
        {
            MusicModel result = GetDataByArtistAsync(artist, songTitle).Result;
            return result;
        }

        public bool WriteData(MusicModel item)
        {
            bool retval = WriteDataAsync(item).Result;
            return retval;
        }

        public bool DeleteData(string artist, string songTitle)
        {
            bool retval = DeleteDataAsync(artist, songTitle).Result;
            return retval;
        }

        private async Task<bool> DeleteDataAsync(string artist, string songTitle)
        {
            try
            {
                await _context.DeleteAsync<MusicModel>(artist, songTitle);
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        private async Task<MusicModel> GetDataByArtistAsync(string artist, string songTitle)
        {
            MusicModel musicRecord = await _context.LoadAsync<MusicModel>(artist, songTitle);

            return musicRecord;
        }

        private async Task<bool> WriteDataAsync(MusicModel item)
        {
            try
            {
                await _context.SaveAsync<MusicModel>(item);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

    }
}
