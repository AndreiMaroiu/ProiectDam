using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelizerUI.ViewModels
{
    internal interface IClose
    {
        public event Action OnClose;

        public bool CanClose { get; }
    }
}
