using CrlTerminal.Models;
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

        private ObservableCollection<Ttfsp> _ttfsp = new ObservableCollection<Ttfsp>();
        public ObservableCollection<Ttfsp> Ttfsp
        {
            get => _ttfsp;
            set => SetProperty(ref _ttfsp, value);
        }

        private int _lastSelectedTime;
        public int LastSelectedTime
        {
            get => _lastSelectedTime;
            set => SetProperty(ref _lastSelectedTime, value);
        }

        public DelegateCommand SelectedDateCommand { get; set; }
        public DelegateCommand<object> TestCommand { get; set; }
        public DelegateCommand<Ttfsp> TimeSelectCommand { get; set; }
        public DelegateCommand  RegisterTalonCommand { get; set; }

        public TalonRegistryViewModel()
        {
            TodayDate = DateTime.Now.Date;
            SelectedDate = DateTime.Now.Date;
            DoctorControll = new MySQLControll();

            SelectedDateCommand = new DelegateCommand(SelectedDateExecute);
            TimeSelectCommand = new DelegateCommand<Ttfsp>(TimeSelectExecute);
            RegisterTalonCommand = new DelegateCommand(RegisterTalonExecute, CanRegisterTalonExecute).ObservesProperty(() => LastSelectedTime);
            TestCommand = new DelegateCommand<object>(Tester);
        }

        private void Tester(object _obj)
        {
            MessageBox.Show(_obj.ToString());
        }

        private void TimeSelectExecute(Ttfsp _ttfsp)
        {
            if (LastSelectedTime != 0)
            {
                Ttfsp.First(el => el.Id == LastSelectedTime).IsChosen = false;
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
                DoctorControll.SpecTimeLoad(Ttfsp, SelectedDate);
                IsVisibleTimeError = (Ttfsp.Count == 0) ? true : false;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void RegisterTalonExecute()
        {

        }

        private bool CanRegisterTalonExecute()
        {
            return (LastSelectedTime > 0) ? true : false; ;
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
            SelectedDate = DateTime.Now.Date;

            var spec = navigationContext.Parameters["spec"] as Spec;

            if (spec != null)
            {
                SelectedSpec = spec;
                DoctorControll.SpecTimeLoad(Ttfsp, SelectedDate);

                IsVisibleTimeError = (Ttfsp.Count == 0) ? true : false;
            }
        }
    }
}
