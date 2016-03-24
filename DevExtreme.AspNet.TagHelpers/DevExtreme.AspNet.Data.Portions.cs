using DevExtreme.AspNet.Data.Helpers;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.ModelBinding;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevExtreme.AspNet.Data {

    [ModelBinder(BinderType = typeof(DataSourceLoadOptionsBinder))]
    public class DataSourceLoadOptions : DataSourceLoadOptionsBase {

        static DataSourceLoadOptions() {
            Compat.EF3361 = true;
        }

    }

    public class DataSourceLoadOptionsBinder : IModelBinder {

        public Task<ModelBindingResult> BindModelAsync(ModelBindingContext bindingContext) {
            var loadOptions = new DataSourceLoadOptions();
            DataSourceLoadOptionsParser.Parse(loadOptions, key => bindingContext.ValueProvider.GetValue(key).FirstOrDefault());
            return ModelBindingResult.SuccessAsync(bindingContext.ModelName, loadOptions);
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
            ContentType = new MediaTypeHeaderValue("application/json");
            Content = JsonConvert.SerializeObject(DataSourceLoader.Load(source, loadOptions));
        }

    }


}
