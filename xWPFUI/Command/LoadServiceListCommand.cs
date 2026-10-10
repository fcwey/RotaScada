using Business.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xWPFUI.ViewModels;

namespace xWPFUI.Command
{
    public class LoadServiceListCommand : AsyncCommandBase
    {
        private readonly ServisListingViewModel _servisListingViewModel;
        private readonly IServisService _servisService;

        public LoadServiceListCommand(ServisListingViewModel servisListingViewModel, IServisService servisService)
        {
            _servisListingViewModel = servisListingViewModel;
            _servisService = servisService;
        }

        public override async Task ExecuteAsync(object parameter)
        {
            _servisListingViewModel.IsLoading= true;
            await Task.WhenAll(LoadServiceListing()); //, ile array eklenebilir
            _servisListingViewModel.IsLoading = false;

        }
        private async Task LoadServiceListing()
        {
            //var list = await _bolumIslemService.GetListBolumIslemDtosAsync(bolumId);
            //lvMGrup.ItemsSource = list.Data;

            var list= await _servisService.GetListDtosAsync();

            _servisListingViewModel.ServisList = list.Data;
        }
    }
}
