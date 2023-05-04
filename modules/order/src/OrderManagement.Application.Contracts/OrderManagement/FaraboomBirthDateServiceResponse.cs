namespace OrderManagement.Application.Contracts
{
    public class FaraboomBirthDateServiceResponse
    {
        public long operation_time { get; set; }

        public string ref_id { get; set; }

        public bool match { get; set; }
    }
}