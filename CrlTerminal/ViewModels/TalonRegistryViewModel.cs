using CrlTerminal.Models;
using CrlTerminal.Views;
using MaterialDesignThemes.Wpf;
using Microsoft.Practices.Unity;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CrlTerminal.ViewModels
{
    public class TalonRegistryViewModel : BindableBase, INavigationAware
    {
        public MySQLControll DoctorControll;
        public IUnityContainer _container { get; set; }
        IUsersService _usersService;

        #region Observed properties

        private Spec _selectedSpec;
        public Spec SelectedSpec
        {
            get => _selectedSpec;
            set => SetProperty(ref _selectedSpec, value);
        }

        private DateTime _selectedDate;
        public DateTime SelectedDate
        {
            get => _selectedDate;
            set
            {
                SetProperty(ref _selectedDate, value);
                /*DoctorControll.SpecTimeLoad(Ttfsp, _selectedDate);*/
            }
        }

        private DateTime _todayDate;
        public DateTime TodayDate
        {
            get => _todayDate;
            set => SetProperty(ref _todayDate, value);
        }

        private bool _isVisibleTimeError = false;
        public bool IsVisibleTimeError
        {
            get => _isVisibleTimeError;
            set => SetProperty(ref _isVisibleTimeError, value);
        }

        private int _lastSelectedTime;
        public int LastSelectedTime
        {
            get => _lastSelectedTime;
            set
            {
                SetProperty(ref _lastSelectedTime, value);
                RegisterTalonCommand.RaiseCanExecuteChanged();
            }
        }

        private string _telefonNumber = "";
        public string TelefonNumber
        {
            get => _telefonNumber;
            set
            {
                SetProperty(ref _telefonNumber, value);
                RegisterTalonCommand.RaiseCanExecuteChanged();
            }
        }

        #endregion

        #region Observed collections

        private ObservableCollection<AppointmentTime> _appointmentTimes = new ObservableCollection<AppointmentTime>();
        public ObservableCollection<AppointmentTime> AppointmentTimes
        {
            get => _appointmentTimes;
            set => SetProperty(ref _appointmentTimes, value);
        }

        #endregion

        #region Delegate Commands

        public DelegateCommand<object> TestCommand { get; set; }
        public DelegateCommand SelectedDateCommand { get; set; }
        public DelegateCommand<AppointmentTime> TimeSelectCommand { get; set; }
        public DelegateCommand RegisterTalonCommand { get; set; }
        public DelegateCommand<string> KeyboardCommand { get; set; }

        #endregion
        //Collection<User> userList;
        public TalonRegistryViewModel(IUnityContainer container)
        {
            _container = container;

            TodayDate = DateTime.Now.Date;
            SelectedDate = DateTime.Now.Date;
            DoctorControll = new MySQLControll();

            _usersService = _container.Resolve<IUsersService>();

            TestCommand = new DelegateCommand<object>(Tester);

            SelectedDateCommand = new DelegateCommand(SelectedDateExecute);
            TimeSelectCommand = new DelegateCommand<AppointmentTime>(TimeSelectExecute);
            RegisterTalonCommand = new DelegateCommand(RegisterTalonExecute, CanRegisterTalonExecute);
            KeyboardCommand = new DelegateCommand<string>(KeyboardExecute);
        }

        #region Execute delegate commands

        private void Tester(object _obj)
        {
            MessageBox.Show(_obj.ToString());
        }

        private void TimeSelectExecute(AppointmentTime _ttfsp)
        {
            if (LastSelectedTime != 0)
            {
                AppointmentTimes.First(el => el.Id == LastSelectedTime).IsChosen = false;
            }

            _ttfsp.IsChosen = true;

            LastSelectedTime = _ttfsp.Id;
        }

        private void SelectedDateExecute()
        {
            try
            {
                LastSelectedTime = 0;
                //var _date = (DateTime)dateObject;
                DoctorControll.SpecTimeLoad(AppointmentTimes, SelectedDate);
                IsVisibleTimeError = (AppointmentTimes.Count == 0) ? true : false;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void RegisterTalonExecute()
        {

            if (_usersService.AnyUser(TelefonNumber))
            {
                var confirmView = new ConfirmDialog
                {
                    DataContext = new ConfirmDialogViewModel(SelectedSpec.Name, AppointmentTimes.First(el => el.IsChosen == true), TelefonNumber)
                };

                var result = await DialogHost.Show(confirmView, "RootDialog", ClosingEventHandler);
            }
            else
            {
                var confirmView = new ConfirmDialog
                {
                    DataContext = new ConfirmDialogViewModel("Користувача не знайдено в базі \nЗареєструйтесь!")
                };

                var result = await DialogHost.Show(confirmView, "RootDialog", ClosingEventHandler);
            }

        }

        private void ClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            //Console.WriteLine("You can intercept the closing event, and cancel here.");
        }

        private bool CanRegisterTalonExecute()
        {
            return ((LastSelectedTime > 0) && (TelefonNumber.Length >= 5)) ? true : false; ;
        }

        private void KeyboardExecute(string key)
        {
            if (key.Length > 0)
            {
                TelefonNumber += key;
            }
        }

        #endregion

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            SelectedDate = DateTime.Now.Date;
            TelefonNumber = "";

            var spec = navigationContext.Parameters["spec"] as Spec;

            if (spec != null)
            {
                SelectedSpec = spec;
                DoctorControll.SpecTimeLoad(AppointmentTimes, SelectedDate);

                IsVisibleTimeError = (AppointmentTimes.Count == 0) ? true : false;
            }
        }
    }
}
