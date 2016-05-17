using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Internal;
using Microsoft.AspNetCore.Razor.Runtime.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace DevExtreme.AspNet.TagHelpers.Tests {

    abstract class TestPage : RazorPage {
        Exception _execError;
        StringBuilder _outputBuilder = new StringBuilder();
        TagHelperRunner _runner = new TagHelperRunner();
        TagHelperScopeManager _scopeManager;
        TagHelperExecutionContext _execContext;

        HierarchicalTagHelper _topLevelTag;

        public TestPage() {
            _scopeManager = new TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);

            HtmlEncoder = HtmlEncoder.Default;
            ViewContext = new ViewContext {
                HttpContext = new DefaultHttpContext {
                    RequestServices = new ServiceProviderMock()
                },
                Writer = new StringWriter(_outputBuilder)
            };
        }

        public HierarchicalTagHelper TopLevelTag {
            get { return _topLevelTag; }
        }

        public string GetOutputText() {
            return _outputBuilder.ToString();
        }

        public void ExecuteSynchronously() {
            ExecuteAsync().Wait();

            if(_execError != null)
                throw _execError;
        }

        public sealed override Task ExecuteAsync() {
            return Task.Factory.StartNew(ExecuteCore);
        }

        protected abstract void ExecuteCore();

        protected void Add(ITagHelper tag, Action executeChildContent = null) {
            Add(new[] { tag }, executeChildContent);
        }

        protected async void Add(ITagHelper[] tags, Action executeChildContent = null) {
            if(_execError != null)
                return;

            _execContext = _scopeManager.Begin("", 0, "", WrapExecuteChildContent(executeChildContent));

            foreach(var tag in tags) {
                if(_topLevelTag == null)
                    _topLevelTag = tag as HierarchicalTagHelper;

                _execContext.Add(tag);
            }

            try {
                await _runner.RunAsync(_execContext);
                Write(_execContext.Output);
            } catch(Exception x) {
#warning see https://github.com/aspnet/Hosting/issues/484
                _execError = x;
            }

            _execContext = _scopeManager.End();
        }

        Func<Task> WrapExecuteChildContent(Action generator) {
            return delegate {
                if(generator != null)
                    generator();

                return Task.FromResult(true); // see http://stackoverflow.com/a/13127229
            };
        }


        class ServiceProviderMock : IServiceProvider {
            IViewBufferScope _viewBufferScope = new MemoryPoolViewBufferScope(
                ArrayPool<ViewBufferValue>.Shared,
                ArrayPool<char>.Shared
            );

            public object GetService(Type serviceType) {
                if(serviceType == typeof(IViewBufferScope))
                    return _viewBufferScope;

                return null;
            }
        }
    }

}
