using Core.Utility.Tools;
using MongoDB.Bson;
using Nest;
using OrderManagement.Application.Contracts;
using OrderManagement.Application.Contracts.OrderManagement.Services;
using OrderManagement.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;


namespace OrderManagement.HttpApi;

public class PropertyCategoryService : ApplicationService, IPropertyCategoryService
{
    private readonly IRepository<PropertyCategory, ObjectId> _propertyCategoryRepository;
    public PropertyCategoryService(IRepository<PropertyCategory, ObjectId> propertyCategoryRepository)
    {
        _propertyCategoryRepository = propertyCategoryRepository;
    }

    public async Task<List<PropertyCategoryDto>> GetList()
    {
        List<PropertyCategory> propertyCategory = new();

        var propertyCategoryQuery = await _propertyCategoryRepository.GetQueryableAsync();
        propertyCategory = propertyCategoryQuery.ToList();
        var getPropertyCategory = ObjectMapper.Map<List<PropertyCategory>, List<PropertyCategoryDto>>(propertyCategory);
        return getPropertyCategory;
    }
    public async Task<PropertyCategoryDto> GetById(string Id)
    {
        ObjectId objectId;
        if (ObjectId.TryParse(Id, out objectId))
        {
            var propertyCategory = (await _propertyCategoryRepository.GetQueryableAsync())
           .FirstOrDefault(x => x.Id == objectId);
            return ObjectMapper.Map<PropertyCategory, PropertyCategoryDto>(propertyCategory);
        }
        else
        {
            throw new UserFriendlyException(OrderConstant.NotValid, OrderConstant.NotValidId);
        }
        
    }
    public async Task<PropertyCategoryDto> Add(PropertyCategoryDto propertyCategoryDto)
    {
        var propertyCategoryQuery = await _propertyCategoryRepository.GetQueryableAsync();
        var getpropertyCategory = propertyCategoryQuery.FirstOrDefault(x => x.Id == ObjectId.Parse(propertyCategoryDto.Id));

        if (getpropertyCategory != null)
        {
            throw new UserFriendlyException(OrderConstant.DuplicatePriority, OrderConstant.DuplicatePriorityId);
        }

        if (propertyCategoryDto.Id == ObjectId.Empty.ToString())
        {
            throw new UserFriendlyException(OrderConstant.InCorrectPriorityNumber, OrderConstant.InCorrectPriorityNumberId);
        }

        propertyCategoryDto.Id = ObjectId.GenerateNewId().ToString();

        var mapPropertyCategoryDto = ObjectMapper.Map<PropertyCategoryDto, PropertyCategory>(propertyCategoryDto);
        var entity = await _propertyCategoryRepository.InsertAsync(mapPropertyCategoryDto, autoSave: true);

        return ObjectMapper.Map<PropertyCategory, PropertyCategoryDto>(entity);
    }
    public async Task<PropertyCategoryDto> Update(PropertyCategoryDto propertyCategoryDto)
    {
        var existingEntity = await _propertyCategoryRepository.FirstOrDefaultAsync(x => x.Id == ObjectId.Parse(propertyCategoryDto.Id));
        if (existingEntity == null)
        {
            throw new UserFriendlyException(OrderConstant.ProductLevelNotFound, OrderConstant.ProductLevelNotFoundId);
        }
        var getPropertyCategory = await _propertyCategoryRepository.FirstOrDefaultAsync(x => x.Id == ObjectId.Parse(propertyCategoryDto.Id) && x.Title == existingEntity.Title);
        if (getPropertyCategory == null)
        {
            throw new UserFriendlyException(OrderConstant.DuplicatePriority, OrderConstant.DuplicatePriorityId);
        }
        existingEntity.Properties = ObjectMapper.Map<List<PropertyDto>, List<Property>>(propertyCategoryDto.Properties);
        existingEntity.Title = propertyCategoryDto.Title;
        await _propertyCategoryRepository.UpdateAsync(existingEntity, autoSave: true);
        return ObjectMapper.Map<PropertyCategory, PropertyCategoryDto>(existingEntity);
    }
    public async Task<bool> Delete(string Id)
    {
        ObjectId objectId;
        if (ObjectId.TryParse(Id, out objectId))
        {
            await _propertyCategoryRepository.DeleteAsync(x => x.Id == objectId, autoSave: true);
            return true;
        }
        else
        {
            throw new UserFriendlyException(OrderConstant.NotValid, OrderConstant.NotValidId);

        }
    }
}

