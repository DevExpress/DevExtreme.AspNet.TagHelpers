using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevExtreme.AspNet.TagHelpers.Generator {

#warning see https://github.com/aspnet/Razor/issues/570
    class CompetitivePropsRegistry {
        static IDictionary<string, string> _types = new Dictionary<string, string>();
        static IDictionary<string, string> _fullNames = new Dictionary<string, string>();

        public static void Register(string fullName, string type) {
            var nameParts = fullName.Substring("DevExtreme.AspNet.TagHelpers.".Length).Split('.');

            if(nameParts.Length < 3)
                return;

            var tailName = String.Join(".", nameParts.Skip(nameParts.Length - 3));

            Validate(fullName, tailName, type);

            _types[tailName] = type;
            _fullNames[tailName] = fullName;
        }

        static void Validate(string fullName, string tailName, string type) {
            if(_types.ContainsKey(tailName) && _types[tailName] != type) {
                throw new Exception($"Competitive props with different type detected: {_fullNames[tailName]} vs {fullName}");
            }
        }
    }

}
