using Microsoft.AspNet.Mvc.Razor;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.AspNet.Razor.Runtime.TagHelpers;
using Microsoft.AspNet.Razor.TagHelpers;
using Microsoft.Extensions.WebEncoders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevExtreme.AspNet.TagHelpers.Tests {

    abstract class TestPage : RazorPage {
        Exception _execError;
        StringBuilder _outputBuilder = new StringBuilder();
        TagHelperRunner _runner = new TagHelperRunner();
        TagHelperScopeManager _scopeManager = new TagHelperScopeManager();
        TagHelperExecutionContext _execContext;

        HierarchicalTagHelper _topLevelTag;

        public TestPage() {
            HtmlEncoder = new HtmlEncoder();
            ViewContext = new ViewContext {
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

            _execContext = _scopeManager.Begin("", 0, "", WrapExecuteChildContent(executeChildContent), StartTagHelperWritingScope, EndTagHelperWritingScope);

            foreach(var tag in tags) {
                if(_topLevelTag == null)
                    _topLevelTag = tag as HierarchicalTagHelper;

                _execContext.Add(tag);
            }

            try {
                _execContext.Output = await _runner.RunAsync(_execContext);
                await WriteTagHelperAsync(_execContext);
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
    }

}
