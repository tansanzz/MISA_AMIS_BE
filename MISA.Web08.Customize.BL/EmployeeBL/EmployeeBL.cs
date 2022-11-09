using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Resources;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MISA.Web08.Customize.API.Result;
using MISA.Web08.Customize.Common.Attributes;
using MISA.Web08.Customize.Common.Entities;
using MISA.Web08.Customize.Common.Enums;
using MISA.Web08.Customize.Common.Resources;
using MISA.Web08.Customize.DL;
using static Dapper.SqlMapper;

namespace MISA.Web08.Customize.BL
{
     public class EmployeeBL : BaseBL<Employee>, IEmployeeBL
     {

          #region Field

          private IEmployeeDL _employeeDL;

          #endregion

          #region Constructor

          public EmployeeBL(IEmployeeDL employeeDL) : base(employeeDL)
          {
               _employeeDL = employeeDL;
          }

          #endregion

          #region Method

          #region GET

          /// <summary>
          /// Lấy mã nhân viên tự tăng
          /// </summary>
          /// <param name="EmployeeID"</param>
          /// <returns>Nhân viên</returns>
          /// NXTSAN 26-09-2022
          public ServiceResponse GetNewEmployeeCode()
          {
               var result = _employeeDL.GetNewEmployeeCode();
               return new ServiceResponse
               {
                    Success = true,
                    Data = result
               };
          }

          /// <summary>
          /// Lấy tất cả nhân viên thoả mãn bộ lọc ()
          /// </summary>
          /// <param name="keyword"></param>
          /// <param name="limit"></param>
          /// <param name="offset"></param>
          /// <returns></returns>
          /// NXTSAN 26-09-2022
          public ServiceResponse FilterEmployee(string? keyword, int? pageSize, int? pageNumber)
          {
               var result = _employeeDL.FilterEmployee(keyword, pageSize, pageNumber);
               return new ServiceResponse
               {
                    Success = true,
                    Data = result
               };
          }

          #endregion

          #region INSERT

          /// <summary>
          /// Nhập dữ liệu
          /// </summary>
          /// <param name="employees"></param>
          /// NXTSAN 09-11-2022
          public async Task<ServiceResponse> ImportData(List<Employee> employees)
          {
               var properties = typeof(EmployeeDTO).GetProperties();

               foreach (var employee in employees)
               {
                    var employeeDTO = new EmployeeDTO
                    {
                         EmployeeCode = employee.EmployeeCode,
                         FullName = employee.FullName,
                         DateOfBirth = employee.DateOfBirth,
                         Gender = employee.Gender,
                         DepartmentName = employee.DepartmentName,
                         IdentityNumber = employee.IdentityNumber,
                         IdentityDate = employee.IdentityDate,
                         PositionName = employee.PositionName
                    };
                    foreach (var property in properties)
                    {
                         // Lấy tất cả thuộc tính của đối tượng
                         var propertyValue = property.GetValue(employeeDTO);

                         // Lấy thuộc tính không được null hoặc rỗng
                         var isNotNullOrEmptyAttribute = (IsNotNullOrEmptyAttribute?)Attribute.GetCustomAttribute(property, typeof(IsNotNullOrEmptyAttribute));

                         // Lấy thuộc tính format
                         var formatAttribute = (FormatAttribute?)Attribute.GetCustomAttribute(property, typeof(FormatAttribute));

                         // Lấy thuộc tính có là ngày sinh không
                         var isDateOfBirthAttribute = (IsDateOfBirthAttribute?)Attribute.GetCustomAttribute(property, typeof(IsDateOfBirthAttribute));

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
                    }

                    ValidateCustom(null, employee);

                    if (validateFailures.Count > 0) return new ServiceResponse
                    {
                         Success = false,
                         Data = validateFailures
                    };
               }

               var result = await _employeeDL.ImportData(employees);
               if (result == employees.Count)
               {
                    return new ServiceResponse
                    {
                         Success = true,
                         Data = result
                    };
               }
               else
               {
                    return new ServiceResponse
                    {
                         Success = false,
                         Data = result
                    };
               }

          }

          #endregion

          #region DELETE

          /// <summary>
          /// Xoa nhieu nhan vien
          /// </summary>
          /// <param name="employeeIDs"></param>
          /// NXTSAN 26-09-2022
          public ServiceResponse BatchDeleteEmployees(List<Guid> employeeIDs)
          {
               var result = _employeeDL.BatchDeleteEmployees(employeeIDs);
               return new ServiceResponse
               {
                    Success = true,
                    Data = result
               };
          }

          #endregion

          #endregion

          protected override List<string> ValidateCustom(Guid? recordID, Employee entity)
          {
               var result = _employeeDL.GetValidEmployeeCode(recordID, entity.EmployeeCode);

               if (result != null)
               {
                    validateFailures.Add(Resource.Employee_EmployeeCode_NotDuplicate);
               }

               if (entity.DateOfBirth != null && entity.IdentityDate != null && DateTime.Compare((DateTime)entity.DateOfBirth, (DateTime)entity.IdentityDate) > 0)
               {
                    validateFailures.Add(Resource.Employee_EmployeeCode_IdentityDateLessThanDOB);
               }

               return validateFailures;
          }

     }
}
