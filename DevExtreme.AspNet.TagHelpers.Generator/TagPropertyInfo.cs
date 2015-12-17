using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DevExtreme.AspNet.TagHelpers.Generator {

    class TagPropertyInfo {
        XElement _element;

        public TagPropertyInfo(XElement element) {
            _element = element;
        }

        public string GetName() {
            return Utils.ToCamelCase(_element.GetName());
        }

        public string GetSummaryText() {
            return Utils.NormalizeDescription(_element.GetDescription());
        }

        public string GetClrType() {
            var clrOverride = _element.Attribute(PropTypeRegistry.CLR_TYPE_OVERRIDE_ATTR)?.Value;
            if(!String.IsNullOrEmpty(clrOverride))
                return clrOverride;

            var rawTypes = _element.GetRawType().Split('|');
            var name = GetName();

            bool
                canBeString = rawTypes.Any(t => t == "string"),
                canBeNumber = rawTypes.Any(t => t == "number" || t == "numeric"),
                canBeBool = rawTypes.Any(t => t == "bool" || t == "boolean"),
                canDeDate = rawTypes.Any(t => t == "date"),
                canBeFunc = rawTypes.Any(t => t.StartsWith("function")),
                canBeJQuery = rawTypes.Any(t => t == "jquery"),
                canBeAny = rawTypes.Any(t => t == "any");

            if(rawTypes.Length == 1) {
                if(canBeString)
                    return PropTypeRegistry.CLR_STRING;

                if(canBeBool)
                    return PropTypeRegistry.CLR_BOOL;

                if(canBeNumber)
                    return PropTypeRegistry.CLR_DOUBLE;

                if(canBeFunc)
                    return PropTypeRegistry.SPECIAL_RAW_STRING;

                if(canBeAny)
                    return PropTypeRegistry.CLR_OBJECT;

                if(canDeDate)
                    return PropTypeRegistry.CLR_DATE;
            }

            if(rawTypes.Length == 2) {
                if(canBeString && canBeNumber)
                    return PropTypeRegistry.CLR_STRING;

                if(canBeFunc && canBeString && Regex.IsMatch(name, "^On[A-Z]"))
                    return PropTypeRegistry.SPECIAL_RAW_STRING;
            }

            if(canBeFunc && canBeJQuery)
                return PropTypeRegistry.SPECIAL_DOM_TEMPLATE;

            throw new Exception("Unable to resolve property type");
        }

        public string GetCustomAttrName() {
            var name = GetName();
            if(Regex.IsMatch(name, "^Data[A-Z]"))
                return "data" + Utils.ToKebabCase(name.Substring(4));
            return null;
        }

    }

}