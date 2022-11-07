using ABE.LTRIC.WpfGui.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ABE.LTRIC.WpfGui.Views
{
    /// <summary>
    /// Interaction logic for CompaniesView.xaml
    /// </summary>
    public partial class CompaniesView : UserControl
    {
        public CompaniesView()
        {
            DataContext = App.Current.ServiceProvider.GetService<CompaniesViewModel>();
            InitializeComponent();
        }
    }
}
