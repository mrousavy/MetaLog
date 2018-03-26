using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MetaLog.WpfTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ILogger Logger { get; } = 
            new MetaLogger(Utilities.RecommendedLogFile);

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ThrowExceptionClick(object sender, RoutedEventArgs e)
        {
            var thread = new Thread(() =>
            {
                try
                {
                    try
                    {
                        ((Button) sender).Content = "Try to change UI from Thread?.. whoops";
                    } catch(Exception inner)
                    {
                        throw new Exception("This is some outer exception", inner);
                    }
                } catch(Exception ex)
                {
                    Logger.Log(LogSeverity.Error, ex);
                }
            });
            thread.Start();
        }
    }
}
