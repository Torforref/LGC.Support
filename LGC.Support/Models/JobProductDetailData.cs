using System.Collections.Generic;

namespace LGC.Support.Models
{
    public class JobProductDetailData
    {
        public int id { get; set; }
        public int job_id { get; set; }
        public int product_id { get; set; }
        public string serial_number { get; set; }

    }

    public class JobDetailDataViewModel
    {
        public List<JobProductDetailData> Products { get; set; }

    }
}
