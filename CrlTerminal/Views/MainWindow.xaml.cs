using MaterialDesignThemes.Wpf;
using Prism.Regions;
using System.Windows;

namespace CrlTerminal.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //public static Snackbar Snackbar;
        //public static ISnackbarMessageQueue snackbarMessageQueue;
        public MainWindow()
        {
            InitializeComponent();
            this.Cursor = System.Windows.Input.Cursors.None;

            //MainSnackbar.MessageQueue.Enqueue("Loaded");

            //Snackbar = this.MainSnackbar;
            //snackbarMessageQueue = MainSnackbar.MessageQueue;
        }
    }
}
