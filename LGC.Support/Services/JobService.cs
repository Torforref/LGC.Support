using Dapper;
using LGC.Suport.Models;
using LGC.Support.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LGC.Support.Services
{
    public class JobService
    {
        IDatabaseConnectionFactory _db;
        public JobService(IDatabaseConnectionFactory _database)
        {
            _db = _database;
        }

        public async Task<JobData> GetJobNumber()
        {
            using var conn = await _db.CreateConnectionAsync();
            var datas = conn.Query<JobData>(@"SELECT MAX(id) as latest_id from Jobs").FirstOrDefault();
            return datas;
        }


    }
}
