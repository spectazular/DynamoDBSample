using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using DynamoDBApp.Shared.Repos.Abstract;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DynamoDBApp.Shared.Repos.Concrete
{
    public class DynamoDBRepo<T> : IDynamoDBRepo<T>
    {
        private DynamoDBContext _context;

        public DynamoDBRepo()
        {
            AmazonDynamoDBClient client = new AmazonDynamoDBClient();
            _context = new DynamoDBContext(client);
        }

        public bool DeleteRecord(string hashKey, string rangeKey)
        {
            return DeleteRecordAsync(hashKey, rangeKey).Result;
        }

        public T GetRecord(string hashKey, string rangeKey)
        {
            return GetRecordAsync(hashKey, rangeKey).Result;
        }

        public bool InserRecord(T item)
        {
            return InserRecordAsync(item).Result;
        }

        public bool UpdateRecord(T item)
        {
            return InserRecordAsync(item).Result;
        }

        private async Task<T> GetRecordAsync(string hashKey, string rangeKey)
        {
            T retval = await _context.LoadAsync<T>(hashKey, rangeKey);
            return retval;
        }

        private async Task<bool> InserRecordAsync(T item)
        {
            try
            {
                await _context.SaveAsync<T>(item);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        private async Task<bool> DeleteRecordAsync(string hashKey, string rangeKey)
        {
            try
            {
                await _context.DeleteAsync<T>(hashKey, rangeKey);
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
