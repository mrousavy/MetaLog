using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace MetaLog.WpfTest
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private ILogger Logger { get; } =
            new MetaLogger(Utilities.RecommendedLogFile);

        private void ThrowExceptionClick(object sender, RoutedEventArgs e)
        {
            var thread = new Thread(() =>
            {
                try
                {
                    try
                    {
                        ((Button) sender).Content = "Try to change UI from Thread?.. whoops";
                    } catch (Exception inner)
                    {
                        throw new Exception("This is some outer exception", inner);
                    }
                } catch (Exception ex)
                {
                    Logger.Log(LogSeverity.Error, ex);
                }
            });
            thread.Start();
        }
    }
}