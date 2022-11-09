using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.Web08.Customize.API.Result;
using MISA.Web08.Customize.BL;
using MISA.Web08.Customize.Common.Attributes;
using MISA.Web08.Customize.Common.Entities;
using MISA.Web08.Customize.Common.Enums;
using MISA.Web08.Customize.Common.Resources;

namespace MISA.Web08.Customize.API.Controllers
{
     [Route("api/v1/[controller]")]
     [ApiController]
     [Authorize]
     public class BaseController<T> : ControllerBase
     {

          #region Field

          private IBaseBL<T> _baseBL;

          #endregion

          #region Constructor

          public BaseController(IBaseBL<T> baseBL)
          {
               _baseBL = baseBL;
          }

          #endregion

          #region API GET

          /// <summary>
          /// API Lấy danh sách toàn bộ bản ghi
          /// </summary>
          /// <returns>Danh sách toàn bộ bản ghi</returns>
          /// NXTSAN 16/09/2022
          [HttpGet, AuthorizeRoles(Role.Admin, Role.User, Role.SuperAdmin)]
          public IActionResult GetAllRecords()
          {
               try
               {
                    // Thực hiện các nghiệp vụ ở BL
                    var result = _baseBL.GetAllRecords();

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
          /// API Lấy thông tin 1 bản ghi theo ID
          /// </summary>
          /// <param name="RecordID"</param>
          /// <returns>Nhân viên</returns>
          /// Created by: NXTSAN (16/09/2022)
          [HttpGet("{RecordID}"), AuthorizeRoles(Role.Admin, Role.User, Role.SuperAdmin)]
          public IActionResult GetRecordByID(Guid RecordID)
          {
               try
               {
                    // Thực hiện các nghiệp vụ ở BL
                    var result = _baseBL.GetRecordByID(RecordID);

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

          #endregion

          #region API Insert

          /// <summary>
          /// API Thêm mới 1 bản ghi
          /// </summary>
          /// <param name="record">Bản ghi cần thêm mới</param>
          /// <returns>ID bản ghi vừa thêm mới</returns>
          /// NXTSAN 16-09-2022
          [HttpPost, AuthorizeRoles(Role.Admin, Role.User, Role.SuperAdmin)]
          public async Task<IActionResult> InsertRecord([FromBody] T record)
          {
               try
               {
                    if (ModelState.IsValid)
                    {
                         // Thực hiện các nghiệp vụ ở BL
                         var result = await _baseBL.InsertRecord(record);

                         // Nếu kết quả trả về không thành công
                         if (!result.Success)
                         {
                              // Kiểm tra các mã lỗi trả về
                              if (result.Data.GetType().Name == typeof(ErrorResult).Name)
                              {
                                   var data = (ErrorResult)result.Data;
                                   var errorCode = data.ErrorCode;
                                   if (errorCode == CustomizeErrorCode.ValidateInput) return StatusCode(StatusCodes.Status400BadRequest, result);
                              }
                              else
                              {
                                   // Trả lỗi nếu không tìm được mã lỗi
                                   return StatusCode(StatusCodes.Status500InternalServerError, result);
                              }
                         }
                         else
                         {
                              // Nếu ID bản ghi vừa được thêm là rỗng thì trả lỗi
                              if ((Guid)result.Data == Guid.Empty)
                              {
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

                         // Trả về kết quả thành công
                         return StatusCode(StatusCodes.Status201Created, result);
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

          #region API Update

          /// <summary>
          /// API Sửa thông tin của bản ghi
          /// </summary>
          /// <param name="recordID"></param>
          /// <param name="record"></param>
          /// <returns></returns>
          /// NXTSAN 16-09-2022
          [HttpPut("{recordID}"), AuthorizeRoles(Role.Admin, Role.User, Role.SuperAdmin)]
          public IActionResult UpdateRecord([FromRoute] Guid recordID, [FromBody] T record)
          {
               try
               {
                    // Thực hiện các nghiệp vụ ở BL
                    var result = _baseBL.UpdateRecord(recordID, record);

                    // Nếu kết quả trả về không thành công
                    if (!result.Success)
                    {
                         // Kiểm tra các mã lỗi trả về
                         if (result.Data.GetType().Name == typeof(ErrorResult).Name)
                         {
                              var data = (ErrorResult)result.Data;
                              var errorCode = data.ErrorCode;
                              if (errorCode == CustomizeErrorCode.ValidateInput) return StatusCode(StatusCodes.Status400BadRequest, result);
                         }
                         else
                         {
                              // Trả lỗi nếu không tìm được mã lỗi
                              return StatusCode(StatusCodes.Status500InternalServerError, result);
                         }
                    }
                    else
                    {
                         // Nếu ID bản ghi vừa được sửa là rỗng thì trả lỗi
                         if ((Guid)result.Data == Guid.Empty)
                         {
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


          #endregion

          #region API Delete

          /// <summary>
          /// API Xoá 1 bản ghi
          /// </summary>
          /// <param name="recordID"></param>
          /// <returns></returns>
          /// NXTSAN 16-09-2022
          [HttpDelete("{recordID}"), AuthorizeRoles(Role.Admin, Role.User, Role.SuperAdmin)]
          public IActionResult DeleteRecord([FromRoute] Guid recordID)
          {
               try
               {
                    // Thực hiện các nghiệp vụ ở BL
                    var result = _baseBL.DeleteRecord(recordID);

                    // Nếu ID bản ghi vừa được xoá là rỗng thì trả lỗi
                    if ((Guid)result.Data == Guid.Empty)
                    {
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

          #endregion

     }
}
