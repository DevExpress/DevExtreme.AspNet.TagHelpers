using DevExtreme.AspNet.TagHelpers.Data;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace DevExtreme.AspNet.TagHelpers.Tests {

    public class DatasourceTests {

        [Fact]
        public void DatasourceDoesNotAddEmptyConfig() {
            var page = new PivotDatasourcePage();
            page.DataSources.Add(new TestStoreDatasourceTagHelper());
            page.DataSources.Add(new PivotGridDatasourceTagHelper());

            page.ExecuteSynchronously();

            var dataSourceConfig = page.TopLevelTag.GetConfigValue<IDictionary<string, object>>("dataSource");
            Assert.Null(dataSourceConfig);
        }

        [Fact]
        public void PivotGridDatasourceExtendsStoreDatasource() {
            var page = new PivotDatasourcePage();
            page.DataSources.Add(new PivotGridDatasourceTagHelper { OnChanged = "any" });
            page.DataSources.Add(new TestStoreDatasourceTagHelper { DatasourceKey = "any" });

            page.ExecuteSynchronously();

            var dataSourceConfig = page.TopLevelTag.GetConfigValue<IDictionary<string, object>>("dataSource");
            Assert.True(dataSourceConfig.ContainsKey("OnChanged"));
            Assert.True(dataSourceConfig.ContainsKey("store"));
        }

        [Fact]
        public void PivotGridDatasourceCreatesConfig() {
            var page = new PivotDatasourcePage();
            page.DataSources.Add(new PivotGridDatasourceTagHelper { OnChanged = "any" });

            page.ExecuteSynchronously();

            var dataSourceConfig = page.TopLevelTag.GetConfigValue<IDictionary<string, object>>("dataSource");
            Assert.True(dataSourceConfig.ContainsKey("OnChanged"));
        }

        [Fact]
        public void ItemsDatasourceDoesNotChangeCase() {
            var page = new PivotDatasourcePage();
            page.DataSources.Add(new ItemsDatasourceTagHelper {
                Items = new[] {
                    new {
                        CamelCaseName = "Value1",
                        lowerCamelCaseName = "value2"
                    }
                }
            });

            page.ExecuteSynchronously();

            var dataSourceConfig = page.TopLevelTag.GetConfigValue<IDictionary<string, object>>("dataSource");

            AssertIgnoreWhitespaces.Contains(
                @"""data"": [
                    {
                        ""CamelCaseName"": ""Value1"",
                        ""lowerCamelCaseName"": ""value2""
                    }
                ]",
                JsonConvert.SerializeObject(dataSourceConfig["store"])
            );
        }

        class TestStoreDatasourceTagHelper : StoreDatasourceTagHelper {
            protected override string FormatStoreFactory(string args) {
                return null;
            }

            protected override void PopulateStoreConfig(IDictionary<string, object> config) {
            }
        }

        class TestParentTagHelper : HierarchicalTagHelper {
            protected override string Key {
                get { return "any"; }
            }

            protected override string FullKey {
                get { return "any"; }
            }

            protected internal override bool ValidateParentFullKey(string parentFullKey) {
                return true;
            }
        }

        class PivotDatasourcePage : TestPage {

            public readonly IList<ITagHelper> DataSources = new List<ITagHelper>();

            protected override void ExecuteCore() {
                Add(new TestParentTagHelper(), delegate {
                    Add(DataSources.ToArray());
                });
            }
        }
    }

}
