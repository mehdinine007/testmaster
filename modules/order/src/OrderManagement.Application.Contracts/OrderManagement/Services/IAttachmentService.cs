using OrderManagement.Application.Contracts.OrderManagement;
using OrderManagement.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace OrderManagement.Application.Contracts
{
    public interface IAttachmentService : IApplicationService
    {
        Task<List<AttachmentDto>> GetList(AttachmentEntityEnum entity,List<int> idList, List<AttachmentEntityTypeEnum> entityType = null,List<AttachmentLocationEnum> location = null);
        //Task<List<AttachmentDto>> GetList(AttachmentEntityEnum entity, List<int> idList, AttachmentEntityTypeEnum? entityType = null);

        Task<Guid> Update(AttachmentUpdateDto attachment);
        Task<Guid> UploadFile(AttachmentEntityEnum entity, UploadFileDto uploadFile);
        Task<bool> DeleteByEntityId(AttachmentEntityEnum entity,int id);
        Task<bool> DeleteById(Guid id);


    }
}
