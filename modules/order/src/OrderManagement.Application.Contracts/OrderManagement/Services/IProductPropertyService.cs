using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using OrderManagement.Domain.Shared.OrderManagement.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace OrderManagement.Application.Contracts.OrderManagement.Services
{
    public interface IProductPropertyService : IApplicationService
    {
        Task<List<PropertyCategoryDto>> GetByProductId(int productId);
        Task<List<ProductPropertyDto>> GetList();
        Task<ProductPropertyDto> GetById(string Id);
        Task<ProductPropertyDto> Add(ProductPropertyDto productPropertylDto);
        Task<ProductPropertyDto> Update(ProductPropertyDto productPropertylDto);
        Task<bool> Delete(string Id);
        Task SeedPeroperty(SaleTypeEnum type);
        Task Import(IFormFile file, SaleTypeEnum type);
    }
}
