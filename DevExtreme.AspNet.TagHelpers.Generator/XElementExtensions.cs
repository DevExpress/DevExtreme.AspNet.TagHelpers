using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DevExtreme.AspNet.TagHelpers.Generator {

    static class XElementExtensions {

        public static string GetName(this XElement el) {
            return el.Attribute("Name").Value;
        }

        public static void SetName(this XElement el, string name) {
            el.Attribute("Name").SetValue(name);
        }

        public static string GetRawType(this XElement el) {
            return el.Attribute("Type").Value;
        }

        public static bool IsChildTagElement(this XElement el) {
            return el.Attribute("{http://www.w3.org/2001/XMLSchema-instance}type")?.Value == "IntellisenseObjectInfo";
        }

        public static string GetDescription(this XElement el) {
            return el.Attribute("Description")?.Value;
        }

        public static XElement GetPropElement(this XElement el, string propName) {
            return el
                .Element("Properties")
                .Elements("IntellisenseObjectPropertyInfo")
                .FirstOrDefault(e => propName.Equals(GetName(e), StringComparison.OrdinalIgnoreCase));
        }
    }

}
