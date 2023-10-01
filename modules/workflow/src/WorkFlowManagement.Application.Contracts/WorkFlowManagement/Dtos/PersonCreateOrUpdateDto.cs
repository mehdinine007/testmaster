using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkFlowManagement.Application.Contracts.WorkFlowManagement.Dtos
{
    public class PersonCreateOrUpdateDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string NationalCode { get; set; }
    }
}
