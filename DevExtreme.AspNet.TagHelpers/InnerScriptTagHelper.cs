using Microsoft.AspNet.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevExtreme.AspNet.TagHelpers {

    public partial class InnerScriptTagHelper : TagHelper {

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output) {
            var content = await output.GetChildContentAsync();

            context.GetInnerScripts().Add(content.GetContent());
        }
    }

}
