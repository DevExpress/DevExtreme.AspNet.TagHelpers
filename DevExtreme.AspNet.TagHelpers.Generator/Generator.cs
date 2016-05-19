using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DevExtreme.AspNet.TagHelpers.Generator {

    class Generator {
        static readonly string[] DEFAULT_USINGS = new[] {
            "System",
            "System.Collections.Generic",
            "Microsoft.AspNetCore.Razor.TagHelpers"
        };

        string _outputRoot;

        public Generator(string outputRoot) {
            _outputRoot = outputRoot;
        }

        public void GenerateClass(TagInfo tag, string customClassName = null, bool isPartial = false, bool generateKeyProps = true) {
            var childTags = new List<string>();
            foreach(var child in tag.GenerateChildTags()) {
                childTags.Add(child.GetTagName());
                GenerateClass(child);
            }

            var childRestrictions = tag.GetChildRestrictions(childTags);
            var className = customClassName ?? tag.GetClassName();

            var builder = new ClassBuilder();

            builder.AppendHeader();
            builder.AppendUsings(DEFAULT_USINGS);
            builder.StartNamespaceBlock(tag.Namespace);

            builder.AppendSummary(tag.GetSummaryText());

            if(!isPartial)
                builder.AppendGeneratedAttribute();

            builder.AppendHtmlTargetAttribute(new TargetElementInfo {
                Tag = tag.GetTagName(),
                ParentTag = tag.ParentTagName,
                IsSelfClosing = !childRestrictions.Any()
            });

            if(childRestrictions.Any())
                builder.AppendAttribute("RestrictChildren", childRestrictions.Select(r => '"' + r + '"'));

            builder.StartClass(className, tag.BaseClassName, isPartial);
            builder.AppendEmptyLine();

            if(generateKeyProps) {
                builder.AppendKeyProperty("Key", tag.Key);
                builder.AppendKeyProperty("FullKey", tag.GetFullKey());
            }

            foreach(var prop in tag.GenerateProperties()) {
                CompetitivePropsRegistry.Register(tag.GetFullKey() + "." + prop.GetName(), prop.GetClrType());
                builder.AppendProp(prop);
            }

            builder.EndBlock();
            builder.AppendEmptyLine();
            builder.EndBlock();

            WriteToFile(tag.Namespace, className, builder.ToString());
        }

        public void GenerateTargetElementsClass(IEnumerable<string> ns, string className, IEnumerable<TargetElementInfo> targets) {
            var builder = new ClassBuilder();

            builder.AppendHeader();
            builder.AppendUsings(DEFAULT_USINGS);
            builder.StartNamespaceBlock(ns);

            foreach(var target in targets)
                builder.AppendHtmlTargetAttribute(target);

            builder.StartClass(className, baseClassName: null, isPartial: true);
            builder.EndBlock();
            builder.AppendEmptyLine();
            builder.EndBlock();

            WriteToFile(ns, className, builder.ToString());
        }

        public void GenerateEnums(IEnumerable<string> ns, string className, IDictionary<string, EnumInfo> knownEnums) {
            var builder = new ClassBuilder();

            builder.AppendHeader();
            builder.AppendUsings(new[] {
                "Newtonsoft.Json",
                "Newtonsoft.Json.Converters",
                "System.Runtime.Serialization",
            });
            builder.StartNamespaceBlock(ns);

            foreach(var entry in knownEnums) {
                builder.AppendAttribute("JsonConverter", "typeof(StringEnumConverter)");
                builder.Append($"public enum {entry.Key} ");
                builder.StartBlock();

                foreach(var item in entry.Value.EnumerateForRendering()) {
                    if(!item.IsFirst)
                        builder.AppendLine(",");

                    builder.AppendAttribute("EnumMember", $"Value = \"{item.JavaScriptValue}\"");
                    builder.Append($"{item.CSharpValue}");
                }

                builder.AppendEmptyLine();
                builder.EndBlock();
                builder.AppendEmptyLine();
            }

            builder.EndBlock();

            WriteToFile(ns, className, builder.ToString());
        }

        public void DeleteGeneratedFiles(string[] ns) {
            var dirName = Path.Combine(_outputRoot, Path.Combine(ns));

            foreach(var path in Directory.GetFiles(dirName, "*.g.cs", SearchOption.AllDirectories))
                File.Delete(path);
        }

        void WriteToFile(IEnumerable<string> ns, string className, string text) {
            var path = Path.Combine(_outputRoot, Path.Combine(ns.ToArray()));

            if(!Directory.Exists(path))
                Directory.CreateDirectory(path);

            File.WriteAllText(Path.Combine(path, className + ".g.cs"), text);
        }
    }

}
