using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrlTerminal.Models
{
    public class SprSpec : BindableBase
    {
        private int _id;
        private string _name;
        private int _published;
        private string _desc;
        private string _photo;
        private int _offphoto;
        private int _checked_out;
        private int _ordering;

        public int Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public int Published
        {
            get => _published;
            set => SetProperty(ref _published, value);
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

        public int Checked_out
        {
            get => _checked_out;
            set => SetProperty(ref _checked_out, value);
        }

        public int Ordering
        {
            get => _ordering;
            set => SetProperty(ref _ordering, value);
        }
    }
}
