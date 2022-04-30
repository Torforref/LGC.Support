using Dapper;
using LGC.Suport.Models;
using LGC.Support.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LGC.Support.Services
{
    public class ProductService
    {
        IDatabaseConnectionFactory _db;
        public ProductService(IDatabaseConnectionFactory _database)
        {
            _db = _database;
        }

        public async Task<List<ProductData>> GetAll()
        {
            using var conn = await _db.CreateConnectionAsync();
            var datas = conn.Query<ProductData>(@"SELECT * FROM Products").ToList();
            return datas;
        }

        public async Task<ProductData> Get(int? id)
        {
            using var conn = await _db.CreateConnectionAsync();
            var datas = conn.Query<ProductData>(@"SELECT * FROM Products WHERE (id = @id)", new { id }).FirstOrDefault();
            return datas;
        }

        public async Task<ProductData> Create(ProductData model)
        {
            using var conn = await _db.CreateConnectionAsync();
            model.name = model.name.ToUpper();
            var datas = conn.Query<ProductData>(@"SELECT * FROM Products WHERE (name = @name)", new { model.name }).FirstOrDefault();
            if (datas != null)
            {
                return datas;
            }
            else
            {
                var sqlStatement = $@"INSERT INTO Products (
                    name, 
                    description, 
                    description2, 
                    category, 
                    brand, 
                    created_by, 
                    created_at, 
                    updated_by, 
                    updated_at
                ) VALUES (
                    @name, 
                    @description, 
                    @description2, 
                    @category, 
                    @brand, 
                    @created_by, 
                    @created_at, 
                    @updated_by, 
                    @updated_at
                    )";
                await conn.ExecuteAsync(sqlStatement, new { model.name, model.description, model.description2, model.category, model.brand, created_by = "Titharat", created_at = DateTime.Now, updated_by = "Titharat", updated_at = DateTime.Now });
                return datas;
            }
        }

        public async Task<ProductData> Update(ProductData model)
        {
            using var conn = await _db.CreateConnectionAsync();
            model.name = model.name.ToUpper();
            var datas = conn.Query<ProductData>(@"SELECT * FROM Products WHERE (id = @id)", new { model.id }).FirstOrDefault();
            if (datas == null)
            {
                return datas;
            }
            else
            {
                var sqlStatement = @"UPDATE Products SET name = @name, description = @description, description2 = @description2, category = @category, brand = @brand, updated_by = @updated_by, updated_at = @updated_at WHERE id = @id";
                await conn.ExecuteAsync(sqlStatement, new { model.name, model.description, model.description2, model.category, model.brand, updated_by = "Tithart", updated_at = DateTime.Now, model.id });
                return datas;
            }
        }

        public async Task<ProductData> Delete(ProductData model)
        {
            using var conn = await _db.CreateConnectionAsync();
            var datas = conn.Query<ProductData>(@"SELECT * FROM Products WHERE (id = @id)", new { model.id }).FirstOrDefault();
            if (datas == null)
            {
                return datas;
            }
            else
            {
                var sqlStatement = @"DELETE FROM Products WHERE id = @id";
                await conn.ExecuteAsync(sqlStatement, new { model.id });
                return datas;
            }
        }
    }
}
