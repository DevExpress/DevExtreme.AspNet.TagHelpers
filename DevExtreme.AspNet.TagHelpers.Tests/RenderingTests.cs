using DevExtreme.AspNet.TagHelpers.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace DevExtreme.AspNet.TagHelpers.Tests {

    public class RenderingTests {
        TestWidgetPage page = new TestWidgetPage();

        [Fact]
        public void Sample1() {
            page.Widget.SetConfigValue("prop1", "value1");
            page.Widget.ID = "id1";

            page.ExecuteSynchronously();

            AssertIgnoreWhitespaces.Equal(
                @"<div id=""id1""></div>
                <script>
                    jQuery(function($) {
                        var options = { ""prop1"": ""value1"" };
                        $(""#id1"").dxTest(options);
                    });
                </script>",
                page.GetOutputText()
            );
        }

        [Fact]
        public void RendersGuidForUnspecifiedId() {
            page.ExecuteSynchronously();

            Assert.Matches("dx-[a-f0-9]{16}", page.GetOutputText());
        }

        [Fact]
        public void RendersRawUnquoted() {
            page.Widget.SetConfigValue("abc", "OnClick", isRaw: true);

            page.ExecuteSynchronously();

            AssertIgnoreWhitespaces.Contains("\"abc\": OnClick", page.GetOutputText());
        }

        [Fact]
        public void RendersLoadActionDatasource_LoadOnly() {
            page.Datasource = new LoadActionDatasourceTagHelper(new UrlHelperMock()) {
                Controller = "Controller1",
                LoadAction = "Action1"
            };

            page.ExecuteSynchronously();

            AssertIgnoreWhitespaces.Contains(
                @"""dataSource"": {
                    ""store"": DevExpress.data.AspNet.createStore({
                        ""loadUrl"": ""ActionMock(Controller1, Action1)""
                    })
                }",
                page.GetOutputText());
        }

        [Fact]
        public void RendersLoadActionDatasource_AllActions() {
            page.Datasource = new LoadActionDatasourceTagHelper(new UrlHelperMock()) {
                Controller = "Controller1",
                UpdateAction = "Action2",
                InsertAction = "Action3",
                DeleteAction = "Action4"
            };

            page.ExecuteSynchronously();

            var markup = page.GetOutputText();

            AssertIgnoreWhitespaces.Contains(@"""updateUrl"": ""ActionMock(Controller1, Action2)""", markup);
            AssertIgnoreWhitespaces.Contains(@"""insertUrl"": ""ActionMock(Controller1, Action3)""", markup);
            AssertIgnoreWhitespaces.Contains(@"""deleteUrl"": ""ActionMock(Controller1, Action4)""", markup);

            AssertIgnoreWhitespaces.DoesNotMatch("(update|insert|delete)Method", markup);
        }

        [Fact]
        public void RendersLoadActionDatasource_AllActionsWithMethods() {
            page.Datasource = new LoadActionDatasourceTagHelper(new UrlHelperMock()) {
                Controller = "Controller1",
                UpdateMethod = "Method1",
                InsertMethod = "Method2",
                DeleteMethod = "Method3"
            };

            page.ExecuteSynchronously();

            var markup = page.GetOutputText();

            AssertIgnoreWhitespaces.Contains(@"""updateMethod"": ""Method1""", markup);
            AssertIgnoreWhitespaces.Contains(@"""insertMethod"": ""Method2""", markup);
            AssertIgnoreWhitespaces.Contains(@"""deleteMethod"": ""Method3""", markup);
        }

        [Fact]
        public void RendersLoadActionDatasource_OnBeforeSend() {
            page.Datasource = new LoadActionDatasourceTagHelper(new UrlHelperMock()) {
                OnBeforeSend = "OnBeforeSendFunc"
            };

            page.ExecuteSynchronously();

            AssertIgnoreWhitespaces.Contains(@"""onBeforeSend"": OnBeforeSendFunc", page.GetOutputText());
        }

        [Fact]
        public void ThrowsIfLoadActionDatasourceActionNotResolved() {
            page.Datasource = new LoadActionDatasourceTagHelper(new UrlHelperMock()) {
                LoadAction = UrlHelperMock.UNRESOLVABLE_ACTION
            };

            var err = Record.Exception(() => page.ExecuteSynchronously());

            Assert.StartsWith("Unable to resolve", err.Message);
        }

        [Theory]
        [InlineData("Field1", @"""Field1""")]
        [InlineData("Field1|Field2", @"[""Field1"",""Field2""]")]
        public void StoreDatasourceRendersKey(string rawKey, string expectedSerializedKey) {
            page.Datasource = new TestStoreDatasourceTagHelper() {
                DatasourceKey = rawKey
            };

            page.ExecuteSynchronously();

            AssertIgnoreWhitespaces.Contains($"\"key\": {expectedSerializedKey}", page.GetOutputText());
        }

        [Fact]
        public void RendersItemsDatasource() {
            page.Datasource = new ItemsDatasourceTagHelper {
                Items = new[] { 123, 456, 789 }
            };

            page.ExecuteSynchronously();

            AssertIgnoreWhitespaces.Contains(
                @"""dataSource"": {
                    ""store"": new DevExpress.data.ArrayStore({
                        ""data"": [ 123, 456, 789 ]
                    })
                }",
                page.GetOutputText());
        }

        [Fact]
        public void RendersUrlDatasource() {
            page.Datasource = new UrlDatasourceTagHelper(new UrlHelperMock()) {
                Url = "LoadUrl1"
            };

            page.ExecuteSynchronously();

            AssertIgnoreWhitespaces.Contains(
                @"""dataSource"": { 
                    ""store"": new DevExpress.data.CustomStore({
                        ""load"": function() { return $.getJSON(""ContentMock(LoadUrl1)""); }
                    })
                }",
                page.GetOutputText());
        }

        [Fact]
        public void RendersInnerScript() {
            page.Widget.ID = "id1";
            page.InnerScript = @"EXPECTED_INNER_SCRIPT";

            page.ExecuteSynchronously();

            AssertIgnoreWhitespaces.Contains(
                @"var options = { };
                  EXPECTED_INNER_SCRIPT
                  $(""#id1"")",
                page.GetOutputText()
            );
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData("#css-id", @"$(""#css-id"")")]
        [InlineData("non-id", "non-id")]
        public void SerializesDomTemplate(string value, string expectedSerializedValue) {
            Assert.Equal(expectedSerializedValue, Utils.WrapDomTemplateValue(value));
        }

        class TestWidget : WidgetTagHelper {

            protected override string Key {
                get { return "dxTest"; }
            }

            protected override string FullKey {
                get { return "dxTest"; }
            }
        }

        class TestWidgetPage : TestPage {
            public readonly TestWidget Widget = new TestWidget();
            public StoreDatasourceTagHelper Datasource;
            public string InnerScript;

            protected override void ExecuteCore() {
                Add(Widget, delegate {
                    if(Datasource != null)
                        Add(Datasource);

                    if(!String.IsNullOrEmpty(InnerScript))
                        Add(new InnerScriptTagHelper(), delegate {
                            Add(new RawStringTagHelper(InnerScript));
                        });
                });
            }
        }

        class RawStringTagHelper : TagHelper {
            string _contet;

            public RawStringTagHelper(string content) {
                _contet = content;
            }

            public override void Process(TagHelperContext context, TagHelperOutput output) {
                output.Content.AppendHtml(_contet);
            }
        }

        class UrlHelperMock : IUrlHelper {
            public const string UNRESOLVABLE_ACTION = "UnresolvableAction";

            public ActionContext ActionContext {
                get { throw new NotImplementedException(); }
            }

            public string Action(UrlActionContext actionContext) {
                if(actionContext.Action == UNRESOLVABLE_ACTION)
                    return null;

                return $"ActionMock({actionContext.Controller}, {actionContext.Action})";
            }

            public string Content(string contentPath) {
                return $"ContentMock({contentPath})";
            }

            public bool IsLocalUrl(string url) {
                throw new NotImplementedException();
            }

            public string Link(string routeName, object values) {
                throw new NotImplementedException();
            }

            public string RouteUrl(UrlRouteContext routeContext) {
                throw new NotImplementedException();
            }
        }

        class TestStoreDatasourceTagHelper : StoreDatasourceTagHelper {

            protected override string FormatStoreFactory(string args) {
                return args;
            }

            protected override void PopulateStoreConfig(IDictionary<string, object> config) {
            }
        }
    }

}
