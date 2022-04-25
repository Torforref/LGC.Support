using Dapper;
using LGC.Support.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LGC.Support.Services
{
    public class CalendarService
    {
        IDatabaseConnectionFactory _db;
        public CalendarService(IDatabaseConnectionFactory _database)
        {
            _db = _database;
        }

        public async Task<List<CalendarData>> GetAll()
        {
            using var conn = await _db.CreateConnectionAsync();
            var datas = conn.Query<CalendarData>(@"SELECT * FROM Calendars").ToList();
            return datas;
        }

        public async Task Create(CalendarData model)
        {
            using var conn = await _db.CreateConnectionAsync();

            var sqlStatement = $@"INSERT INTO Calendars (
                    title,
                    event_date,
                    remind_before_day,
                    description, 
                    created_by, 
                    created_at, 
                    updated_by, 
                    updated_at
                ) VALUES (
                    @title, 
                    @event_date, 
                    @remind_before_day,
                    @description, 
                    @created_by, 
                    @created_at, 
                    @updated_by, 
                    @updated_at
                    )";
            await conn.ExecuteAsync(sqlStatement, new { model.title, model.event_date, model.remind_before_day, model.description, created_by = "Titharat", created_at = DateTime.Now, updated_by = "Titharat", updated_at = DateTime.Now });
        }


        public async Task<CalendarData> Update(CalendarData model)
        {
            using var conn = await _db.CreateConnectionAsync();
            var datas = conn.Query<CalendarData>(@"SELECT * FROM Calendars WHERE (id = @id)", new { model.id }).FirstOrDefault();
            if (datas == null)
            {
                return datas;
            }
            else
            {
                var sqlStatement = @"UPDATE Calendars SET title = @title, event_date = @event_date, remind_before_day = @remind_before_day, description = @description, updated_by = @updated_by, updated_at = @updated_at WHERE id = @id";
                await conn.ExecuteAsync(sqlStatement, new { model.title, model.event_date, model.remind_before_day, model.description, updated_by = "Titharat", updated_at = DateTime.Now, id = model.id });

                return datas;
            }
        }

        public async Task<CalendarData> Delete(CalendarData model)
        {
            using var conn = await _db.CreateConnectionAsync();
            var datas = conn.Query<CalendarData>(@"SELECT * FROM Calendars WHERE (id = @id)", new { model.id }).FirstOrDefault();
            if (datas == null)
            {
                return datas;
            }
            else
            {
                var sqlStatement = @"DELETE FROM Calendars WHERE id = @id";
                await conn.ExecuteAsync(sqlStatement, new { model.id });
                return datas;
            }
        }

    }
}





