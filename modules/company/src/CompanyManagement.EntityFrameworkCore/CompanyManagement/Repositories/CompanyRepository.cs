using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using CompanyManagement.EfCore.CompanyManagement.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using CompanyManagement.Domain.CompanyManagement;
using CompanyManagement.Domain.Shared.CompanyManagement;

namespace CompanyManagement.EfCore.CompanyManagement.Repositories
{
    public class CompanyRepository : EfCoreRepository<CompanyManagementDbContext, CompanyPaypaidPrices, long>, ICompanyRepository
    {
        private readonly IConfiguration _configuration;
        public CompanyRepository(IConfiguration configuration, IDbContextProvider<CompanyManagementDbContext> dbContextProvider)
          : base(dbContextProvider)
        {
            _configuration = configuration;
        }
        
        public   List<CustomersWithCars> GetCustomerOrderList(CustomersAndCarsInputDto customersAndCarsInputDto)
        {
            List<CustomersWithCars> lsCustomersWithCars = new List<CustomersWithCars>();

            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("OrderManagement")))
            {
                SqlCommand cmd = new SqlCommand("CustomerCarDeliveryTime", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyId", SqlDbType.Int).Value = customersAndCarsInputDto.CompanyId;
                cmd.Parameters.Add("@SaleId", SqlDbType.Int).Value = customersAndCarsInputDto.SaleId;
                cmd.Parameters.Add("@PageIndex", SqlDbType.Int).Value = customersAndCarsInputDto.PageNo;
                cmd.Parameters.Add("@Type", SqlDbType.Int).Value = 0;

                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    CustomersWithCars customersWithCars = new CustomersWithCars();
                    customersWithCars.Id = long.Parse(rdr["Id"].ToString());
                    customersWithCars.orderid = long.Parse(rdr["orderid"].ToString());
                    customersWithCars.cartipid = int.Parse(rdr["cartipid"].ToString());
                    customersWithCars.title = (rdr["title"].ToString());
                    customersWithCars.NationalCode = (rdr["NationalCode"].ToString());
                    customersWithCars.Mobile = (rdr["Mobile"].ToString());
                    customersWithCars.Name = (rdr["Name"].ToString());
                    customersWithCars.Surname = (rdr["Surname"].ToString());
                    customersWithCars.FatherName = (rdr["FatherName"].ToString());
                    customersWithCars.BirthCertId = (rdr["BirthCertId"].ToString());
                    customersWithCars.BirthDate = DateTime.Parse(rdr["BirthDate"].ToString());
                    customersWithCars.Gender = short.Parse(rdr["Gender"].ToString());
                    customersWithCars.Radif = int.Parse(rdr["Radif"].ToString() == "" ? "0" : rdr["Radif"].ToString());
                    customersWithCars.CityID = int.Parse(rdr["CityID"].ToString() == "" ? "0" : rdr["CityID"].ToString());
                    customersWithCars.ProvinceId = int.Parse(rdr["ProvinceId"].ToString() == "" ? "0" : rdr["ProvinceId"].ToString());
                    customersWithCars.City = (rdr["City"].ToString());
                    customersWithCars.Province = (rdr["Province"].ToString());
                    customersWithCars.PostalCode = (rdr["PostalCode"].ToString());
                    customersWithCars.Address = (rdr["Address"].ToString());
                    customersWithCars.IssuingDate = DateTime.Parse(rdr["IssuingDate"].ToString());
                    customersWithCars.Shaba = (rdr["Shaba"].ToString());
                    customersWithCars.DeliveryDateDescription = (rdr["DeliveryDateDescription"].ToString());
                    customersWithCars.OrderRejectionStatus = int.Parse(rdr["OrderRejectionStatus"].ToString() == "" ? "0" : rdr["OrderRejectionStatus"].ToString());
                    customersWithCars.sherkat = (rdr["sherkat"].ToString());
                    customersWithCars.ESaleTypeId = int.Parse(rdr["ESaleTypeId"].ToString());
                    customersWithCars.PlaqueStatus = true;
                    customersWithCars.CertificateStatus = true;
                    customersWithCars.ShahkarStatus = true;
                    customersWithCars.BlackList = "";
                    lsCustomersWithCars.Add(customersWithCars);
                }
                con.Close();
                rdr.Close();
               
            }

            return lsCustomersWithCars;
        }
    }
}
