using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrlTerminal.Models
{
    public class AppointmentTime : BindableBase
    {
        private int _id;
        public int Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        private int _idspec;
        public int Idspec
        {
            get => _idspec;
            set => SetProperty(ref _idspec, value);
        }

        private int _iduser;
        public int Iduser
        {
            get => _iduser;
            set => SetProperty(ref _iduser, value);
        }

        private int _reception;
        public int Reception
        {
            get => _reception;
            set => SetProperty(ref _reception, value);
        }

        private DateTime _dttime;
        public DateTime Dttime
        {
            get => _dttime;
            set => SetProperty(ref _dttime, value);
        }

        private string _hrtime;
        public string Hrtime
        {
            get => _hrtime;
            set => SetProperty(ref _hrtime, value);
        }

        private string _mntime;
        public string Mntime
        {
            get => _mntime;
            set => SetProperty(ref _mntime, value);
        }

        private string _rfio;
        public string Rfio
        {
            get => _rfio;
            set => SetProperty(ref _rfio, value);
        }

        private string _rphone;
        public string Rphone
        {
            get => _rphone;
            set => SetProperty(ref _rphone, value);
        }

        private string _info;
        public string Info
        {
            get => _info;
            set => SetProperty(ref _info, value);
        }

        private string _rmail;
        public string Rmail
        {
            get => _rmail;
            set => SetProperty(ref _rmail, value);
        }

        private bool _isChosen = false;
        public bool IsChosen
        {
            get => _isChosen;
            set => SetProperty(ref _isChosen, value);
        }
    }
}
