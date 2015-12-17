using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevExtreme.AspNet.TagHelpers {

    public abstract class CollectionItemTagHelper : HierarchicalTagHelper {

        protected internal override void AppendConfig(IDictionary<string, object> parentConfig) {
            if(Config.Count < 1)
                return;

            if(!parentConfig.ContainsKey(Key))
                parentConfig[Key] = new List<object>();

            var collectionConfig = (ICollection<object>)parentConfig[Key];
            collectionConfig.Add(Config);
        }
    }

}
