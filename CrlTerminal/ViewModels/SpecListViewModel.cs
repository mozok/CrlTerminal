﻿using CrlTerminal.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CrlTerminal.Domain;
using System.Printing;
using System.Windows;

namespace CrlTerminal.ViewModels
{
    class SpecListViewModel : BindableBase, INavigationAware
    {
        IRegionManager _regionManager;
        IEventAggregator _ea;

        public MySQLControll DoctorControll;

        private static ObservableCollection<Specialization> _specializationsList = new ObservableCollection<Specialization>();
        public ObservableCollection<Specialization> SpecializationsList
        {
            get => _specializationsList;
            set => SetProperty(ref _specializationsList, value);
        }

        private Collection<Spec> spec = new Collection<Spec>();
        //private static ObservableCollection<Spec> _spec = new ObservableCollection<Spec>();
        //public ObservableCollection<Spec> Spec
        //{
        //    get => _spec;
        //    set => SetProperty(ref _spec, value);
        //}

        public DelegateCommand<Spec> SpecSelectCommand { get; set; }

        public SpecListViewModel(RegionManager regionManager, IEventAggregator ea)
        {
            _regionManager = regionManager;
            _ea = ea;

            DoctorControll = new MySQLControll();
            DoctorControll.SpecListLoad(SpecializationsList, spec);
            
            SpecialistSort();

            SpecSelectCommand = new DelegateCommand<Spec>(SpecSelected);

        }

        private void SpecialistSort()
        {
            foreach(Spec _spec in spec)
            {
                string idString = Regex.Match(_spec.Idsprspec, @"\d+").Value;
                int id = Int32.Parse(idString);

                _spec.Specialization = SpecializationsList.First(el => el.Id == id).Name;
                SpecializationsList.First(i => i.Id == id).Spec.Add(_spec);
            }
        }

        private void SpecSelected(Spec spec)
        {
            if (CheckPrinterErrors()) return;

            var parameters = new NavigationParameters();
            parameters.Add("spec", spec);

            _ea.GetEvent<SnackbarEvent>().Publish("Завантажую розклад " + spec.Name);

            if (spec != null)
                _regionManager.RequestNavigate("ContentRegion", "TalonRegistry", parameters);
        }

        private bool CheckPrinterErrors()
        {
            LocalPrintServer ps = new LocalPrintServer();
            PrintQueue pq = ps.DefaultPrintQueue;
            bool error = false;
            string msg = "Проблеми з друком\n";

            if ((pq.QueueStatus & PrintQueueStatus.PaperProblem) == PrintQueueStatus.PaperProblem)
            {
                msg += "В Принтері Закінчилась Бумага.\n";
                error = true;
            }
            if ((pq.QueueStatus & PrintQueueStatus.NoToner) == PrintQueueStatus.NoToner)
            {
                error = true;
            }
            if ((pq.QueueStatus & PrintQueueStatus.DoorOpen) == PrintQueueStatus.DoorOpen)
            {
                error = true;
            }
            if ((pq.QueueStatus & PrintQueueStatus.Error) == PrintQueueStatus.Error)
            {
                error = true;
            }
            if ((pq.QueueStatus & PrintQueueStatus.NotAvailable) == PrintQueueStatus.NotAvailable)
            {
                error = true;
            }
            if ((pq.QueueStatus & PrintQueueStatus.Offline) == PrintQueueStatus.Offline)
            {
                error = true;
            }
            if ((pq.QueueStatus & PrintQueueStatus.OutOfMemory) == PrintQueueStatus.OutOfMemory)
            {
                error = true;
            }
            if ((pq.QueueStatus & PrintQueueStatus.PaperOut) == PrintQueueStatus.PaperOut)
            {
                error = true;
            }
            if ((pq.QueueStatus & PrintQueueStatus.OutputBinFull) == PrintQueueStatus.OutputBinFull)
            {
                error = true;
            }
            if ((pq.QueueStatus & PrintQueueStatus.PaperJam) == PrintQueueStatus.PaperJam)
            {
                error = true;
            }
            if ((pq.QueueStatus & PrintQueueStatus.Paused) == PrintQueueStatus.Paused)
            {
                error = true;
            }
            if ((pq.QueueStatus & PrintQueueStatus.TonerLow) == PrintQueueStatus.TonerLow)
            {
                error = true;
            }
            if ((pq.QueueStatus & PrintQueueStatus.UserIntervention) == PrintQueueStatus.UserIntervention)
            {
                error = true;
            }

            if (pq.NumberOfJobs > 0)
            {
               //msg += "Проблема з друком.\nЗверніться до реєстратури!";
               error = true;
            }

            if (error) MessageBox.Show(msg + "Зверніться до реєстратури!");

            return error;
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            //SprSpec = navigationContext.Parameters["SprSpec"] as ObservableCollection<sprSpec>;
            _ea.GetEvent<HintEvent>().Publish("ОБЕРІТЬ ЛІКАРЯ ЗІ СПИСКУ");
            _ea.GetEvent<MainViewEvent>().Publish(false);
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            
        }
    }
}
