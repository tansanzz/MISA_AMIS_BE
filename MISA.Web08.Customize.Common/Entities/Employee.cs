using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using MISA.Web08.Customize.Common.Attributes;
using MISA.Web08.Customize.Common.Enums;
using MaxLengthAttribute = MISA.Web08.Customize.Common.Attributes.MaxLengthAttribute;

namespace MISA.Web08.Customize.Common.Entities
{
     /// <summary>
     /// Nhân viên
     /// </summary>
     /// NXTSAN 21-09-2022
     public class Employee : BaseEntity
     {
          #region Field

          /// <summary>
          /// ID nhân viên
          /// </summary>
          /// NXTSAN 21-09-2022
          [PrimaryKey]
          [DisplayName("STT")]
          public Guid EmployeeID { get; set; }

          /// <summary>
          /// Mã nhân viên
          /// </summary>
          /// NXTSAN 21-09-2022
          [IsNotNullOrEmpty("Mã nhân viên không được để trống")]
          [Format("^NV-[0-9]+$", "Mã nhân viên không hợp lệ")]
          [MaxLength(20, "Mã nhân viên vượt quá 20 ký tự")]
          [DisplayName("Mã nhân viên")]
          public string EmployeeCode { get; set; }

          /// <summary>
          /// Tên đầy đủ 
          /// </summary>
          /// NXTSAN 21-09-2022
          [IsNotNullOrEmpty("Tên nhân viên không được để trống")]
          [MaxLength(100, "Tên nhân viên vượt quá 100 ký tự")]
          [DisplayName("Tên nhân viên")]

          public string FullName { get; set; }

          /// <summary>
          /// Ngày sinh
          /// </summary>
          /// NXTSAN 21-09-2022
          [IsDateOfBirth("Ngày sinh không được vượt quá ngày hiện tại")]
          [DisplayName("Ngày sinh")]
          public DateTime? DateOfBirth { get; set; }

          /// <summary>
          /// Chức vụ 
          /// </summary>
          /// NXTSAN 21-09-2022
          [DisplayName("Chức danh")]
          public string? PositionName { get; set; }

          /// <summary>
          /// ID đơn vị
          /// </summary>
          /// NXTSAN 21-09-2022
          public Guid DepartmentID { get; set; }

          /// <summary>
          /// Tên đơn vị
          /// </summary>
          /// NXTSAN 21-09-2022
          [DisplayName("Tên đơn vị")]
          [IsNotNullOrEmpty("Đơn vị không được để trống")]
          public string DepartmentName { get; set; }

          /// <summary>
          /// Ngày cấp cmnd
          /// </summary>
          /// NXTSAN 21-09-2022
          [IsDateOfBirth("Ngày cấp chứng minh nhân dân không được vượt quá ngày hiện tại")]
          public DateTime? IdentityDate { get; set; }

          /// <summary>
          /// Nơi cấp cmnd
          /// </summary>
          /// NXTSAN 21-09-2022
          public string? IdentityPlace { get; set; }

          /// <summary>
          /// Số cmnd
          /// </summary>
          /// NXTSAN 21-09-2022
          [Format("^[0-9]+$", "Số chứng minh nhân dân không hợp lệ")]
          [MaxLength(25, "Số chứng minh nhân dân vượt quá 25 ký tự")]
          public string? IdentityNumber { get; set; }

          /// <summary>
          /// Tài khoản ngân hàng
          /// </summary>
          /// NXTSAN 21-09-2022
          //[Format("^[0-9]+$", "Số tài khoản ngân hàng không hợp lệ")]
          [MaxLength(25, "Số tài khoản vượt quá 25 ký tự")]
          [DisplayName("Số tài khoản")]
          public string? BankAccount { get; set; }

          /// <summary>
          /// Tên ngân hàng
          /// </summary>
          /// NXTSAN 21-09-2022
          [DisplayName("Tên ngân hàng")] 
          public string? BankName { get; set; }

          /// <summary>
          /// Địa điểm ngân hàng
          /// </summary>
          /// NXTSAN 21-09-2022
          public string? BankPlace { get; set; }

          /// <summary>
          /// Địa chỉ
          /// </summary>
          /// NXTSAN 21-09-2022
          public string? Address { get; set; }

          /// <summary>
          /// Số điện thoại
          /// </summary>
          /// NXTSAN 21-09-2022
          //[Format("^[0-9]+$", "Số điện thoại không hợp lệ")]
          public string? PhoneNumber { get; set; }

          /// <summary>
          /// Số điện thoại cố định
          /// </summary>
          /// NXTSAN 21-09-2022
          //[Format("^[0-9]+$", "Số điện thoại cố định không hợp lệ")]
          public string? FixedPhoneNumber { get; set; }

          /// <summary>
          /// Email
          /// </summary>
          /// NXTSAN 21-09-2022
          [IsEmail("Email không hợp lệ")]
          public string? Email { get; set; }

          /// <summary>
          /// Giới tính 
          /// </summary>
          /// NXTSAN 21-09-2022
          [DisplayName("Giới tính")]
          public Gender? Gender { get; set; }

          #endregion

          #region Constructor



          #endregion
     }
}
