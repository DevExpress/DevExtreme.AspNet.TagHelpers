using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevExtreme.AspNet.TagHelpers.Generator {

    class ClassBuilder {
        const int TAB_SIZE = 4;
        int _indent;
        bool _isLineStart = true;

        StringBuilder _builder = new StringBuilder();

        public void Append(string text) {
            _builder.Append(AlignText(text));
            _isLineStart = false;
        }

        public void AppendLine(string text) {
            _builder.AppendLine(AlignText(text));
            _isLineStart = true;
        }

        public void AppendEmptyLine() {
            _builder.AppendLine();
            _isLineStart = true;
        }

        string AlignText(string text) {
            if(_isLineStart)
                return new String(' ', TAB_SIZE * _indent) + text;

            return text;
        }

        public void StartBlock() {
            AppendLine("{");
            _indent += 1;
        }

        public void EndBlock() {
            if(_indent > 0)
                _indent -= 1;

            AppendLine("}");
        }

        public void AppendHeader() {
            AppendLine("//  THIS FILE WAS GENERATED AUTOMATICALLY.");
            AppendLine("//  ALL CHANGES WILL BE LOST THE NEXT TIME THE FILE IS GENERATED.");
            AppendEmptyLine();
        }

        public void AppendUsings(IEnumerable<string> usings) {
            foreach(var entry in usings)
                AppendLine($"using {entry};");

            AppendEmptyLine();
        }

        public void StartNamespaceBlock(IEnumerable<string> ns) {
            Append($"namespace {String.Join(".", ns)} ");
            StartBlock();
            AppendEmptyLine();
        }

        public void AppendSummary(string summary) {
            AppendLine($"/// <summary>{summary}</summary>");
        }

        public void StartClass(string className, string baseClassName, bool isPartial) {
            if(isPartial)
                Append("partial ");
            else
                Append("public ");

            Append($"class {className} ");

            if(!String.IsNullOrEmpty(baseClassName))
                Append($": {baseClassName} ");

            StartBlock();
        }

        public void AppendGeneratedAttribute() {
            AppendAttribute("Generated");
        }

        public void AppendHtmlTargetAttribute(TargetElementInfo targetElement) {
            var attributeParams = new List<string> { $"\"{targetElement.Tag}\"" };

            if(!String.IsNullOrEmpty(targetElement.BindingAttribute))
                attributeParams.Add($"Attributes = \"{targetElement.BindingAttribute}\"");

            if(targetElement.IsSelfClosing)
                attributeParams.Add("TagStructure = TagStructure.WithoutEndTag");

            if(!String.IsNullOrEmpty(targetElement.ParentTag))
                attributeParams.Add($"ParentTag = \"{targetElement.ParentTag}\"");

            AppendAttribute("HtmlTargetElement", attributeParams);
        }

        public void AppendAttribute(string attributeName) {
            AppendAttribute(attributeName, Enumerable.Empty<string>());
        }

        public void AppendAttribute(string attributeName, string attributeParam) {
            AppendAttribute(attributeName, new[] { attributeParam });
        }

        public void AppendAttribute(string attributeName, IEnumerable<string> attributeParams) {
            Append($"[{attributeName}");

            if(attributeParams.Any())
                Append($"({String.Join(", ", attributeParams)})");

            AppendLine("]");
        }

        public void AppendKeyProperty(string keyPropName, string key) {
            AppendGeneratedAttribute();
            Append($"protected override string {keyPropName} ");
            StartBlock();

            AppendLine($"get {{ return \"{key}\"; }}");

            EndBlock();
            AppendEmptyLine();
        }

        public void AppendProp(TagPropertyInfo prop) {
            AppendSummary(prop.GetSummaryText());

            var customAttr = prop.GetCustomAttrName();
            if(!String.IsNullOrEmpty(customAttr))
                AppendAttribute("HtmlAttributeName", $"\"{customAttr}\"");

            var name = prop.GetName();
            var dirtyType = prop.GetClrType();
            var type = PropTypeRegistry.StripSpecialType(dirtyType);

            AppendGeneratedAttribute();
            Append($"public {type} {name} ");
            StartBlock();

            AppendLine($"get {{ return GetConfigValue<{type}>(\"{name}\"); }}");
            Append($"set {{ SetConfigValue(\"{name}\", ");

            if(dirtyType == PropTypeRegistry.SPECIAL_DOM_TEMPLATE)
                Append("Utils.WrapDomTemplateValue(value)");
            else
                Append("value");

            if(dirtyType == PropTypeRegistry.SPECIAL_RAW_STRING || dirtyType == PropTypeRegistry.SPECIAL_DOM_TEMPLATE)
                Append(", isRaw: true");

            AppendLine("); }");
            EndBlock();
            AppendEmptyLine();
        }

        public override string ToString() {
            return _builder.ToString();
        }
    }

}
