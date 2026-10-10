using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using xWPFUI.State.Navigators;
using xWPFUI.ViewModels;
using xWPFUI.ViewModels.Factories;

namespace xWPFUI.Command
{
    public class UpdateCurrentViewModelCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;
        private readonly INavigator _navigator;
        private readonly IMalKabulViewModelFactory _viewModelFactory;

        public UpdateCurrentViewModelCommand(INavigator navigator, IMalKabulViewModelFactory viewModelFactory)
        {
            _navigator = navigator;
            _viewModelFactory = viewModelFactory;
        }

        public bool CanExecute(object? parameter)
        {
           return true;
        }

        public void Execute(object? parameter)
        {
            if (parameter is ViewType)
            {
                ViewType viewType = (ViewType)parameter;
             //   _navigator.CurrentViewModel = _viewModelFactory.CreateViewModel(viewType);
            }
        }
    }
}