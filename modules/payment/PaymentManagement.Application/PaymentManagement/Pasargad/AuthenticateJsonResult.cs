namespace PaymentManagement.Application.Pasargad
{   
    public class AuthenticateJsonResult
    {
        public string Token { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string BusinessName { get; set; }
        public string UserId { get; set; }
        public string UserIp { get; set; }
        public List<Role> Roles { get; set; }
    }
    public class Role
    {
        public string Authority { get; set; }
    }
}


