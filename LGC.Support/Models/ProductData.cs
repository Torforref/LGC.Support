using System;
using System.ComponentModel.DataAnnotations;

namespace LGC.Suport.Models
{
    public class ProductData
    {
        public int id { get; set; }
        public string product_name { get; set; }
        public string description { get; set; }
        public string product_category { get; set; }
        public string created_by { get; set; }
        public DateTime created_at { get; set; } = DateTime.Now;
        public string updated_by { get; set; }
        public DateTime updated_at { get; set; } = DateTime.Now;

    }
}
