using AdminPanelManagement.Application.Contracts.AdminPanelManagement.Constants.Permissions;
using AdminPanelManagement.Application.Contracts.AdminPanelManagement.Dtos;
using AdminPanelManagement.Application.Contracts.IServices;
using AdminPanelManagement.Domain.Shared.AdminPanelManagement.Db;
using AdminPanelManagement.Domain.Shared.AdminPanelManagement.Enum;
using AdminPanelManagement.EntityFrameworkCore.AdminPanelManagement.Repositories;
using Esale.Share.Authorize;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;


namespace AdminPanelManagement.Application.AdminPanelManagement.Implementations
{
    public class ReportService : ApplicationService, IReportService
    {
        private readonly ICustomerOrderRepository _customerOrderRepository;
        private readonly IQuestionnaireRepository _questionnaireRepository;

        public ReportService(ICustomerOrderRepository customeOrderRepository, IQuestionnaireRepository questionnaireRepository)
        {
            _customerOrderRepository = customeOrderRepository;
            _questionnaireRepository = questionnaireRepository;
        }

        [SecuredOperation(ReportServicePermissionConstants.ReportQuestionnaire)]
        public async Task<List<ReportQuestionnaireDto>> ReportQuestionnaire(ReportQueryDto input)
        {
            List<ReportQuestionnaireDto> ReportQuestionnaire = new List<ReportQuestionnaireDto>();
            List<ReportQuestionnaireDb> result = new List<ReportQuestionnaireDb>();

            if (input.Type != ReportQuestionnaireTypeEnum.ReportWithNationalCode && input.Type !=  ReportQuestionnaireTypeEnum.ReportWithOutNationalCode) 
            {
                throw new UserFriendlyException("نوع گزارش معتبر نمیباشد");
            }


            if (input.Type == ReportQuestionnaireTypeEnum.ReportWithNationalCode)
            {
                if (input.NationalCode.IsNullOrEmpty())
                {
                    throw new UserFriendlyException("لطفا کد ملی را وارد کنید");
                }
                    result = await _questionnaireRepository.GetReportQuestionnaire(input.NationalCode, input.Type, -1, -1);
                    ReportQuestionnaire = result.Select(x => new ReportQuestionnaireDto
                    {
                        CustomAnswerValue = x.CustomAnswerValue,
                        Description = x.Description,
                        QuestionTitle = x.QuestionTitle,
                        FirstName = x.FirstName,
                        LastName = x.LastName,
                        MobileNumber = x.MobileNumber,
                        NationalCode = x.NationalCode,
                        QuestionnaireId = x.QuestionnaireId,
                        QuestionnaireTitle = x.QuestionnaireTitle,
                        Age = x.Age,
                        EducationLevel = x.EducationLevel,
                        Gender = x.Gender,
                        TitleGender=x.Gender ==GenderTypeEnum.Male ? "مرد":"زن",
                        ManufactureDate = x.ManufactureDate,
                        VehicleName = x.VehicleName,
                        Vin = x.Vin


                    }).ToList();
            }
            else if (input.Type == ReportQuestionnaireTypeEnum.ReportWithOutNationalCode)
            {
                if (input.SkipCount<=0)
                {
                    throw new UserFriendlyException("شماره صفحه نمیتواند کوچتر از صفر یا صفر باشد");
                }
                if (input.MaxResultCount <= 0)
                {
                    throw new UserFriendlyException("تعداد نمایش در صفحه نمیتواند کوچتر از صفر یا صفر باشد");
                }
                result = await _questionnaireRepository.GetReportQuestionnaire("", input.Type, input.MaxResultCount, input.SkipCount);
                ReportQuestionnaire = result.Select(x => new ReportQuestionnaireDto
                {
                    CustomAnswerValue = x.CustomAnswerValue,
                    Description = x.Description,
                    QuestionTitle = x.QuestionTitle,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    MobileNumber = x.MobileNumber,
                    NationalCode = x.NationalCode,
                    QuestionnaireId = x.QuestionnaireId,
                    QuestionnaireTitle = x.QuestionnaireTitle,
                    Age = x.Age,
                    EducationLevel = x.EducationLevel,
                    Gender = x.Gender,
                    TitleGender = x.Gender == GenderTypeEnum.Male ? "مرد" : "زن",
                    ManufactureDate = x.ManufactureDate,
                    VehicleName = x.VehicleName,
                    Vin = x.Vin
                }).ToList();
            }


            return ReportQuestionnaire;

        }
    }
}
