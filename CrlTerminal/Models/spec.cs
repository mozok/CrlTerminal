using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrlTerminal.Models
{
    public class Spec : BindableBase
    {
        private int _id;
        private string _idsprspec;
        private int _idsprsect;
        private int _pricespec;
        private int _idsprtime;
        private string _name;
        private string _desc;
        private string _photo;
        private int _offphoto;
        private int _published;
        private int _ordering;
        private int _checked_out;
        private int _idusr;
        private int _adddt;
        private int _addtm;
        private string _number_cabinet;
        private string _specphone;

        public int Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        public string Idsprspec
        {
            get => _idsprspec;
            set => SetProperty(ref _idsprspec, value);
        }

        public int Idsprsect
        {
            get => _idsprsect;
            set => SetProperty(ref _idsprsect, value);
        }

        public int Idsprtime
        {
            get => _idsprtime;
            set => SetProperty(ref _idsprtime, value);
        }

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public string Desc
        {
            get => _desc;
            set => SetProperty(ref _desc, value);
        }

        public string Photo
        {
            get => _photo;
            set => SetProperty(ref _photo, value);
        }

        public int Offphoto
        {
            get => _offphoto;
            set => SetProperty(ref _offphoto, value);
        }

        public int Published
        {
            get => _published;
            set => SetProperty(ref _published, value);
        }

        public int Ordering
        {
            get => _ordering;
            set => SetProperty(ref _ordering, value);
        }

        public int Checked_out
        {
            get => _checked_out;
            set => SetProperty(ref _checked_out, value);
        }

        public int Idusr
        {
            get => _idusr;
            set => SetProperty(ref _idusr, value);
        }

        public int Adddt
        {
            get => _adddt;
            set => SetProperty(ref _adddt, value);
        }

        public int Addtm
        {
            get => _addtm;
            set => SetProperty(ref _addtm, value);
        }

        public string Number_cabinet
        {
            get => _number_cabinet;
            set => SetProperty(ref _number_cabinet, value);
        }

        public string Specphone
        {
            get => _specphone;
            set => SetProperty(ref _specphone, value);
        }

        private string _specialization;
        public string Specialization
        {
            get { return _specialization; }
            set { SetProperty(ref _specialization, value); }
        }

        private string _specMail;
        public string SpecMail
        {
            get { return _specMail; }
            set { SetProperty(ref _specMail, value); }
        }
    }
}
