using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.Contracts.CompanyManagement
{
    public class TurnDateDto
    {
        public DateTime StartTurnDate { get; set; }
        public DateTime EndTurnDate { get; set; }
    }
}
