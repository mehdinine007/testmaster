namespace OrderManagement.Application.Contracts
{
    public class UserDataAccessDto
    {
        public Guid UserId { get; set; }
        public string Nationalcode { get; set; }
        public RoleTypeEnum RoleTypeId { get; set; }
        public string Data { get; set; }
    }
}
