using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ABE.LTRIC.WpfGui.Models
{
    [ObservableObject]
    public partial class Progressbar
    {
        [ObservableProperty]
        private string text = string.Empty;

        [ObservableProperty]
        private Visibility visibility = Visibility.Collapsed;

    }
}
