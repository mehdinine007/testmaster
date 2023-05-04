namespace OrderManagement.Application.Contracts
{
    public class CaptchaInputDto
    {
        public string Token { get; set; }
        public string Method { get; set; }
        public CaptchaInputDto(string _token, string _method)
        {
            Token = _token;
            _method = Method;
        }
    }
}