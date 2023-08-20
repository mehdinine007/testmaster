using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.Contracts
{
    public enum OperatorFilterEnum
    {
        Equal = 1,
        EqualOpposite = 2,
        Bigger = 3,
        Smaller = 4,
        Like = 5,
        StartWith = 6,
        EndWith = 7
    }
}
