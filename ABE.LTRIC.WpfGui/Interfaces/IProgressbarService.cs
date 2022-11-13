using ABE.LTRIC.WpfGui.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ABE.LTRIC.WpfGui.Interfaces
{
    public interface IProgressbarService
    {
        void SetProgressbar(string text, bool visible = true);
        Progressbar GetProgressbar();
        void ClearProgressbar();
    }
}
