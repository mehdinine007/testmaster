using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.Contracts
{
    public static class PermissionConstant
    {
        public static string OrderModule = "0001";

        public static string SaleSchema = "00010002";

        public static string SaleSchemaGetList = "000100020001";
        public static string SaleSchemaAdd = "000100020002";
        public static string SaleSchemaUpdate = "000100020003";
        public static string SaleSchemaDelete = "000100020004";
        public static string SaleSchemaGetById = "000100020005";
        public static string SaleSchemaUploadFile = "000100020006";

        public static string SaleDetail = "00010003";

        public static string SaleDetailGetSaleDetails = "000100030001";
        public static string SaleDetailSave = "000100030002";
        public static string SaleDetailUpdate = "000100030003";
        public static string SaleDetailDelete = "000100030004";
        public static string SaleDetailGetById = "000100030005";
        public static string SaleDetailGetActiveList = "000100030007";
        
    }
}
