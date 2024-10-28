using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee_Management.BLL.IServices
{
    public interface IAuthService
    {
        Task<string> LoginAsync(string username, string password);
    }
}
