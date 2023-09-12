using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagement.Application.Contracts.Models;

public class UserInfoPriorityDto
{
    public string NationalCode { get; set; }
    [JsonProperty(PropertyName = "شهرمحل تولد")]
    public string BirthCityName { get; set; }

    public string Mobile { get; set; }

}
