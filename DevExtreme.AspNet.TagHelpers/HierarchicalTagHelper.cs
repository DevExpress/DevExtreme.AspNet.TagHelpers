using Microsoft.AspNet.Razor.TagHelpers;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevExtreme.AspNet.TagHelpers {

    public abstract class HierarchicalTagHelper : TagHelper {
        protected readonly IDictionary<string, object> Config = new Dictionary<string, object>();

        protected abstract string Key { get; }

#warning see https://github.com/aspnet/Razor/issues/576
        protected abstract string FullKey { get; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output) {
            var parentConfig = context.GetParentConfig();
            var parentFullKey = context.GetParentFullKey();

            if(!ValidateParentFullKey(parentFullKey))
                return;

            try {
                context.SetParentConfig(Config);
                context.SetParentFullKey(FullKey);
                await output.GetChildContentAsync();
            } finally {
                context.SetParentConfig(parentConfig);
                context.SetParentFullKey(parentFullKey);
            }

            if(parentConfig != null)
                AppendConfig(parentConfig);
        }

        protected internal virtual bool ValidateParentFullKey(string parentFullKey) {
            if(parentFullKey == null)
                return false;

            var keyParts = FullKey.Split('.');
            var parentKeyParts = parentFullKey.Split('.');
            return keyParts.Take(keyParts.Length - 1).SequenceEqual(parentKeyParts);
        }

        protected internal virtual void AppendConfig(IDictionary<string, object> parentConfig) {
            if(Config.Count > 0)
                parentConfig[Key] = Config;
        }

        protected internal T GetConfigValue<T>(string key) {
            if(!Config.ContainsKey(key))
                return default(T);

            var jRaw = Config[key] as JRaw;

            if(jRaw != null)
                return (T)jRaw.Value;

            return (T)Config[key];
        }

        protected internal void SetConfigValue(string key, object value, bool isRaw = false) {
            if(isRaw)
                value = new JRaw(value);

            Config[key] = value;
        }
    }

}
