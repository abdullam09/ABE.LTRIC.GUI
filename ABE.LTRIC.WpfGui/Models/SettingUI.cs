using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABE.LTRIC.WpfGui.Models
{
    public partial class SettingUI : ObservableObject
    {
        [ObservableProperty]
        private string key;
        [ObservableProperty]
        private string value;
        [ObservableProperty]
        private DateTime effectiveDate;
    }
}
