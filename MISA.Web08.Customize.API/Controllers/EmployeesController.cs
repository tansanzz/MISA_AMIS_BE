using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.Web08.Customize.Common.Attributes;
using MISA.Web08.Customize.Common.Entities;
using MISA.Web08.Customize.Common.Enums;
using MISA.Web08.Customize.Common.Resources;
using MISA.Web08.Customize.API.Result;
using MISA.Web08.Customize.BL;
using MySqlConnector;
using MISA.Web08.Customize.DL;
using System.IO;
using System.Net.Http.Headers;
using OfficeOpenXml;
using Dapper;
using OfficeOpenXml.Style;
using System.Drawing;
using System.Reflection;
using Microsoft.OpenApi.Extensions;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using MISA.Web08.Customize.Common.Extensions;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace MISA.Web08.Customize.API.Controllers
{
     [ApiController]
     public class EmployeesController : BaseController<Employee>
     {
          #region Field

          private IEmployeeBL _employeeBL;

          #endregion

          #region Constructor

          public EmployeesController(IEmployeeBL employeeBL) : base(employeeBL)
          {
               _employeeBL = employeeBL;
          }

          #endregion

          #region API Get

          /// <summary>
          /// API Lấy mã nhân viên tự tăng
          /// </summary>
          /// <param name="EmployeeID"</param>
          /// <returns>Nhân viên</returns>
          /// Created by: NXTSAN (17/09/2022)
          [HttpGet("employee-code"), AuthorizeRoles(Role.Admin, Role.User, Role.SuperAdmin)]
          public IActionResult GetNewEmployeeCode()
          {

               try
               {
                    // Thực hiện các nghiệp vụ ở BL
                    var result = _employeeBL.GetNewEmployeeCode();

                    // Trả về kết quả thành công
                    return StatusCode(StatusCodes.Status200OK, result);
               }
               catch (Exception ex)
               {
                    Console.WriteLine(ex.Message);

                    // Trả về lỗi khi gặp vấn đề 
                    return StatusCode(StatusCodes.Status500InternalServerError, new ServiceResponse
                    {
                         Success = false,
                         Data = new ErrorResult(
                                   CustomizeErrorCode.Exception,
                                   Resource.DevMsg_Exception,
                                   Resource.UserMsg_Exception,
                                   Resource.MoreInfo_Exception,
                                   HttpContext.TraceIdentifier
                                   )
                    });
               }

          }

          /// <summary>
          /// API Lấy tất cả nhân viên thoả mãn bộ lọc ()
          /// </summary>
          /// <param name="keyword"></param>
          /// <param name="limit"></param>
          /// <param name="offset"></param>
          /// <returns></returns>
          /// Created by: NXTSAN (16/09/2022) 
          [HttpGet("filter"), AuthorizeRoles(Role.Admin, Role.User, Role.SuperAdmin)]
          public IActionResult FilterEmployee(
              [FromQuery] string? keyword,
              [FromQuery] int? pageSize,
              [FromQuery] int? pageNumber)
          {
               try
               {
                    // Thực hiện các nghiệp vụ ở BL
                    var result = _employeeBL.FilterEmployee(keyword, pageSize, pageNumber);

                    // Trả về kết quả thành công
                    return StatusCode(StatusCodes.Status200OK, result);
               }
               catch (Exception ex)
               {
                    Console.WriteLine(ex.Message);

                    // Trả về lỗi khi gặp vấn đề 
                    return StatusCode(StatusCodes.Status500InternalServerError, new ServiceResponse
                    {
                         Success = false,
                         Data = new ErrorResult(
                                   CustomizeErrorCode.Exception,
                                   Resource.DevMsg_Exception,
                                   Resource.UserMsg_Exception,
                                   Resource.MoreInfo_Exception,
                                   HttpContext.TraceIdentifier
                                   )
                    });
               }
          }

          /// <summary>
          /// API Xuất khẩu dữ liệu
          /// </summary>
          /// <returns></returns>
          /// Created by: NXTSAN (11/10/2022) 
          [HttpGet("export-data"), AuthorizeRoles(Role.Admin, Role.SuperAdmin)]
          public IActionResult ExportData([FromQuery] string? keyword)
          {
               try
               {
                    // Số bản ghi
                    const int pageSize = -1;

                    // Trang bắt đầu lấy bản ghi
                    const int pageNumber = 1;

                    // Lấy bản ghi thoả mãn keyword
                    var result = _employeeBL.FilterEmployee(keyword, pageSize, pageNumber);
                    if (result.Success)
                    {
                         var data = (PagingData)result.Data;
                         List<Employee> exportData = data.Data;
                         ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                         var property = typeof(Employee);
                         using (var excel = new ExcelPackage())
                         {
                              // Style cho title file excel
                              var workSheet = excel.Workbook.Worksheets.Add(Resource.Excel_Title);
                              workSheet.Cells["A1"].Value = Resource.Excel_Title;
                              workSheet.Cells["A1:I1"].Style.Font.Size = 16;
                              workSheet.Cells["A1:I1"].Style.Font.Bold = true;
                              workSheet.Cells["A1:I1"].Style.Font.Name = "Arial";
                              workSheet.Cells["A1:I1"].Style.Indent = 5;
                              workSheet.Cells["A1:I1"].Merge = true;
                              workSheet.Cells["A1:I1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                              workSheet.Cells["A1:I1"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                              workSheet.Cells["A2:I2"].Merge = true;
                              workSheet.Cells["A2:I2"].Style.Font.Size = 16;
                              workSheet.Cells["A3:I3"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                              workSheet.Cells["A3:I3"].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#D8D8D8"));
                              workSheet.Cells["A3:I3"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                              workSheet.Cells["A3:I3"].Style.Font.Bold = true;
                              workSheet.Cells["A3:I3"].Style.Font.Size = 10;
                              workSheet.Cells["A3:I3"].Style.Font.Name = "Arial";
                              workSheet.Cells["A3:I3"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                              workSheet.Cells["A3:I3"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                              workSheet.Cells["A3:I3"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                              workSheet.Cells["A3:I3"].Style.Border.Left.Style = ExcelBorderStyle.Thin;

                              // Lấy các trường cần hiển thị
                              var members = new MemberInfo[]
                              {
                              property.GetProperty("EmployeeCode"),
                              property.GetProperty("FullName"),
                              property.GetProperty("Gender"),
                              property.GetProperty("DateOfBirth"),
                              property.GetProperty("PositionName"),
                              property.GetProperty("DepartmentName"),
                              property.GetProperty("BankAccount"),
                              property.GetProperty("BankName")
                              };

                              // Style cho header file excel
                              workSheet.Cells[3, 1].Value = "STT";
                              workSheet.Cells[3, 1].AutoFitColumns();
                              for (int j = 1; j <= members.Length; j++)
                              {
                                   Object[] attribute = ((PropertyInfo)members[j - 1]).GetCustomAttributes(typeof(System.ComponentModel.DisplayNameAttribute), true);
                                   if (attribute.Length > 0)
                                   {
                                        workSheet.Cells[3, j + 1].Value = (attribute[0] as System.ComponentModel.DisplayNameAttribute).DisplayName;
                                   }
                              }

                              // Style cho body file excel
                              for (int i = 0; i < exportData.Count(); i++)
                              {
                                   workSheet.Cells[i + 4, 1].Value = i + 1;
                                   workSheet.Cells[i + 4, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                   workSheet.Cells[i + 4, 1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                   workSheet.Cells[i + 4, 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                   workSheet.Cells[i + 4, 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                   workSheet.Cells[i + 4, 1].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                   workSheet.Cells[i + 4, 1].Style.Font.Size = 11;
                                   workSheet.Cells[i + 4, 1].Style.Font.Name = "Times New Roman";

                                   for (int j = 1; j <= members.Length; j++)
                                   {
                                        var value = ((PropertyInfo)members[j - 1]).GetValue(exportData[i]);
                                        if (value != null && value.GetType().Name.Equals("DateTime"))
                                        {
                                             workSheet.Cells[i + 4, j + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                             value = String.Format("{0:dd/MM/yyyy}", ((PropertyInfo)members[j - 1]).GetValue(exportData[i]));
                                        }
                                        if (value != null && value.GetType().Name.Equals("Gender")) value = value.GetType()?.GetMember(value.ToString())?.First()?.GetCustomAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>()?.Name;
                                        workSheet.Cells[i + 4, j + 1].Value = value;
                                        workSheet.Cells[i + 4, j + 1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                        workSheet.Cells[i + 4, j + 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                        workSheet.Cells[i + 4, j + 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                        workSheet.Cells[i + 4, j + 1].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                        workSheet.Cells[i + 4, j + 1].Style.Font.Size = 11;
                                        workSheet.Cells[i + 4, j + 1].Style.Font.Name = "Times New Roman";
                                   }
                              }

                              workSheet.Column(1).Width = 5;
                              workSheet.Column(2).Width = 15;
                              workSheet.Column(3).Width = 25;
                              workSheet.Column(4).Width = 10;
                              workSheet.Column(5).Width = 15;
                              workSheet.Column(6).Width = 25;
                              workSheet.Column(7).Width = 25;
                              workSheet.Column(8).Width = 20;
                              workSheet.Column(9).Width = 25;


                              // Export file excel
                              var excelData = excel.GetAsByteArray();
                              excel.Dispose();
                              var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                              var fileName = $"{Resource.Excel_FileName}.xlsx";
                              return File(excelData, contentType, fileName);
                         }
                    }
                    else
                    {
                         // Trả về kết quả thành công
                         return StatusCode(StatusCodes.Status500InternalServerError, result);
                    }
               }
               catch (Exception ex)
               {
                    Console.WriteLine(ex.Message);

                    // Trả về lỗi khi gặp vấn đề 
                    return StatusCode(StatusCodes.Status500InternalServerError, new ServiceResponse
                    {
                         Success = false,
                         Data = new ErrorResult(
                                   CustomizeErrorCode.Exception,
                                   Resource.DevMsg_Exception,
                                   Resource.UserMsg_Exception,
                                   Resource.MoreInfo_Exception,
                                   HttpContext.TraceIdentifier
                                   )
                    });
               }
          }

          #endregion

          #region API INSERT


          /// <summary>
          /// API Nhập khẩu dữ liệu
          /// </summary>
          /// NXTSAN 09-11-2022
          [HttpPost("import-data"), AuthorizeRoles(Role.Admin, Role.SuperAdmin, Role.User)]
          public async Task<IActionResult> ImportData([FromForm(Name = "file")] IFormFile data)
          {
               try
               {
                    if (ModelState.IsValid)
                    {
                         if (data?.Length > 0)
                         {
                              // Mở stream
                              var stream = data.OpenReadStream();

                              List<Employee> employees = new List<Employee>();

                              // Tạo ExcelPackage
                              using (var package = new ExcelPackage(stream))
                              {
                                   ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                                   // Lấy WorkSheet
                                   var worksheet = package.Workbook.Worksheets.First();
                                   var rowCount = worksheet.Dimension.Rows;
                                   // Đọc dữ liệu
                                   for (int row = 4; row <= rowCount; row++)
                                   {
                                        var employeeCode = worksheet.Cells[row, 1].Value?.ToString();
                                        var fullName = worksheet.Cells[row, 2].Value?.ToString();
                                        var dateOfBirthAsString = worksheet.Cells[row, 3].Value?.ToString();
                                        var genderAsString = worksheet.Cells[row, 4].Value?.ToString();
                                        var departmentName = worksheet.Cells[row, 5].Value?.ToString();
                                        var identityNumber = worksheet.Cells[row, 6].Value?.ToString();
                                        var identityDateAsString = worksheet.Cells[row, 7].Value?.ToString();
                                        var positionName = worksheet.Cells[row, 8].Value?.ToString();

                                        var employee = new Employee
                                        {
                                             EmployeeCode = employeeCode,
                                             FullName = fullName,
                                             DateOfBirth = DateTime.TryParseExact(dateOfBirthAsString, "dd/MM/yyyy",CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateOfBirth) ? dateOfBirth : null,
                                             Gender = Extensions.GenderParse(genderAsString),
                                             DepartmentName = departmentName,
                                             IdentityNumber = identityNumber,
                                             IdentityDate = DateTime.TryParseExact(identityDateAsString, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime identityDate) ? identityDate : null,
                                             PositionName = positionName
                                        };

                                        employees.Add(employee);
                                   }
                              }

                              var result = await _employeeBL.ImportData(employees);
                              if (result.Success)
                              {
                                   // Trả về kết quả thành công
                                   return StatusCode(StatusCodes.Status201Created, employees);
                              }
                              else
                              {
                                   return StatusCode(StatusCodes.Status500InternalServerError, result);
                              }

                         }
                    }

                    var errors = ModelState.Values.SelectMany(v => v.Errors);

                    return StatusCode(StatusCodes.Status500InternalServerError, new ServiceResponse
                    {
                         Success = false,
                         Data = new ErrorResult(
                                   CustomizeErrorCode.Exception,
                                   Resource.DevMsg_Exception,
                                   Resource.UserMsg_Exception,
                                   errors,
                                   HttpContext.TraceIdentifier
                                   )
                    });
               }
               catch (Exception ex)
               {
                    Console.WriteLine(ex.Message);

                    // Trả về lỗi khi gặp vấn đề 
                    return StatusCode(StatusCodes.Status500InternalServerError, new ServiceResponse
                    {
                         Success = false,
                         Data = new ErrorResult(
                                   CustomizeErrorCode.Exception,
                                   Resource.DevMsg_Exception,
                                   Resource.UserMsg_Exception,
                                   Resource.MoreInfo_Exception,
                                   HttpContext.TraceIdentifier
                                   )
                    });
               }
          }

          #endregion

          #region API Delete

          /// <summary>
          /// API Xoá nhiều người dùng batch-delete
          /// </summary>
          /// <param name="employeeIDs"></param>
          /// <returns></returns>
          /// Created by: NXTSAN (16/09/2022)
          [HttpPost("batch-delete"), AuthorizeRoles(Role.Admin, Role.User, Role.SuperAdmin)]
          public IActionResult BatchDeleteEmployees([FromBody] List<Guid> employeeIDs)
          {
               try
               {
                    // Thực hiện các nghiệp vụ ở BL
                    var result = _employeeBL.BatchDeleteEmployees(employeeIDs);

                    // Trả về kết quả thành công
                    if ((int)result.Data == employeeIDs.Count)
                    {
                         return StatusCode(StatusCodes.Status200OK, result);
                    }

                    // Trả về lỗi khi gặp vấn đề 
                    return StatusCode(StatusCodes.Status500InternalServerError, new ServiceResponse
                    {
                         Success = false,
                         Data = new ErrorResult(
                         CustomizeErrorCode.Exception,
                         Resource.DevMsg_Exception,
                         Resource.UserMsg_Exception,
                         Resource.MoreInfo_Exception,
                         HttpContext.TraceIdentifier
                         )
                    });
               }
               catch (Exception ex)
               {
                    Console.WriteLine(ex.Message);

                    // Trả về lỗi khi gặp vấn đề 
                    return StatusCode(StatusCodes.Status500InternalServerError, new ServiceResponse
                    {
                         Success = false,
                         Data = new ErrorResult(
                                   CustomizeErrorCode.Exception,
                                   Resource.DevMsg_Exception,
                                   Resource.UserMsg_Exception,
                                   Resource.MoreInfo_Exception,
                                   HttpContext.TraceIdentifier
                                   )
                    });
               }
          }
          #endregion
     }
}
