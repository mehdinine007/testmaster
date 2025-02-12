﻿using AdminPanelManagement.Application.Contracts.AdminPanelManagement.Dtos;
using CompanyManagement.Application.Contracts;
using IFG.Core.Infrastructures.TokenAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanelManagement.Application.Contracts.AdminPanelManagement.IServices
{
    public interface IUserGrpcClientService
    {
        Task<AuthenticateResponseDto> Athenticate(AuthenticateReqDto input);
    }
}
