using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Web08.Customize.Common.Entities
{
     public class BaseEntity
     {
          /// <summary>
          /// Ngày tạo
          /// </summary>
          /// NXTSAN 21-09-2022
          public DateTime? CreatedDate { get; set; }

          /// <summary>
          /// Tạo bởi
          /// </summary>
          /// NXTSAN 21-09-2022
          public string? CreatedBy { get; set; }

          /// <summary>
          /// Ngày sửa gần nhất
          /// </summary>
          /// NXTSAN 21-09-2022
          public DateTime? ModifiedDate { get; set; }

          /// <summary>
          /// Người sửa gần nhất
          /// </summary>
          /// NXTSAN 21-09-2022
          public string? ModifiedBy { get; set; }
     }
}
