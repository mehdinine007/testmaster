
namespace CompanyManagement.Domain.Shared.CompanyManagement
{
    public class CustomersWithCars
    {
        public long Id { get; set; }
        public long orderid { get; set; }
        public int cartipid { get; set; }
        public string title { get; set; }
        public string NationalCode { get; set; }
        public string Mobile { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string FatherName { get; set; }
        public string BirthCertId { get; set; }
        public DateTime BirthDate { get; set; }
        public short Gender { get; set; }
        public int Radif { get; set; }
        public int CityID { get; set; }
        public string City { get; set; }
        public int ProvinceId { get; set; }
        public string Province { get; set; }
        public string Tel { get; set; }
        public string PostalCode { get; set; }
        public string Address { get; set; }
        public DateTime IssuingDate { get; set; }
        public string Shaba { get; set; }
        public string DeliveryDateDescription { get; set; }
        public int? OrderRejectionStatus { get; set; }
        public string sherkat { get; set; }
        public int ESaleTypeId { get; set; }
        public bool? ShahkarStatus { get; set; }
        public bool CertificateStatus { get; set; }
        public bool PlaqueStatus { get; set; }
        public string BlackList { get; set; }
        public int SaleId { get; set; }
        public string TrackingCode { get; set; }
        public string Vin { get; set; }
        public string EngineNo { get; set; }
        public string ChassiNo { get; set; }
        public string Vehicle { get; set; }

    }
}
