using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace MISA.Web08.Customize.Common.Attributes
{
     public class AuthorizeRolesAttribute : AuthorizeAttribute
     {
          public AuthorizeRolesAttribute(params string[] roles) : base()
          {
               Roles = string.Join(",", roles);
          }
     }
}
