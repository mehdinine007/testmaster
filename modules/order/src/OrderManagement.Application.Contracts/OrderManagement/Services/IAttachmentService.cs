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
        Task<List<AttachmentDto>> GetList(AttachmentEntityEnum entity,List<int> idList,AttachmentEntityTypeEnum? entityType = null);
        Task<bool> UploadFile(AttachFileDto attachDto);

    }
}
