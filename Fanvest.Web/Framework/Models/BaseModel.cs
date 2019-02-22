using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
namespace Fanvest.Web.Framework.Models
{
    public partial class BaseModel
    {
        public BaseModel()
        {
            PostInitialize();
            CustomProperties = new Dictionary<string, object>();
        }

        public virtual void BindModel(ModelBindingContext bindingContext)
        {

        }

        protected virtual void PostInitialize()
        {

        }

        public IFormCollection Forms { get; set; }
        public Dictionary<string, object> CustomProperties { get; set; }
    }

    public partial class BaseEntityModel : BaseModel
    {
        public virtual int Id { get; set; }
    }
}