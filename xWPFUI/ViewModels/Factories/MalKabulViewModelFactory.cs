using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xWPFUI.State.Navigators;

namespace xWPFUI.ViewModels.Factories
{
    public class MalKabulViewModelFactory:IMalKabulViewModelFactory
    { 
        private readonly CreateViewModel<HomeViewModel> _createHomeViewModel;
        private readonly CreateViewModel<DefinitionViewModel> _createDefineViewModel;

       

        public MalKabulViewModelFactory(CreateViewModel<HomeViewModel> createHomeViewModel, CreateViewModel<DefinitionViewModel> createPortfolioViewModel)
        {
            _createHomeViewModel = createHomeViewModel;
            _createDefineViewModel = createPortfolioViewModel;
        }

        

        //ViewModelBase IMalKabulViewModelFactory.CreateViewModel(ViewType viewType)
        //{
        //    switch (viewType)
        //    {

        //        case ViewType.Home:
        //            return _createHomeViewModel();
        //        case ViewType.Define:
        //            return _createDefineViewModel();
        //        default:
        //            throw new ArgumentException("The ViewType does not have a ViewModel.", "viewType");
        //    }
        //}
    }
}
