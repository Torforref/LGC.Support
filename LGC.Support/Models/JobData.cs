using LGC.Support.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LGC.Suport.Models
{
    public class JobData
    {
        public int id { get; set; }
        public string job_number { get; set; }
        public string service_plan { get; set; }
        public int service_year_period { get; set; }
        public int onsite_limited { get; set; }
        public string customer_po { get; set; }
        public string description { get; set; }
        public int customer_id { get; set; }
        public int quantity { get; set; }
        public float price { get; set; }
        public string created_by { get; set; }
        public DateTime created_at { get; set; } = DateTime.Now;
        public DateTime expire_date { get; set; }
        public bool is_exp { get; set; }
        public string updated_by { get; set; }
        public DateTime updated_at { get; set; } = DateTime.Now;

        public List<JobProductDetailData> JodDetails { get; set; }
        public List<JobProductDetailData> Count_Product_Unique { get; set; }
        public List<JobProductDetailData> SN_form_DB { get; set; }
        public CustomerData CustomerDetails { get; set; }

    }
}
