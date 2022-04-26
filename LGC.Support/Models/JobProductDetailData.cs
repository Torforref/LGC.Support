using System.Collections.Generic;

namespace LGC.Support.Models
{
    public class JobProductDetailData
    {
        public int id { get; set; }
        public int job_id { get; set; }
        public int product_id { get; set; }
        public string serial_number { get; set; }
        public string product_name { get; set; }
        public string old_serial_number { get; set; }
        public string new_serial_number { get; set; }

    }

}
