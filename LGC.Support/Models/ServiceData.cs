using System.ComponentModel.DataAnnotations;

namespace LGC.Suport.Models
{
    public class ServiceData
    {
        [Key]
        public int id { get; set; }
        public string service_type { get; set; }
        public int onsite_limited { get; set; }
        public int service_period_in_year { get; set; }
    }
}
