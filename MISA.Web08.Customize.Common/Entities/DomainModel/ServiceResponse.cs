using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Web08.Customize.Common.Entities
{
     /// <summary>
     /// Dữ liệu trả về từ tầng BL
     /// </summary>
     /// NXTSAN 26-09-2022
     public class ServiceResponse
     {
          #region Field

          /// <summary>
          /// Thành công hay không
          /// </summary>
          /// NXTSAN 26-09-2022
          public bool Success { get; set; }

          /// <summary>
          /// Dữ liệu đi kèm khi thành công hoặc thất bại
          /// </summary>
          /// NXTSAN 26-09-2022
          public object? Data { get; set; }

          #endregion
     }
}
