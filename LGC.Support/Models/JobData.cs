using System;
using System.ComponentModel.DataAnnotations;

namespace LGC.Suport.Models
{
    public class JobData
    {
        [Key]
        public int id { get; set; }
        [Required]
        public string job_number { get; set; }
        public string service_type { get; set; }
        public int onsite_count { get; set; }
        public string? description { get; set; }
        public string? status { get; set; }
        public string? condition { get; set; }
        public string? detail { get; set; }
        public int customer_id { get; set; }
        public int product_id { get; set; }
        public int quantity { get; set; }
        public string created_by { get; set; }
        public DateTime created_at { get; set; } = DateTime.Now;
        public bool is_exp { get; set; }
        public string updated_by { get; set; }
        public DateTime updated_at { get; set; } = DateTime.Now;

    }
}
