using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevExtreme.AspNet.TagHelpers.Generator {

    static class TargetElementsRegistry {
        public readonly static ICollection<string> InnerScriptTargets = new List<string>();
        public readonly static ICollection<string> DatasourceTargets = new List<string>();
    }

}
