using Data_Management;
using Data_Management.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
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
    /// Interaction logic for ReportWindow.xaml
    /// </summary>
    public partial class ReportWindow : Window
    {
        readonly DataAccess data = new DataAccess();

        public ReportWindow()
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
                }
                catch (Exception) { }
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            // Closes the current window when the close button is clicked
            this.Close();
        }

        private void btnMinimise_Click(object sender, RoutedEventArgs e)
        {
            // Minimises the window when the custom minimise button is clicked
            WindowState = WindowState.Minimized;
        }

        private void btnReport1_Click(object sender, RoutedEventArgs e)
        {
            // Gets a list of teams from the from the database through the datamanager
            List<Team> teamList = data.GetEntries<Team>();

            // Passes what type of report we are creating
            // and a string version of the team list sorted by the amount of points to the create report method
            FileManager.CreateReport("Team Report", teamList.OrderBy(o => o.Points).Select(x => x.ToString()));
        }

        private void btnReport2_Click(object sender, RoutedEventArgs e)
        {
            // Gets a list of results from the from the database through the datamanager
            List<ResultView> resultList = data.GetEntries<Result, ResultView>();

            // Passes what type of report we are creating
            // and a string version of the result list sorted by the event name to the create report method
            FileManager.CreateReport("Results (By Event) report", resultList.OrderBy(o => o.EventName).Select(x => x.ToString()));
        }

        private void btnReport3_Click(object sender, RoutedEventArgs e)
        {
            // Gets a list of results from the from the database through the datamanager
            List<ResultView> resultList = data.GetEntries<Result, ResultView>();

            // Passes what type of report we are creating
            // and a string version of the result list sorted by the team name to the create report method
            FileManager.CreateReport("Results (By Team) report", resultList.OrderBy(o => o.Team1Name).Select(x => x.ToString()));
        }
    }
}
