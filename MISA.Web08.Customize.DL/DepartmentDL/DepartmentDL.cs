using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using MISA.Web08.Customize.API.Result;
using MISA.Web08.Customize.Common.Attributes;
using MISA.Web08.Customize.Common.Entities;
using MISA.Web08.Customize.Common.Enums;
using MISA.Web08.Customize.Common.Resources;
using MySqlConnector;

namespace MISA.Web08.Customize.DL
{
     /// <summary>
     /// Lớp thực thi các hàm trong Interface IEmployeeDL
     /// </summary>
     /// NXTSAN 26-09-2022
     public class DepartmentDL : BaseDL<Department>, IDepartmentDL
     {
     }
}
