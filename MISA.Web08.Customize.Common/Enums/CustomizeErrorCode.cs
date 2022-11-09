namespace MISA.Web08.Customize.Common.Enums
{
     /// <summary>
     /// Danh sách mã lỗi khi gọi API
     /// </summary>
     /// NXTSAN 21-09-2022
     public enum CustomizeErrorCode : int
     {
          /// <summary>
          /// Lỗi do exception
          /// </summary>
          /// NXTSAN 21-09-2022
          Exception = 1,

          /// <summary>
          /// Lỗi do thong tin truyen vao
          /// </summary>
          /// NXTSAN 21-09-2022
          BadRequest = 2,

          /// <summary>
          /// Lỗi do trùng mã
          /// </summary>
          /// NXTSAN 21-09-2022
          DuplicateEntry = 1062,

          /// <summary>
          /// Lỗi do khoa ngoai
          /// </summary>
          /// NXTSAN 21-09-2022
          ForeignKeyConstraint = 1452,

          /// <summary>
          /// Lỗi do khoa ngoai
          /// </summary>
          /// NXTSAN 21-09-2022
          SqlNo = 1002,

          /// <summary>
          /// Lỗi do mã bị trống
          /// </summary>
          /// NXTSAN 21-09-2022
          EmptyCode = 3,

          /// <summary>
          /// Lỗi do thêm thất bại
          /// </summary>
          /// NXTSAN 21-09-2022
          InsertFailed = 4,

          /// <summary>
          /// Lỗi do xoá thất bại
          /// </summary>
          /// NXTSAN 21-09-2022
          DeleteFailed = 5,

          /// <summary>
          /// Lỗi do sửa thất bại
          /// </summary>
          /// NXTSAN 21-09-2022
          UpdateFailed = 6,

          /// <summary>
          /// Lỗi do nhập thất bại
          /// </summary>
          /// NXTSAN 21-09-2022
          ValidateInput = 7,

          /// <summary>
          /// Lỗi do ID truyền vào sai
          /// </summary>
          /// NXTSAN 21-09-2022
          IDNotMatch = 8
     }
}
//