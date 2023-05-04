using System.Collections.Generic;

namespace OrderManagement.Application.Contracts
{
    public class CarTipDto
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public int CarTypeId { get; set; }

        public List<string> CarImageUrls { get; set; }
    }
}