using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABE.LTRIC.WpfGui.Models
{
    public partial class ReportsFilter : ObservableObject
    {
        [ObservableProperty]
        private int companyId;
        [ObservableProperty]
        private string documentNumber;
        [ObservableProperty]
        private DateTime from;
        [ObservableProperty]
        private DateTime to;
        [ObservableProperty]
        private bool includeDates;
    }
}
