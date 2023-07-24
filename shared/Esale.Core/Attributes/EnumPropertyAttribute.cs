using System;
using System.Collections.Generic;
using System.Text;

namespace Esale.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public sealed class EnumPropertyAttribute : Attribute
    {
        public string Description { get; set; }
    }
}
