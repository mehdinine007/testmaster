﻿using IFG.Core.Bases;
using Licence;

namespace OrderManagement.Application.Contracts;

public class SaleSchemaServicePermissionConstants
{
    public const string GetById = ConstantInfo.ModuleOrder + ServiceIdentifier + "0001";
    public const string GetById_DisplayName = "نمایش بخشنامه توسط شناسه";
    public const string GetList = ConstantInfo.ModuleOrder + ServiceIdentifier + "0002";
    public const string GetList_DisplayName = "نمایش بخشنامه ها";
    public const string Delete = ConstantInfo.ModuleOrder + ServiceIdentifier + "0003";
    public const string Delete_DisplayName = "حذف";
    public const string Add = ConstantInfo.ModuleOrder + ServiceIdentifier + "0004";
    public const string Add_DisplayName = "ثبت";
    public const string Update = ConstantInfo.ModuleOrder + ServiceIdentifier + "0005";
    public const string Update_DisplayName = "بروزرسانی";
    public const string UploadFile = ConstantInfo.ModuleOrder + ServiceIdentifier + "0006";
    public const string UploadFile_DisplayName = "بروزرسانی فایل";

        public const string ServiceIdentifier = "0007";
    public const string ServiceDisplayName = "سرویس مدیریت بخش نامه های فروش";
}
