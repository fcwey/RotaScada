using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xWPFUI.State.Navigators;

namespace xWPFUI.ViewModels
{
    public class HomeViewModel:ViewModelBase
    {
        public HomeViewModel(ServisListingViewModel servisListingViewModel)
        {
            ServisListingViewModel = servisListingViewModel;
        }

        public ServisListingViewModel ServisListingViewModel { get; }
        public override void Dispose()
        {
            ServisListingViewModel.Dispose();

            base.Dispose();
        }
    }
}
