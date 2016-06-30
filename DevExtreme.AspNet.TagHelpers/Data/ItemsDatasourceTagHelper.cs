using Microsoft.AspNetCore.Mvc.Formatters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DevExtreme.AspNet.TagHelpers.Data {

    public partial class ItemsDatasourceTagHelper : StoreDatasourceTagHelper {
        JsonOutputFormatter _jsonFormatter;

        public ItemsDatasourceTagHelper(JsonOutputFormatter jsonFormatter) {
            _jsonFormatter = jsonFormatter;
        }

        public IEnumerable Items { get; set; }

        protected override string FormatStoreFactory(string args) {
            return "new DevExpress.data.ArrayStore(" + args + ")";
        }

        protected override void PopulateStoreConfig(IDictionary<string, object> config) {
            using(var writer = new StringWriter()) {
                _jsonFormatter.WriteObject(writer, Items);
                config["data"] = new JRaw(writer.ToString());
            }
        }
    }

}
