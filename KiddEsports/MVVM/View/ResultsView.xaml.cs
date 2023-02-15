using Data_Management;
using Data_Management.Models;
using KiddEsports.MVVM.View.WindowViews;
using KiddEsports.MVVM.ViewModel;
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

namespace KiddEsports.MVVM.View
{
    /// <summary>
    /// Interaction logic for GamesView.xaml
    /// </summary>
    public partial class ResultsView : UserControl
    {
        DataAccess data = new DataAccess();
        private ResultsViewModel context;

        /// <summary>
        /// Main constructor for this class. 
        /// Runs the Initialization method from the base class as 
        /// as well as initializing the ViewModel as the data context
        /// </summary>
        public ResultsView()
        {
            InitializeComponent();
            context = DataContext as ResultsViewModel;
            context.SetDataGrid(ref dgvResultGrid);
            context.UpdateDataGrid();
        }

        private void btnUpdateGrid_Click(object sender, RoutedEventArgs e)
        {
            context.UpdateDataGrid();
        }

        private void txtTeam1_TextChanged(object sender, TextChangedEventArgs e)
        {
            context.SearchTeam1Name = txtTeam1.Text;
        }
        
        private void txtTeam2_TextChanged(object sender, TextChangedEventArgs e)
        {
            context.SearchTeam2Name = txtTeam2.Text;
        }

        private void txtEvent_TextChanged(object sender, TextChangedEventArgs e)
        {
            context.SearchEventName = txtEvent.Text;
        }

        private void txtGameName_TextChanged(object sender, TextChangedEventArgs e)
        {
            context.SearchGameName = txtGameName.Text;
        }

        private void cboSearchMethod_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cboSearchMethod.SelectedIndex != -1 && context != null)
            {
                context.ContainsAny = cboSearchMethod.SelectedIndex == 0;
            }
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            ResultsWindowView popup = new ResultsWindowView(new ResultView(), this);
            popup.ShowDialog();
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            ResultsWindowView popup = new ResultsWindowView((ResultView)dgvResultGrid.SelectedItem, this);
            popup.ShowDialog();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dgvResultGrid.SelectedItem != null)
            {
                int id = ((ResultView)dgvResultGrid.SelectedItem).Id;
                MessageBoxResult boxresult = MessageBox.Show(
                    $"You are about to remove the result with the ID of {id}. \n " +
                    $"Would you like to remove the points given by this result?",
                    "Delete confirmation", MessageBoxButton.YesNoCancel);
                switch (boxresult)
                {
                    case MessageBoxResult.Cancel:
                        break;
                    case MessageBoxResult.Yes:
                        data.RemoveResultEffects(id);
                        data.DeleteEntry<Result>(id);
                        break;
                    case MessageBoxResult.No:
                        data.DeleteEntry<Result>(id);
                        break;
                    default:
                        break;
                }
                context.UpdateDataGrid();
            }
        }

        private void btnReport_Click(object sender, RoutedEventArgs e)
        {
            // Passes what type of report we are creating
            // and a string version of the currently displayed result list to the create report method
            FileManager.CreateReport("Results report", dgvResultGrid.ItemsSource.OfType<ResultView>().Select(x => x.ToString()));
        }

        public void PassEntry(Result result)
        {
            if (result.Id == 0)
            {
                data.NewResultTransaction(result);
            }
            else
            {
                MessageBoxResult boxresult = MessageBox.Show(
                    $"You are about to update the result with the ID of {result.Id}. \n " +
                    $"Would you also like to update the relevant teams points accordingly?",
                    "Update confirmation", MessageBoxButton.YesNoCancel);
                switch (boxresult)
                {
                    case MessageBoxResult.Cancel:
                        break;
                    case MessageBoxResult.Yes:
                        data.RemoveResultEffects(result.Id);
                        data.UpdateResultTransaction(result);
                        break;
                    case MessageBoxResult.No:
                        data.UpdateResultTransaction(result);
                        break;
                    default:
                        break;
                }
            }
            context.UpdateDataGrid();
        }

        private void cboResult_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cboResult.SelectedIndex != -1 && context != null)
            {
                context.ResultValue = cboResult.SelectedIndex - 1;
            }
        }
    }
}

