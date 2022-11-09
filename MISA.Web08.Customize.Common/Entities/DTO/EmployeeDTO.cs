using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MISA.Web08.Customize.Common.Attributes;
using MISA.Web08.Customize.Common.Enums;

namespace MISA.Web08.Customize.Common.Entities
{
     /// <summary>
     /// Nhân viên
     /// </summary>
     /// NXTSAN 21-09-2022
     public class EmployeeDTO 
     {
          #region Field

          /// <summary>
          /// Mã nhân viên
          /// </summary>
          /// NXTSAN 21-09-2022
          [IsNotNullOrEmpty("Mã nhân viên không được để trống")]
          [Format("^NV-[0-9]+$", "Mã nhân viên không hợp lệ")]
          [DisplayName("Mã nhân viên")]
          public string? EmployeeCode { get; set; }

          /// <summary>
          /// Tên đầy đủ 
          /// </summary>
          /// NXTSAN 21-09-2022
          [IsNotNullOrEmpty("Tên nhân viên không được để trống")]
          [DisplayName("Tên nhân viên")]

          public string? FullName { get; set; }

          /// <summary>
          /// Ngày sinh
          /// </summary>
          /// NXTSAN 21-09-2022
          [IsDateOfBirth("Ngày sinh không được vượt quá ngày hiện tại")]
          [IsNotNullOrEmpty("Ngày sinh không được để trống")]
          [DisplayName("Ngày sinh")]
          public DateTime? DateOfBirth { get; set; }

          /// <summary>
          /// Chức vụ 
          /// </summary>
          /// NXTSAN 21-09-2022
          [DisplayName("Chức danh")]
          [IsNotNullOrEmpty("Chức danh không được để trống")]
          public string? PositionName { get; set; }

          /// <summary>
          /// Tên đơn vị
          /// </summary>
          /// NXTSAN 21-09-2022
          [DisplayName("Tên đơn vị")]
          [IsNotNullOrEmpty("Đơn vị không được để trống")]
          public string? DepartmentName { get; set; }

          /// <summary>
          /// Ngày cấp cmnd
          /// </summary>
          /// NXTSAN 21-09-2022
          [IsDateOfBirth("Ngày cấp chứng minh nhân dân không được vượt quá ngày hiện tại")]
          [IsNotNullOrEmpty("Ngày cấp chứng minh nhân dân không được để trống")]
          public DateTime? IdentityDate { get; set; }

          /// <summary>
          /// Số cmnd
          /// </summary>
          /// NXTSAN 21-09-2022
          [Format("^[0-9]+$", "Số chứng minh nhân dân không hợp lệ")]
          [IsNotNullOrEmpty("Số chứng minh nhân dân không được để trống")]
          public string? IdentityNumber { get; set; }

          /// <summary>
          /// Giới tính 
          /// </summary>
          /// NXTSAN 21-09-2022
          [DisplayName("Giới tính")]
          [IsNotNullOrEmpty("Giới tính không được để trống")]
          public Gender? Gender { get; set; }

          #endregion

          #region Constructor



          #endregion
     }
}
