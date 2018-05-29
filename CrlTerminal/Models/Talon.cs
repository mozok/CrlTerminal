using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrlTerminal.Models
{
    public class Talon : BindableBase
    {
        private int _id;
        public int Id
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }

        private int _iduser;
        public int Iduser
        {
            get { return _iduser; }
            set { SetProperty(ref _iduser, value); }
        }

        private int _idSpecialist;
        public int IdSpecialist
        {
            get { return _idSpecialist; }
            set { SetProperty(ref _idSpecialist, value); }
        }

        private string _specName;
        public string SpecName
        {
            get { return _specName; }
            set { SetProperty(ref _specName, value); }
        }

        private string _specialization;
        public string Specialization
        {
            get { return _specialization; }
            set { SetProperty(ref _specialization, value); }
        }

        private string _numberCabinet;
        public string NumberCabinet
        {
            get { return _numberCabinet; }
            set { SetProperty(ref _numberCabinet, value); }
        }

        private string _rfio;
        public string Rfio
        {
            get { return _rfio; }
            set { SetProperty(ref _rfio, value); }
        }

        private string _rphone;
        public string Rphone
        {
            get { return _rphone; }
            set { SetProperty(ref _rphone, value); }
        }

        private string _rmail;
        public string Rmail
        {
            get { return _rmail; }
            set { SetProperty(ref _rmail, value); }
        }

        private string _numberOrder;
        public string NumberOrder
        {
            get { return _numberOrder; }
            set { SetProperty(ref _numberOrder, value); }
        }

        private DateTime _date;
        public DateTime Date
        {
            get { return _date; }
            set { SetProperty(ref _date, value); }
        }

        private string _hours;
        public string Hours
        {
            get { return _hours; }
            set { SetProperty(ref _hours, value); }
        }
        private string _minutes;
        public string Minutes
        {
            get { return _minutes; }
            set { SetProperty(ref _minutes, value); }
        }

        private bool _isSelected = false;
        public bool IsSelected
        {
            get { return _isSelected; }
            set { SetProperty(ref _isSelected, value); }
        }


    }
}
