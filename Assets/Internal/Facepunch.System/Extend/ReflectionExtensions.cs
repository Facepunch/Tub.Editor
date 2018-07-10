using System;
using System.Reflection;

namespace Facepunch.Extend
{
    public static class ReflectionExtensions
    {
        /// <summary>
        /// Returns true if this member has this attribute
        /// </summary>
        public static bool HasAttribute( this MemberInfo method, Type attribute )
        {
            var attributes = method.GetCustomAttributes( attribute, true );
            return attributes.Length > 0;
        }
    }
}
