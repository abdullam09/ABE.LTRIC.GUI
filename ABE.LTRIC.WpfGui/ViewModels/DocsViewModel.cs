using ABE.LTRIC.Core.Entities;
using ABE.LTRIC.Core.Interfaces;
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
        private IDocumentService _documentService;

        [ObservableProperty]
        private Document document;

        [ObservableProperty]
        private Document editDocument;

        [ObservableProperty]
        private DocumentDetail editDocumentDetail;

        private Doc editDoc;
        private DocDtl editDocDtl;

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

        [ObservableProperty]
        private bool isEditDocOpen;

        [ObservableProperty]
        private bool isEditDocDtlOpen;


        public DocsViewModel(ICompanyRepository companyRepository, ISnackbarMessageQueue snackbarMessageQueue, IProgressbarService progressbarService, IDocRepository docRepository, IDocDtlRepository docDtlRepository, IDocumentService documentService)
        {
            _companyRepository = companyRepository;
            _snackbarMessageQueue = snackbarMessageQueue;
            _progressbarService = progressbarService;
            _docRepository = docRepository;
            _docDtlRepository = docDtlRepository;
            _documentService = documentService;
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
        public async Task EditDoc(Doc doc)
        {
            IsEditDocOpen = true;
            EditDocument = new Document();
            EditDocument.Id = doc.Id;
            EditDocument.DocNumber = doc.DocNumber;
            EditDocument.Comments = doc.Comments;
            EditDocument.CompanyId = doc.CompanyId;
            EditDocument.IsODEnded = doc.IsODEnded;
            EditDocument.IsEnded = doc.IsEnded;
            EditDocument.ExpectedDueDate = doc.ExpectedDueDate;
            EditDocument.PaymentDate = doc.PaymentDate;
            EditDocument.PrincipleAmount = doc.PrincipleAmount;
            EditDocument.ODDueDate = doc.ODDueDate;
            var docDetailsSp = new DocDtlsByDocId(doc.Id);
            editDoc = doc;
            editDoc.DocDtls = (List<DocDtl>)await _docDtlRepository.Get(docDetailsSp);
            await Task.CompletedTask;
        }

        [RelayCommand]
        public async Task EditDocDtl(DocDtl docDtl)
        {
            IsEditDocDtlOpen = true;
            EditDocumentDetail = new DocumentDetail();
            EditDocumentDetail.Id = docDtl.Id;
            EditDocumentDetail.EarlySattleToBank = docDtl.EarlySattleToBank;
            EditDocumentDetail.RecoveredFromCompany = docDtl.RecoveredFromCompany;
            editDocDtl = docDtl;
            await Task.CompletedTask;
        }

        [RelayCommand]
        public async Task UpdateDocDtl()
        {
            _progressbarService.SetProgressbar("please wait");
            editDocDtl.EarlySattleToBank = EditDocumentDetail.EarlySattleToBank;
            editDocDtl.RecoveredFromCompany = EditDocumentDetail.RecoveredFromCompany;
            await CloseEditDocDtl();
            await _docDtlRepository.Update(editDocDtl);
            var docByIdSp = new DocByIdSp(editDocDtl.DocId);
            var doc = (List<Doc>)await _docRepository.Get(docByIdSp);
            await _documentService.ProcessDocument(doc[0]);
            _snackbarMessageQueue.Enqueue("Document Detail has been updated successfully");
            await DocDetails(doc[0]);
            _progressbarService.ClearProgressbar();
        }

        [RelayCommand]
        public async Task CloseEditDocDtl()
        {
            IsEditDocDtlOpen = false;
            await Task.CompletedTask;
        }

        [RelayCommand]
        public async Task CloseEditDoc()
        {
            IsEditDocOpen = false;
            await Task.CompletedTask;
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

        [RelayCommand]
        public async Task UpdateDoc()
        {
            _progressbarService.SetProgressbar("please wait");
            editDoc.Comments = EditDocument.Comments;
            editDoc.IsODEnded = EditDocument.IsODEnded;
            editDoc.IsEnded = EditDocument.IsEnded;
            editDoc.PaymentDate = EditDocument.PaymentDate;
            editDoc.CompanyId = EditDocument.CompanyId;
            editDoc.DocNumber = EditDocument.DocNumber;
            editDoc.ExpectedDueDate = EditDocument.ExpectedDueDate;
            editDoc.PrincipleAmount = EditDocument.PrincipleAmount;
            editDoc.ODDueDate = EditDocument.ODDueDate;
            await CloseEditDoc();
            await _docRepository.Update(editDoc);
            await _documentService.ProcessDocument(editDoc);
            _snackbarMessageQueue.Enqueue("Document has been updated successfully");
            await SearchDocs();
            _progressbarService.ClearProgressbar();
        }

        [RelayCommand]
        public async Task AddNewDoc()
        {
            _progressbarService.SetProgressbar("please wait");
            var doc = new Doc
            {
                Comments = Document.Comments,
                CompanyId = Document.CompanyId,
                DocNumber = Document.DocNumber,
                ExpectedDueDate = Document.ExpectedDueDate,
                InsertDate = DateTime.Now,
                IsEnded = Document.IsEnded,
                IsODEnded = Document.IsODEnded,
                ODDueDate = Document.ODDueDate,
                PaymentDate = Document.PaymentDate,
                PrincipleAmount = Document.PrincipleAmount,
                DocDtls = new List<DocDtl>(),
            };
            await _documentService.ProcessDocument(doc);
            _snackbarMessageQueue.Enqueue("Document has been created successfully");
            await SearchDocs();
            _progressbarService.ClearProgressbar();
        }
    }
}
