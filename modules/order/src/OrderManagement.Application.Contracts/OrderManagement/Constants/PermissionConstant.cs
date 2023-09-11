using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.Contracts
{
    public static class PermissionConstant
    {
        public const string OrderModule = "0001";

        public const string SaleSchema = "00010002";

        public const string SaleSchemaGetList = "000100020001";
        public const string SaleSchemaAdd = "000100020002";
        public const string SaleSchemaUpdate = "000100020003";
        public const string SaleSchemaDelete = "000100020004";
        public const string SaleSchemaGetById = "000100020005";
        public const string SaleSchemaUploadFile = "000100020006";

        public const string SaleDetail = "00010003";

        public const string SaleDetailGetSaleDetails = "000100030001";
        public const string SaleDetailSave = "000100030002";
        public const string SaleDetailUpdate = "000100030003";
        public const string SaleDetailDelete = "000100030004";
        public const string SaleDetailGetById = "000100030005";
        public const string SaleDetailGetActiveList = "000100030007";
        
    }
}
