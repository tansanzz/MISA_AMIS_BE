using MISA.Web08.Customize.Common.Enums;

namespace MISA.Web08.Customize.API.Result
{
     /// <summary>
     /// Danh sách mã lỗi
     /// </summary>
     /// NXTSAN 21-09-2022
     public class ErrorResult
     {
          #region Property
          /// <summary>
          /// Mã lỗi
          /// </summary>
          /// NXTSAN 21-09-2022
          public CustomizeErrorCode ErrorCode { get; set; }
          /// <summary>
          /// Lỗi cho dev
          /// </summary>
          /// NXTSAN 21-09-2022
          public string? DevMsg { get; set; }
          /// <summary>
          /// Lỗi cho người dùng
          /// </summary>
          /// NXTSAN 21-09-2022
          public string? UserMsg { get; set; }
          /// <summary>
          /// Thêm thông tin
          /// </summary>
          /// NXTSAN 21-09-2022
          public object? MoreInfo { get; set; }
          /// <summary>
          /// ID lỗi
          /// </summary>
          /// NXTSAN 21-09-2022
          public string? TraceID { get; set; }

          #endregion

          #region Constructor
          public ErrorResult()
          {

          }

          public ErrorResult(CustomizeErrorCode errorCode, string devMsg, string userMsg, object? moreInfo)
          {
               ErrorCode = errorCode;
               DevMsg = devMsg;
               UserMsg = userMsg;
               MoreInfo = moreInfo;
          }

          public ErrorResult(CustomizeErrorCode errorCode, string devMsg, string userMsg, object? moreInfo, string traceID)
          {
               ErrorCode = errorCode;
               DevMsg = devMsg;
               UserMsg = userMsg;
               MoreInfo = moreInfo;
               TraceID = traceID;
          }
          #endregion


     }
}
