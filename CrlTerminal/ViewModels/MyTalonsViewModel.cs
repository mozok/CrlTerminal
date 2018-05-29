using CrlTerminal.Domain;
using CrlTerminal.Models;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media.Imaging;

namespace CrlTerminal.ViewModels
{
    class MyTalonsViewModel : BindableBase, INavigationAware
    {
        public MySQLControll TalonsControll;
        IRegionManager _regionManager;
        IEventAggregator _ea;

        private string _telephoneNumber = "";
        public string TelephoneNumber
        {
            get { return _telephoneNumber; }
            set
            {
                SetProperty(ref _telephoneNumber, value);
                TalonsLoadCommand.RaiseCanExecuteChanged();
            }
        }

        private bool _isTalonsEmpty = true;
        public bool IsTalonsEmpty
        {
            get { return _isTalonsEmpty; }
            set { SetProperty(ref _isTalonsEmpty, value); }
        }

        private int _lastSelectedTalon;
        public int LastSelectedTalon
        {
            get { return _lastSelectedTalon; }
            set
            {
                SetProperty(ref _lastSelectedTalon, value);
                PrintTalonCommand.RaiseCanExecuteChanged();
            }
        }

        private ObservableCollection<Talon> _talons = new ObservableCollection<Talon>();
        public ObservableCollection<Talon> Talons
        {
            get { return _talons; }
            set { SetProperty(ref _talons, value); }
        }

        public DelegateCommand<Talon> TalonSelectCommand { get; set; }
        public DelegateCommand<string> KeyboardCommand { get; set; }
        public DelegateCommand TalonsLoadCommand { get; set; }
        public DelegateCommand PrintTalonCommand { get; set; }

        public MyTalonsViewModel(RegionManager regionManager, IEventAggregator ea)
        {
            _regionManager = regionManager;
            _ea = ea;

            TalonsControll = new MySQLControll();

            TalonsLoadCommand = new DelegateCommand(TalonsLoadExecute, CanTalonsLoadExecute);
            TalonSelectCommand = new DelegateCommand<Talon>(TalonSelectExecute);
            KeyboardCommand = new DelegateCommand<string>(KeyboardExecute);
            PrintTalonCommand = new DelegateCommand(PrintTalonExecute, CanTalonPrintExecute);
        }

        private void TalonsLoadExecute()
        {
            _ea.GetEvent<SnackbarEvent>().Publish("Завантажую талони для " + TelephoneNumber);

            TalonsControll.TalonsLoad(Talons, TelephoneNumber);

            int TalonsCount = Talons.Count();

            if (TalonsCount > 0)
            {
                IsTalonsEmpty = false;
                _ea.GetEvent<SnackbarEvent>().Publish("Знайдено " + TalonsCount + " талон(ів)");
            }
            else
            {
                IsTalonsEmpty = true;
                _ea.GetEvent<SnackbarEvent>().Publish("Талонів не знайдено");
            }

        }

        private bool CanTalonsLoadExecute()
        {
            return TelephoneNumber.Length >= 5 ? true : false;
        }

        private bool CanTalonPrintExecute()
        {
            return LastSelectedTalon > 0 ? true : false;
        }

        private void PrintTalonExecute()
        {
            try
            {
                Talon currentTalon = Talons.First(el => el.Id == LastSelectedTalon);
                PrintDialog printDialog = new PrintDialog();
                FlowDocument flowDocument = new FlowDocument();

                Image image = new Image();

                BitmapImage bimg = new BitmapImage();
                bimg.BeginInit();
                bimg.UriSource = new Uri("pack://application:,,,/images/logo.png");
                bimg.EndInit();

                image.Source = bimg;
                image.Width = 150;
                image.Height = 80;

                flowDocument.Blocks.Add(new BlockUIContainer(image));

                Bold TalonRunBold = new Bold();
                Run TalonRun = new Run("Талон №: " + currentTalon.NumberOrder);
                TalonRunBold.Inlines.Add(TalonRun);

                Paragraph p = new Paragraph();
                p.Inlines.Add(TalonRunBold);
                p.FontSize = 18;

                flowDocument.Blocks.Add(p);

                Bold Specialization = new Bold();
                Run specialization = new Run("Лікар:\n");
                Specialization.Inlines.Add(specialization);
                Run SpecializationName = new Run(currentTalon.Specialization);

                Bold Cabinet = new Bold();
                Run cabinet = new Run("\nКабінет: ");
                Cabinet.Inlines.Add(cabinet);
                Run CabinetNumber = new Run(currentTalon.NumberCabinet);

                Bold Bold3 = new Bold();
                Run Run3 = new Run("\nІм'я лікаря:\n");
                Bold3.Inlines.Add(Run3);

                Run Run4 = new Run(currentTalon.SpecName);

                Bold Bold1 = new Bold();
                Run Run1 = new Run("\nІм'я пацієнта:\n");
                Bold1.Inlines.Add(Run1);

                Run Run2 = new Run(currentTalon.Rfio);

                Bold Bold5 = new Bold();
                Run Run5 = new Run("\nЧас прийому:\n");
                Bold5.Inlines.Add(Run5);

                Run Run6 = new Run(currentTalon.Hours + ":" + currentTalon.Minutes);

                Bold Bold7 = new Bold();
                Run Run7 = new Run("\nДата прийому:\n");
                Bold7.Inlines.Add(Run7);

                Run Run8 = new Run(currentTalon.Date.ToShortDateString());

                Bold Bold9 = new Bold();
                Run Run9 = new Run("\nм. Шостка, Поліклініка №4");
                Bold9.Inlines.Add(Run9);

                p = new Paragraph();
                p.Inlines.Add(Specialization);
                p.Inlines.Add(SpecializationName);
                p.Inlines.Add(Cabinet);
                p.Inlines.Add(CabinetNumber);
                p.Inlines.Add(Bold3);
                p.Inlines.Add(Run4);
                p.Inlines.Add(Bold1);
                p.Inlines.Add(Run2);
                p.Inlines.Add(Bold7);
                p.Inlines.Add(Run8);
                p.Inlines.Add(Bold5);
                p.Inlines.Add(Run6);
                p.Inlines.Add(Bold9);

                p.FontSize = 18;

                flowDocument.Blocks.Add(p);
                flowDocument.PageHeight = printDialog.PrintableAreaHeight;
                flowDocument.PageWidth = printDialog.PrintableAreaWidth;
                flowDocument.PagePadding = new Thickness(5);
                flowDocument.IsHyphenationEnabled = true;
                flowDocument.Blocks.Add(p);
                IDocumentPaginatorSource idpSource = flowDocument;

                printDialog.PrintDocument(idpSource.DocumentPaginator, "Талон №" + currentTalon.NumberOrder);

                returnToSpecList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void TalonSelectExecute(Talon talon)
        {
            if (LastSelectedTalon != 0)
            {
                Talons.First(el => el.Id == LastSelectedTalon).IsSelected = false;
            }

            talon.IsSelected = true;
            LastSelectedTalon = talon.Id;
        }

        private void KeyboardExecute(string key)
        {
            if (key == "-")
            {
                if (TelephoneNumber.Length > 0)
                    TelephoneNumber = TelephoneNumber.Remove(TelephoneNumber.Length - 1);

                return;
            }
            if (key.Length > 0)
            {
                TelephoneNumber += key;
            }
        }

        private void returnToSpecList()
        {
            _regionManager.RequestNavigate("ContentRegion", "SpecList");
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            //throw new NotImplementedException();
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            //throw new NotImplementedException();
            TelephoneNumber = "";
            LastSelectedTalon = 0;
            Talons.Clear();

            _ea.GetEvent<HintEvent>().Publish("ВВЕДІТЬ СВІЙ НОМЕР ТЕЛЕФОНУ ТА ОБЕРІТЬ ТАЛОН");
            _ea.GetEvent<MainViewEvent>().Publish(true);


        }
    }
}
