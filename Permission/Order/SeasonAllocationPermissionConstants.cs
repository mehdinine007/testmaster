﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Permission.Order
{
    public class SeasonAllocationPermissionConstants
    {

        public const string Add = ConstantInfo.ModuleOrder + ServiceIdentifier + "0001";
        public const string Add_DisplayName = "ثبت";
        public const string Delete = ConstantInfo.ModuleOrder + ServiceIdentifier + "0002";
        public const string Delete_DisplayName = "حذف";
        public const string GetById = ConstantInfo.ModuleOrder + ServiceIdentifier + "0003";
        public const string GetById_DisplayName = "نمایش تخصیص فصل با شناسه";
        public const string GetList = ConstantInfo.ModuleOrder + ServiceIdentifier + "0004";
        public const string GetList_DisplayName = "نمایش تخصیص فصل ";
        public const string Update = ConstantInfo.ModuleOrder + ServiceIdentifier + "0005";
        public const string Update_DisplayName = "بروزرسانی";

        public const string ServiceIdentifier = "0028";
        public const string ServiceDisplayName = "سرویس تخصیص فصل";
    }
}
