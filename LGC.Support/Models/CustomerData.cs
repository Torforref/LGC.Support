using System;
using System.ComponentModel.DataAnnotations;

namespace LGC.Suport.Models
{
    public class CustomerData
    {
        public int id { get; set; }
        public string name { get; set; }
        public string address { get; set; }
        public string contact { get; set; }
        public string email { get; set; }
        public string phone_number { get; set; }
        public string created_by { get; set; }
        public DateTime created_at { get; set; } = DateTime.Now;
        public string updated_by { get; set; }
        public DateTime updated_at { get; set; } = DateTime.Now;

    }
}
