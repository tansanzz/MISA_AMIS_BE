using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MISA.Web08.Customize.Common.Entities;

namespace MISA.Web08.Customize.DL 
{
     public interface IBaseDL<T>
     {
          #region GET

          /// <summary>
          /// Lấy danh sách toàn bộ bản ghi trong 1 bảng
          /// </summary>
          /// <returns>Danh sách toàn bộ nhân viên</returns>
          /// NXTSAN 26-09-2022
          public IEnumerable<T> GetAllRecords();

          /// <summary>
          /// Lấy thông tin 1 bản ghi theo ID
          /// </summary>
          /// <param name="RecordID"</param>
          /// <returns>bản ghi</returns>
          /// NXTSAN 26-09-2022
          public T GetRecordByID(Guid RecordID);

          #endregion

          #region POST

          /// <summary>
          /// API Thêm mới 1 bản ghi
          /// </summary>
          /// <param name="record">Bản ghi cần thêm mới</param>
          /// <returns>ID bản ghi vừa thêm mới</returns>
          /// NXTSAN 16-09-2022
          public Task<Guid> InsertRecord(T record);

          #endregion

          #region PUT

          /// <summary>
          /// API Sửa thông tin của bản ghi
          /// </summary>
          /// <param name="recordID"></param>
          /// <param name="record"></param>
          /// <returns></returns>
          /// NXTSAN 16-09-2022
          public Guid UpdateRecord(Guid recordID, T record);

          #endregion

          #region DELETE

          /// <summary>
          /// Xoá 1 bản ghi
          /// </summary>
          /// <param name="recordID"></param>
          /// <returns></returns>
          /// NXTSAN 26-09-2022
          public Guid DeleteRecord(Guid recordID);

          #endregion

     }
}
