using CrlTerminal.Models;
using CrlTerminal.Views;
using MaterialDesignThemes.Wpf;
using Microsoft.Practices.Unity;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Xps.Packaging;
using System.Windows.Xps.Serialization;

namespace CrlTerminal.ViewModels
{
    public class TalonRegistryViewModel : BindableBase, INavigationAware
    {
        public MySQLControll DoctorControll;
        public IUnityContainer _container { get; set; }
        IUsersService _usersService;
        IRegionManager _regionManager;

        #region Observed properties

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

        private int _lastSelectedTime;
        public int LastSelectedTime
        {
            get => _lastSelectedTime;
            set
            {
                SetProperty(ref _lastSelectedTime, value);
                RegisterTalonCommand.RaiseCanExecuteChanged();
            }
        }

        private string _telefonNumber = "";
        public string TelefonNumber
        {
            get => _telefonNumber;
            set
            {
                SetProperty(ref _telefonNumber, value);
                RegisterTalonCommand.RaiseCanExecuteChanged();
            }
        }

        private bool IsUser = false;

        #endregion

        private User selectedUser = new User();

        #region Observed collections

        private ObservableCollection<AppointmentTime> _appointmentTimes = new ObservableCollection<AppointmentTime>();
        public ObservableCollection<AppointmentTime> AppointmentTimes
        {
            get => _appointmentTimes;
            set => SetProperty(ref _appointmentTimes, value);
        }

        #endregion

        #region Delegate Commands

        public DelegateCommand<object> TestCommand { get; set; }
        public DelegateCommand SelectedDateCommand { get; set; }
        public DelegateCommand<AppointmentTime> TimeSelectCommand { get; set; }
        public DelegateCommand RegisterTalonCommand { get; set; }
        public DelegateCommand<string> KeyboardCommand { get; set; }

        #endregion
        //Collection<User> userList;
        public TalonRegistryViewModel(RegionManager regionManager, IUnityContainer container)
        {
            _container = container;
            _regionManager = regionManager;

            TodayDate = DateTime.Now.Date;
            SelectedDate = DateTime.Now.Date;
            DoctorControll = new MySQLControll();

            _usersService = _container.Resolve<IUsersService>();

            TestCommand = new DelegateCommand<object>(Tester);

            SelectedDateCommand = new DelegateCommand(SelectedDateExecute);
            TimeSelectCommand = new DelegateCommand<AppointmentTime>(TimeSelectExecute);
            RegisterTalonCommand = new DelegateCommand(RegisterTalonExecute, CanRegisterTalonExecute);
            KeyboardCommand = new DelegateCommand<string>(KeyboardExecute);
        }

        #region Execute delegate commands

        private void Tester(object _obj)
        {
            MessageBox.Show(_obj.ToString());
        }

        private void TimeSelectExecute(AppointmentTime _ttfsp)
        {
            if (LastSelectedTime != 0)
            {
                AppointmentTimes.First(el => el.Id == LastSelectedTime).IsChosen = false;
            }

            _ttfsp.IsChosen = true;

            LastSelectedTime = _ttfsp.Id;
        }

        private void SelectedDateExecute()
        {
            try
            {
                LastSelectedTime = 0;
                //var _date = (DateTime)dateObject;
                DoctorControll.SpecTimeLoad(AppointmentTimes, SelectedDate, SelectedSpec.Id);
                IsVisibleTimeError = (AppointmentTimes.Count == 0) ? true : false;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private AppointmentTime selectedTime = new AppointmentTime();
        private async void RegisterTalonExecute()
        {
            //TestPrintTalon();
            IsUser = false;

            IsUser = _usersService.AnyUser(TelefonNumber);

            ConfirmDialog confirmView;

            if (IsUser)
            {
                selectedUser = _usersService.GetUser(TelefonNumber);
                selectedTime = AppointmentTimes.First(el => el.IsChosen == true);

                confirmView = new ConfirmDialog
                {
                    DataContext = new ConfirmDialogViewModel(SelectedSpec, selectedTime, TelefonNumber)
                };
            }
            else
            {
                confirmView = new ConfirmDialog
                {
                    DataContext = new ConfirmDialogViewModel("Користувача не знайдено в базі \nЗареєструйтесь!")
                };
            }

            var result = await DialogHost.Show(confirmView, "RootDialog", ClosingEventHandler);
        }

        private void ClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            //Console.WriteLine("You can intercept the closing event, and cancel here.");

            if (!IsUser) return;
            if ((bool)eventArgs.Parameter == false) return;

           string numberTalon = DoctorControll.InsertAppointment(selectedUser, selectedTime, SelectedSpec);

            PrintTalon(numberTalon);
            //TestPrintTalon();


            returnToSpecList();
        }

        private bool CanRegisterTalonExecute()
        {
            return ((LastSelectedTime > 0) && (TelefonNumber.Length >= 5)) ? true : false; ;
        }

        private void KeyboardExecute(string key)
        {
            if (key == "-")
            {
                TelefonNumber = TelefonNumber.Remove(TelefonNumber.Length - 1);

                return;
            }
            if (key.Length > 0)
            {
                TelefonNumber += key;
            }
        }

        #endregion

        private void PrintTalon(string numberTalon)
        {
            try
            {
                PrintDialog printDialog = new PrintDialog();
                FlowDocument flowDocument = new FlowDocument();

                Image image = new Image();

                BitmapImage bimg = new BitmapImage();
                bimg.BeginInit();
                bimg.UriSource = new Uri("pack://application:,,,/images/logo.png");
                bimg.EndInit();

                image.Source = bimg;
                image.Width = 188;
                image.Height = 100;

                flowDocument.Blocks.Add(new BlockUIContainer(image));
                                
                Bold TalonRunBold = new Bold();
                Run TalonRun = new Run("Талон №: " + numberTalon);
                TalonRunBold.Inlines.Add(TalonRun);

                Paragraph p = new Paragraph();
                p.Inlines.Add(TalonRunBold);
                p.FontSize = 20;

                flowDocument.Blocks.Add(p);

                Bold Bold1 = new Bold();
                Run Run1 = new Run("Ім'я пацієнта:\n");
                Bold1.Inlines.Add(Run1);

                Run Run2 = new Run(selectedUser.Name);

                Bold Bold3 = new Bold();
                Run Run3 = new Run("\nІм'я лікаря:\n");
                Bold3.Inlines.Add(Run3);

                Run Run4 = new Run(SelectedSpec.Name);

                Bold Bold5 = new Bold();
                Run Run5 = new Run("\nЧас прийому:\n");
                Bold5.Inlines.Add(Run5);

                Run Run6 = new Run(selectedTime.Hrtime + ":" + selectedTime.Mntime);

                Bold Bold7 = new Bold();
                Run Run7 = new Run("\nДата прийому:\n");
                Bold7.Inlines.Add(Run7);

                Run Run8 = new Run(selectedTime.Dttime.Date.ToShortDateString());

                Bold Bold9 = new Bold();
                Run Run9 = new Run("\nм. Шостка, Поліклініка №4");
                Bold9.Inlines.Add(Run9);

                p = new Paragraph();
                p.Inlines.Add(Bold1);
                p.Inlines.Add(Run2);
                p.Inlines.Add(Bold3);
                p.Inlines.Add(Run4);
                p.Inlines.Add(Bold5);
                p.Inlines.Add(Run6);
                p.Inlines.Add(Bold7);
                p.Inlines.Add(Run8);

                p.FontSize = 20;

                flowDocument.Blocks.Add(p);
                flowDocument.PageHeight = printDialog.PrintableAreaHeight;
                flowDocument.PageWidth = printDialog.PrintableAreaWidth;
                flowDocument.PagePadding = new Thickness(5);
                flowDocument.IsHyphenationEnabled = true;
                flowDocument.Blocks.Add(p);
                IDocumentPaginatorSource idpSource = flowDocument;

                printDialog.PrintDocument(idpSource.DocumentPaginator, "Талон №" + numberTalon);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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

        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            SelectedDate = DateTime.Now.Date;
            TodayDate = DateTime.Now.Date;
            
            TelefonNumber = "";

            var spec = navigationContext.Parameters["spec"] as Spec;

            if (spec != null)
            {
                SelectedSpec = spec;
                DoctorControll.SpecTimeLoad(AppointmentTimes, SelectedDate, SelectedSpec.Id);

                IsVisibleTimeError = (AppointmentTimes.Count == 0) ? true : false;
            }

            LastSelectedTime = 0;
            //TestPrintTalon();
        }

        private void TestPrintTalon()
        {
            try
            {
                PrintDialog printDialog = new PrintDialog();
                FlowDocument flowDocument = new FlowDocument();

                Image image = new Image();

                BitmapImage bimg = new BitmapImage();
                bimg.BeginInit();
                bimg.UriSource = new Uri("pack://application:,,,/images/logo.png");
                //bimg.DecodePixelHeight = 50;
                //bimg.DecodePixelWidth = 94;
                //bimg.UriSource = new Uri("CrlTerminal;component/images/logo.png", UriKind.RelativeOrAbsolute);
                bimg.EndInit();

                image.Source = bimg;
                image.Width = 94;
                image.Height = 50;

                flowDocument.Blocks.Add(new BlockUIContainer(image));
                //if (printDialog.ShowDialog() == true)
                //{
                Bold TalonRunBold = new Bold();
                Run TalonRun = new Run("Талон №: " + "test");
                TalonRunBold.Inlines.Add(TalonRun);

                Paragraph p = new Paragraph();
                p.Inlines.Add(TalonRunBold);
                p.FontSize = 14;

                flowDocument.Blocks.Add(p);

                Bold Bold1 = new Bold();
                Run Run1 = new Run("Ім'я пацієнта:\n");
                Bold1.Inlines.Add(Run1);

                Run Run2 = new Run("Test User");

                Bold Bold3 = new Bold();
                Run Run3 = new Run("\nІм'я лікаря:\n");
                Bold3.Inlines.Add(Run3);

                Run Run4 = new Run("Test Doctor");

                Bold Bold5 = new Bold();
                Run Run5 = new Run("\nЧас прийому:\n");
                Bold5.Inlines.Add(Run5);

                Run Run6 = new Run("Test time");

                Bold Bold7 = new Bold();
                Run Run7 = new Run("\nДата прийому:\n");
                Bold7.Inlines.Add(Run7);

                Run Run8 = new Run("Test date");

                Bold Bold9 = new Bold();
                Run Run9 = new Run("\nм. Шостка, Поліклініка №4");
                Bold9.Inlines.Add(Run9);

                p = new Paragraph();
                p.Inlines.Add(Bold1);
                p.Inlines.Add(Run2);
                p.Inlines.Add(Bold3);
                p.Inlines.Add(Run4);
                p.Inlines.Add(Bold5);
                p.Inlines.Add(Run6);
                p.Inlines.Add(Bold7);
                p.Inlines.Add(Run8);

                p.FontSize = 12;

                flowDocument.Blocks.Add(p);
                flowDocument.PageHeight = printDialog.PrintableAreaHeight;
                flowDocument.PageWidth = printDialog.PrintableAreaWidth;
                flowDocument.PagePadding = new Thickness(0);
                flowDocument.Blocks.Add(p);
                IDocumentPaginatorSource idpSource = flowDocument;

                printDialog.PrintDocument(idpSource.DocumentPaginator, "Талон №" + "Test talon");

                //using (var stream = new FileStream("d:\test.xps", FileMode.Create))
                //{
                //    using (var package = Package.Open(stream, FileMode.Create, FileAccess.ReadWrite))
                //    {
                //        using (var xpsDoc = new XpsDocument(package, CompressionOption.Maximum))
                //        {
                //            var rsm = new XpsSerializationManager(new XpsPackagingPolicy(xpsDoc), false);

                //            var paginator = ((IDocumentPaginatorSource)flowDocument).DocumentPaginator;

                //            rsm.SaveAsXaml(paginator);
                //            rsm.Commit();
                //        }
                //    }

                //    stream.Position = 0;
                //    //var pdfXpsDoc = PdfSharp.Xps.XpsModel.XpsDocument.Open(stream);
                //    var pdfXpsDoc = PdfSharp.Pdf.
                //}

                //using (FileStream fs = new FileStream(@"d:\test.pak", FileMode.OpenOrCreate, FileAccess.Write))
                //{
                //    TextRange textRenge = new TextRange(flowDocument.ContentStart, flowDocument.ContentEnd);
                //    textRenge.Save(fs, DataFormats.XamlPackage);
                //}



                //TextRange textRenge = new TextRange(flowDocument.ContentStart, flowDocument.ContentEnd);
                //using (var fs = System.IO.File.Create("test.xaml"))
                //{

                //    textRenge.Save(fs, DataFormats.Xaml);
                //}

                MessageBox.Show("Testing talon to file");
                //printDialog.PrintQueue =
                //printDialog.PrintVisual((Visual)flowDocument, "Талон №" + "Test talon");
                //printDialog.PrintVisual(idpSource.DocumentPaginator, "Талон №" + "Test talon");
                //printDialog.PrintDocument(idpSource.DocumentPaginator, "Талон №" + "Test talon");
                //}
                //printDialog.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
