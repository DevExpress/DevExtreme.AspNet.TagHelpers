using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevExtreme.AspNet.TagHelpers.Generator {

    public static class EnumerableExtensions {

        public static IEnumerable<T> Concat<T>(this IEnumerable<T> items, T item) {
            return items.Concat(new[] { item });
        }
    }

}
