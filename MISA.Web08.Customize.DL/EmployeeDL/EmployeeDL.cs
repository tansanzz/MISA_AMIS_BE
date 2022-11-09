using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using MISA.Web08.Customize.API.Result;
using MISA.Web08.Customize.Common.Attributes;
using MISA.Web08.Customize.Common.Entities;
using MISA.Web08.Customize.Common.Enums;
using MISA.Web08.Customize.Common.Resources;
using MySqlConnector;

namespace MISA.Web08.Customize.DL
{
     /// <summary>
     /// Lớp thực thi các hàm trong Interface IEmployeeDL
     /// </summary>
     /// NXTSAN 26-09-2022
     public class EmployeeDL : BaseDL<Employee>, IEmployeeDL
     {
          private IUnitOfWork _unitOfWork;

          public EmployeeDL(IUnitOfWork unitOfWork)
          {
               _unitOfWork = unitOfWork;
          }

          #region GET

          /// <summary>
          /// Lấy mã nhân viên tự tăng
          /// </summary>
          /// <returns>Mã nhân viên</returns>
          /// NXTSAN 26-09-2022
          public object GetNewEmployeeCode()
          {
               // Kết quả database trả về
               object newEmployeeCode;

               // Lấy chuỗi kết nối với database
               string? connectionString = DataContext.MySqlConnectionString;

               // Lấy tên của procedure
               string procedureName = String.Format(Resource.Proc_GetNewCode, typeof(Employee).Name);

               // Thực thi câu lệnh truy vấn
               using (var mysqlConnection = new MySqlConnection(connectionString))
               {
                    newEmployeeCode = mysqlConnection.QueryFirstOrDefault<object>(procedureName, commandType: System.Data.CommandType.StoredProcedure);
               }

               // Trả về kết quả sau khi thực hiện truy vấn
               return newEmployeeCode;
          }

          /// <summary>
          /// Lấy thông tin 1 nhân viên theo EmployeeCode(Mã nhân viên)
          /// </summary>
          /// <param name="RecordID"</param>
          /// <returns>bản ghi</returns>
          /// NXTSAN 26-09-2022
          public string GetValidEmployeeCode(Guid? employeeID, string employeeCode)
          {
               // Kết quả database trả về
               string employeeCodeDuplicate;

               // Lấy chuỗi kết nối với database
               string? connectionString = DataContext.MySqlConnectionString;

               // Lấy tên của procedure
               string storedProcedureName = String.Format(Resource.Proc_GetValidEmployeeCode, typeof(Employee).Name);

               // Thêm các tham số
               var parameters = new DynamicParameters();
               parameters.Add($"v_EmployeeCode", employeeCode);
               parameters.Add($"v_EmployeeID", employeeID);

               // Thực thi câu lệnh truy vấn
               using (var mysqlConnection = new MySqlConnection(connectionString))
               {
                    employeeCodeDuplicate = mysqlConnection.QueryFirstOrDefault<string>(
                         storedProcedureName,
                         parameters,
                         commandType: System.Data.CommandType.StoredProcedure
                    );
               }

               // Trả về kết quả sau khi thực hiện truy vấn
               return employeeCodeDuplicate;
          }

          /// <summary>
          /// Lấy tất cả nhân viên thoả mãn bộ lọc
          /// </summary>
          /// <param name="keyword"></param>
          /// <param name="limit"></param>
          /// <param name="offset"></param>
          /// <returns>Danh sách nhân viên</returns>
          /// NXTSAN 26-09-2022 
          public PagingData FilterEmployee(string? keyword, int? pageSize, int? pageNumber)
          {
               // Kết quả database trả về
               PagingData result;

               // Lấy chuỗi kết nối với database
               string? connectionString = DataContext.MySqlConnectionString;

               // Lấy tên của procedure
               string procedureName = String.Format(Resource.Proc_Filter, typeof(Employee).Name);

               // Thêm các tham số
               int? offset = (pageNumber - 1) * pageSize;
               var parameters = new DynamicParameters();
               parameters.Add("v_Offset", offset);
               parameters.Add("v_Limit", pageSize);
               parameters.Add("v_Where", keyword);

               // Thực thi câu lệnh truy vấn
               using (var mysqlConnection = new MySqlConnection(connectionString))
               {
                    var listEmployees = mysqlConnection.QueryMultiple(procedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);
                    result = new PagingData
                    {
                         Data = listEmployees.Read<Employee>().ToList(),
                         TotalCount = listEmployees.Read<Int32>().First(),
                    };
               }

               // Trả về kết quả sau khi thực hiện truy vấn
               return result;
          }

          #endregion

          #region INSERT

          /// <summary>
          /// Thêm mới 1 nhân viên
          /// </summary>
          /// <param name="employee"></param>
          /// <returns>ID nhân viên vừa thêm mới</returns>
          /// NXTSAN 16-09-2022
          public override async Task<Guid> InsertRecord(Employee employee)
          {
               // Kết quả database trả về số bản ghi thành công
               var numberOfAffectedDepartment = 0;
               var numberOfAffectedEmployee = 0;

               // Tạo GUID mới cho bản ghi cần thêm vào
               var newEmployeeID = Guid.NewGuid();
               try
               {
                    // Lấy tên của procedure
                    string storeAddEmployee = String.Format(Resource.Proc_Insert, typeof(Employee).Name);
                    string storeAddDepartment = String.Format(Resource.Proc_Insert, typeof(Department).Name);
                    string storeFindDepartment = String.Format(Resource.Proc_GetByName, typeof(Department).Name);

                    // Mở kết nối
                    await _unitOfWork.Connection.OpenAsync();


                    // Bắt đầu transaction
                    await _unitOfWork.BeginAsync();


                    Department department;

                    var parametersFindDepartment = SetParametersFindDepartment(employee.DepartmentName);
                    // Nếu ID đơn vị tồn tại thì tìm đơn vị
                    department = await _unitOfWork.Connection.QueryFirstOrDefaultAsync<Department>(
                         storeFindDepartment,
                         parametersFindDepartment,
                         transaction: _unitOfWork.Transaction,
                         commandType: System.Data.CommandType.StoredProcedure);

                    if (department == null)
                    {
                         // Nếu đơn vị rỗng thì tạo đơn vị mới
                         department = new Department
                         {
                              DepartmentID = Guid.NewGuid(),
                              DepartmentName = employee.DepartmentName,
                         };

                         var parametersAddDepartment = SetParametersAddDepartment(department);

                         // Nếu đơn vị không tồn tại thì thêm đơn vị đó vào bảng đơn vị
                         numberOfAffectedDepartment = await _unitOfWork.Connection.ExecuteAsync(
                              storeAddDepartment,
                              parametersAddDepartment,
                              transaction: _unitOfWork.Transaction,
                              commandType: System.Data.CommandType.StoredProcedure);

                    }

                    var parametersAddEmployee = SetParametersAddEmployee(newEmployeeID, employee);
                    parametersAddEmployee.Add("v_DepartmentID", department.DepartmentID);
                    // Thêm mới nhân viên
                    numberOfAffectedEmployee = await _unitOfWork.Connection.ExecuteAsync(
                    storeAddEmployee,
                    parametersAddEmployee,
                    transaction: _unitOfWork.Transaction,
                    commandType: System.Data.CommandType.StoredProcedure
                    );

                    // Không gặp lỗi thì commit
                    await _unitOfWork.CommitAsync();
               }
               catch (Exception)
               {
                    // Nếu gặp lỗi thì rollback
                    await _unitOfWork.RollbackAsync();
               }
               finally
               {
                    _unitOfWork.Dispose();
               }

               // Trả về kết quả sau khi thực hiện truy vấn
               if (numberOfAffectedEmployee > 0)
               {
                    // Nếu thành công thì trả về GUID bản ghi vừa thêm
                    return newEmployeeID;
               }
               else
               {
                    // Nếu thất bại thì trả về GUID rỗng
                    return Guid.Empty;
               }
          }

          /// <summary>
          /// Nhập dữ liệu
          /// </summary>
          /// <param name="employees"></param>
          /// NXTSAN 09-11-2022
          public async Task<int> ImportData(List<Employee> employees)
          {
               var numberOfAffectedEmployee = 0;
               // Lấy tên của procedure

               try
               {
                    // Mở kết nối
                    await _unitOfWork.Connection.OpenAsync();

                    // Bắt đầu transaction
                    await _unitOfWork.BeginAsync();

                    foreach (var employee in employees)
                    {
                         // Kết quả database trả về số bản ghi thành công
                         var numberOfAffectedDepartment = 0;

                         // Tạo GUID mới cho bản ghi cần thêm vào
                         var newEmployeeID = Guid.NewGuid();
                         // Lấy tên của procedure
                         string storeAddEmployee = String.Format(Resource.Proc_Insert, typeof(Employee).Name);
                         string storeAddDepartment = String.Format(Resource.Proc_Insert, typeof(Department).Name);
                         string storeFindDepartment = String.Format(Resource.Proc_GetByName, typeof(Department).Name);

                         Department department;

                         var parametersFindDepartment = SetParametersFindDepartment(employee.DepartmentName);
                         // Nếu ID đơn vị tồn tại thì tìm đơn vị
                         department = await _unitOfWork.Connection.QueryFirstOrDefaultAsync<Department>(
                              storeFindDepartment,
                              parametersFindDepartment,
                              transaction: _unitOfWork.Transaction,
                              commandType: System.Data.CommandType.StoredProcedure);

                         if (department == null)
                         {
                              // Nếu đơn vị rỗng thì tạo đơn vị mới
                              department = new Department
                              {
                                   DepartmentID = Guid.NewGuid(),
                                   DepartmentName = employee.DepartmentName,
                              };

                              var parametersAddDepartment = SetParametersAddDepartment(department);

                              // Nếu đơn vị không tồn tại thì thêm đơn vị đó vào bảng đơn vị
                              numberOfAffectedDepartment = await _unitOfWork.Connection.ExecuteAsync(
                                   storeAddDepartment,
                                   parametersAddDepartment,
                                   transaction: _unitOfWork.Transaction,
                                   commandType: System.Data.CommandType.StoredProcedure);

                         }

                         var parametersAddEmployee = SetParametersAddEmployee(newEmployeeID, employee);
                         parametersAddEmployee.Add("v_DepartmentID", department.DepartmentID);
                         // Thêm mới nhân viên
                         numberOfAffectedEmployee += await _unitOfWork.Connection.ExecuteAsync(
                         storeAddEmployee,
                         parametersAddEmployee,
                         transaction: _unitOfWork.Transaction,
                         commandType: System.Data.CommandType.StoredProcedure);
                    }

                    await _unitOfWork.CommitAsync();
               }
               catch (Exception)
               {
                    await _unitOfWork.RollbackAsync();
               }
               finally
               {
                    _unitOfWork.Dispose();
               }

               return numberOfAffectedEmployee;
          }

          #endregion

          #region DELETE

          /// <summary>
          /// Xoa nhieu nhan vien
          /// </summary>
          /// NXTSAN 08-10-2022
          public int BatchDeleteEmployees(List<Guid> employeeIDs)
          {
               // Kết quả database trả về
               var numberOfAffectedRows = 0;

               // Lấy chuỗi kết nối với database
               string? connectionString = DataContext.MySqlConnectionString;

               // Lấy tên của procedure
               string procedureName = String.Format(Resource.Proc_Delete, typeof(Employee).Name);

               // Lấy các thuộc tính của đối tượng
               var properties = typeof(Employee).GetProperties();

               // Thực thi câu lệnh truy vấn
               using (var mySqlConnection = new MySqlConnection(connectionString))
               {
                    mySqlConnection.Open();

                    // Thực thi transaction
                    using (var transaction = mySqlConnection.BeginTransaction())
                    {
                         foreach (var employeeID in employeeIDs)
                         {
                              var parameters = new DynamicParameters();
                              foreach (var property in properties)
                              {
                                   var primaryKeyAttribute = (PrimaryKeyAttribute?)Attribute.GetCustomAttribute(property, typeof(PrimaryKeyAttribute));
                                   object propertyValue;
                                   string propertyName = property.Name;
                                   if (primaryKeyAttribute != null)
                                   {
                                        propertyValue = employeeID;
                                        parameters.Add($"v_{propertyName}", propertyValue);
                                        break;
                                   }
                              }
                              numberOfAffectedRows += mySqlConnection.Execute(procedureName, parameters, commandType: System.Data.CommandType.StoredProcedure, transaction: transaction);
                         }
                         if (numberOfAffectedRows == employeeIDs.Count)
                         {
                              transaction.Commit();
                         }
                         else
                         {
                              transaction.Rollback();
                         }
                    }
               }

               // Trả về kết quả sau khi thực hiện truy vấn
               return numberOfAffectedRows;
          }

          #endregion

          /// <summary>
          /// Thêm các tham số cho thêm nhân viên
          /// </summary>
          /// <param name="newEmployeeID"></param>
          /// <param name="employee"></param>
          /// NXTSAN 07-11-2022
          private static DynamicParameters SetParametersAddEmployee(Guid newEmployeeID, Employee employee)
          {
               // Lấy các thuộc tính của đối tượng
               var properties = typeof(Employee).GetProperties();
               var parameters = new DynamicParameters();
               foreach (var property in properties)
               {
                    var primaryKeyAttribute = (PrimaryKeyAttribute?)Attribute.GetCustomAttribute(property, typeof(PrimaryKeyAttribute));
                    object propertyValue;
                    string propertyName = property.Name;
                    if (primaryKeyAttribute != null)
                    {
                         propertyValue = newEmployeeID;
                    }
                    else
                    {
                         propertyValue = property.GetValue(employee);
                    }
                    parameters.Add($"v_{propertyName}", propertyValue);
               }

               return parameters;
          }

          /// <summary>
          /// Thêm các tham số cho tìm đơn vị
          /// </summary>
          /// <param name="departmentID"></param>
          /// NXTSAN 07-11-2022
          private static DynamicParameters SetParametersFindDepartment(string departmentName)
          {
               // Lấy các thuộc tính của đối tượng
               var parameters = new DynamicParameters();
               parameters.Add($"v_DepartmentName", departmentName);

               return parameters;
          }

          /// <summary>
          /// Thêm các tham số cho thêm đơn vị
          /// </summary>
          /// <param name="department"></param>
          /// NXTSAN 07-11-2022
          private static DynamicParameters SetParametersAddDepartment(Department department)
          {
               // Lấy các thuộc tính của đối tượng
               var properties = typeof(Department).GetProperties();
               var parameters = new DynamicParameters();
               foreach (var property in properties)
               {
                    var primaryKeyAttribute = (PrimaryKeyAttribute?)Attribute.GetCustomAttribute(property, typeof(PrimaryKeyAttribute));
                    object propertyValue;
                    string propertyName = property.Name;
                    if (primaryKeyAttribute != null)
                    {
                         propertyValue = new Guid();
                    }
                    else
                    {
                         propertyValue = property.GetValue(department);
                    }
                    parameters.Add($"v_{propertyName}", propertyValue);
               }

               return parameters;
          }
     }
}
