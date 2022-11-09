namespace MISA.Web08.Customize.Common.Entities
{
     /// <summary>
     /// Lấy dữ liệu phân trang
     /// </summary>
     /// NXTSAN 21-09-2022
     public class PagingData
    {
          /// <summary>
          /// Danh sách nhân viên
          /// </summary>
          /// NXTSAN 21-09-2022
          public List<Employee> Data { get; set; }
          /// <summary>
          /// Tổng số bản ghi
          /// </summary>
          /// NXTSAN 21-09-2022
          public int TotalCount { get; set; }
    }
}
