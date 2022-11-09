using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace MISA.Web08.Customize.Common.Enums
{
     /// <summary>
     /// Danh sách giới tính
     /// </summary>
     /// NXTSAN 21-09-2022
     public enum Gender: int
    {
          /// <summary>
          /// Nam
          /// </summary>
          /// NXTSAN 21-09-2022
          [Display(Name = "Nam")]
          Male = 0,

          /// <summary>
          /// Nữ
          /// </summary>
          /// NXTSAN 21-09-2022
          [Display(Name = "Nữ")]
          Female = 1,

          /// <summary>
          /// Khác
          /// </summary>
          /// NXTSAN 21-09-2022
          [Display(Name = "Khác")]
          Other = 2 
    }
}
