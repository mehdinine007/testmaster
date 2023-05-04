namespace OrderManagement.Application.Contracts
{
    public class CompanyDto
    {

        public int Id { get; set; }

        public string Name { get; set; }

        public string LogoUrl { get; set; }

        public string LogoInPageUrl { get; set; }

        public string BannerUrl { get; set; }

        public bool Visible { get; set; }
    }
}