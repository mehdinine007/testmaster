using Volo.Abp;

namespace OrderManagement
{
    public class OrderCodeAlreadyExistsException : BusinessException
    {
        public OrderCodeAlreadyExistsException(string OrderCode)
            : base("PM:000001", $"A Order with code {OrderCode} has already exists!")
        {

        }
    }
}