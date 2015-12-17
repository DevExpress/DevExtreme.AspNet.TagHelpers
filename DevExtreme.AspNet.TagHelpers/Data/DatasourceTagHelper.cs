using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevExtreme.AspNet.TagHelpers.Data {

    public abstract class DatasourceTagHelper : HierarchicalTagHelper {

        protected sealed override string Key {
            get { return "dataSource"; }
        }

        protected sealed override string FullKey {
            get { return "DevExtreme.AspNet.TagHelpers.Data.Datasource"; }
        }

        protected internal override bool ValidateParentFullKey(string parentFullKey) {
            return parentFullKey != null;
        }
    }

}
