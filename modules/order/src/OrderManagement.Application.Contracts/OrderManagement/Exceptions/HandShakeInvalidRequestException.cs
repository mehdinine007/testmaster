using Volo.Abp;

namespace OrderManagement.Application.Contracts.OrderManagement.Exceptions
{
    public class HandShakeInvalidRequestException : BusinessException
    {
        public HandShakeInvalidRequestException(int errorCode,string errorMessage)
            : base($"200{errorCode}", errorMessage)
        {
        }

        public HandShakeInvalidRequestException(string errorMessage)
            : base("2001000",errorMessage)
        {
        }

        public const int InvalidAmount = 5000;

        public const int EmptyNationlCode = 2000;

        public const int CallBackUrlIsInvalid = 3000;
    }
}
