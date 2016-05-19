using DevExtreme.AspNet.Data.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevExtreme.AspNet.Data {

    [ModelBinder(BinderType = typeof(DataSourceLoadOptionsBinder))]
    public class DataSourceLoadOptions : DataSourceLoadOptionsBase {
    }

    public class DataSourceLoadOptionsBinder : IModelBinder {

        public Task BindModelAsync(ModelBindingContext bindingContext) {
            var loadOptions = new DataSourceLoadOptions();
            DataSourceLoadOptionsParser.Parse(loadOptions, key => bindingContext.ValueProvider.GetValue(key).FirstOrDefault());
            bindingContext.Result = ModelBindingResult.Success(bindingContext.ModelName, loadOptions);
            return Task.CompletedTask;
        }

    }

    // Temporary workaround for https://github.com/aspnet/Mvc/issues/4652    
    public class DataSourceLoadOptionsAttribute : ModelBinderAttribute {

        public DataSourceLoadOptionsAttribute() {
            BinderType = typeof(DataSourceLoadOptionsBinder);
        }

    }

    [Obsolete("Use DataSourceLoader.Load instead")]
    public static class DataSourceLoadResult {
        public static ContentResult Create<T>(IEnumerable<T> source, DataSourceLoadOptions loadOptions) {
            return new DataSourceLoadResult<T>(source, loadOptions);
        }
    }

    [Obsolete("Use DataSourceLoader.Load instead")]
    public class DataSourceLoadResult<T> : ContentResult {

        public DataSourceLoadResult(IEnumerable<T> source, DataSourceLoadOptions loadOptions) {
            ContentType = "application/json";
            Content = JsonConvert.SerializeObject(DataSourceLoader.Load(source, loadOptions));
        }

    }


}
