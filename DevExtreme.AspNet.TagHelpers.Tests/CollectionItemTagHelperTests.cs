using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace DevExtreme.AspNet.TagHelpers.Tests {

    public class CollectionItemTagHelperTests {

        class TestCollectionItemTagHelper : CollectionItemTagHelper {
            string _key;

            public TestCollectionItemTagHelper(string key) {
                _key = key;
            }

            protected override string FullKey {
                get { throw new NotImplementedException(); }
            }

            protected override string Key {
                get { return _key; }
            }

            public TestCollectionItemTagHelper EnsureConfigNotEmpty() {
                SetConfigValue("any", "any");
                return this;
            }

        }

        [Fact]
        public void CreatesListOnAppend() {
            var tagHelper = new TestCollectionItemTagHelper("key1");
            var parentConfig = new Dictionary<string, object>();

            tagHelper.EnsureConfigNotEmpty();
            tagHelper.AppendConfig(parentConfig);

            Assert.IsAssignableFrom(typeof(IList), parentConfig["key1"]);
        }

        [Fact]
        public void DoesNotOverrideCreatedList() {
            var tagHelper1 = new TestCollectionItemTagHelper("key1");
            var tagHelper2 = new TestCollectionItemTagHelper("key1");
            var parentConfig = new Dictionary<string, object>();

            tagHelper1.EnsureConfigNotEmpty();
            tagHelper1.AppendConfig(parentConfig);

            var collection = parentConfig["key1"] as IList<object>;
            var item1 = collection[0];

            tagHelper2.EnsureConfigNotEmpty();
            tagHelper2.AppendConfig(parentConfig);

            Assert.Equal(2, collection.Count);
            Assert.Contains(item1, collection);
        }

        [Fact]
        public void IgnoresEmptyItems() {
            var parentConfig = new Dictionary<string, object>();
            new TestCollectionItemTagHelper("key1").AppendConfig(parentConfig);

            Assert.False(parentConfig.ContainsKey("key1"));

            new TestCollectionItemTagHelper("key1").EnsureConfigNotEmpty().AppendConfig(parentConfig);
            new TestCollectionItemTagHelper("key1").AppendConfig(parentConfig);

            Assert.Equal(1, (parentConfig["key1"] as IList).Count);
        }
    }

}
