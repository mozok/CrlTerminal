using CrlTerminal.Models;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrlTerminal.ViewModels
{
    public class TalonRegistryViewModel : BindableBase, INavigationAware
    {
        public MySQLControll DoctorControll;

        private Spec _selectedSpec;
        public Spec SelectedSpec
        {
            get => _selectedSpec;
            set => SetProperty(ref _selectedSpec, value);
        }

        private DateTime _futureValidatingDate;
        public DateTime FutureValidatingDate
        {
            get => _futureValidatingDate;
            set => SetProperty(ref _futureValidatingDate, value);
        }

        private ObservableCollection<Ttfsp> _ttfsp = new ObservableCollection<Ttfsp>();
        public ObservableCollection<Ttfsp> Ttfsp
        {
            get => _ttfsp;
            set => SetProperty(ref _ttfsp, value);
        }

        public TalonRegistryViewModel()
        {
            FutureValidatingDate = DateTime.Now.Date;
            DoctorControll = new MySQLControll();
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            var spec = navigationContext.Parameters["spec"] as Spec;

            if (spec != null)
            {
                SelectedSpec = spec;
                DoctorControll.SpecTimeLoad(Ttfsp, FutureValidatingDate);
            }
        }
    }
}
