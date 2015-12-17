using Microsoft.AspNet.Razor.TagHelpers;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DevExtreme.AspNet.TagHelpers.Data {

    public abstract class StoreDatasourceTagHelper : DatasourceTagHelper {

        [HtmlAttributeName("key")]
        public string DatasourceKey { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output) {
            var store = new Dictionary<string, object>();

            var key = ExpandCompoundKey(DatasourceKey);
            if(key != null)
                store["Key"] = key;

            PopulateStoreConfig(store);
            if(store.Count > 0)
                Config["store"] = new JRaw(FormatStoreFactory(Utils.SerializeConfig(store)));

            await base.ProcessAsync(context, output);
        }

        protected abstract string FormatStoreFactory(string args);

        protected abstract void PopulateStoreConfig(IDictionary<string, object> config);

        static object ExpandCompoundKey(string key) {
            if(String.IsNullOrEmpty(key))
                return null;

            var components = Regex.Split(key, "\\s*[;,|]\\s*");
            if(components.Length < 2)
                return components[0];

            return components;
        }
    }

}
