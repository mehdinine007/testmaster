﻿using IFG.Core.Bases;

namespace Permission.WorkFlow
{
    public class ProccessServicePermissionConstants: BasePermissionConstants
    {
        public const string StartProcess = "000400020001";
        public const string StartProcess_DisplayName = "شروع پردازش";
        public const string GetOutBox = "000400020002";
        public const string GetOutBox_DisplayName = "نمایش کارتابل ارسال شده";


        public override string ModuleIdentifier => "0004";
        public override string ServiceIdentifier => "0002";
        public override string ServiceDisplayName => "سرویس کارتابل ارسال شده";
    }
}
