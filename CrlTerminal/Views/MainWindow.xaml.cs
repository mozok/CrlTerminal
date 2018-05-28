using MaterialDesignThemes.Wpf;
using Prism.Regions;
using System;
using System.Windows;

namespace CrlTerminal.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            try
            {
                InitializeComponent();
                this.Cursor = System.Windows.Input.Cursors.None;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.InnerException.ToString());
            }

        }
    }
}
