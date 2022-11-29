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
using System.Windows;

namespace ABE.LTRIC.WpfGui.ViewModels
{
    public partial class DocsViewModel : ObservableObject
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly ISnackbarMessageQueue _snackbarMessageQueue;
        private readonly IDocRepository _docRepository;
        private IProgressbarService _progressbarService;
        private readonly IDocDtlRepository _docDtlRepository;

        [ObservableProperty]
        private Document document;

        [ObservableProperty]
        private ObservableCollection<Company> companies;

        [ObservableProperty]
        private ObservableCollection<Doc> docs;

        [ObservableProperty]
        private ObservableCollection<DocDtl> docDtls;

        [ObservableProperty]
        private ObservableCollection<string> isCompletes;

        [ObservableProperty]
        private int? searchCompanyId;
        [ObservableProperty]
        private string searchDocNum;
        [ObservableProperty]
        private bool searchEnablePaymentDate;
        [ObservableProperty]
        private DateTime searchFromPaymentDate = DateTime.Now;
        [ObservableProperty]
        private DateTime searchToPaymentDate = DateTime.Now;
        [ObservableProperty]
        private decimal? searchAmount;
        [ObservableProperty]
        private string searchIsComplete;
        [ObservableProperty]
        private string searchIsOdComplete;

        [ObservableProperty]
        private Doc selectedDoc;

        [ObservableProperty]
        private Doc selectedDocDtl;


        public DocsViewModel(ICompanyRepository companyRepository, ISnackbarMessageQueue snackbarMessageQueue, IProgressbarService progressbarService, IDocRepository docRepository, IDocDtlRepository docDtlRepository)
        {
            _companyRepository = companyRepository;
            _snackbarMessageQueue = snackbarMessageQueue;
            _progressbarService = progressbarService;
            _docRepository = docRepository;
            _docDtlRepository = docDtlRepository;
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
        public async Task DeleteDoc(Doc doc)
        {
            if (MessageBox.Show("Are you sure you want to delete this document?", "LTRIC", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                _progressbarService.SetProgressbar("please wait");
                await _docRepository.Delete(doc);
                await SearchDocs();
                _snackbarMessageQueue.Enqueue("Document has been deleted successfully");
                _progressbarService.ClearProgressbar();
            }
        }

        [RelayCommand]
        public async Task DocDetails(Doc doc)
        {

            _progressbarService.SetProgressbar("please wait");
            if (doc.DocDtls != null)
            {
                DocDtls = new ObservableCollection<DocDtl>(doc.DocDtls);
            }
            else
            {
                var docDtlsByDocId = new DocDtlsByDocId(doc.Id);
                DocDtls = new ObservableCollection<DocDtl>((List<DocDtl>)await _docDtlRepository.Get(docDtlsByDocId));
            }
            await Task.CompletedTask;
            _progressbarService.ClearProgressbar();

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
