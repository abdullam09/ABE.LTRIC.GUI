using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABE.LTRIC.WpfGui.Models
{
    public partial class DocumentDetail : ObservableObject
    {
        [ObservableProperty]
        private int id;
        [ObservableProperty]
        private int docId;
        [ObservableProperty]
        private decimal earlySattleToBank;
        [ObservableProperty]
        private decimal recoveredFromCompany;
    }
}
