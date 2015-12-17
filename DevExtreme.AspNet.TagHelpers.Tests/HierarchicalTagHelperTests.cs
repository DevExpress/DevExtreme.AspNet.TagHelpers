using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace DevExtreme.AspNet.TagHelpers.Tests {

    public class HierarchicalTagHelperTests {

        class HierarchyTestPage : TestPage {

            protected override void ExecuteCore() {
                var child = new TestTagHelper("child");
                child.SetConfigValue("childProp", "yes");

                var grandChild = new TestTagHelper("grandChild");
                grandChild.SetConfigValue("grandChildProp", "yes");

                Add(new TestTagHelper("parent"), delegate {
                    Add(child, delegate { Add(grandChild); });
                });
            }
        }

        class ParentValidationTestPage : TestPage {

            protected override void ExecuteCore() {
                Add(new TestTagHelper("parent"), delegate {
                    Add(new TestTagHelper("invalid", false).EnsureConfigNotEmpty());
                });
            }
        }

        [Fact]
        public void ProvidesConfigAndFullKeyToChildren() {
            var page = new HierarchyTestPage();
            page.ExecuteSynchronously();

            var child = page.TopLevelTag.GetConfigValue<IDictionary<string, object>>("child");
            Assert.Equal("yes", child["childProp"]);

            var grandChild = child["grandChild"] as IDictionary<string, object>;
            Assert.Equal("yes", grandChild["grandChildProp"]);
        }

        [Fact]
        public void AppendsConfigByKey() {
            var tagHelper = new TestTagHelper("key1");
            var parentConfig = new Dictionary<string, object>();

            tagHelper.EnsureConfigNotEmpty();
            tagHelper.AppendConfig(parentConfig);

            Assert.True(parentConfig.ContainsKey("key1"));
        }

        [Fact]
        public void DoesNotAppendEmptyConfig() {
            var tagHelper = new TestTagHelper("key1");
            var parentConfig = new Dictionary<string, object>();

            tagHelper.AppendConfig(parentConfig);

            Assert.Equal(0, parentConfig.Count);
        }

        [Fact]
        public void ReturnsDefaultIfValueNotSet() {
            Assert.Equal(0, new TestTagHelper("any").GetConfigValue<int>("any"));
        }

        [Fact]
        public void ReturnsRawValue() {
            var tagHelper = new TestTagHelper("any");
            tagHelper.SetConfigValue("ABC", "OnClick", isRaw: true);

            Assert.Equal("OnClick", tagHelper.GetConfigValue<string>("ABC"));
        }

        [Fact]
        public void DoesNotUpdateConfigIfParentIsWrong() {
            var page = new ParentValidationTestPage();
            page.ExecuteSynchronously();
            Assert.Null(page.TopLevelTag.GetConfigValue<object>("invalid"));
        }

        class TestTagHelper : HierarchicalTagHelper {
            string _key;
            bool _isValid;

            public TestTagHelper(string key, bool isValid = true) {
                _key = key;
                _isValid = isValid;
            }

            protected override string Key {
                get { return _key; }
            }

            protected override string FullKey {
                get { return null; }
            }

            public TestTagHelper EnsureConfigNotEmpty() {
                SetConfigValue("any", "any");
                return this;
            }

            protected internal override bool ValidateParentFullKey(string parentFullKey) {
                return _isValid;
            }
        }
    }

}
