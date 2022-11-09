using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MISA.Web08.Customize.Common.Entities;

namespace MISA.Web08.Customize.BL
{
     public interface IEmployeeBL : IBaseBL<Employee>
     {

          #region GET

          /// <summary>
          /// Lấy mã nhân viên tự tăng
          /// </summary>
          /// <param name="EmployeeID"</param>
          /// <returns>Nhân viên</returns>
          /// NXTSAN 26-09-2022
          public ServiceResponse GetNewEmployeeCode();

          /// <summary>
          /// Lấy tất cả nhân viên thoả mãn bộ lọc ()
          /// </summary>
          /// <param name="keyword"></param>
          /// <param name="limit"></param>
          /// <param name="offset"></param>
          /// <returns>Danh sách nhân viên</returns>
          /// NXTSAN 26-09-2022
          public ServiceResponse FilterEmployee(string? keyword, int? pageSize, int? pageNumber);

          #endregion

          #region POST

          /// <summary>
          /// Nhập dữ liệu
          /// </summary>
          /// <param name="employees"></param>
          /// NXTSAN 09-11-2022
          public Task<ServiceResponse> ImportData(List<Employee> employees);

          #endregion

          #region DELETE

          /// <summary>
          /// Xoa nhieu nhan vien
          /// </summary>
          /// <param name="employeeIDs"></param>
          /// NXTSAN 26-09-2022
          public ServiceResponse BatchDeleteEmployees(List<Guid> employeeIDs);

          #endregion

     }
}
