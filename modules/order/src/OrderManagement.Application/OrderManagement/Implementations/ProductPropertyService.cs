using Esale.Share.Authorize;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using Nest;
using OfficeOpenXml;
using OrderManagement.Application.Contracts;
using OrderManagement.Application.Contracts.OrderManagement;
using OrderManagement.Application.Contracts.OrderManagement.Constants.Permissions;
using OrderManagement.Application.Contracts.OrderManagement.Services;
using OrderManagement.Domain;
using OrderManagement.Domain.OrderManagement;
using OrderManagement.Domain.OrderManagement.MongoWrite;
using OrderManagement.Domain.Shared;
using OrderManagement.Domain.Shared.OrderManagement.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;


namespace OrderManagement.Application
{
    public class ProductPropertyService : ApplicationService, IProductPropertyService
    {
        private readonly IRepository<ProductProperty, ObjectId> _productPropertyRepository;
        private readonly IRepository<ProductPropertyWrite, ObjectId> _productPropertyWriteRepository;
        private readonly IRepository<PropertyCategory, ObjectId> _propertyDefinitionRepository;
        private readonly IRepository<PropertyCategoryWrite, ObjectId> _propertyDefinitionWriteRepository;
        private readonly IRepository<ProductAndCategory, int> _productAndCategoryRepository;


        public ProductPropertyService(IRepository<ProductProperty, ObjectId> productPropertyRepository,
            IRepository<PropertyCategory, ObjectId> propertyDefinitionRepository,
            IRepository<ProductAndCategory, int> productAndCategoryRepository,
            IRepository<ProductPropertyWrite, ObjectId> productPropertyWriteRepository,
            IRepository<PropertyCategoryWrite, ObjectId> propertyDefinitionWriteRepository)
        {
            _productPropertyRepository = productPropertyRepository;
            _propertyDefinitionRepository = propertyDefinitionRepository;
            _productAndCategoryRepository = productAndCategoryRepository;
            _productPropertyWriteRepository = productPropertyWriteRepository;
            _propertyDefinitionWriteRepository = propertyDefinitionWriteRepository;
        }

        public async Task<List<PropertyCategoryDto>> GetByProductId(int productId)
        {
            var productPropertyQuery = await _productPropertyRepository.GetQueryableAsync();
            var productProperty = productPropertyQuery
                .FirstOrDefault(x => x.ProductId == productId);
            if (productProperty == null)
                return null;
            return ObjectMapper.Map<List<PropertyCategory>, List<PropertyCategoryDto>>(productProperty.PropertyCategories);
        }
        
        public async Task<List<ProductPropertyDto>> GetList()
        {
            List<ProductProperty> productProperty = new();
            var productPropertyQuery = await _productPropertyRepository.GetQueryableAsync();
            productProperty = productPropertyQuery.ToList();
            var getProductProperty = ObjectMapper.Map<List<ProductProperty>, List<ProductPropertyDto>>(productProperty);
            return getProductProperty;
        }
        public async Task<ProductPropertyDto> GetById(string Id)
        {
            ObjectId objectId;
            if (ObjectId.TryParse(Id, out objectId))
            {
                var productLevel = (await _productPropertyRepository.GetQueryableAsync())
               .FirstOrDefault(x => x.Id == objectId);
                return ObjectMapper.Map<ProductProperty, ProductPropertyDto>(productLevel);
            }
            else
            {
                throw new UserFriendlyException(OrderConstant.NotValid, OrderConstant.NotValidId);
            }
        }

        [SecuredOperation(ProductPropertyServicePermissionConstants.Add)]
        public async Task<ProductPropertyDto> Add(ProductPropertyDto productPropertyDto)
        {
            var productPropertyQuery = await _productPropertyRepository.GetQueryableAsync();
            var getProductProperty = productPropertyQuery.FirstOrDefault(x => x.ProductId == productPropertyDto.ProductId);
            if (getProductProperty != null)
            {
                throw new UserFriendlyException(OrderConstant.DuplicatePriority, OrderConstant.DuplicatePriorityId);
            }
            if (productPropertyDto.ProductId <= 0)
            {
                throw new UserFriendlyException(OrderConstant.InCorrectPriorityNumber, OrderConstant.InCorrectPriorityNumberId);
            }
            var mapProductPropertyDto = ObjectMapper.Map<ProductPropertyDto, ProductPropertyWrite>(productPropertyDto);
            var entity = await _productPropertyWriteRepository.InsertAsync(mapProductPropertyDto, autoSave: true);
            return ObjectMapper.Map<ProductProperty, ProductPropertyDto>(entity);
        }

        [SecuredOperation(ProductPropertyServicePermissionConstants.Update)]
        public async Task<ProductPropertyDto> Update(ProductPropertyDto productPropertyDto)
        {

            var existingEntity = await _productPropertyWriteRepository.FindAsync(x => x.ProductId == productPropertyDto.ProductId);
            if (existingEntity == null)
            {
                throw new UserFriendlyException(OrderConstant.ProductLevelNotFound, OrderConstant.ProductLevelNotFoundId);
            }

            var duplicateProductProperty = await _productPropertyWriteRepository.FirstOrDefaultAsync(x => x.ProductId != existingEntity.ProductId && x.ProductId == productPropertyDto.ProductId);
            if (duplicateProductProperty != null)
            {
                throw new UserFriendlyException(OrderConstant.DuplicatePriority, OrderConstant.DuplicatePriorityId);
            }

            if (productPropertyDto.ProductId <= 0)
            {
                throw new UserFriendlyException(OrderConstant.InCorrectPriorityNumber, OrderConstant.InCorrectPriorityNumberId);
            }

            existingEntity.PropertyCategories = ObjectMapper.Map<List<ProductPropertyCategoryDto>, List<PropertyCategoryWrite>>(productPropertyDto.PropertyCategories)
                .Select(pc => new PropertyCategory
                {
                    Title = pc.Title,
                    Properties = pc.Properties,
                    Priority = pc.Priority,
                    Display = pc.Display
                })
                .ToList();


            await _productPropertyWriteRepository.UpdateAsync(existingEntity, autoSave: true);

            return ObjectMapper.Map<ProductProperty, ProductPropertyDto>(existingEntity);

            
        }

        [SecuredOperation(ProductPropertyServicePermissionConstants.Delete)]
        public async Task<bool> Delete(string Id)
        {
            ObjectId objectId;
            if (ObjectId.TryParse(Id, out objectId))
            {
                await _productPropertyWriteRepository.DeleteAsync(x => x.Id == objectId, autoSave: true);
                return true;
            }
            else
            {
                throw new UserFriendlyException(OrderConstant.NotValid, OrderConstant.NotValidId);

            }
        }

        public async Task SeedPeroperty(SaleTypeEnum type)
        {
            var propertydto = new PropertyCategoryDto();

            if (type == SaleTypeEnum.esalecar)
            {
                propertydto = new PropertyCategoryDto()
                {
                    Title = "مشخصات اصلی",
                    Display = false,
                    Priority = 0,
                    Properties = new List<PropertyDto>()
            {
                new PropertyDto()
                {
                    Id = ObjectId.GenerateNewId(),
                    Title = "مورد علاقه",
                    Key = "isfavorite",
                    Type = PropertyTypeEnum.Boolean,
                    Value = "",
                    Priority=1
                },
                new PropertyDto()
                {
                    Id = ObjectId.GenerateNewId(),
                    Title = "کلاس خودرو",
                    Key = "carclass",
                    Type = PropertyTypeEnum.Coding,
                    CodingType = CodingTypeEnum.CarClass,
                    Value = "",
                    Priority=2
                },
                 new PropertyDto()
                {
                    Id = ObjectId.GenerateNewId(),
                    Title = "استاندارد 85 گانه",
                    Key = "standard85",
                    Type = PropertyTypeEnum.Number,
                    Value = "",
                    Priority=0
                },
            }
                };
                await _propertyDefinitionWriteRepository.InsertAsync(ObjectMapper.Map<PropertyCategoryDto, PropertyCategoryWrite>(propertydto));
                propertydto = new PropertyCategoryDto()
                {
                    Title = "مشخصات فنی",
                    Display = true,
                    Priority = 1,
                    Properties = new List<PropertyDto>()
            {
                new PropertyDto()
                {
                    Id = ObjectId.GenerateNewId(),
                    Title = "محور محرک",
                    Key = "P001",
                    Type = PropertyTypeEnum.Text,
                    Value = "",
                    Priority=0
                },
                new PropertyDto()
                {
                    Id = ObjectId.GenerateNewId(),
                    Title = "گیربکس",
                    Key = "P002",
                    Type = PropertyTypeEnum.Text,
                    Value = "",
                    Priority=1,
                },
                 new PropertyDto()
                {
                    Id = ObjectId.GenerateNewId(),
                    Title = "حجم موتور",
                    Key = "P003",
                    Type = PropertyTypeEnum.Text,
                    Value = "",
                    Priority=2,
                },
                 new PropertyDto()
                {

                    Id = ObjectId.GenerateNewId(),
                    Title = "تنفس موتور",
                    Key = "P004",
                    Type = PropertyTypeEnum.Text,
                    Value = "",
                    Priority=3,
                },

                 new PropertyDto()
                {

                    Id = ObjectId.GenerateNewId(),
                    Title = "پیشرانه",
                    Key = "P005",
                    Type = PropertyTypeEnum.Text,
                    Value = "",
                    Priority=0
                },
            }
                };
                await _propertyDefinitionWriteRepository.InsertAsync(ObjectMapper.Map<PropertyCategoryDto, PropertyCategoryWrite>(propertydto));
                propertydto = new PropertyCategoryDto()
                {
                    Title = "عملکرد خودرو",
                    Display = true,
                    Priority = 2,
                    Properties = new List<PropertyDto>()
            {
                new PropertyDto()
                {
                    Id = ObjectId.GenerateNewId(),
                    Title = "توان موتور",
                    Key = "P006",
                    Type = PropertyTypeEnum.Text,
                    Value = "",
                    Priority=1
                },
                new PropertyDto()
                {
                    Id = ObjectId.GenerateNewId(),
                    Title = "سرعت",
                    Key = "P007",
                    Type = PropertyTypeEnum.Text,
                    Value = "",
                    Priority=2
                },
                new PropertyDto()
                {
                    Id = ObjectId.GenerateNewId(),
                    Title = "شتاب",
                    Key = "P008",
                    Type = PropertyTypeEnum.Text,
                    Value = "",
                    Priority=3
                },
                new PropertyDto()
                {
                    Id = ObjectId.GenerateNewId(),
                    Title = "مصرف سوخت",
                    Key = "P009",
                    Type = PropertyTypeEnum.Text,
                    Value = "",
                    Priority=0
                },
                new PropertyDto()
                {
                    Id = ObjectId.GenerateNewId(),
                    Title = "گشتاور",
                    Key = "P010",
                    Type = PropertyTypeEnum.Text,
                    Value = "",
                    Priority=0
                },
            }
                };
                await _propertyDefinitionWriteRepository.InsertAsync(ObjectMapper.Map<PropertyCategoryDto, PropertyCategoryWrite>(propertydto));
                propertydto = new PropertyCategoryDto()
                {
                    Title = "بدنه و شاسی",
                    Display = true,
                    Priority = 0,
                    Properties = new List<PropertyDto>()
            {
                new PropertyDto()
                {
                    Id = ObjectId.GenerateNewId(),
                    Title = "(طول(میلیمتر",
                    Key = "P011",
                    Type = PropertyTypeEnum.Number,
                    Value = "",
                    Priority=1
                },
                new PropertyDto()
                {
                    Id = ObjectId.GenerateNewId(),
                    Title = "(عرض(میلیمتر",
                    Key = "P012",
                    Type = PropertyTypeEnum.Number,
                    Value = "",
                    Priority=2
                },
                new PropertyDto()
                {
                    Id = ObjectId.GenerateNewId(),
                    Title = "(ارتفاع (میلیمتر",
                    Key = "P013",
                    Type = PropertyTypeEnum.Number,
                    Value = "",
                    Priority=3
                },
                new PropertyDto()
                {
                    Id = ObjectId.GenerateNewId(),
                    Title = "حجم باک",
                    Key = "P014",
                    Type = PropertyTypeEnum.Text,
                    Value = "",
                    Priority=0
                },
                new PropertyDto()
                {
                    Id = ObjectId.GenerateNewId(),
                    Title = "نوع شاسی ",
                    Key = "P015",
                    Type = PropertyTypeEnum.Text,
                    Value = "",
                    Priority=0
                },
                 new PropertyDto()
                {
                    Id = ObjectId.GenerateNewId(),
                    Title = "سیستم تعلیق جلو",
                    Key = "P016",
                    Type = PropertyTypeEnum.Text,
                    Value = "",
                    Priority=0
                },
                 new PropertyDto()
                 {
                   Id = ObjectId.GenerateNewId(),
                    Title = "سیستم تعلیق عقب",
                    Key = "P017",
                    Type = PropertyTypeEnum.Text,
                    Value = "",
                    Priority=0

                 },
                 new PropertyDto()
                 {
                   Id = ObjectId.GenerateNewId(),
                    Title = "فرمان",
                    Key = "P018",
                    Type = PropertyTypeEnum.Text,
                    Value = "",
                    Priority=0

                 }

            },

                };
                await _propertyDefinitionWriteRepository.InsertAsync(ObjectMapper.Map<PropertyCategoryDto, PropertyCategoryWrite>(propertydto));
                propertydto = new PropertyCategoryDto()
                {
                    Title = "ایمنی و امنیت",
                    Display = true,
                    Priority = 0,
                    Properties = new List<PropertyDto>()

            {
                new PropertyDto()
                {
                    Id = ObjectId.GenerateNewId(),
                    Title = "سیستم کروز کنترل",
                    Key = "P019",
                    Type = PropertyTypeEnum.Text,
                    Value = "",
                    Priority=1
                },
                new PropertyDto()
                {
                    Id = ObjectId.GenerateNewId(),
                    Title = "ایربگ راننده",
                    Key = "P020",
                    Type = PropertyTypeEnum.Boolean,
                    Value = "",
                    Priority=2
                },
                new PropertyDto()
                {
                    Id = ObjectId.GenerateNewId(),
                    Title = "ایربگ سرنشین جلو",
                    Key = "P021",
                    Type = PropertyTypeEnum.Boolean,
                    Value = "",
                    Priority=3
                },
                new PropertyDto()
                {
                    Id = ObjectId.GenerateNewId(),
                    Title = "ترمز  ABS",
                    Key = "P022",
                    Type = PropertyTypeEnum.Boolean,
                    Value = "",
                    Priority=0
                },
                new PropertyDto()
                {
                    Id = ObjectId.GenerateNewId(),
                    Title = "ترمز  EBD",
                    Key = "P023",
                    Type = PropertyTypeEnum.Boolean,
                    Value = "",
                    Priority=0
                },
                 new PropertyDto()
                {
                    Id = ObjectId.GenerateNewId(),
                    Title = "کنترل پایداری ESP",
                    Key = "P024",
                    Type = PropertyTypeEnum.Boolean,
                    Value = "",
                    Priority=0
                },
                 new PropertyDto()
                 {
                   Id = ObjectId.GenerateNewId(),
                    Title = "ترمز جلو",
                    Key = "P025",
                    Type = PropertyTypeEnum.Text,
                    Value = "",
                    Priority=0

                 },
                 new PropertyDto()
                 {
                   Id = ObjectId.GenerateNewId(),
                    Title = "ترمز عقب",
                    Key = "P026",
                    Type = PropertyTypeEnum.Text,
                    Value = "",
                    Priority=0

                 }

            },

                };
                await _propertyDefinitionWriteRepository.InsertAsync(ObjectMapper.Map<PropertyCategoryDto, PropertyCategoryWrite>(propertydto));
                propertydto = new PropertyCategoryDto()
                {
                    Title = "تجهیزات و امکانات",
                    Display = true,
                    Priority = 3,
                    Properties = new List<PropertyDto>()
            {
                new PropertyDto()
                {
                    Id = ObjectId.GenerateNewId(),
                    Title = "استارت",
                    Key = "P027",
                    Type = PropertyTypeEnum.Text,
                    Value = "",
                    Priority=1
                },
                new PropertyDto()
                {
                    Id = ObjectId.GenerateNewId(),
                    Title = "صفحه نمایش مرکزی",
                    Key = "P028",
                    Type = PropertyTypeEnum.Text,
                    Value = "",
                    Priority=2
                },
                new PropertyDto()
                {
                    Id = ObjectId.GenerateNewId(),
                    Title = "دوربین عقب",
                    Key = "P029",
                    Type = PropertyTypeEnum.Boolean,
                    Value = "",
                    Priority=3
                },
                new PropertyDto()
                {
                    Id = ObjectId.GenerateNewId(),
                    Title = "مه شکن عقب",
                    Key = "P030",
                    Type = PropertyTypeEnum.Boolean,
                    Value = "",
                    Priority=0
                },
                new PropertyDto()
                {
                    Id = ObjectId.GenerateNewId(),
                    Title = "سنسور پارک جلو",
                    Key = "P031",
                    Type = PropertyTypeEnum.Boolean,
                    Value = "",
                    Priority=0
                },
                 new PropertyDto()
                {
                    Id = ObjectId.GenerateNewId(),
                    Title = "سنسور پارک عقب",
                    Key = "P032",
                    Type = PropertyTypeEnum.Boolean,
                    Value = "",
                    Priority=0
                },
                 new PropertyDto()
                {
                   Id = ObjectId.GenerateNewId(),
                    Title = "سنسور باران",
                    Key = "P033",
                    Type = PropertyTypeEnum.Text,
                    Value = "",
                    Priority=0

                },
                 new PropertyDto()
                {
                   Id = ObjectId.GenerateNewId(),
                    Title = "تعداد بلندگو ",
                    Key = "P034",
                    Type = PropertyTypeEnum.Number,
                    Value = "",
                    Priority=0

                },
                 new PropertyDto()
                {
                   Id = ObjectId.GenerateNewId(),
                    Title = "صندلی راننده",
                    Key = "P035",
                    Type = PropertyTypeEnum.Text,
                    Value = "",
                    Priority=0

                },
                 new PropertyDto()
                {
                   Id = ObjectId.GenerateNewId(),
                    Title = "تعداد صندلی ",
                    Key = "P036",
                    Type = PropertyTypeEnum.Number,
                    Value = "",
                    Priority=0

                },
                 new PropertyDto()
                {
                   Id = ObjectId.GenerateNewId(),
                    Title = "تهویه خودکار",
                    Key = "P037",
                    Type = PropertyTypeEnum.Boolean,
                    Value = "",
                    Priority=0

                },
                  new PropertyDto()
                {
                   Id = ObjectId.GenerateNewId(),
                    Title = "GPS",
                    Key = "P038",
                    Type = PropertyTypeEnum.Boolean,
                    Value = "",
                    Priority=0

                },
                   new PropertyDto()
                {
                   Id = ObjectId.GenerateNewId(),
                    Title = "بلوتوث",
                    Key = "P039",
                    Type = PropertyTypeEnum.Boolean,
                    Value = "",
                    Priority=0

                },
                    new PropertyDto()
                {
                   Id = ObjectId.GenerateNewId(),
                    Title = "USB",
                    Key = "P040",
                    Type = PropertyTypeEnum.Boolean,
                    Value = "",
                    Priority=0

                }

            },

                };
                await _propertyDefinitionWriteRepository.InsertAsync(ObjectMapper.Map<PropertyCategoryDto, PropertyCategoryWrite>(propertydto));
                propertydto = new PropertyCategoryDto()
                {
                    Title = "سایر ویژگی",
                    Display = true,
                    Priority = 0,
                    Properties = new List<PropertyDto>()
            {
                new PropertyDto()
                {
                    Id = ObjectId.GenerateNewId(),
                    Title = "ویژگی",
                    Key = "P041",
                    Type = PropertyTypeEnum.Text,
                    Value = "",
                    Priority=0
                },


            },

                };
                await _propertyDefinitionWriteRepository.InsertAsync(ObjectMapper.Map<PropertyCategoryDto, PropertyCategoryWrite>(propertydto));
            }

            else if (type == SaleTypeEnum.saleauto)
            {
                propertydto = new PropertyCategoryDto()
                {
                    Title = "مشخصات اصلی",
                    Display = false,
                    Priority = 0,
                    Properties = new List<PropertyDto>()
            {
                new PropertyDto()
                {
                    Id = ObjectId.GenerateNewId(),
                    Title = "مورد علاقه",
                    Key = "isfavorite",
                    Type = PropertyTypeEnum.Boolean,
                    Value = "",
                    Priority=1
                },
                new PropertyDto()
                {
                    Id = ObjectId.GenerateNewId(),
                    Title = "کلاس خودرو",
                    Key = "carclass",
                    Type = PropertyTypeEnum.Coding,
                    CodingType = CodingTypeEnum.CarClass,
                    Value = "",
                    Priority=2
                },
            }
                };
                await _propertyDefinitionWriteRepository.InsertAsync(ObjectMapper.Map<PropertyCategoryDto, PropertyCategoryWrite>(propertydto));
                propertydto = new PropertyCategoryDto()
                {
                    Title = "مشخصات فنی",
                    Display = true,
                    Priority = 1,
                    Properties = new List<PropertyDto>()
            {
                new PropertyDto()
                {
                    Id = ObjectId.GenerateNewId(),
                    Title = "محور محرک",
                    Key = "P001",
                    Type = PropertyTypeEnum.Text,
                    Value = "",
                    Priority=0
                },
                new PropertyDto()
                {
                    Id = ObjectId.GenerateNewId(),
                    Title = "گیربکس",
                    Key = "P002",
                    Type = PropertyTypeEnum.Text,
                    Value = "",
                    Priority=1,
                },
                 new PropertyDto()
                {
                    Id = ObjectId.GenerateNewId(),
                    Title = "حجم موتور",
                    Key = "P003",
                    Type = PropertyTypeEnum.Text,
                    Value = "",
                    Priority=2,
                },
                 new PropertyDto()
                {

                    Id = ObjectId.GenerateNewId(),
                    Title = "تنفس موتور",
                    Key = "P004",
                    Type = PropertyTypeEnum.Text,
                    Value = "",
                    Priority=3,
                },

                 new PropertyDto()
                {

                    Id = ObjectId.GenerateNewId(),
                    Title = "پیشرانه",
                    Key = "P005",
                    Type = PropertyTypeEnum.Text,
                    Value = "",
                    Priority=0
                },
            }
                };
                await _propertyDefinitionWriteRepository.InsertAsync(ObjectMapper.Map<PropertyCategoryDto, PropertyCategoryWrite>(propertydto));
                propertydto = new PropertyCategoryDto()
                {
                    Title = "عملکرد خودرو",
                    Display = true,
                    Priority = 2,
                    Properties = new List<PropertyDto>()
            {
                new PropertyDto()
                {
                    Id = ObjectId.GenerateNewId(),
                    Title = "توان موتور",
                    Key = "P006",
                    Type = PropertyTypeEnum.Text,
                    Value = "",
                    Priority=1
                },
                new PropertyDto()
                {
                    Id = ObjectId.GenerateNewId(),
                    Title = "سرعت",
                    Key = "P007",
                    Type = PropertyTypeEnum.Text,
                    Value = "",
                    Priority=2
                },
                new PropertyDto()
                {
                    Id = ObjectId.GenerateNewId(),
                    Title = "شتاب",
                    Key = "P008",
                    Type = PropertyTypeEnum.Text,
                    Value = "",
                    Priority=3
                },
                new PropertyDto()
                {
                    Id = ObjectId.GenerateNewId(),
                    Title = "مصرف سوخت",
                    Key = "P009",
                    Type = PropertyTypeEnum.Text,
                    Value = "",
                    Priority=0
                },
                new PropertyDto()
                {
                    Id = ObjectId.GenerateNewId(),
                    Title = "گشتاور",
                    Key = "P010",
                    Type = PropertyTypeEnum.Text,
                    Value = "",
                    Priority=0
                },
            }
                };
                await _propertyDefinitionWriteRepository.InsertAsync(ObjectMapper.Map<PropertyCategoryDto, PropertyCategoryWrite>(propertydto));
                propertydto = new PropertyCategoryDto()
                {
                    Title = "بدنه و شاسی",
                    Display = true,
                    Priority = 0,
                    Properties = new List<PropertyDto>()
            {
                new PropertyDto()
                {
                    Id = ObjectId.GenerateNewId(),
                    Title = "(طول(میلیمتر",
                    Key = "P011",
                    Type = PropertyTypeEnum.Number,
                    Value = "",
                    Priority=1
                },
                new PropertyDto()
                {
                    Id = ObjectId.GenerateNewId(),
                    Title = "(عرض(میلیمتر",
                    Key = "P012",
                    Type = PropertyTypeEnum.Number,
                    Value = "",
                    Priority=2
                },
                new PropertyDto()
                {
                    Id = ObjectId.GenerateNewId(),
                    Title = "(ارتفاع (میلیمتر",
                    Key = "P013",
                    Type = PropertyTypeEnum.Number,
                    Value = "",
                    Priority=3
                },
                new PropertyDto()
                {
                    Id = ObjectId.GenerateNewId(),
                    Title = "حجم باک",
                    Key = "P014",
                    Type = PropertyTypeEnum.Text,
                    Value = "",
                    Priority=0
                },
                new PropertyDto()
                {
                    Id = ObjectId.GenerateNewId(),
                    Title = "نوع شاسی ",
                    Key = "P015",
                    Type = PropertyTypeEnum.Text,
                    Value = "",
                    Priority=0
                },
                 new PropertyDto()
                {
                    Id = ObjectId.GenerateNewId(),
                    Title = "سیستم تعلیق جلو",
                    Key = "P016",
                    Type = PropertyTypeEnum.Text,
                    Value = "",
                    Priority=0
                },
                 new PropertyDto()
                 {
                   Id = ObjectId.GenerateNewId(),
                    Title = "سیستم تعلیق عقب",
                    Key = "P017",
                    Type = PropertyTypeEnum.Text,
                    Value = "",
                    Priority=0

                 },
                 new PropertyDto()
                 {
                   Id = ObjectId.GenerateNewId(),
                    Title = "فرمان",
                    Key = "P018",
                    Type = PropertyTypeEnum.Text,
                    Value = "",
                    Priority=0

                 }

            },

                };
                await _propertyDefinitionWriteRepository.InsertAsync(ObjectMapper.Map<PropertyCategoryDto, PropertyCategoryWrite>(propertydto));
                propertydto = new PropertyCategoryDto()
                {
                    Title = "ایمنی و امنیت",
                    Display = true,
                    Priority = 0,
                    Properties = new List<PropertyDto>()

            {
                new PropertyDto()
                {
                    Id = ObjectId.GenerateNewId(),
                    Title = "سیستم کروز کنترل",
                    Key = "P019",
                    Type = PropertyTypeEnum.Text,
                    Value = "",
                    Priority=1
                },
                new PropertyDto()
                {
                    Id = ObjectId.GenerateNewId(),
                    Title = "ایربگ راننده",
                    Key = "P020",
                    Type = PropertyTypeEnum.Boolean,
                    Value = "",
                    Priority=2
                },
                new PropertyDto()
                {
                    Id = ObjectId.GenerateNewId(),
                    Title = "ایربگ سرنشین جلو",
                    Key = "P021",
                    Type = PropertyTypeEnum.Boolean,
                    Value = "",
                    Priority=3
                },
                new PropertyDto()
                {
                    Id = ObjectId.GenerateNewId(),
                    Title = "ترمز  ABS",
                    Key = "P022",
                    Type = PropertyTypeEnum.Boolean,
                    Value = "",
                    Priority=0
                },
                new PropertyDto()
                {
                    Id = ObjectId.GenerateNewId(),
                    Title = "ترمز  EBD",
                    Key = "P023",
                    Type = PropertyTypeEnum.Boolean,
                    Value = "",
                    Priority=0
                },
                 new PropertyDto()
                {
                    Id = ObjectId.GenerateNewId(),
                    Title = "کنترل پایداری ESP",
                    Key = "P024",
                    Type = PropertyTypeEnum.Boolean,
                    Value = "",
                    Priority=0
                },
                 new PropertyDto()
                 {
                   Id = ObjectId.GenerateNewId(),
                    Title = "ترمز جلو",
                    Key = "P025",
                    Type = PropertyTypeEnum.Text,
                    Value = "",
                    Priority=0

                 },
                 new PropertyDto()
                 {
                   Id = ObjectId.GenerateNewId(),
                    Title = "ترمز عقب",
                    Key = "P026",
                    Type = PropertyTypeEnum.Text,
                    Value = "",
                    Priority=0

                 }

            },

                };
                await _propertyDefinitionWriteRepository.InsertAsync(ObjectMapper.Map<PropertyCategoryDto, PropertyCategoryWrite>(propertydto));
                propertydto = new PropertyCategoryDto()
                {
                    Title = "تجهیزات و امکانات",
                    Display = true,
                    Priority = 3,
                    Properties = new List<PropertyDto>()
            {
                new PropertyDto()
                {
                    Id = ObjectId.GenerateNewId(),
                    Title = "استارت",
                    Key = "P027",
                    Type = PropertyTypeEnum.Text,
                    Value = "",
                    Priority=1
                },
                new PropertyDto()
                {
                    Id = ObjectId.GenerateNewId(),
                    Title = "صفحه نمایش مرکزی",
                    Key = "P028",
                    Type = PropertyTypeEnum.Text,
                    Value = "",
                    Priority=2
                },
                new PropertyDto()
                {
                    Id = ObjectId.GenerateNewId(),
                    Title = "دوربین عقب",
                    Key = "P029",
                    Type = PropertyTypeEnum.Boolean,
                    Value = "",
                    Priority=3
                },
                new PropertyDto()
                {
                    Id = ObjectId.GenerateNewId(),
                    Title = "مه شکن عقب",
                    Key = "P030",
                    Type = PropertyTypeEnum.Boolean,
                    Value = "",
                    Priority=0
                },
                new PropertyDto()
                {
                    Id = ObjectId.GenerateNewId(),
                    Title = "سنسور پارک جلو",
                    Key = "P031",
                    Type = PropertyTypeEnum.Boolean,
                    Value = "",
                    Priority=0
                },
                 new PropertyDto()
                {
                    Id = ObjectId.GenerateNewId(),
                    Title = "سنسور پارک عقب",
                    Key = "P032",
                    Type = PropertyTypeEnum.Boolean,
                    Value = "",
                    Priority=0
                },
                 new PropertyDto()
                {
                   Id = ObjectId.GenerateNewId(),
                    Title = "سنسور باران",
                    Key = "P033",
                    Type = PropertyTypeEnum.Text,
                    Value = "",
                    Priority=0

                },
                 new PropertyDto()
                {
                   Id = ObjectId.GenerateNewId(),
                    Title = "تعداد بلندگو ",
                    Key = "P034",
                    Type = PropertyTypeEnum.Number,
                    Value = "",
                    Priority=0

                },
                 new PropertyDto()
                {
                   Id = ObjectId.GenerateNewId(),
                    Title = "صندلی راننده",
                    Key = "P035",
                    Type = PropertyTypeEnum.Text,
                    Value = "",
                    Priority=0

                },
                 new PropertyDto()
                {
                   Id = ObjectId.GenerateNewId(),
                    Title = "تعداد صندلی ",
                    Key = "P036",
                    Type = PropertyTypeEnum.Number,
                    Value = "",
                    Priority=0

                },
                 new PropertyDto()
                {
                   Id = ObjectId.GenerateNewId(),
                    Title = "تهویه خودکار",
                    Key = "P037",
                    Type = PropertyTypeEnum.Boolean,
                    Value = "",
                    Priority=0

                },
                  new PropertyDto()
                {
                   Id = ObjectId.GenerateNewId(),
                    Title = "GPS",
                    Key = "P038",
                    Type = PropertyTypeEnum.Boolean,
                    Value = "",
                    Priority=0

                },
                   new PropertyDto()
                {
                   Id = ObjectId.GenerateNewId(),
                    Title = "بلوتوث",
                    Key = "P039",
                    Type = PropertyTypeEnum.Boolean,
                    Value = "",
                    Priority=0

                },
                    new PropertyDto()
                {
                   Id = ObjectId.GenerateNewId(),
                    Title = "USB",
                    Key = "P040",
                    Type = PropertyTypeEnum.Boolean,
                    Value = "",
                    Priority=0

                }

            },

                };
                await _propertyDefinitionWriteRepository.InsertAsync(ObjectMapper.Map<PropertyCategoryDto,PropertyCategoryWrite >(propertydto));
                propertydto = new PropertyCategoryDto()
                {
                    Title = "سایر ویژگی",
                    Display = true,
                    Priority = 0,
                    Properties = new List<PropertyDto>()
            {
                new PropertyDto()
                {
                    Id = ObjectId.GenerateNewId(),
                    Title = "ویژگی",
                    Key = "P041",
                    Type = PropertyTypeEnum.Text,
                    Value = "",
                    Priority=0
                },
                

            },

                };
                await _propertyDefinitionWriteRepository.InsertAsync(ObjectMapper.Map<PropertyCategoryDto, PropertyCategoryWrite>(propertydto));
            }

        }

        public async Task Import(IFormFile file, SaleTypeEnum type)
        {
            if (type == SaleTypeEnum.esalecar)
            {
                using (var stream = new MemoryStream())
                {
                    await file.CopyToAsync(stream);
                    using (var package = new ExcelPackage(stream))
                    {
                        foreach (var item in package.Workbook.Worksheets)
                        {
                            ExcelWorksheet worksheet = item;
                            var rowcount = worksheet.Dimension.Rows;
                            var colcount = worksheet.Dimension.Columns;
                            var productQuery = await _productAndCategoryRepository.GetQueryableAsync();
                            var product = productQuery.FirstOrDefault(x => x.Code == item.Name);
                            if (product is null)
                            {
                                throw new UserFriendlyException("محصول وجود ندارد");
                            };
                            var propertyCategories = (await _propertyDefinitionRepository.GetMongoQueryableAsync()).ToList();
                            List<PropertyDto> propertyList = new List<PropertyDto>();
                            for (int row = 2; row <= rowcount; row++)
                            {

                                var key = item.Cells[row, 1].Value.ToString();
                                var title = item.Cells[row, 2].Value.ToString();
                                var value = item.Cells[row, 3].Value.ToString();
                                foreach (var category in propertyCategories)
                                {
                                    foreach (var property in category.Properties)
                                    {
                                        if (property.Key == key)
                                            property.Value = value;
                                    }
                                }
                            }
                            var productPropertyDto = new ProductPropertyDto()
                            {
                                ProductId = product.Id,
                                PropertyCategories = ObjectMapper.Map<List<PropertyCategory>, List<ProductPropertyCategoryDto>>(propertyCategories)
                            };
                            await _productPropertyWriteRepository.InsertAsync(ObjectMapper.Map<ProductPropertyDto, ProductPropertyWrite>(productPropertyDto));
                        }
                    }
                }
            }
            else if (type == SaleTypeEnum.saleauto)
            {
                using (var stream = new MemoryStream())
                {
                    await file.CopyToAsync(stream);
                    using (var package = new ExcelPackage(stream))
                    {
                        foreach (var item in package.Workbook.Worksheets)
                        {
                            ExcelWorksheet worksheet = item;
                            var rowcount = worksheet.Dimension.Rows;
                            var colcount = worksheet.Dimension.Columns;
                            var productQuery = await _productAndCategoryRepository.GetQueryableAsync();
                            var product = productQuery.FirstOrDefault(x => x.Code == item.Name);
                            if (product is null)
                            {
                                throw new UserFriendlyException("محصول وجود ندارد");
                            };
                            var propertyCategories = (await _propertyDefinitionRepository.GetMongoQueryableAsync()).ToList();
                            List<PropertyDto> propertyList = new List<PropertyDto>();
                            for (int row = 2; row <= rowcount; row++)
                            {

                                var key = item.Cells[row, 1].Value.ToString();
                                var title = item.Cells[row, 2].Value.ToString();
                                var value = item.Cells[row, 3].Value.ToString();
                                foreach (var category in propertyCategories)
                                {
                                    foreach (var property in category.Properties)
                                    {
                                        if (property.Key == key)
                                            property.Value = value;
                                    }
                                }
                            }
                            var productPropertyDto = new ProductPropertyDto()
                            {
                                ProductId = product.Id,
                                PropertyCategories = ObjectMapper.Map<List<PropertyCategory>, List<ProductPropertyCategoryDto>>(propertyCategories)
                            };
                            await _productPropertyWriteRepository.InsertAsync(ObjectMapper.Map<ProductPropertyDto, ProductPropertyWrite>(productPropertyDto));
                        }
                    }
                }
            }
        }

    }
}
