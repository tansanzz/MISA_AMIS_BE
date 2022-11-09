using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MISA.Web08.Customize.Common.Attributes;

namespace MISA.Web08.Auth.Common.Entities
{
     public class Role
     {
          #region Field

          /// <summary>
          /// ID vai trò
          /// </summary>
          /// NXTSAN 27-10-2022
          [PrimaryKey]
          public Guid RoleID { get; set; }

          /// <summary>
          /// Mã vai trò
          /// </summary>
          /// NXTSAN 27-10-2022
          public string RoleCode { get; set; }

          /// <summary>
          /// Tên vai trò
          /// </summary>
          /// NXTSAN 27-10-2022
          public string RoleName { get; set; }

          #endregion
     }
}
