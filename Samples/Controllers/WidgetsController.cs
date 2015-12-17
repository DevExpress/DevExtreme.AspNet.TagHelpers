using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Samples.Controllers {

    public class WidgetsController {

        public IActionResult DataGrid() {
            return new ViewResult();
        }

        public IActionResult PivotGrid() {
            return new ViewResult();
        }

        public IActionResult Sparkline() {
            return new ViewResult();
        }

        public IActionResult RangeSelector() {
            return new ViewResult();
        }

        public IActionResult Chart() {
            return new ViewResult();
        }

        public IActionResult PieChart() {
            return new ViewResult();
        }

        public IActionResult Scheduler() {
            return new ViewResult();
        }
    }

}
