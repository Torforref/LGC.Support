using Dapper;
using LGC.Support.Models;
using System;
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
        public async Task<UserData> LoginService(UserData model)
        {
            using var conn = await _db.CreateConnectionAsync();
            model.password = EncDecHelper.EncryptData(model.password);
            var datas = conn.Query<UserData>(@"SELECT * FROM Users WHERE (username = @user) AND password = @pass", new { user = model.username, pass = model.password }).FirstOrDefault();
            return datas;
        }
        public async Task<UserData> RegisterService(UserData model)
        {
            using var conn = await _db.CreateConnectionAsync();
            model.password = EncDecHelper.EncryptData(model.password);
            var datas = conn.Query<UserData>(@"SELECT * FROM Users WHERE (username = @user)", new { user = model.username }).FirstOrDefault();
            if (datas != null)
            {
                return datas;
            }
            else
            {
                if (model.username == "admin@logicode.co.th")
                {
                    model.user_is = "Admin";
                }
                else
                {
                    model.user_is = "Staff";
                }
                var sqlStatement = $@"INSERT INTO Users (username, password, first_name, last_name, user_is, create_at) VALUES (@username, @password, @first_name, @last_name, @user_is, @create_at)";
                await conn.ExecuteAsync(sqlStatement, new { model.username, model.password, model.first_name, model.last_name, model.user_is, create_at = DateTime.Now });
                return datas;
            }
        }

        public async Task<UserData> Get(int? id)
        {
            using var conn = await _db.CreateConnectionAsync();
            var datas = conn.Query<UserData>(@"SELECT * FROM Products WHERE (id = @id)", new { id }).FirstOrDefault();
            return datas;
        }
    }
}
