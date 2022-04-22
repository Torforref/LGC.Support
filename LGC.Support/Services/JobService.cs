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
            var datas = conn.Query<JobData>(@"SELECT id FROM Jobs WHERE id = (SELECT MAX(id) FROM Jobs)").FirstOrDefault();
            return datas;
        }

        public async Task<List<JobData>> GetAll()
        {
            using var conn = await _db.CreateConnectionAsync();
            var datas = conn.Query<JobData>(@"SELECT * FROM Jobs").ToList();
            return datas;
        }

        public async Task<JobData> GetForEdit(int? id)
        {
            using var conn = await _db.CreateConnectionAsync();
            var datas = conn.Query<JobData>(@"SELECT * FROM Jobs WHERE (id = @id)", new { id }).FirstOrDefault();

            datas.JodDetails = conn.Query<JobProductDetailData>(@"SELECT j.[id]
              ,[job_id]
              ,[product_id], p.name as product_name
              ,[serial_number]
              FROM [dbo].[JobProductDetails] as j
              inner join [dbo].[Products] as p
              on j.product_id = p.id where job_id = @id", new { id }).ToList();

            datas.CustomerDetails = conn.Query<CustomerData>(@"SELECT * FROM Customers WHERE (id = @customer_id)", new { datas.customer_id } ).FirstOrDefault();

            return datas;
        }

        public async Task<JobData> Get(int? id)
        {
            using var conn = await _db.CreateConnectionAsync();
            var datas = conn.Query<JobData>(@"SELECT * FROM Jobs WHERE (id = @id)", new { id }).FirstOrDefault();
            return datas;
        }


        public async Task<JobData> Create(JobData model)
        {
            using var conn = await _db.CreateConnectionAsync();
            var datas = conn.Query<JobData>(@"SELECT * FROM Jobs WHERE (job_number = @job_number)", new { model.job_number }).FirstOrDefault();
            var job_id = int.Parse(model.job_number.Split("LGCSV")[1]);
            if (datas != null)
            {
                return datas;
            }
            else
            {
                foreach (var product in model.JodDetails)
                {
                    product.serial_number = product.serial_number.ToUpper();
                    var sqlStatement1 = $@"INSERT INTO JobProductDetails (
                    job_id, 
                    product_id, 
                    serial_number 
                 ) VALUES (
                    @job_id, 
                    @product_id, 
                    @serial_number 
                )";
                    await conn.ExecuteAsync(sqlStatement1, new
                    {
                        job_id,
                        product.product_id,
                        product.serial_number,
                    });
                }


                model.onsite_limited = model.service_type == "MA" ? 2 : 3;
                model.expire_date = model.service_type == "MA" ? DateTime.Now.AddYears(1) : DateTime.Now.AddYears(3);
                var job_detail_id = conn.Query<JobProductDetailData>(@"SELECT * FROM JobProductDetails WHERE (job_id = @job_id)", new { job_id }).FirstOrDefault();
                var sqlStatement2 = $@"INSERT INTO Jobs (
                    job_number, 
                    service_type, 
                    onsite_limited, 
                    onsite_date, 
                    customer_po, 
                    description, 
                    status, 
                    customer_id,
                    job_product_detail_id,
                    quantity,
                    price,
                    created_by,
                    created_at,
                    expire_date,                
                    is_exp,
                    updated_by,
                    updated_at
                ) VALUES (
                    @job_number, 
                    @service_type, 
                    @onsite_limited, 
                    @onsite_date, 
                    @customer_po, 
                    @description, 
                    @status, 
                    @customer_id,
                    @job_detail_id,
                    @quantity,
                    @price,
                    @created_by,
                    @created_at,
                    @expire_date,                
                    @is_exp,
                    @updated_by,
                    @updated_at
                )";
                await conn.ExecuteAsync(sqlStatement2, new
                {
                    model.job_number,
                    model.service_type,
                    model.onsite_limited,
                    model.onsite_date,
                    model.customer_po,
                    model.description,
                    model.customer_id,
                    job_detail_id = job_detail_id.id,
                    model.quantity,
                    status = "Activate",
                    model.price,
                    created_by = "Titharat",
                    created_at = DateTime.Now,
                    model.expire_date,
                    is_exp = false,
                    updated_by = "Titharat",
                    updated_at = DateTime.Now
                });
                return datas;
            }
        }

        public async Task<JobData> Update(JobData model)
        {
            using var conn = await _db.CreateConnectionAsync();
            var datas = conn.Query<JobData>(@"SELECT * FROM Jobs WHERE (id = @id)", new { model.id }).FirstOrDefault();
            if (datas == null)
            {
                return datas;
            }
            else
            {
                var sqlStatement = @"UPDATE Jobs SET onsite_date = @onsite_date, description = @description, updated_by = @updated_by, updated_at = @updated_at WHERE id = @id";
                await conn.ExecuteAsync(sqlStatement, new { model.onsite_date, model.description, updated_by = "Tithart", updated_at = DateTime.Now, model.id });
                return datas;
            }
        }

        public async Task<JobData> GetJobBySN(JobData model)
        {
            using var conn = await _db.CreateConnectionAsync();
            var Product_detail = conn.Query<JobProductDetailData>(@"SELECT * FROM JobProductDetails WHERE (serial_number = @serial_number)", new { model.JodDetails[0].serial_number }).FirstOrDefault();

            var datas = conn.Query<JobData>(@"SELECT * FROM Jobs WHERE (id = @job_id)", new { Product_detail.job_id }).FirstOrDefault();

            datas.JodDetails = conn.Query<JobProductDetailData>(@"SELECT j.[id]
              ,[job_id]
              ,[product_id], p.name as product_name
              ,[serial_number]
              FROM [dbo].[JobProductDetails] as j
              inner join [dbo].[Products] as p
              on j.product_id = p.id where serial_number = @serial_number", new { Product_detail.serial_number }).ToList();

            return datas;
        }

        public async Task<List<JobProductDetailData>> GetAllJobDetails()
        {
            using var conn = await _db.CreateConnectionAsync();
            var datas = conn.Query<JobProductDetailData>(@"SELECT * FROM JobProductDetails").ToList();
            return datas;
        }


    }
}
