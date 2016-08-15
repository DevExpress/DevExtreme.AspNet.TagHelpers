using System;
using System.Linq;
using System.Threading.Tasks;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DevExtreme.AspNet.TagHelpers
{
    [ModelBinder(BinderType = typeof(DataSourceLoadOptionsBinder))]
    public class DataSourceLoadOptions : DataSourceLoadOptionsBase
    {
    }

    public class DataSourceLoadOptionsBinder : IModelBinder
    {

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var loadOptions = new DataSourceLoadOptions();
            DataSourceLoadOptionsParser.Parse(loadOptions, key => bindingContext.ValueProvider.GetValue(key).FirstOrDefault());
            bindingContext.Result = ModelBindingResult.Success(loadOptions);
            return null;
        }
    }

    [Obsolete("This attribute is now obsolete and is no longer used. It was used as a workaround for the issue discussed at https://github.com/aspnet/Mvc/issues/4652")]
    public class DataSourceLoadOptionsAttribute : ModelBinderAttribute
    {

        public DataSourceLoadOptionsAttribute()
        {
            BinderType = typeof(DataSourceLoadOptionsBinder);
        }
    }
}
