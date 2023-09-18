using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Domain.UserManagement.Enums;

namespace UserManagement.Domain.UserManagement.CommonService.Dto.Getway
{
    public class SendBoxServiceInput
    {
        public string Recipient { get; set; }
        public string Text { get; set; }
        public ProviderSmsTypeEnum Provider { get; set; }
        public TypeMessageEnum Type { get; set; }
    }
}
