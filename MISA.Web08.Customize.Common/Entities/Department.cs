namespace MISA.Web08.Customize.Common.Entities
{
     /// <summary>
     /// Đơn vị
     /// </summary>
     /// NXTSAN 21-09-2022
     public class Department : BaseEntity
     {
          /// <summary>
          /// ID đơn vị
          /// </summary>
          /// NXTSAN 21-09-2022
          public Guid DepartmentID { get; set; }

          /// <summary>
          /// Mã đơn vị
          /// </summary>
          /// NXTSAN 21-09-2022
          public string DepartmentCode { get; set; }

          /// <summary>
          /// Tên đơn vị
          /// </summary>
          /// NXTSAN 21-09-2022
          public string DepartmentName { get; set; }
    }
}
