using Esale.Core.Attributes;


namespace UserManagement.Domain.UserManagement.Enums
{
    public enum BankEnum
    {
        [EnumProperty(Description = "ملی")]
        Meli = 1,
        [EnumProperty(Description = "تجارت")]
        Tejarat = 2,
        [EnumProperty(Description = "ملت")]
        Mellat = 3,
        [EnumProperty(Description = "سامان")]
        Saman = 4
    }

}
