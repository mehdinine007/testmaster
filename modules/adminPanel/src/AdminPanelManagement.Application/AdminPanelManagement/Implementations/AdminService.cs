using AdminPanelManagement.Application.Contracts.AdminPanelManagement.Dtos;
using AdminPanelManagement.Application.Contracts.AdminPanelManagement.IServices;
using AdminPanelManagement.Domain.Shared.AdminPanelManagement.Db;
using AdminPanelManagement.EntityFrameworkCore.AdminPanelManagement.Repositories;
using IFG.Core.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace AdminPanelManagement.Application.AdminPanelManagement.Implementations
{
    public class AdminService : ApplicationService, IAdminService
    {
        private readonly ICustomerOrderRepository _customerOrderRepository;
        public AdminService(ICustomerOrderRepository customeOrderRepository)
        {
            _customerOrderRepository = customeOrderRepository;
        }
        public async Task<UserInfo_CustomerOrderDto> GetCustomerOrderList(string nationalCode)
        {
            UserInfo_CustomerOrderDto userInfo_CustomerOrderDto = new UserInfo_CustomerOrderDto();
            List<CustomerOrderDto> customerOrderList = new List<CustomerOrderDto>();
            var customerOrders = new List<CustomerOrderDb>();
            List<string> cancellationDate = new List<string>();
            List<string> advocacyUsersDate = new List<string>();
            var userInfoDb = await _customerOrderRepository.UserInfo(nationalCode);
            var userRejectionAdvocacy = await _customerOrderRepository.GetUserRejectionAdvocacy(nationalCode);
            userRejectionAdvocacy.ForEach(u =>
            {

                var CreationTime = u.CreationTime;

                cancellationDate.Add(CreationTime);
            });

            var advocacyUsersFromBank = await _customerOrderRepository.GetAdvocacyUsersFromBank(nationalCode);


            advocacyUsersFromBank.ForEach(u =>
            {

                var dateTime = u.dateTime;
                advocacyUsersDate.Add(dateTime);
            });

            if (userInfoDb.UID != null)
            {

                userInfo_CustomerOrderDto.UserId = userInfoDb.Id;
                userInfo_CustomerOrderDto.UID = userInfoDb.UID;
                userInfo_CustomerOrderDto.FirstName = userInfoDb.FirstName;
                userInfo_CustomerOrderDto.LastName = userInfoDb.LastName;
                userInfo_CustomerOrderDto.Mobile = userInfoDb.Mobile;
                customerOrders = await _customerOrderRepository.GetCustomerOrderList(userInfoDb.UID);
                var orderRejections = await _customerOrderRepository.GetOrderRejectionType();
                var orderStatusTypes = await _customerOrderRepository.GetOrderStatusType();


                customerOrders.ForEach(o =>
                {
                    string OrderRejectionTitle = "";
                    string orderStatusTitle = "";
                    if (o.OrderRejectionStatus.HasValue)
                    {
                        var orderRejection = orderRejections.FirstOrDefault(x => x.Code == o.OrderRejectionStatus);
                        if (orderRejection != null)
                        {
                            OrderRejectionTitle = orderRejection.Title;
                        }

                    }
                    if (o.OrderStatus != null)
                    {
                        var orderStatus = orderStatusTypes.FirstOrDefault(x => x.Code == o.OrderStatus);
                        if (orderStatus != null)
                        {
                            orderStatusTitle = orderStatus.Title;
                        }

                    }

                    var customeOrderDto = new CustomerOrderDto
                    {
                        OrderRejectionStatus = OrderRejectionTitle,
                        CompanyName = o.CompanyName,
                        CarName = o.CarName,
                        DeliveryDateDescription = o.DeliveryDateDescription,
                        Id = o.Id,
                        OrderStatus = orderStatusTitle,
                        EsaleType = o.EsaleTypeId == 1 ? "فروش عادی" : o.EsaleTypeId == 2 ? "فروش طرح جوانی" : "",
                        SaleDetailId = o.SaleDetailId,
                        PriorityId = o.PriorityId.HasValue ? (int)o.PriorityId : null,
                        OrderRegistrationPersianDate = o.CreationTime,
                        LastModificationTime = o.LastModificationTime,

                    };

                    customerOrderList.Add(customeOrderDto);
                });

                userInfo_CustomerOrderDto.CustomerOrders = customerOrderList;
            }
            userInfo_CustomerOrderDto.CancellationDate = cancellationDate;
            userInfo_CustomerOrderDto.AdvocacyUsersDate = advocacyUsersDate;
            return userInfo_CustomerOrderDto;

        }
    }
}
