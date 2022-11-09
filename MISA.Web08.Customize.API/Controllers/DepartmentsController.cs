using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.Web08.Customize.Common.Entities;
using MISA.Web08.Customize.Common.Enums;
using MISA.Web08.Customize.API.Result;
using MySqlConnector;
using MISA.Web08.Customize.Common.Resources;
using MISA.Web08.Customize.BL;

namespace MISA.Web08.Customize.API.Controllers
{
     #region API Get
     [Route("api/v1/[controller]")]
     [ApiController]
     public class DepartmentsController : BaseController<Department>
     {
          #region Field

          private IDepartmentBL _departmentBL;

          #endregion

          #region Constructor

          public DepartmentsController(IDepartmentBL departmentBL) : base(departmentBL)
          {
               _departmentBL = departmentBL;
          }

          #endregion
     } 
     #endregion
}
