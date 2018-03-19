using CrlTerminal.Models;
using Prism.Mvvm;
using System.Collections.ObjectModel;

namespace CrlTerminal.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private string _title = "Prism Application";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private static ObservableCollection<sprSpec> _sprSpec = new ObservableCollection<sprSpec>();
        public ObservableCollection<sprSpec> SprSpec
        {
            get => _sprSpec;
            set => SetProperty(ref _sprSpec, value);
        }

        private static ObservableCollection<spec> _spec = new ObservableCollection<spec>();
        public ObservableCollection<spec> Spec
        {
            get => _spec;
            set => SetProperty(ref _spec, value);
        }

        public MainWindowViewModel()
        {

        }
    }
}
