using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using xWPFUI.ViewModels;

namespace xWPFUI.State.Navigators
{
    enum ViewType
    {
        Home,
        Define,
        Ayarlar,
        MalKayit,
        Islem
        
    }
    public interface INavigator
    {
        ViewModelBase CurrentViewModel { get; set; }
        event Action StateChanged;

    }
}
