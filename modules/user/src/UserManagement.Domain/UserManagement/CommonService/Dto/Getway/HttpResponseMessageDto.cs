using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagement.Domain.UserManagement.CommonService.Dto.Getway
{
    public class HttpResponseMessageDto
    {
        public bool IsSuccessStatusCode { get; set; }
        public string StringContent { get; set; }
    }
}
