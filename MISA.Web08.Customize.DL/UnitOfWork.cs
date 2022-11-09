using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Web08.Customize.DL
{
     public class UnitOfWork : IUnitOfWork
     {
          #region Field

          /// <summary>
          /// Kết nối
          /// </summary>
          /// NXTSAN 07-11-2022
          public DbConnection Connection { get; set; }

          /// <summary>
          /// Transaction
          /// </summary>
          /// NXTSAN 07-11-2022
          public DbTransaction Transaction { get; set; }

          #endregion

          #region Constructor

          public UnitOfWork(DbConnection connection)
          {
               Connection = connection;
          }

          #endregion

          #region Methods

          /// <summary>
          /// Bắt đầu transaction
          /// </summary>
          /// NXTSAN 07-11-2022
          public async Task BeginAsync()
          {
               Transaction = await Connection.BeginTransactionAsync();
          }

          /// <summary>
          /// Commit transaction
          /// </summary>
          /// NXTSAN 07-11-2022
          public async Task CommitAsync()
          {
               await Transaction.CommitAsync();
          }

          /// <summary>
          /// Rollback transaction
          /// </summary>
          /// NXTSAN 07-11-2022
          public async Task RollbackAsync()
          {
               await Transaction.RollbackAsync();
          }

          /// <summary>
          /// Dispose transaction
          /// </summary>
          /// NXTSAN 07-11-2022
          public void Dispose()
          {
               if(Transaction != null) Transaction.Dispose();

               Transaction = null;
          }
 
          #endregion

     }
}
