using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsLibrary.Models
{
    public class LoginResponse
    {
        public AuthenticationResponse authResponse { get;set; }
        public Object data { get; set; }    
    }
}
