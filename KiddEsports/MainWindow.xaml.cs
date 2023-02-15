using System;
using System.Windows;
using System.Windows.Input;

namespace KiddEsports
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Main constructor for this class. 
        /// Runs the Initialization method from the base class
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // Makes it so the user is able to drag the window around 
            // by clicking and dragging on anything with this click event
            if (e.ChangedButton == MouseButton.Left)
            {
                windowBorder.Focus();
                try
                {
                    DragMove();
                } catch (Exception) { }
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            // Completely shuts down the application when the close button is clicked
            Application.Current.Shutdown();
        }

        private void btnMinimise_Click(object sender, RoutedEventArgs e)
        {
            // Minimises the window when the custom minimise button is clicked
            WindowState = WindowState.Minimized;
        }

        private void btnGame_Click(object sender, RoutedEventArgs e)
        {
            // Opens the entertainment window
            GameSelectWindow window = new GameSelectWindow();
            window.Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Opens the reports window
            ReportWindow window = new ReportWindow();
            window.Show();
        }
    }
}
