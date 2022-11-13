using ABE.LTRIC.WpfGui.Interfaces;
using ABE.LTRIC.WpfGui.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static System.Net.Mime.MediaTypeNames;

namespace ABE.LTRIC.WpfGui.Services
{
    [ObservableObject]
    public partial class ProgressbarService : IProgressbarService
    {
        [ObservableProperty]
        private Progressbar progressbar;

        public void ClearProgressbar()
        {
            SetProgressbar(String.Empty, false);
        }

        public Progressbar GetProgressbar()
        {
            return progressbar;
        }

        public void SetProgressbar(string text, bool visible = true)
        {
            if (progressbar == null)
            {
                progressbar = new Progressbar();
            }
            progressbar.Text = text;

            if (visible)
            {
                progressbar.Visibility = Visibility.Visible;
            }
            else
            {
                progressbar.Visibility = Visibility.Collapsed;
            }
        }
    }
}
