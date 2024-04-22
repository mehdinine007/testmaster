using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.Contracts.OrderManagement.Dtos.Sign
{
    public class DocumentParameterSign
    {
        public List<DataFields> dataFields { get; set; }
        public SignatureImageTextParameter signatureImageTextParameter { get; set; }
    }
    public class DataFields
    {
        public string dataFieldType { get; set; }
        public int pageNumber { get; set; }
        public string tag { get; set; }
        public decimal topRel { get; set; }
        public decimal leftRel { get; set; }
        public decimal heightRel { get; set; }
        public decimal widthRel { get; set; }
    }
    public class SignatureImageTextParameter
    {
        public string customText { get; set; }
        public bool name { get; set; }
        public bool signDate { get; set; }
    }
}
