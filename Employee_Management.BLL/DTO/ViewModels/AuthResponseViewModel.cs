using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee_Management.BLL.DTO.ViewModels
{
    public class AuthResponseViewModel
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}