using CrlTerminal.Models;
using CrlTerminal.Views;
using Prism.Mvvm;
using Prism.Regions;
using System.Collections.ObjectModel;

namespace CrlTerminal.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private string _title = "Термінал Лікарні";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private readonly IRegionManager _regionManager;

        //public MySQLControll DoctorControll;

        //private static ObservableCollection<sprSpec> _sprSpec = new ObservableCollection<sprSpec>();
        //public ObservableCollection<sprSpec> SprSpec
        //{
        //    get => _sprSpec;
        //    set => SetProperty(ref _sprSpec, value);
        //}

        //private static ObservableCollection<spec> _spec = new ObservableCollection<spec>();
        //public ObservableCollection<spec> Spec
        //{
        //    get => _spec;
        //    set => SetProperty(ref _spec, value);
        //}

        public MainWindowViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            _regionManager.RegisterViewWithRegion("ContentRegion", typeof(SpecList));

            //DoctorControll = new MySQLControll();
            //DoctorControll.SpecListLoad(SprSpec, Spec);

            //var parameters = new NavigationParameters();
            //parameters.Add("SprSpec", SprSpec);
            //parameters.Add("Spec", Spec);

            //_regionManager.RequestNavigate("ContentRegion", "SpecList", parameters);
        }
    }
}
