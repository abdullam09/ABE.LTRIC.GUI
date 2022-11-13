using ABE.LTRIC.Core.Entities;
using ABE.LTRIC.Infrastructure.Interfaces;
using ABE.LTRIC.WpfGui.Interfaces;
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
    public partial class CompaniesViewModel : ObservableObject
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly ISnackbarMessageQueue _snackbarMessageQueue;
        private IProgressbarService _progressbarService;

        public CompaniesViewModel(ICompanyRepository companyRepository, ISnackbarMessageQueue snackbarMessageQueue, IProgressbarService progressbarService)
        {
            _companyRepository = companyRepository;
            _snackbarMessageQueue = snackbarMessageQueue;
            _progressbarService = progressbarService;
        }

        [ObservableProperty]
        private ObservableCollection<Company> companies;

        [RelayCommand]
        public async Task Load()
        {
            _progressbarService.SetProgressbar("please wait");
            Companies = new ObservableCollection<Company>(await _companyRepository.GetAll());
            _progressbarService.ClearProgressbar();
        }

        [RelayCommand]
        public async Task AddNewCompany()
        {
            Companies.Add(new Company());
        }

        [RelayCommand]
        public async Task SaveCompanies()
        {
            _progressbarService.SetProgressbar("please wait");
            await _companyRepository.SaveAll(Companies);
            _snackbarMessageQueue.Enqueue("Companies has been saved successfully.");
            _progressbarService.ClearProgressbar();
        }

        [RelayCommand]
        public async Task DeleteCompany(object obj)
        {
            _progressbarService.SetProgressbar("please wait");
            var company = (Company)obj;
            if (company.Id != 0)
            {
                await _companyRepository.Delete((Company)obj);
            }
            Companies.Remove((Company)obj);
            _snackbarMessageQueue.Enqueue("Compnay has been deleted successfully.");
            _progressbarService.ClearProgressbar();
        }
    }
}
