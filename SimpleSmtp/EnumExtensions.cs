using System;
using System.Reflection;

namespace BAMCIS.SimpleSmtp
{
    public static class EnumExtensions
    {
        public static T GetAttribute<T>(this Enum value) where T : Attribute
        {
            Type ValType = value.GetType();
            MemberInfo[] Info = ValType.GetMember(value.ToString());
            return (T)Info[0].GetCustomAttribute(typeof(T), false);
        }
    }
}
