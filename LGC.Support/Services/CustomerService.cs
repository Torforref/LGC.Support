using Dapper;
using LGC.Suport.Models;
using LGC.Support.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LGC.Support.Services
{
    public class CustomerService
    {
        IDatabaseConnectionFactory _db;
        public CustomerService(IDatabaseConnectionFactory _database)
        {
            _db = _database;
        }

        public async Task<List<CustomerData>> GetAll()
        {
            using var conn = await _db.CreateConnectionAsync();
            var datas = conn.Query<CustomerData>(@"SELECT * FROM Customers").ToList();
            return datas;
        }

        public async Task<CustomerData> Get(int? id)
        {
            using var conn = await _db.CreateConnectionAsync();
            var datas = conn.Query<CustomerData>(@"SELECT * FROM Customers WHERE (id = @id)", new { id }).FirstOrDefault();
            return datas;
        }

        public async Task<CustomerData> Create(CustomerData model)
        {
            using var conn = await _db.CreateConnectionAsync();
            var datas = conn.Query<CustomerData>(@"SELECT * FROM Customers WHERE (name = @name)", new { model.name }).FirstOrDefault();
            if (datas != null)
            {
                return datas;
            }
            else
            {
                var sqlStatement = $@"INSERT INTO Customers (
                    name, 
                    address, 
                    contact, 
                    email,
                    phone_number,
                    created_by, 
                    created_at, 
                    updated_by, 
                    updated_at
                ) VALUES (
                    @name, 
                    @address, 
                    @contact, 
                    @email,
                    @phone_number,
                    @created_by, 
                    @created_at, 
                    @updated_by, 
                    @updated_at
                    )";
                await conn.ExecuteAsync(sqlStatement, new { model.name, model.address, model.contact, model.email, model.phone_number, created_by = "Titharat", created_at = DateTime.Now, updated_by = "Titharat", updated_at = DateTime.Now });
                return datas;
            }
        }

        public async Task<CustomerData> Update(CustomerData model)
        {
            using var conn = await _db.CreateConnectionAsync();
            var datas = conn.Query<CustomerData>(@"SELECT * FROM Customers WHERE (id = @id)", new { model.id }).FirstOrDefault();
            if (datas == null)
            {
                return datas;
            }
            else
            {
                var sqlStatement = $@"UPDATE Customers SET name = @name, address = @address, contact = @contact, email = @email, phone_number = @phone_number, updated_by = @updated_by, updated_at = @updated_at WHERE id = @id";
                await conn.ExecuteAsync(sqlStatement, new { model.name, model.address, model.contact, model.email, model.phone_number, updated_by = "Titharat", updated_at = DateTime.Now, model.id });
                return datas;
            }
        }

        public async Task<CustomerData> Delete(CustomerData model)
        {
            using var conn = await _db.CreateConnectionAsync();
            var datas = conn.Query<CustomerData>(@"SELECT * FROM Customers WHERE (id = @id)", new { model.id }).FirstOrDefault();
            if (datas == null)
            {
                return datas;
            }
            else
            {
                var sqlStatement = $@"DELETE FROM Customers WHERE id = @id";
                await conn.ExecuteAsync(sqlStatement, new { model.id });
                return datas;
            }
        }
    }
}




