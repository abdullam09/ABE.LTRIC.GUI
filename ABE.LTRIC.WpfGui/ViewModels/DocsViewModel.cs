using ABE.LTRIC.Core.Entities;
using ABE.LTRIC.Core.Specifications;
using ABE.LTRIC.Infrastructure.Interfaces;
using ABE.LTRIC.WpfGui.Interfaces;
using ABE.LTRIC.WpfGui.Models;
using ABE.LTRIC.WpfGui.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MaterialDesignThemes.Wpf;
using Microsoft.Xaml.Behaviors.Media;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABE.LTRIC.WpfGui.ViewModels
{
    public partial class DocsViewModel : ObservableObject
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly ISnackbarMessageQueue _snackbarMessageQueue;
        private readonly IDocRepository _docRepository;
        private IProgressbarService _progressbarService;

        [ObservableProperty]
        private Document document;

        [ObservableProperty]
        private ObservableCollection<Company> companies;

        [ObservableProperty]
        private ObservableCollection<Doc> docs;

        [ObservableProperty]
        private ObservableCollection<string> isCompletes;

        [ObservableProperty]
        private int? searchCompanyId;
        [ObservableProperty]
        private string searchDocNum;
        [ObservableProperty]
        private bool searchEnablePaymentDate;
        [ObservableProperty]
        private DateTime searchFromPaymentDate;
        [ObservableProperty]
        private DateTime searchToPaymentDate;
        [ObservableProperty]
        private decimal? searchAmount;
        [ObservableProperty]
        private string searchIsComplete;
        [ObservableProperty]
        private string searchIsOdComplete;


        public DocsViewModel(ICompanyRepository companyRepository, ISnackbarMessageQueue snackbarMessageQueue, IProgressbarService progressbarService, IDocRepository docRepository)
        {
            _companyRepository = companyRepository;
            _snackbarMessageQueue = snackbarMessageQueue;
            _progressbarService = progressbarService;
            _docRepository = docRepository;
        }

        [RelayCommand]
        public async Task Load()
        {
            _progressbarService.SetProgressbar("please wait");
            Companies = new ObservableCollection<Company>(await _companyRepository.GetAll());
            await SearchDocs();
            IsCompletes = new ObservableCollection<string>(StaticsValues.IsCompleteBase);
            _progressbarService.ClearProgressbar();
            Document = new Document
            {
                PaymentDate = DateTime.Now,
                ExpectedDueDate = DateTime.Now,
                ODDueDate = DateTime.Now,
            };
        }

        [RelayCommand]
        public async Task SearchDocs()
        {
            var searchDocs = new DocsSearchSp(searchCompanyId, searchDocNum, searchEnablePaymentDate, searchFromPaymentDate, searchToPaymentDate, searchAmount, searchIsComplete, searchIsOdComplete);
            Docs = new ObservableCollection<Doc>((List<Doc>)await _docRepository.Get(searchDocs));
        }

        [RelayCommand]
        public async Task ClearSearchDocs()
        {
            SearchCompanyId = null;
            SearchDocNum = string.Empty;
            SearchEnablePaymentDate = false;
            SearchFromPaymentDate = DateTime.Now;
            SearchToPaymentDate = DateTime.Now;
            SearchAmount = null;
            SearchIsComplete = "Any";
            SearchIsOdComplete = "Any";
            await Task.CompletedTask;
        }
    }
}
