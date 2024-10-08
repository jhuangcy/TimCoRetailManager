﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimCoRetailManager_API._3.Models
{
    public class UserDTO
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public Dictionary<string, string> Roles { get; set; } = new Dictionary<string, string>();
    }
}