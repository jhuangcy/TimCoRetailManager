using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimCoRetailManager_WPF.Library.Models
{
    // This will be a singleton user for wpf
    public interface IUser
    {
        string IdentityUserId { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        string Email { get; set; }
        string CreatedDate { get; set; }

        string Token { get; set; }
    }

    public class User : IUser
    {
        public string IdentityUserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string CreatedDate { get; set; }

        public string Token { get; set; }
    }
}
