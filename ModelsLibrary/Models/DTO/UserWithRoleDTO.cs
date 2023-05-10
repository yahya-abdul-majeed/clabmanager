using Microsoft.AspNetCore.DataProtection.KeyManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsLibrary.Models.DTO
{
    public class UserWithRoleDTO
    {
        public string Id { get; set; }  
        public string Email { get; set; }
        public string UserName { get; set; }    
        public string Role { get; set; }
    }
}
