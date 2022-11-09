using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using MISA.Web08.Customize.API.Result;
using MISA.Web08.Customize.Common.Entities;
using MISA.Web08.Customize.Common.Enums;

namespace MISA.Web08.Customize.DL
{
     /// <summary>
     /// Interface truy cập dữ liệu nhân viên
     /// </summary>
     /// NXTSAN 26-09-2022
     public interface IEmployeeDL : IBaseDL<Employee>
     {
          #region GET

          /// <summary>
          /// Lấy mã nhân viên tự tăng
          /// </summary>
          /// NXTSAN 26-09-2022
          public object GetNewEmployeeCode();          

          /// <summary>
          /// Lấy tất cả nhân viên thoả mãn bộ lọc ()
          /// </summary>
          /// <param name="keyword"></param>
          /// <param name="limit"></param>
          /// <param name="offset"></param>
          /// NXTSAN 26-09-2022
          public PagingData FilterEmployee(string? keyword, int? pageSize, int? pageNumber);

          /// <summary>
          /// Lấy thông tin 1 nhân viên theo EmployeeCode(Mã nhân viên)
          /// </summary>
          /// <param name="RecordID"</param>
          /// <returns>bản ghi</returns>
          /// NXTSAN 26-09-2022
          public string GetValidEmployeeCode(Guid? employeeID, string employeeCode);

          #endregion

          #region INSERT

          /// <summary>
          /// Nhập dữ liệu
          /// </summary>
          /// <param name="employees"></param>
          /// NXTSAN 09-11-2022
          public Task<int> ImportData(List<Employee> employees);

          #endregion

          #region DELETE

          /// <summary>
          /// Xoa nhieu nhan vien
          /// </summary>
          /// <param name="employeeIDs"></param>
          /// NXTSAN 26-09-2022
          public int BatchDeleteEmployees(List<Guid> employeeIDs);

          #endregion
     }
}
