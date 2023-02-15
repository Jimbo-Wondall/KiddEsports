using Data_Management;
using Data_Management.Models;
using KiddEsports.MVVM.ViewModel;
using KiddEsports.MVVM.ViewModel.WindowViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace KiddEsports.MVVM.View.WindowViews
{
    /// <summary>
    /// Interaction logic for ResultsWindow.xaml
    /// </summary>
    public partial class ResultsWindowView : Window
    {
        ResultsView WindowParent;
        private ResultsWindowViewModel context;
        private DataAccess data = new DataAccess();

        /// <summary>
        /// Main constructor for this class. 
        /// Runs the Initialization method from the base class as 
        /// as well as initializing the ViewModel as the data context
        /// </summary>
        public ResultsWindowView(ResultView inputResultView, ResultsView windowParent)
        {
            WindowParent = windowParent;
            InitializeComponent();
            context = DataContext as ResultsWindowViewModel;
            context.SetComboBoxes(ref cboTeam1, ref cboTeam2, ref cboGameType, ref cboEvent);
            Game game;
            context.CurrentResult = inputResultView.Id == 0 ? new Result() : data.GetEntryById<Result>(inputResultView.Id);
            cboTeam1.Text = inputResultView.Team1Name;
            cboTeam2.Text = inputResultView.Team2Name;
            cboGameType.Text = (game = data.GetEntryById<Game>(context.CurrentResult.fkGameType_Id)) == null ? "" : game.GameType;
            cboEvent.Text = inputResultView.EventName;

            context.UpdateComboBoxes();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                windowBorder.Focus();
                DragMove();
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnMinimise_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void cboTeam1_TextChanged(object sender, TextChangedEventArgs e)
        {
            context.SearchTeam1Name = cboTeam1.Text;
        }

        private void cboTeam2_TextChanged(object sender, TextChangedEventArgs e)
        {
            context.SearchTeam2Name = cboTeam2.Text;
        }

        private void cboGameType_TextChanged(object sender, TextChangedEventArgs e)
        {
            context.SearchGameType = cboGameType.Text;
        }

        private void cboEvent_TextChanged(object sender, TextChangedEventArgs e)
        {
            context.SearchEventName = cboEvent.Text;
        }

        private void btnTeam1Win_Click(object sender, RoutedEventArgs e)
        {
            CreateResult(1);
        }

        private void btnDraw_Click(object sender, RoutedEventArgs e)
        {
            CreateResult(0);
        }

        private void btnTeam2Win_Click(object sender, RoutedEventArgs e)
        {
            CreateResult(2);
        }

        private void CreateResult(byte resultValue)
        {
            context.CurrentResult.ResultValue = resultValue;

            Game gameType;
            Team team1;
            Team team2;
            Event @event;



            if ((gameType = (Game)cboGameType.SelectedItem) == null ||
                (team1 = (Team)cboTeam1.SelectedItem) == null ||
                (team2 = (Team)cboTeam2.SelectedItem) == null ||
                (@event = (Event)cboEvent.SelectedItem) == null)
            {
                MessageBox.Show("Please fill all fields correctly before proceeding", "Error", MessageBoxButton.OK);
            }
            else
            {
                Result result = context.CurrentResult;

                string points1 = "";
                string points2 = "";
                string resultString = "";

                switch (resultValue)
                {
                    case 0:
                        resultString = "the result was a draw";
                        points1 = "1 point";
                        points2 = "1 point";
                        break;
                    case 1:
                        resultString = $"{team1.TeamName} won";
                        points1 = "2 points";
                        points2 = "0 points";
                        break;
                    case 2:
                        resultString = $"{team2.TeamName} won";
                        points1 = "0 points";
                        points2 = "2 points";
                        break;
                    default:
                        break;
                }

                MessageBoxResult messageBoxResult;
                if (result.Id == 0)
                {
                    messageBoxResult = MessageBox.Show(
                        $"The result you are about to create is between {team1.TeamName} and {team2.TeamName} where {resultString}.\n" +
                        $"Selecting 'Yes' will add {points1} to {team1.TeamName}, and {points2} to {team2.TeamName}.\n" +
                        "Do you wish to continue?", "New match result confirmation", MessageBoxButton.YesNo);
                }
                else
                {
                    messageBoxResult = MessageBox.Show(
                        $"You are about to update this result to be between {team1.TeamName} and {team2.TeamName} where {resultString}.\n" +
                        $"Selecting 'Yes' will adjust both teams points permanently.\n" +
                        "Do you wish to continue?", "Update match result confirmation", MessageBoxButton.YesNo);
                }

                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    WindowParent.PassEntry(result);
                    this.Close();
                }
            }
        }
    }
}
