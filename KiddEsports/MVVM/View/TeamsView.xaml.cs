using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Data_Management;
using Data_Management.Models;
using KiddEsports.MVVM.View.WindowViews;
using KiddEsports.MVVM.ViewModel;

namespace KiddEsports.MVVM.View
{
    /// <summary>
    /// Interaction logic for TeamsView.xaml
    /// </summary>
    public partial class TeamsView : UserControl
    {
        DataAccess data = new DataAccess();
        private TeamsViewModel context;

        /// <summary>
        /// Main constructor for this class. 
        /// Runs the Initialization method from the base class as 
        /// as well as initializing the ViewModel as the data context
        /// </summary>
        public TeamsView()
        {
            InitializeComponent();
            context = DataContext as TeamsViewModel;
            context.SetDataGrid(ref dgvTeamGrid);
            context.UpdateDataGrid();
        }

        public void PassEntry(Team inputTeam)
        {
            if (inputTeam.Id == 0)
            {
                data.AddEntry(inputTeam);
            }
            else
            {
                data.UpdateEntry(inputTeam);
            }
            context.UpdateDataGrid();
        }

        private void txtTeamName_TextChanged(object sender, TextChangedEventArgs e)
        {
            context.SearchTeamName = txtTeamName.Text;
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            TeamWindowView popup = new TeamWindowView(new Team(), this);
            popup.ShowDialog();
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            TeamWindowView popup = new TeamWindowView((Team)dgvTeamGrid.SelectedItem, this);
            popup.ShowDialog();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dgvTeamGrid.SelectedItem != null)
            {
                int id = ((Team)dgvTeamGrid.SelectedItem).Id;
                if (MessageBox.Show($"You are about to remove the entry with the ID of {id}. \n Do you wish to continue?",
                                    "Delete confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    if (data.DeleteEntry<Team>(id))
                    {
                        context.UpdateDataGrid();
                    }
                    else
                    {
                        MessageBox.Show($"Could not delete selected entry", "Error", MessageBoxButton.OK);
                    }
                }
            }
        }
        private void btnReport_Click(object sender, RoutedEventArgs e)
        {
            // Passes what type of report we are creating
            // and a string version of the currently displayed team list to the create report method
            FileManager.CreateReport("Team details report", dgvTeamGrid.ItemsSource.OfType<Team>().Select(x => x.ToString()));
        }
    }
}