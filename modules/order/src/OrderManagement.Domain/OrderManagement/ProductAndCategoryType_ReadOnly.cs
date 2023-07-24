using Esale.Core.Bases;

namespace OrderManagement.Domain.OrderManagement
{
    public class ProductAndCategoryType_ReadOnly : BaseReadOnlyTable
    {
        public ProductAndCategoryType_ReadOnly(int id, string title_En, string title, int code) 
            : base(id, title_En, title, code)
        {
        }


        public ProductAndCategoryType_ReadOnly()
        {

        }
    }
}
