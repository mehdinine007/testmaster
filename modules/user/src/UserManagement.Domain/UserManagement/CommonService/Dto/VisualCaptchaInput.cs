using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagement.Domain.UserManagement.CommonService.Dto
{
    public class VisualCaptchaInput
    {
        public string CT { get; set; }
        public string CK { get; set; }//captcha text
        public string CIT { get; set; }//captcha random
        public VisualCaptchaInput(string ct, string ck, string cit)
        {
            CT = ct;
            CK = ck;
            CIT = cit;
        }
        public VisualCaptchaInput(string ck, string cit)
        {
            CK = ck;
            CIT = cit;
        }
    }
}
