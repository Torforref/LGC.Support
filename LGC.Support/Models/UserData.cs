using System.ComponentModel.DataAnnotations;

namespace LGC.Support.Models
{
    public class UserData
    {
        public int id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string user_is { get; set; }
        public string create_at { get; set; }
        public string new_password { get; set; }
        public string confirm_password { get; set; }
        public string error_mess { get; set; }
    }
}
