using CrlTerminal.Models;
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
        public MySQLControll DoctorControll;

        private static ObservableCollection<SprSpec> _sprSpec = new ObservableCollection<SprSpec>();
        public ObservableCollection<SprSpec> SprSpec
        {
            get => _sprSpec;
            set => SetProperty(ref _sprSpec, value);
        }

        private Collection<Spec> spec = new Collection<Spec>();
        //private static ObservableCollection<Spec> _spec = new ObservableCollection<Spec>();
        //public ObservableCollection<Spec> Spec
        //{
        //    get => _spec;
        //    set => SetProperty(ref _spec, value);
        //}

        public SpecListViewModel()
        {
            DoctorControll = new MySQLControll();
            DoctorControll.SpecListLoad(SprSpec, spec);
            
            SpecialistSort();

        }

        private void SpecialistSort()
        {
            foreach(Spec _spec in spec)
            {
                string idString = Regex.Match(_spec.Idsprspec, @"\d+").Value;
                int id = Int32.Parse(idString);

                SprSpec.First(i => i.Id == id).Spec.Add(_spec);
            }
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
