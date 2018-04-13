using CrlTerminal.Models;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrlTerminal.ViewModels
{
    class ConfirmDialogViewModel : BindableBase
    {
        //private int id;
        //public int ID
        //{
        //    get { return id; }
        //    set { SetProperty(ref id, value); }
        //}

        //private string phone;
        //public string Phone
        //{
        //    get { return phone; }
        //    set { SetProperty(ref phone, value); }
        //}
        private string _message;
        public string Message
        {
            get { return _message; }
            set { SetProperty(ref _message, value); }
        }

        private string _specName;
        public string SpecName
        {
            get { return _specName; }
            set { SetProperty(ref _specName, value); }
        }

        private AppointmentTime _selectedTime;
        public AppointmentTime SelectedTime
        {
            get { return _selectedTime; }
            set { SetProperty(ref _selectedTime, value); }
        }

        private bool _isUserFound = false;
        public bool IsUserFound
        {
            get { return _isUserFound; }
            set { SetProperty(ref _isUserFound, value); }
        }

        private Spec _selectedSpec;
        public Spec SelectedSpec
        {
            get { return _selectedSpec; }
            set { SetProperty(ref _selectedSpec, value); }
        }

        private string TelephoneNumber;

        public ConfirmDialogViewModel(Spec selectedSpec, AppointmentTime selectedTime, string telephoneNumber)
        {
            //ID = id;
            //Phone = phone;
            //Message = "Виконую обробку";
            IsUserFound = true;
            SelectedTime = selectedTime;
            TelephoneNumber = telephoneNumber;

            //SpecName = specName;
            SelectedSpec = selectedSpec;
            SelectedTime = selectedTime;
        }

        public ConfirmDialogViewModel(string message)
        {
            Message = message;

        }

    }
}
