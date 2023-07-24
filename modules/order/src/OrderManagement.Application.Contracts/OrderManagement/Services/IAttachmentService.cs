using Nest;
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
        Task<bool> UploadFile(AttachFileDto attachDto);

    }
}
