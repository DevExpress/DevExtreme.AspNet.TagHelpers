using Microsoft.AspNet.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevExtreme.AspNet.TagHelpers {

    static class TagHelperContextExtensions {
        const string
            PARENT_CONFIG_KEY = "devextreme-parent-config",
            PARENT_FULLKEY_KEY = "devextreme-parent-fullkey",
            INNER_SCRIPTS_KEY = "devextreme-inner-scripts";

        public static IDictionary<string, object> GetParentConfig(this TagHelperContext context) {
            return context.GetItem<IDictionary<string, object>>(PARENT_CONFIG_KEY);
        }

        public static string GetParentFullKey(this TagHelperContext context) {
            return context.GetItem<string>(PARENT_FULLKEY_KEY);
        }

        public static ICollection<string> GetInnerScripts(this TagHelperContext context) {
            return context.GetItem<ICollection<string>>(INNER_SCRIPTS_KEY);
        }

        public static void SetParentConfig(this TagHelperContext context, IDictionary<string, object> config) {
            context.Items[PARENT_CONFIG_KEY] = config;
        }

        public static void SetParentFullKey(this TagHelperContext context, string fullKey) {
            context.Items[PARENT_FULLKEY_KEY] = fullKey;
        }

        public static void SetInnerScripts(this TagHelperContext context, ICollection<string> scripts) {
            context.Items[INNER_SCRIPTS_KEY] = scripts;
        }

        static T GetItem<T>(this TagHelperContext context, string key) {
            return context.Items.ContainsKey(key) ? (T)context.Items[key] : default(T);
        }
    }

}
