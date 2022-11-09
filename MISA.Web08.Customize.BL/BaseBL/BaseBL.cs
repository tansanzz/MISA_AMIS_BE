using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MISA.Web08.Customize.API.Result;
using MISA.Web08.Customize.Common.Attributes;
using MISA.Web08.Customize.Common.Entities;
using MISA.Web08.Customize.Common.Enums;
using MISA.Web08.Customize.Common.Resources;
using MISA.Web08.Customize.DL;
using static Dapper.SqlMapper;

namespace MISA.Web08.Customize.BL
{
     public class BaseBL<T> : IBaseBL<T>
     {

          #region Field

          private IBaseDL<T> _baseDL;
          protected List<string> validateFailures;

          #endregion

          #region Constructor

          public BaseBL(IBaseDL<T> baseDL)
          {
               _baseDL = baseDL;
               validateFailures = new List<string>();
          }

          #endregion

          #region Method

          #region GET

          /// <summary>
          /// Lấy danh sách toàn bộ nhân viên
          /// </summary>
          /// <returns>Danh sách toàn bộ nhân viên</returns>
          /// NXTSAN 26-09-2022
          public ServiceResponse GetAllRecords()
          {
               // Thực hiện lấy tất cả bản ghi từ tầng DL
               var result = _baseDL.GetAllRecords();

               // Trả về tất cả bản ghi vừa lấy
               return new ServiceResponse
               {
                    Success = true,
                    Data = result
               };
          }

          /// <summary>
          /// Lấy thông tin 1 bản ghi theo ID
          /// </summary>
          /// <param name="RecordID"</param>
          /// <returns>bản ghi</returns>
          /// NXTSAN 26-09-2022
          public ServiceResponse GetRecordByID(Guid RecordID)
          {
               // Thực hiện lấy 1 bản ghi từ tầng DL
               var result = _baseDL.GetRecordByID(RecordID);

               // Trả về 1 bản ghi vừa lấy
               return new ServiceResponse
               {
                    Success = true,
                    Data = result
               };
          }

          #endregion

          #region POST

          /// <summary>
          /// API Thêm mới 1 bản ghi
          /// </summary>
          /// <param name="record">Bản ghi cần thêm mới</param>
          /// <returns>ID bản ghi vừa thêm mới</returns>
          /// NXTSAN 16-09-2022
          public async Task<ServiceResponse> InsertRecord(T record)
          {
               // Xác thực các thông tin bản ghi 
               var isValidateValid = ValidateData(null, record);

               // Nếu không hợp lệ thì trả về lỗi
               if (!isValidateValid)
               {
                    return new ServiceResponse
                    {
                         Success = false,
                         Data = new ErrorResult(
                         CustomizeErrorCode.ValidateInput,
                         Resource.DevMsg_InsertEmployeeFailed,
                         Resource.UserMsg_InsertEmployeeFailed,
                         validateFailures)
                    };
               }
               else
               {
                    // Nếu hợp lệ thì thực hiện thêm bản ghi ở tầng DL 
                    var newRecordID = await _baseDL.InsertRecord(record);

                    // Trả về ID của bản ghi vừa được thêm
                    return new ServiceResponse
                    {
                         Success = true,
                         Data = newRecordID
                    };
               }
          }

          #endregion

          #region PUT

          /// <summary>
          /// API Sửa thông tin của bản ghi
          /// </summary>
          /// <param name="recordID"></param>
          /// <param name="record"></param>
          /// <returns></returns>
          /// NXTSAN 16-09-2022
          public ServiceResponse UpdateRecord(Guid recordID, T record)
          {
               // Xác thực các thông tin bản ghi 
               var isValidateValid = ValidateData(recordID, record);

               // Nếu không hợp lệ thì trả về lỗi
               if (!isValidateValid)
               {
                    return new ServiceResponse
                    {
                         Success = false,
                         Data = new ErrorResult(
                         CustomizeErrorCode.ValidateInput,
                         Resource.DevMsg_UpdateEmployeesFailed,
                         Resource.UserMsg_UpdateEmployeeFailed,
                         validateFailures)
                    };
               }
               else
               {
                    // Nếu hợp lệ thì thực hiện thêm bản ghi ở tầng DL 
                    var updateRecordID = _baseDL.UpdateRecord(recordID, record);

                    // Trả về ID của bản ghi vừa được sửa
                    return new ServiceResponse
                    {
                         Success = true,
                         Data = updateRecordID
                    };
               }
          }

          #endregion

          #region DELETE

          /// <summary>
          /// Xoá 1 bản ghi
          /// </summary>
          /// <param name="recordID"></param>
          /// <returns></returns>
          /// NXTSAN 26-09-2022
          public ServiceResponse DeleteRecord(Guid recordID)
          {
               // Nếu hợp lệ thì thực hiện xoá bản ghi ở tầng DL 
               var deleteRecordID = _baseDL.DeleteRecord(recordID);

               // Trả về ID của bản ghi vừa được xoá
               return new ServiceResponse
               {
                    Success = true,
                    Data = deleteRecordID
               };
          }

          #endregion

          #endregion

          /// <summary>
          /// Validate dữ liệu truyền lên
          /// </summary>
          /// <param name="employee">Đối tượng nhân viên cần Validate</param>
          /// <returns>Đối tượng ServiceResponse mô tả validate thành công hay thất bại</returns>
          /// NXTSAN 26-09-2022
          public bool ValidateData(Guid? recordID, T record)
          {
               var properties = typeof(T).GetProperties();

               foreach (var property in properties)
               {
                    // Lấy tất cả thuộc tính của đối tượng
                    var propertyValue = property.GetValue(record);

                    // Lấy thuộc tính không được null hoặc rỗng
                    var isNotNullOrEmptyAttribute = (IsNotNullOrEmptyAttribute?)Attribute.GetCustomAttribute(property, typeof(IsNotNullOrEmptyAttribute));

                    // Lấy thuộc tính format
                    var formatAttribute = (FormatAttribute?)Attribute.GetCustomAttribute(property, typeof(FormatAttribute));

                    // Lấy thuộc tính có là ngày sinh không
                    var isDateOfBirthAttribute = (IsDateOfBirthAttribute?)Attribute.GetCustomAttribute(property, typeof(IsDateOfBirthAttribute));

                    // Lấy thuộc tính có là email không
                    var isEmailAttribute = (IsEmailAttribute?)Attribute.GetCustomAttribute(property, typeof(IsEmailAttribute));

                    // Lấy thuộc tính độ dài tối đa
                    var maxLengthAttribute = (MaxLengthAttribute?)Attribute.GetCustomAttribute(property, typeof(MaxLengthAttribute));

                    if (isNotNullOrEmptyAttribute != null && string.IsNullOrEmpty(propertyValue?.ToString()))
                    {
                         validateFailures.Add(isNotNullOrEmptyAttribute.ErrorMessage);
                    }

                    if (formatAttribute != null)
                    {
                         var regex = new Regex(formatAttribute.Format);
                         if (propertyValue != null && !regex.IsMatch(propertyValue?.ToString()))
                         {
                              validateFailures.Add(formatAttribute.ErrorMessage);
                         }
                    }

                    if (isDateOfBirthAttribute != null)
                    {
                         if (propertyValue != null && DateTime.Compare(DateTime.Parse(propertyValue?.ToString()), DateTime.Now) > 0)
                         {
                              validateFailures.Add(isDateOfBirthAttribute.ErrorMessage);
                         }

                    }

                    if (isEmailAttribute != null)
                    {
                         if (propertyValue != null && !IsValidEmail(propertyValue?.ToString()))
                         {
                              validateFailures.Add(isEmailAttribute.ErrorMessage);
                         }
                    }

                    if (maxLengthAttribute != null)
                    {
                         if (propertyValue != null && propertyValue?.ToString().Length > maxLengthAttribute.MaxLength)
                         {
                              validateFailures.Add(maxLengthAttribute.ErrorMessage);
                         }
                    }
               }

               ValidateCustom(recordID, record);

               if (validateFailures.Count > 0) return false;
               return true;
          }

          /// <summary>
          /// Validate riêng cho các đối tượng
          /// </summary>
          /// <param name="entity">Đối tượng cần validate</param>
          /// <returns>List string các lỗi validate</returns>
          /// NXTSAN 19-10-2022
          protected virtual List<string> ValidateCustom(Guid? recordID, T record)
          {
               return null;
          }

          /// <summary>
          /// Validate dữ liệu email
          /// </summary>
          /// <param name="email">Email của đối tượng nhân viên cần Validate</param>
          /// <returns>Thành công hay thất bại</returns>
          /// NXTSAN 26-09-2022
          private static bool IsValidEmail(string email)
          {
               var trimmedEmail = email.Trim();

               if (trimmedEmail.EndsWith("."))
               {
                    return false;
               }
               try
               {
                    var addr = new System.Net.Mail.MailAddress(email);
                    return addr.Address == trimmedEmail;
               }
               catch
               {
                    return false;
               }
          }
     }
}
