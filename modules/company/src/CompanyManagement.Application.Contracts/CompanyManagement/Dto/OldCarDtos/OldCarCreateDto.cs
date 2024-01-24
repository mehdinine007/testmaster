using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyManagement.Application.Contracts.CompanyManagement.Dto.OldCarDtos
{

    public class OldCarCreateDtoList
    {
        public string Vehicle { get; set; }
        public List<OldCarCreateDto> OldCars { get; set; }
    }

    public class OldCarCreateDto
    {
        public string Vehicle { get; set; }
        public string Nationalcode { get; set; }
        public string Vin { get; set; }
        public string ChassiNo { get; set; }
        public string EngineNo { get; set; }
    }
}

