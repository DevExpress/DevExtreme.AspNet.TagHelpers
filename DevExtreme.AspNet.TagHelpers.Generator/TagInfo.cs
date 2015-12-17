using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DevExtreme.AspNet.TagHelpers.Generator {

    class TagInfo {
        readonly TagInfoPreProcessor _preProcessor;

        public readonly XElement Element;
        public string Key;
        public readonly IEnumerable<string> Namespace;
        public readonly string ParentTagName;
        public string BaseClassName = "HierarchicalTagHelper";
        public readonly List<XElement> ChildTagElements = new List<XElement>();
        public readonly List<XElement> PropElements = new List<XElement>();
        public readonly List<string> ExtraChildRestrictions = new List<string>();

        public TagInfo(XElement element, TagInfoPreProcessor preProcessor, IEnumerable<string> ns, string parentTagName) {
            _preProcessor = preProcessor;
            Element = element;
            Namespace = ns;
            ParentTagName = parentTagName;
            Key = Element.GetName();

            foreach(var prop in element.Element("Properties").Elements("IntellisenseObjectPropertyInfo")) {
                if(prop.IsChildTagElement())
                    ChildTagElements.Add(prop);
                else
                    PropElements.Add(prop);
            }

            preProcessor.Process(this);
        }

        public string GetTagName() {
            return Utils.ToKebabCase(Element.GetName());
        }

        public string GetNamespaceEntry() {
            var elementName = Element.GetName();
            return elementName.StartsWith("dx") ? elementName : Utils.ToCamelCase(elementName);
        }

        public string GetFullKey() {
            return String.Join(".", Namespace) + "." + GetNamespaceEntry();
        }

        public string GetClassName() {
            return GetNamespaceEntry() + "TagHelper";
        }

        public string GetSummaryText() {
            return Utils.NormalizeDescription(Element.GetDescription());
        }

        public TagInfo[] GenerateChildTags() {
            return ChildTagElements
                .Select(el => new TagInfo(el, _preProcessor, Namespace.Concat(GetNamespaceEntry()), GetTagName()))
                .OrderBy(t => t.GetTagName())
                .ToArray();
        }

        public string[] GetChildRestrictions(IEnumerable<string> childTags) {
            return childTags.Concat(ExtraChildRestrictions).OrderBy(t => t).ToArray();
        }

        public TagPropertyInfo[] GenerateProperties() {
            return PropElements
                .Select(el => new TagPropertyInfo(el))
                .OrderBy(p => p.GetName())
                .ToArray();
        }
    }

}
