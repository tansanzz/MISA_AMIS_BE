using System;
using System.Collections.Generic;
using System.Linq;
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
     public class BaseDL<T> : IBaseDL<T>
     {

          #region GET

          /// <summary>
          /// Lấy danh sách toàn bộ bản ghi trong 1 bảng
          /// </summary>
          /// <returns>Danh sách toàn bộ nhân viên</returns>
          /// NXTSAN 26-09-2022
          public IEnumerable<T> GetAllRecords()
          {
               // Kết quả database trả về
               IEnumerable<T> result;

               // Lấy tên của procedure
               string storedProcedureName = String.Format(Resource.Proc_GetAll, typeof(T).Name);

               // Lấy chuỗi kết nối với database
               string? connectionString = DataContext.MySqlConnectionString;

               // Thực thi câu lệnh truy vấn
               using (var mysqlConnection = new MySqlConnection(connectionString))
               {
                    result = mysqlConnection.Query<T>(
                         storedProcedureName,
                         commandType: System.Data.CommandType.StoredProcedure
                    );
               }

               // Trả về kết quả sau khi thực hiện truy vấn
               return result;
          }

          /// <summary>
          /// Lấy thông tin 1 bản ghi theo ID
          /// </summary>
          /// <param name="RecordID"</param>
          /// <returns>bản ghi</returns>
          /// NXTSAN 26-09-2022
          public T GetRecordByID(Guid recordID)
          {
               // Kết quả database trả về
               T result;

               // Lấy tên của procedure
               string storedProcedureName = String.Format(Resource.Proc_GetByID, typeof(T).Name);

               // Lấy chuỗi kết nối với database
               string? connectionString = DataContext.MySqlConnectionString;

               // Lấy các thuộc tính của đối tượng
               var properties = typeof(T).GetProperties();

               // Thêm các tham số
               var parameters = new DynamicParameters();
               foreach (var property in properties)
               {
                    var primaryKeyAttribute = (PrimaryKeyAttribute?)Attribute.GetCustomAttribute(property, typeof(PrimaryKeyAttribute));
                    object propertyValue;
                    string propertyName = property.Name;
                    if (primaryKeyAttribute != null)
                    {
                         propertyValue = recordID;
                         parameters.Add($"v_{propertyName}", propertyValue);
                    }
               }

               // Thực thi câu lệnh truy vấn
               using (var mysqlConnection = new MySqlConnection(connectionString))
               {
                    result = mysqlConnection.QueryFirstOrDefault<T>(
                         storedProcedureName,
                         parameters,
                         commandType: System.Data.CommandType.StoredProcedure
                    );
               }

               // Trả về kết quả sau khi thực hiện truy vấn
               return result;
          }

          #endregion

          #region INSERT

          /// <summary>
          /// Thêm mới 1 bản ghi
          /// </summary>
          /// <param name="record">Bản ghi cần thêm mới</param>
          /// <returns>ID bản ghi vừa thêm mới</returns>
          /// NXTSAN 26-09-2022
          public virtual async Task<Guid> InsertRecord(T record)
          {
               // Kết quả database trả về số bản ghi thành công
               var numberOfAffectedRows = 0;

               // Tạo GUID mới cho bản ghi cần thêm vào
               var newRecordID = Guid.NewGuid();

               // Lấy chuỗi kết nối với database
               string? connectionString = DataContext.MySqlConnectionString;

               // Lấy tên của procedure
               string storedProcedureName = String.Format(Resource.Proc_Insert, typeof(T).Name);

               // Lấy các thuộc tính của đối tượng
               var properties = typeof(T).GetProperties();

               // Thêm các tham số
               var parameters = new DynamicParameters();
               foreach (var property in properties)
               {
                    var primaryKeyAttribute = (PrimaryKeyAttribute?)Attribute.GetCustomAttribute(property, typeof(PrimaryKeyAttribute));
                    object propertyValue;
                    string propertyName = property.Name;
                    if (primaryKeyAttribute != null)
                    {
                         propertyValue = newRecordID;
                    }
                    else
                    {
                         propertyValue = property.GetValue(record);
                    }
                    parameters.Add($"v_{propertyName}", propertyValue);
               }

               // Thực thi câu lệnh truy vấn
               using (var mysqlConnection = new MySqlConnection(connectionString))
               {
                    numberOfAffectedRows = mysqlConnection.Execute(
                    storedProcedureName,
                    parameters,
                    commandType: System.Data.CommandType.StoredProcedure
                    );
               }

               // Trả về kết quả sau khi thực hiện truy vấn
               if (numberOfAffectedRows > 0)
               {
                    // Nếu thành công thì trả về GUID bản ghi vừa thêm
                    return newRecordID;
               }
               else
               {
                    // Nếu thất bại thì trả về GUID rỗng
                    return Guid.Empty;
               }
          }

          #endregion

          #region PUT

          /// <summary>
          /// Sửa thông tin của người dùng
          /// </summary>
          /// <param name="employeeID"></param>
          /// <param name="employee"></param>
          /// <returns>ID của người dùng đã sửa</returns>
          /// NXTSAN 26-09-2022
          public Guid UpdateRecord(Guid recordID, T record)
          {
               // Kết quả database trả về số bản ghi thành công
               var numberOfAffectedRows = 0;

               // Lấy chuỗi kết nối với database
               string? connectionString = DataContext.MySqlConnectionString;

               // Lấy tên của procedure
               string procedureName = String.Format(Resource.Proc_Update, typeof(T).Name);

               // Lấy các thuộc tính của đối tượng
               var properties = typeof(T).GetProperties();

               // Thêm các tham số
               var parameters = new DynamicParameters();
               foreach (var property in properties)
               {
                    var primaryKeyAttribute = (PrimaryKeyAttribute?)Attribute.GetCustomAttribute(property, typeof(PrimaryKeyAttribute));
                    var propertyName = property.Name;
                    var propertyValue = property.GetValue(record);

                    if (primaryKeyAttribute != null)
                    {
                         propertyValue = recordID;
                    }
                    parameters.Add($"v_{propertyName}", propertyValue);
               }

               // Thực thi câu lệnh truy vấn
               using (var mySqlConnection = new MySqlConnection(connectionString))
               {
                    numberOfAffectedRows = mySqlConnection.Execute(procedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);
               }


               if (numberOfAffectedRows > 0)
               {
                    // Nếu thành công thì trả về GUID bản ghi vừa thêm
                    return recordID;
               }
               else
               {
                    // Nếu thất bại thì trả về GUID rỗng
                    return Guid.Empty;
               }
          }

          #endregion

          #region DELETE

          /// <summary>
          /// Xoá 1 nhân viên
          /// </summary>
          /// <param name="recordID"></param>
          /// <returns>ID nhân viên đã xoá</returns>
          /// NXTSAN 26-09-2022
          public Guid DeleteRecord(Guid recordID)
          {
               // Kết quả database trả về số bản ghi thành công
               var numberOfAffectedRows = 0;

               // Lấy chuỗi kết nối với database
               string? connectionString = DataContext.MySqlConnectionString;

               // Lấy tên của procedure
               string procedureName = String.Format(Resource.Proc_Delete, typeof(T).Name);

               // Lấy các thuộc tính của đối tượng
               var properties = typeof(T).GetProperties();

               // Thêm các tham số
               var parameters = new DynamicParameters();
               foreach (var property in properties)
               {
                    var primaryKeyAttribute = (PrimaryKeyAttribute?)Attribute.GetCustomAttribute(property, typeof(PrimaryKeyAttribute));
                    object propertyValue;
                    string propertyName = property.Name;
                    if (primaryKeyAttribute != null)
                    {
                         propertyValue = recordID;
                         parameters.Add($"v_{propertyName}", propertyValue);
                    }
               }

               // Thực thi câu lệnh truy vấn
               using (var mySqlConnection = new MySqlConnection(connectionString))
               {
                    numberOfAffectedRows = mySqlConnection.Execute(procedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);
               }


               if (numberOfAffectedRows > 0)
               {
                    // Nếu thành công thì trả về GUID bản ghi vừa thêm
                    return recordID;
               }
               else
               {
                    // Nếu thất bại thì trả về GUID rỗng
                    return Guid.Empty;
               }
          }

          #endregion
     }
}
