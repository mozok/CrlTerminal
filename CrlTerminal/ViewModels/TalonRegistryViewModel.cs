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

        public DelegateCommand SelectedDateCommand { get; set; }

        public TalonRegistryViewModel()
        {
            TodayDate = DateTime.Now.Date;
            SelectedDate = DateTime.Now.Date;
            DoctorControll = new MySQLControll();

            SelectedDateCommand = new DelegateCommand(SelectedDateExecute);
        }

        private void SelectedDateExecute()
        {
            try
            {
                //var _date = (DateTime)dateObject;
                DoctorControll.SpecTimeLoad(Ttfsp, SelectedDate);
                IsVisibleTimeError = (Ttfsp.Count == 0) ? true : false;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
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
