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
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Windows;

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
        //public static Snackbar Snackbar;
        //private Snackbar _snackbar;
        //public Snackbar Snackbar
        //{
        //    get { return _snackbar; }
        //    set { SetProperty(ref _snackbar, value); }
        //}
        //public static ISnackbarMessageQueue snackbarMessageQueue;
        public static SnackbarMessageQueue snackbarMessageQueue { get; } = new SnackbarMessageQueue();

        public MainWindowViewModel(IRegionManager regionManager, IUnityContainer container, IEventAggregator ea)
        {
            //Snackbar = new Snackbar();
            //snackbarMessageQueue = Snackbar.MessageQueue;

            _regionManager = regionManager;
            _regionManager.RegisterViewWithRegion("ContentRegion", typeof(SpecList));
            _container = container;
            _ea = ea;

            _ea.GetEvent<HintEvent>().Subscribe(HintUpdate);
            _ea.GetEvent<MainViewEvent>().Subscribe(ViewUpdate);
            _ea.GetEvent<SnackbarEvent>().Subscribe(SnackbarUpdate);            

            NavigateCommand = new DelegateCommand<string>(Navigate);

            var userService = _container.Resolve<IUsersService>();
            userService.UpdateUsersList();

            string version = GetPublishedVersion();
            //Title = Title + "-" + version;
            Title = "ОБЕРІТЬ ЛІКАРЯ ЗІ СПИСКУ";

            //snackbar = App.Current.MainWindow.MainSnackbar;
            //snackbar = MainWindow.Snackbar;
            //snackbarMessageQueue.Enqueue("Welcome");
            //DoctorControll = new MySQLControll();
            //DoctorControll.SpecListLoad(SprSpec, Spec);

            //var parameters = new NavigationParameters();
            //parameters.Add("SprSpec", SprSpec);
            //parameters.Add("Spec", Spec);

            //_regionManager.RequestNavigate("ContentRegion", "SpecList", parameters);
            //Task.Factory.StartNew(() => MainWindow.snackbarMessageQueue.Enqueue("Loaded"));

            //MainWindow.snackbarMessageQueue.Enqueue("Loaded");
            //Dispatcher.Invoke(() => { Task.Factory.StartNew(() => Snackbar.MessageQueue.Enqueue("Loaded")); });
            //Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Normal, );
            //Application.Current.Dispatcher.Invoke(() => {Snackbar.MessageQueue.Enqueue("Loaded"); }, DispatcherPriority.ContextIdle);
            //Snackbar.MessageQueue.Enqueue("Loaded");
            Task.Factory.StartNew(() => snackbarMessageQueue.Enqueue("Привіт"));
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
        private void SnackbarUpdate(string msg)
        {
            Task.Factory.StartNew(() => snackbarMessageQueue.Enqueue(msg));
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
