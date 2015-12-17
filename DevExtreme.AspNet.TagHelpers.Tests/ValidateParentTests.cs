using DevExtreme.AspNet.TagHelpers.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace DevExtreme.AspNet.TagHelpers.Tests {

    public class ValidateParentTests {

        class SampleInnerTag : HierarchicalTagHelper {
            string _fullKey;

            public SampleInnerTag(string fullKey) {
                _fullKey = fullKey;
            }

            protected override string Key {
                get { throw new NotImplementedException(); }
            }

            protected override string FullKey {
                get { return _fullKey; }
            }
        }

        class SampleWidgetTag : WidgetTagHelper {
            string _fullKey;

            public SampleWidgetTag(string fullKey) {
                _fullKey = fullKey;
            }

            protected override string Key {
                get { throw new NotImplementedException(); }
            }

            protected override string FullKey {
                get { return _fullKey; }
            }
        }

        class SampleDatasourceTag : DatasourceTagHelper { }

        [Fact]
        public void SameBranch_ImmediateChild() {
            var tag = new SampleInnerTag("MyTags.Parent.Child");
            Assert.True(tag.ValidateParentFullKey("MyTags.Parent"));
        }

        [Fact]
        public void SameBranch_SkippedChildren() {
            var tag = new SampleInnerTag("MyTags.Parent.Child.Grandchild");
            Assert.False(tag.ValidateParentFullKey("MyTags.Parent"));
        }

        [Fact]
        public void DifferentBranches() {
            var tag = new SampleInnerTag("MyTags.Parent1.Child.Grandchild");
            Assert.False(tag.ValidateParentFullKey("MyTags.Parent2.Child"));
        }

        [Fact]
        public void OrphanChild() {
            var tag = new SampleInnerTag("MyTags.Parent.Child");
            Assert.False(tag.ValidateParentFullKey(null));
        }

        [Fact]
        public void WidgetValidationSucceedsIfNoParent() {
            var tag = new SampleWidgetTag("MyTags.Grid");
            Assert.True(tag.ValidateParentFullKey(null));
        }

        [Fact]
        public void WidgetValidationFailsIfParentExists() {
            var tag = new SampleWidgetTag("MyTags.Grid");
            Assert.False(tag.ValidateParentFullKey("MyTags.Chart.Label"));
        }

        [Fact]
        public void DatasourceIsValidEverywhere() {
            var tag = new SampleDatasourceTag();
            Assert.True(tag.ValidateParentFullKey("MyTags.Everywhere"));
            Assert.False(tag.ValidateParentFullKey(null));
        }
    }

}
