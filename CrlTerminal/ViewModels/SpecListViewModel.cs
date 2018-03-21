using CrlTerminal.Models;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrlTerminal.ViewModels
{
    class SpecListViewModel : BindableBase
    {
        public MySQLControll DoctorControll;

        private static ObservableCollection<sprSpec> _sprSpec = new ObservableCollection<sprSpec>();
        public ObservableCollection<sprSpec> SprSpec
        {
            get => _sprSpec;
            set => SetProperty(ref _sprSpec, value);
        }

        private static ObservableCollection<spec> _spec = new ObservableCollection<spec>();
        public ObservableCollection<spec> Spec
        {
            get => _spec;
            set => SetProperty(ref _spec, value);
        }

        public SpecListViewModel()
        {
            DoctorControll = new MySQLControll();
            DoctorControll.SpecListLoad(SprSpec, Spec);

        }
    }
}
