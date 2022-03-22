﻿using Dapper;
using LGC.Support.Models;
using System.Linq;
using System.Threading.Tasks;

namespace LGC.Support.Services
{
    public class UserService
    {
        IDatabaseConnectionFactory _db;
        public UserService(IDatabaseConnectionFactory _database)
        {
            _db = _database;
        }
        public async Task<UserData> LoginService(string username, string password)
        {
            using var conn = await _db.CreateConnectionAsync();
            string _pwd = "qvh/3K34ENe9gmoIAnm1vw==";
            var datas = conn.Query<UserData>(@"SELECT * FROM  Users WHERE (username = @user) AND password = @pass", new { user = username, pass = _pwd }).FirstOrDefault();
            return datas;
        }
    }
}
