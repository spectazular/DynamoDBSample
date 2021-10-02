using System;
using System.Collections.Generic;
using System.Text;

namespace DynamoDBApp.Shared.Repos.Abstract
{
    interface IDynamoDBRepo<T>
    {
        public bool InserRecord(T item);
        public T GetRecord(string hashKey, string rangeKey);
        public bool DeleteRecord(string hashKey, string rangeKey);
        public bool UpdateRecord(T item);
    }
}
