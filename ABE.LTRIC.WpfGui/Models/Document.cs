using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABE.LTRIC.WpfGui.Models
{
    public partial class Document : ObservableObject
    {
        [ObservableProperty]
        private int id;
        [ObservableProperty]
        private int companyId;
        [ObservableProperty]
        private string docNumber;
        [ObservableProperty]
        private DateTime paymentDate;
        [ObservableProperty]
        private DateTime expectedDueDate;
        [ObservableProperty]
        private decimal principleAmount;
        [ObservableProperty]
        private string comments;
        [ObservableProperty]
        private bool isEnded;
        [ObservableProperty]
        private bool isODEnded;
        [ObservableProperty]
        private DateTime oDDueDate;
    }
}
