using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DevExtreme.AspNet.TagHelpers {

    static class Utils {

        public static string WrapDomTemplateValue(string text) {
            if(text != null && text.StartsWith("#"))
                return "$(" + JsonConvert.SerializeObject(text) + ")";

            return text;
        }

        public static string SerializeConfig(object config) {
            return JsonConvert.SerializeObject(config, new JsonSerializerSettings {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
#if DEBUG
                Formatting = Formatting.Indented
#endif
            });
        }

    }

}
