using CrlTerminal.Models;
using CrlTerminal.Views;
using CrlTerminal.Domain;
using MaterialDesignThemes.Wpf;
using Microsoft.Practices.Unity;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System.Collections.ObjectModel;
using Prism.Events;

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

        private bool _isSpecList = false;
        public bool IsSpecList
        {
            get { return _isSpecList; }
            set { SetProperty(ref _isSpecList, value); }
        }

        private readonly IRegionManager _regionManager;
        public IUnityContainer _container { get; set; }
        IEventAggregator _ea;

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
        //ISnackbarMessageQueue snackbarMessageQueue;
        

        public MainWindowViewModel(IRegionManager regionManager, IUnityContainer container, IEventAggregator ea)
        {
            _regionManager = regionManager;
            _regionManager.RegisterViewWithRegion("ContentRegion", typeof(SpecList));
            _container = container;
            _ea = ea;

            _ea.GetEvent<HintEvent>().Subscribe(HintUpdate);
            _ea.GetEvent<MainViewEvent>().Subscribe(ViewUpdate);

            NavigateCommand = new DelegateCommand<string>(Navigate);

            var userService = _container.Resolve<IUsersService>();
            userService.UpdateUsersList();

            string version = GetPublishedVersion();
            //Title = Title + "-" + version;
            Title = "ОБЕРІТЬ ЛІКАРЯ ЗІ СПИСКУ";
            
            //snackbarMessageQueue.Enqueue("Welcome");
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

        private void HintUpdate (string hint)
        {
            Title = hint;
        }

        private void ViewUpdate (bool update)
        {
            IsSpecList = update;
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
