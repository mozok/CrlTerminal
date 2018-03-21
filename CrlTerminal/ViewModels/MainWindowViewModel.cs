﻿using CrlTerminal.Models;
using CrlTerminal.Views;
using Prism.Mvvm;
using Prism.Regions;
using System.Collections.ObjectModel;
using CrlTerminal.Views;

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

        private readonly IRegionManager _regionManager;
        

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

        public MainWindowViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            regionManager.RegisterViewWithRegion("ContentRegion", typeof(SpecList));
        }
    }
}
