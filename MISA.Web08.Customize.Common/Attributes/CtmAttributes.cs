using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace MISA.Web08.Customize.Common.Attributes
{
     /// <summary>
     /// Attribute dùng để xác định 1 property là khoá chính
     /// </summary>
     /// NXTSAN 23-09-2022
     [AttributeUsage(AttributeTargets.Property)]
     public class PrimaryKeyAttribute : Attribute
     {
          #region Field

          public string ErrorMessage { get; set; }

          #endregion

          #region Constructor

          public PrimaryKeyAttribute()
          {

          }

          public PrimaryKeyAttribute(string errorMessage)
          {
               ErrorMessage = errorMessage;
          }

          #endregion

     }

     /// <summary>
     /// Attribute dùng để xác định 1 property không null hoặc rỗng
     /// </summary>
     /// NXTSAN 23-09-2022
     [AttributeUsage(AttributeTargets.Property)]
     public class IsNotNullOrEmptyAttribute : Attribute
     {

          #region Field

          public string ErrorMessage { get; set; }

          #endregion

          #region Constructor

          public IsNotNullOrEmptyAttribute(string errorMessage)
          {
               ErrorMessage = errorMessage;
          }

          #endregion

     }

     /// <summary>
     /// Attribute dùng để xác định 1 có format hợp lệ hay không
     /// </summary>
     /// NXTSAN 06-10-2022
     [AttributeUsage(AttributeTargets.Property)]
     public class FormatAttribute : Attribute
     {

          #region Field

          public string Format { get; set; }
          public string ErrorMessage { get; set; }

          #endregion

          #region Constructor

          public FormatAttribute(string format, string errorMessage)
          {
               Format = format;
               ErrorMessage = errorMessage;
          }

          #endregion

     }

     /// <summary>
     /// Attribute dùng để giới hạn ngày
     /// </summary>
     /// NXTSAN 06-10-2022
     [AttributeUsage(AttributeTargets.Property)]
     public class IsDateOfBirthAttribute : Attribute
     {

          #region Field

          public string ErrorMessage { get; set; }

          #endregion

          #region Constructor

          public IsDateOfBirthAttribute(string errorMessage)
          {
               ErrorMessage = errorMessage;
          }

          #endregion

     }

     /// <summary>
     /// Attribute dùng để kiểm tra email hợp lệ hay không
     /// </summary>
     /// NXTSAN 06-10-2022
     [AttributeUsage(AttributeTargets.Property)]
     public class IsEmailAttribute : Attribute
     {

          #region Field

          public string ErrorMessage { get; set; }

          #endregion

          #region Constructor

          public IsEmailAttribute(string errorMessage)
          {
               ErrorMessage = errorMessage;
          }

          #endregion

     }

     /// <summary>
     /// Attribute dùng để kiểm tra độ dài hợp lệ hay không
     /// </summary>
     /// NXTSAN 06-10-2022
     [AttributeUsage(AttributeTargets.Property)]
     public class MaxLengthAttribute : Attribute
     {

          #region Field

          public string ErrorMessage { get; set; }

          public int MaxLength { get; set; }

          #endregion

          #region Constructor

          public MaxLengthAttribute(int maxLength, string errorMessage)
          {
               ErrorMessage = errorMessage;
               MaxLength = maxLength;
          }

          #endregion

     }

     [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
     public class DisableFormValueModelBindingAttribute : Attribute, IResourceFilter
     {
          public void OnResourceExecuting(ResourceExecutingContext context)
          {
               var factories = context.ValueProviderFactories;
               factories.RemoveType<FormValueProviderFactory>();
               factories.RemoveType<JQueryFormValueProviderFactory>();
          }

          public void OnResourceExecuted(ResourceExecutedContext context)
          {
          }
     }
}
