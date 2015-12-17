using Microsoft.AspNet.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevExtreme.AspNet.TagHelpers.Data {

    public partial class PivotGridDatasourceTagHelper : DatasourceTagHelper {

        public override int Order {
            get { return base.Order + 1; }
        }

        protected internal override void AppendConfig(IDictionary<string, object> parentConfig) {
            if(Config.Count < 1)
                return;

            if(!parentConfig.ContainsKey(Key))
                parentConfig[Key] = new Dictionary<string, object>();

            var sharedConfig = parentConfig[Key] as IDictionary<string, object>;
            foreach(var entry in Config)
                sharedConfig.Add(entry);
        }
    }

}
