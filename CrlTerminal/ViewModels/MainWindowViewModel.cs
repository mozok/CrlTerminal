using CrlTerminal.Models;
using CrlTerminal.Views;
using Microsoft.Practices.Unity;
using Prism.Commands;
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
        public IUnityContainer _container { get; set; }

        public DelegateCommand<string> NavigateCommand { get; set; }

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

        public MainWindowViewModel(IRegionManager regionManager, IUnityContainer container)
        {
            _regionManager = regionManager;
            _regionManager.RegisterViewWithRegion("ContentRegion", typeof(SpecList));
            _container = container;

            NavigateCommand = new DelegateCommand<string>(Navigate);

            var userService = _container.Resolve<IUsersService>();
            userService.UpdateUsersList();

            string version = GetPublishedVersion();
            Title = Title + "-" + version;

            
            //DoctorControll = new MySQLControll();
            //DoctorControll.SpecListLoad(SprSpec, Spec);

            //var parameters = new NavigationParameters();
            //parameters.Add("SprSpec", SprSpec);
            //parameters.Add("Spec", Spec);

            //_regionManager.RequestNavigate("ContentRegion", "SpecList", parameters);
        }

        private void Navigate(string uri)
        {
            _regionManager.RequestNavigate("ContentRegion", uri);
        }

        private string GetPublishedVersion()
        {
            if (System.Deployment.Application.ApplicationDeployment.IsNetworkDeployed)
            {
                return System.Deployment.Application.ApplicationDeployment.CurrentDeployment.
                    CurrentVersion.ToString();
            }
            return "Not network deployed";
        }
    }
}
