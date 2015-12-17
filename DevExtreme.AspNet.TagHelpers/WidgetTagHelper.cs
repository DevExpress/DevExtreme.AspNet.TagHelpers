using Microsoft.AspNet.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using Newtonsoft.Json;

namespace DevExtreme.AspNet.TagHelpers {

    public abstract class WidgetTagHelper : HierarchicalTagHelper {

        public string ID { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output) {
            var innerScripts = new List<string>();
            context.SetInnerScripts(innerScripts);

            await base.ProcessAsync(context, output);

            var id = GetIDForRendering(ID);
            output.TagName = "div";
            output.Attributes["id"] = id;
            output.Content.Clear();

            output.PostElement.AppendHtml("<script>" + FormatStartupScript(id, innerScripts) + "</script>");
        }

        string FormatStartupScript(string id, IEnumerable<string> innerScripts) {
            return "jQuery(function($) {"
                + "var options = " + Utils.SerializeConfig(Config) + ";"
                + "\n"
                + String.Join("\n", innerScripts.Select(s => s.TrimEnd()))
                + "\n"
                + $"$({JsonConvert.SerializeObject("#" + id)}).{Key}(options);"
                + "});";
        }

        static string GetIDForRendering(string id) {
            if(String.IsNullOrWhiteSpace(id))
                return "dx-" + Guid.NewGuid().ToString("N");

            return id;
        }

        protected internal override bool ValidateParentFullKey(string parentFullKey) {
            return parentFullKey == null;
        }
    }

}
