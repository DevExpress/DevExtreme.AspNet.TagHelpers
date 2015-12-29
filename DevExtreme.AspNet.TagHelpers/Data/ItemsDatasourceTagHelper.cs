using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevExtreme.AspNet.TagHelpers.Data {

    public partial class ItemsDatasourceTagHelper : StoreDatasourceTagHelper {

        public IEnumerable Items { get; set; }

        protected override string FormatStoreFactory(string args) {
            return "new DevExpress.data.ArrayStore(" + args + ")";
        }

        protected override void PopulateStoreConfig(IDictionary<string, object> config) {
            config["data"] = new JRaw(JsonConvert.SerializeObject(Items));
        }
    }

}
