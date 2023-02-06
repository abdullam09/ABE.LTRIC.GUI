using ABE.LTRIC.Core.Entities;
using ABE.LTRIC.Infrastructure.Interfaces;
using ABE.LTRIC.WpfGui.Interfaces;
using ABE.LTRIC.WpfGui.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABE.LTRIC.WpfGui.ViewModels
{
    public partial class SettingsViewModel : ObservableObject
    {
        private readonly ISettingRepository _settingRepository;
        private readonly ISnackbarMessageQueue _snackbarMessageQueue;
        private IProgressbarService _progressbarService;

        public SettingsViewModel(ISettingRepository settingRepository, ISnackbarMessageQueue snackbarMessageQueue, IProgressbarService progressbarService)
        {
            _settingRepository = settingRepository;
            _snackbarMessageQueue = snackbarMessageQueue;
            _progressbarService = progressbarService;
        }

        [ObservableProperty]
        private ObservableCollection<Setting> settings;

        [ObservableProperty]
        private SettingUI settingToAdd;

        [RelayCommand]
        public async Task Load()
        {
            _progressbarService.SetProgressbar("please wait");
            SettingToAdd = new SettingUI
            {
                EffectiveDate = DateTime.Now,
            };
            await LoadSettings();
            _progressbarService.ClearProgressbar();
        }

        [RelayCommand]
        public async Task AddSetting()
        {
            _progressbarService.SetProgressbar("please wait");
            await _settingRepository.SaveAll(
                new List<Setting> {
                    new Setting { Key = SettingToAdd.Key, Value = SettingToAdd.Value, EffectiveDate = SettingToAdd.EffectiveDate }
                });
            await LoadSettings();
            _snackbarMessageQueue.Enqueue("Settting has been added successfully");
            _progressbarService.ClearProgressbar();
        }

        [RelayCommand]
        public async Task DeleteSettings(Setting setting)
        {
            _progressbarService.SetProgressbar("please wait");
            await _settingRepository.Delete(setting);
            await LoadSettings();
            _snackbarMessageQueue.Enqueue("Settting has been deleted successfully");
            _progressbarService.ClearProgressbar();
        }

        private async Task LoadSettings()
        {
            Settings = new ObservableCollection<Setting>(await _settingRepository.GetAll());
        }
    }
}
