using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WDS.ViewModels
{
    public class VmDashboardCard
    {
        public string CardTitle { get; set; }
        public decimal CardQuantity { get; set; }
        public decimal Percentage { get; set; }
        public List<ChartClass> ChartClasses { get; set; }
    }

    public class ChartClass
    {
        public string Day { get; set; }
        public decimal PerDayQty { get; set; }
    }
}