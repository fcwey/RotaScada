using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using xWPFUI.Base;
using xWPFUI.Command;
using xWPFUI.ViewModels;

namespace xWPFUI.State.Navigators
{
    public class Navigator :  INavigator
    {
        private ViewModelBase _currentViewModel;
        public ViewModelBase CurrentViewModel
        {
            get
            {
                return _currentViewModel;
            }
            set
            {
                _currentViewModel?.Dispose();
                _currentViewModel = value;
                StateChanged?.Invoke();
            }
        }

        public event Action StateChanged;

    }
}
