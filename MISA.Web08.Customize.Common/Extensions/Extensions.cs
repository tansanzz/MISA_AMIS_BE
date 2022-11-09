using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MISA.Web08.Customize.Common.Enums;

namespace MISA.Web08.Customize.Common.Extensions
{
     public static class Extensions
     {
          /// <summary>
          ///     A generic extension method that aids in reflecting 
          ///     and retrieving any attribute that is applied to an `Enum`.
          /// </summary>
          public static TAttribute GetAttribute<TAttribute>(this Enum enumValue)
                  where TAttribute : Attribute
          {
               return enumValue.GetType()
                               .GetMember(enumValue.ToString())
                               .First()
                               .GetCustomAttribute<TAttribute>();
          }


          /// <summary>
          /// Chuyển giới tính sang enum
          /// </summary>
          /// NXTSAN 09-11-2022
          public static Gender? GenderParse(string? genderAsString)
          {
               if (genderAsString == null) return null;

               foreach (Gender gender in (Gender[])Enum.GetValues(typeof(Gender)))
               {
                    var displayGenderName = gender.GetAttribute<DisplayAttribute>().Name;
                    if(displayGenderName.ToLower().Equals(genderAsString.ToLower()))
                    {
                         return gender;
                    }
               }

               return null;
          }
     }
}
