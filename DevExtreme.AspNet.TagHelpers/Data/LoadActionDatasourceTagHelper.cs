using Microsoft.AspNet.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevExtreme.AspNet.TagHelpers.Data {

    public partial class LoadActionDatasourceTagHelper : StoreDatasourceTagHelper {
        IUrlHelper _urlHelper;

        public LoadActionDatasourceTagHelper(IUrlHelper urlHelper) {
            _urlHelper = urlHelper;
        }

        public string Controller { get; set; }
        public string LoadAction { get; set; }
        public string UpdateAction { get; set; }
        public string UpdateMethod { get; set; }
        public string InsertAction { get; set; }
        public string InsertMethod { get; set; }
        public string DeleteAction { get; set; }
        public string DeleteMethod { get; set; }
        public string OnBeforeSend { get; set; }

        protected override string FormatStoreFactory(string args) {
            return "DevExpress.data.AspNet.createStore(" + args + ")";
        }

        protected override void PopulateStoreConfig(IDictionary<string, object> config) {
            AddAction(config, "load", LoadAction, null);
            AddAction(config, "insert", InsertAction, InsertMethod);
            AddAction(config, "update", UpdateAction, UpdateMethod);
            AddAction(config, "delete", DeleteAction, DeleteMethod);

            if(!String.IsNullOrEmpty(OnBeforeSend))
                config["onBeforeSend"] = new JRaw(OnBeforeSend);
        }


        void AddAction(IDictionary<string, object> config, string name, string action, string method) {
            if(!String.IsNullOrEmpty(action))
                config[name + "Url"] = GetActionUrl(action);

            if(!String.IsNullOrEmpty(method))
                config[name + "Method"] = method;
        }

        string GetActionUrl(string action) {
            var result = _urlHelper.Action(action, Controller);
            if(String.IsNullOrEmpty(result))
                throw new Exception($"Unable to resolve Datasource action: '{Controller}.{action}'");

            return result;
        }
    }

}
