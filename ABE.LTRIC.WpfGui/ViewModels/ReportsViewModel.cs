using ABE.LTRIC.Core.Entities;
using ABE.LTRIC.Core.Interfaces;
using ABE.LTRIC.Infrastructure.Interfaces;
using ABE.LTRIC.WpfGui.Interfaces;
using ABE.LTRIC.WpfGui.Models;
using ABE.LTRIC.WpfGui.Services;
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
    public partial class ReportsViewModel : ObservableObject
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly ISnackbarMessageQueue _snackbarMessageQueue;
        private IProgressbarService _progressbarService;
        private IReportsService _reportsService;
        public ReportsViewModel(ICompanyRepository companyRepository, ISnackbarMessageQueue snackbarMessageQueue, IProgressbarService progressbarService, IReportsService reportsService)
        {
            _companyRepository = companyRepository;
            _snackbarMessageQueue = snackbarMessageQueue;
            _progressbarService = progressbarService;
            _reportsService = reportsService;
        }

        [ObservableProperty]
        private ReportsFilter reportsFilter;
        [ObservableProperty]
        private ObservableCollection<Company> companies;
        [ObservableProperty]
        private ObservableCollection<string> reports = new ObservableCollection<string>();

        [RelayCommand]
        public async Task Load()
        {
            _progressbarService.SetProgressbar("please wait");
            Companies = new ObservableCollection<Company>(await _companyRepository.GetAll());
            ReportsFilter = new ReportsFilter
            {
                From = DateTime.Now,
                To = DateTime.Now
            };
            await GetAllReports();
            _progressbarService.ClearProgressbar();
        }

        [RelayCommand]
        public async Task GenerateReport()
        {
            _progressbarService.SetProgressbar("please wait");
            await _reportsService.GenerateReport(ReportsFilter.CompanyId, ReportsFilter.DocumentNumber, ReportsFilter.From, ReportsFilter.To, ReportsFilter.IncludeDates);
            await GetAllReports();
            _snackbarMessageQueue.Enqueue("Report has been generated successfully");
            _progressbarService.ClearProgressbar();
        }

        [RelayCommand]
        public async Task ClearGenerateReport()
        {
            ReportsFilter.DocumentNumber = string.Empty;
            ReportsFilter.IncludeDates = false;
            ReportsFilter.CompanyId = -1;
            ReportsFilter.From = DateTime.Now;
            ReportsFilter.To = DateTime.Now;
            await Task.CompletedTask;
        }

        [RelayCommand]
        public async Task DeleteReport(string reportName)
        {
            var result = await _reportsService.DeleteReport(reportName);

            if (result == "SUCCESS")
            {
                _snackbarMessageQueue.Enqueue("Report has been deleted successfully");
            }
            else
            {
                _snackbarMessageQueue.Enqueue(result);
            }
            await GetAllReports();
            await Task.CompletedTask;
        }

        [RelayCommand]
        public async Task ViewReport(string reportName)
        {
            await _reportsService.OpenReport(reportName);
            await Task.CompletedTask;
        }

        private async Task GetAllReports()
        {
            Reports = new ObservableCollection<string>((List<string>)_reportsService.GetAllReports());
            await Task.CompletedTask;
        }

    }
}
