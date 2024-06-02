using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.Contracts.OrderManagement.Constants
{
    public class ValidationConstant
    {
        public const string GetListByQuestionner = "پرسشنامه تعریف نشده";
        public const string GetListByQuestionnerId = "0005";

        public const string ItemNotFound = "مورد یافت نشد";
        public const string ItemNotFoundId = "0006";

        public const string QuestionnerNotFound = "پرسشنامه خالی است";
        public const string QuestionnerNotFoundId = "0007";

        public const string TitleNotFound = "عنوان خالی است";
        public const string TitleNotFoundId = "0008";

        public const string QuestionnerNotExist = "پرسشنامه وجود ندارد";
        public const string QuestionnerNotExistId = "00012";

        public const string QuestionNotFound = "پرسش وجود ندارد";
        public const string QuestionNotFoundId = "0009";
        public const string AnswerNotFound = "پاسخ وجود ندارد";
        public const string AnswerNotFoundId = "0010";
        public const string OperatorNotFound = "عملگر وجود ندارد";
        public const string OperatorNotFoundId = "0011";
        public const string NotEqualQuestionIdAndQuestionRelation = "شناسه پرسشنامه نباید با شناسه ارتباط سوال برابرباشند  ";
        public const string NotEqualQuestionIdAndQuestionRelationId = "0012";
        public const string EqualQuestionIdAndAnswerQuestion = "شناسه پرسشنامه باید با شناسه پرسشنامه پاسخ برابر باشد";
        public const string EqualQuestionIdAndAnswerQuestionId = "0013";
        public const string AdvertisementNotFound = "جایگاه وجود ندارد";
        public const string AdvertisementNotFoundId = "0014";
        public const string AddAdvertisementIdValue = "شناسه وارد شده معتبر نمیباشد";
        public const string AddAdvertisementIdValueid = "0015";
        public const string CodeNotFound = "کد خالی است";
        public const string CodeNotFoundId = "0016";
        public const string ProvinceNotFound = "استان خالی است";
        public const string ProvinceNotFoundId = "0018";
        public const string SalePlanDescriptionNotFound = "توضیحات برنامه فروش  خالی است";
        public const string SalePlanDescriptionNotFoundId = "0019";
        public const string SalePlanStartDateNotFound = "تاریخ و ساعت شروع برنامه فروش خالی است";
        public const string SalePlanStartDateNotFoundId = "0020";
        public const string SalePlanEndDateNotFound = "تاریخ و ساعت پایان برنامه فروش برنامه فروش خالی است";
        public const string SalePlanEndDateNotFoundId = "0021";
        public const string EsaleTypeIdNotFound = "مقدار نوع فروش معتبر نیست";
        public const string EsaleTypeIdNotFoundId = "0022";
        public const string CarFeeNotFound = "مقدار قیمت خودرو معتبرنیست";
        public const string CarFeeNotFoundId = "0023";
        public const string MinimumAmountOfProxyDepositNotFound = "مقدار حداقل مبلغ پیش پرداخت، وکالتی  معتبر نیست";
        public const string MinimumAmountOfProxyDepositNotFoundId = "0025";
        public const string SaleIdNotFound = " مقدار شناسه بخشنامه معتبر نیست";
        public const string SaleIdNotFoundId = "0026";
        public const string SaleProcessNotFound = "مقدار نوع طرح معتبر نیست";
        public const string SaleProcessNotFoundId = "0027";
        public const string AgencyTypeNotFound = "مقدار نوع نمایندگی معتبر نیست";
        public const string AgencyTypeNotFoundId = "0028";
        public const string YearNotValid = "مقدار سال معتبر نیست";
        public const string YearNotValidId = "0029";
        public const string SeasonIdNotFound = "مقدار شناسه فصل معتبر نیست";
        public const string SeasonIdNotFoundId = "0030";

    }
    public static class RuleSets
    {
        public const string Add = "Add";
        public const string AddId = "0001";
        public const string Edit = "Edit";
        public const string EditId = "0002";
        public const string Delete = "Delete";
        public const string DeleteId = "0003";
        public const string GetById = "GetById";
        public const string GetByIdId = "0004";
        public const string GetListByQuestionId = "GetListByQuestionId";
        public const string GetListByQuestionIdId = "0006";
        public const string Move = "Move";
        public const string MoveId = "0007";
        public const string UploadFile = "UploadFile";
        public const string UploadFileId = "0008";
      
    }
}
