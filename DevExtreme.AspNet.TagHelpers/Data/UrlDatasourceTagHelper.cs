using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevExtreme.AspNet.TagHelpers.Data {

    public partial class UrlDatasourceTagHelper : StoreDatasourceTagHelper {
        IUrlHelperFactory _urlHelperFactory;

        public UrlDatasourceTagHelper(IUrlHelperFactory urlHelper) {
            _urlHelperFactory = urlHelper;
        }

        public string Url { get; set; }

        [ViewContext, HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        protected override string FormatStoreFactory(string args) {
            return "new DevExpress.data.CustomStore(" + args + ")";
        }

        protected override void PopulateStoreConfig(IDictionary<string, object> config) {
            var urlHelper = _urlHelperFactory.GetUrlHelper(ViewContext);
            config["load"] = new JRaw("function() { return $.getJSON(" + JsonConvert.SerializeObject(urlHelper.Content(Url)) + "); }");
        }

    }

}
