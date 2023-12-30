using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.EntityFrameworkCore.OrderManagement.Repositories
{
    public interface IMigrationsHistoryRepository
    {
        void Execute(string commandText);
        void Drop(string name, string type);

    }
}
