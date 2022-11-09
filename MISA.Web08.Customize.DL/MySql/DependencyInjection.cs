using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using MySqlConnector;

namespace MISA.Web08.Customize.DL.MySql
{

     /// <summary>
     /// Dependency Injection
     /// </summary>
     /// NXTSAN 07-11-2022
     public static class DependencyInjection
    {

          /// <summary>
          /// Inject DB connection và UnitOfWork
          /// </summary>
          /// NXTSAN 07-11-2022
          public static void AddDapperMySql(this IServiceCollection services, string connectionString)
          {
               services.AddScoped<DbConnection>(provider =>
               {
                    return new MySqlConnection(connectionString);
               });

               services.AddScoped<IUnitOfWork, UnitOfWork>();
          }
    }
}
