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

        public async Task<List<ProductData>> FindService()
        {
            using var conn = await _db.CreateConnectionAsync();
            var datas = conn.Query<ProductData>(@"SELECT * FROM Products").ToList(); ;

            return datas;
        }

        //public async Task<ProductData> CreateService(string username, string password)
        //{
        //    using var conn = await _db.CreateConnectionAsync();
        //    var newProduct = new ProductData();
        //    var datas = conn.Query<ProductData>(@"SELECT * FROM Products WHERE (product_name = @product_name)", new { product_name = username }).FirstOrDefault();
        //    if (datas != null)
        //    {
        //        return datas;
        //    }
        //    else
        //    {
        //        var sqlStatement = $@"INSERT INTO Users (
        //                product_name, 
        //                description, 
        //                product_category, 
        //                created_by, 
        //                created_at, 
        //                updated_by) 
        //        VALUES (
        //                @product_name, 
        //                @description, 
        //                @product_category, 
        //                @created_by, 
        //                @created_at, 
        //                @updated_by)";
        //        await conn.ExecuteAsync(sqlStatement, new { product_name, description, product_category, create_by = "Titharat", create_at = DateTime.Now, updated_by = "Titharat" });
        //        return datas;
        //    }
        //}
    }
}
