using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace StagwellTech.SEIU.CommonCoreEntities.BaseAPI
{
    public static class DataAnnotationHelper
    {
        public static T GetAttributeFrom<T>(this object instance, string propertyName) where T : Attribute
        {
            var attrType = typeof(T);
            var property = instance.GetType().GetProperty(propertyName);
            return (T)property.GetCustomAttributes(attrType, false).First();
        }
    }
}
