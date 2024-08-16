using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimCoRetailManager_API._3.Models
{
    public record Register(
        string FirstName, 
        string LastName, 
        string Email, 
        string Password
    );
}
