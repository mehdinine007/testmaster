namespace OrderManagement.Application.Contracts
{
    public class ApiResult<TModel> where TModel : class
    {
        public TModel Result { get; set; }

        public bool Success { get; set; }

        public object Error { get; set; }
    }
}