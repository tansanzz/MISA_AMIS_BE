using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MISA.Web08.Customize.API.Result;
using MISA.Web08.Customize.Common.Attributes;
using MISA.Web08.Customize.Common.Entities;
using MISA.Web08.Customize.Common.Enums;
using MISA.Web08.Customize.Common.Resources;
using MISA.Web08.Customize.DL;

namespace MISA.Web08.Customize.BL
{
     public class BaseValidate
     {
          /// <summary>
          /// Validate dữ liệu email
          /// </summary>
          /// <param name="email">Email của đối tượng nhân viên cần Validate</param>
          /// <returns>Thành công hay thất bại</returns>
          /// NXTSAN 26-09-2022
          private static bool IsValidEmail(string email)
          {
               var trimmedEmail = email.Trim();

               if (trimmedEmail.EndsWith("."))
               {
                    return false;
               }
               try
               {
                    var addr = new System.Net.Mail.MailAddress(email);
                    return addr.Address == trimmedEmail;
               }
               catch
               {
                    return false;
               }
          }
     }
}
