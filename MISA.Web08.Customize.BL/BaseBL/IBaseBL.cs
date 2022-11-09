using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MISA.Web08.Customize.Common.Entities;

namespace MISA.Web08.Customize.BL
{
     public interface IBaseBL<T>
     {

          #region GET

          /// <summary>
          /// Lấy danh sách toàn bộ bản ghi
          /// </summary>
          /// <returns>Danh sách toàn bộ bản ghi</returns>
          /// NXTSAN 26-09-2022
          public ServiceResponse GetAllRecords();

          /// <summary>
          /// API Lấy thông tin 1 bản ghi theo ID
          /// </summary>
          /// <param name="RecordID"</param>
          /// <returns>bản ghi</returns>
          /// NXTSAN 16-09-2022
          public ServiceResponse GetRecordByID(Guid RecordID);

          #endregion

          #region POST

          /// <summary>
          /// API Thêm mới 1 bản ghi
          /// </summary>
          /// <param name="record">Bản ghi cần thêm mới</param>
          /// <returns>ID bản ghi vừa thêm mới</returns>
          /// NXTSAN 16-09-2022
          public Task<ServiceResponse> InsertRecord(T record);

          #endregion

          #region PUT

          /// <summary>
          /// API Sửa thông tin của bản ghi
          /// </summary>
          /// <param name="recordID"></param>
          /// <param name="record"></param>
          /// <returns></returns>
          /// NXTSAN 16-09-2022
          public ServiceResponse UpdateRecord(Guid recordID, T record);

          #endregion

          #region DELETE

          /// <summary>
          /// API Xoá 1 bản ghi
          /// </summary>
          /// <param name="recordID"></param>
          /// <returns></returns>
          /// NXTSAN 16-09-2022
          public ServiceResponse DeleteRecord(Guid recordID);

          #endregion

     }
}
