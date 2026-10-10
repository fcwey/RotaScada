using Business.Abstract;
using Entities.Concrete;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using xWPFUI.Command;

namespace xWPFUI.ViewModels
{
    public class ServisListingViewModel:ViewModelBase
    {
       // List<Servis> islemler = _servisService.GetList().Data;
        private List<ServisDto> _servisList;
        public List<ServisDto> ServisList
        {
            get
            {
                return _servisList;
            }
            set
            {
                _servisList = value;
                OnPropertyChanged(nameof(ServisList));
            }
        }
        private bool _isLoading;

      

        public bool IsLoading
        {
            get
            {
                return _isLoading;
            }
            set
            {
                _isLoading = value;
                OnPropertyChanged(nameof(IsLoading));
            }
        }
        public ICommand LoadServiceListCommand { get; }
        public ServisListingViewModel(IServisService _servisService)
        {
            LoadServiceListCommand = new LoadServiceListCommand (this, _servisService);
        }
        public static ServisListingViewModel LoadServisListViewModel(IServisService _servisService)
        {
            ServisListingViewModel servisListingViewModel = new ServisListingViewModel(_servisService);

            servisListingViewModel.LoadServiceListCommand.Execute(null);

            return servisListingViewModel;
        }
    }
}
