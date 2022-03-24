using System.ComponentModel.DataAnnotations;

namespace LGC.Support.Models
{
    public class UserData
    {
        [Key]
        public long id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string email { get; set; }
        public string create_at { get; set; }
        public string confirm_password { get; set; }
        public string error_mess { get; set; }

    }
}
