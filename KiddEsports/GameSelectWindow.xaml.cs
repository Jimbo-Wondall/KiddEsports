using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace KiddEsports
{
    /// <summary>
    /// Interaction logic for GameSelectWindow.xaml
    /// </summary>
    public partial class GameSelectWindow : Window
    {
        string gamesFolder = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Games\");

        /// <summary>
        /// Main constructor for this class. 
        /// Runs the Initialization method from the base class
        /// </summary>
        public GameSelectWindow()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnMinimise_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                windowBorder.Focus();
                DragMove();
            }
        }

        private void Raycaster_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(@"Games\MonoRay.exe");
        }
        private void Pong_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(@"Games\MonoRay.exe");
        }
        private void NaC_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(@"Games\Naughts and Crosses.exe");
        }
        private void GOL_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(@"Games\NewGOL.exe");
        }
        private void Snake_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(@"Games\WPF Snake.exe");
        }
        private void Minesweeper_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(@"Games\WPF_Minesweeper.exe");
        }
    }
}
