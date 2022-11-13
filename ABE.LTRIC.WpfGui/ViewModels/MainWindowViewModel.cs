using ABE.LTRIC.WpfGui.Interfaces;
using ABE.LTRIC.WpfGui.Models;
using ABE.LTRIC.WpfGui.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABE.LTRIC.WpfGui.ViewModels
{

    public partial class MainWindowViewModel : ObservableObject
    {
        public MainWindowViewModel(ISnackbarMessageQueue notificationsMessageQueue,IProgressbarService progressbarService)
        {
            NotificationsMessageQueue = notificationsMessageQueue;
            progressbarService.ClearProgressbar();
            progressbar = progressbarService.GetProgressbar();
        }

        public ISnackbarMessageQueue NotificationsMessageQueue { get; set; }

        [ObservableProperty]
        private Progressbar progressbar;
    }
}
