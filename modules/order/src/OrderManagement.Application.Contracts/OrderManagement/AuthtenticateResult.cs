namespace OrderManagement.Application.Contracts
{
    public class AuthtenticateResult
    {
        public int Status { get; set; }
        public object Message { get; set; }
        public Model Model { get; set; }
    }
}