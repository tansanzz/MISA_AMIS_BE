using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Web08.Customize.DL
{
     public interface IUnitOfWork : IDisposable
     {
          /// <summary>
          /// Kết nối
          /// </summary>
          /// NXTSAN 07-11-2022
          public DbConnection Connection { get; }

          /// <summary>
          /// Transaction
          /// </summary>
          /// NXTSAN 07-11-2022
          public DbTransaction Transaction { get; }

          /// <summary>
          /// Begin
          /// </summary>
          /// NXTSAN 07-11-2022
          public Task BeginAsync();

          /// <summary>
          /// Commit
          /// </summary>
          /// NXTSAN 07-11-2022
          public Task CommitAsync();

          /// <summary>
          /// Rollback
          /// </summary>
          /// NXTSAN 07-11-2022
          public Task RollbackAsync();
     }
}
