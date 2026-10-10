using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xWPFUI.State.Navigators;

namespace xWPFUI.ViewModels
{
    public class MainViewModel:ViewModelBase
    {
        public INavigator Navigator { get; set; } = new Navigator();
    }
}
