using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MISA.Web08.Customize.API.Result;
using MISA.Web08.Customize.Common.Attributes;
using MISA.Web08.Customize.Common.Entities;
using MISA.Web08.Customize.Common.Enums;
using MISA.Web08.Customize.Common.Resources;
using MISA.Web08.Customize.DL;

namespace MISA.Web08.Customize.BL
{
     public class DepartmentBL : BaseBL<Department>, IDepartmentBL
     {

          #region Field

          private IDepartmentDL _departmentDL;

          #endregion

          #region Constructor

          public DepartmentBL(IDepartmentDL departmentDL) : base(departmentDL)
          {
               _departmentDL = departmentDL;
          }

          #endregion

          #region Method

          #endregion
     }
}
