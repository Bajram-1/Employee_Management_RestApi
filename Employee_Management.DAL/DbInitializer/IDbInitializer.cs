using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee_Management.DAL.DbInitializer
{
    public interface IDbInitializer
    {
        Task InitializeAsync();
    }
}