using CrlTerminal.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CrlTerminal.ViewModels
{
    class SpecListViewModel : BindableBase, INavigationAware
    {
        IRegionManager _regionManager;

        public MySQLControll DoctorControll;

        private static ObservableCollection<Specialization> _specializationsList = new ObservableCollection<Specialization>();
        public ObservableCollection<Specialization> SpecializationsList
        {
            get => _specializationsList;
            set => SetProperty(ref _specializationsList, value);
        }

        private Collection<Spec> spec = new Collection<Spec>();
        //private static ObservableCollection<Spec> _spec = new ObservableCollection<Spec>();
        //public ObservableCollection<Spec> Spec
        //{
        //    get => _spec;
        //    set => SetProperty(ref _spec, value);
        //}

        public DelegateCommand<Spec> SpecSelectCommand { get; set; }
        
        public SpecListViewModel(RegionManager regionManager)
        {
            _regionManager = regionManager;

            DoctorControll = new MySQLControll();
            DoctorControll.SpecListLoad(SpecializationsList, spec);
            
            SpecialistSort();

            SpecSelectCommand = new DelegateCommand<Spec>(SpecSelected);

        }

        private void SpecialistSort()
        {
            foreach(Spec _spec in spec)
            {
                string idString = Regex.Match(_spec.Idsprspec, @"\d+").Value;
                int id = Int32.Parse(idString);

                SpecializationsList.First(i => i.Id == id).Spec.Add(_spec);
            }
        }

        private void SpecSelected(Spec spec)
        {
            var parameters = new NavigationParameters();
            parameters.Add("spec", spec);

            if (spec != null)
                _regionManager.RequestNavigate("ContentRegion", "TalonRegistry", parameters);
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            //SprSpec = navigationContext.Parameters["SprSpec"] as ObservableCollection<sprSpec>;
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            
        }
    }
}
