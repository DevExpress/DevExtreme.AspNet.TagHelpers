using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevExtreme.AspNet.TagHelpers.Data {

    public partial class UrlDatasourceTagHelper : StoreDatasourceTagHelper {
        IUrlHelper _urlHelper;

        public UrlDatasourceTagHelper(IUrlHelper urlHelper) {
            _urlHelper = urlHelper;
        }

        public string Url { get; set; }

        protected override string FormatStoreFactory(string args) {
            return "new DevExpress.data.CustomStore(" + args + ")";
        }

        protected override void PopulateStoreConfig(IDictionary<string, object> config) {
            config["load"] = new JRaw("function() { return $.getJSON(" + JsonConvert.SerializeObject(_urlHelper.Content(Url)) + "); }");
        }

    }

}
