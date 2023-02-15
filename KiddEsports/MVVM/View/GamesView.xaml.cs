using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Data_Management;
using Data_Management.Models;
using KiddEsports.MVVM.View.WindowViews;
using KiddEsports.MVVM.ViewModel;

namespace KiddEsports.MVVM.View
{
    /// <summary>
    /// Interaction logic for GamesView.xaml
    /// </summary>
    public partial class GamesView : UserControl
    {
        DataAccess data = new DataAccess();
        private GamesViewModel context;

        /// <summary>
        /// Main constructor for this class. 
        /// Runs the Initialization method from the base class as 
        /// as well as initializing the ViewModel as the data context
        /// </summary>
        public GamesView()
        {
            InitializeComponent();
            context = DataContext as GamesViewModel;
            context.SetDataGrid(ref dgvGameGrid);
            context.UpdateDataGrid();
        }

        private void txtGameName_TextChanged(object sender, TextChangedEventArgs e)
        {
            context.SearchGameName = txtGameName.Text;
        }

        private void txtGameType_TextChanged(object sender, TextChangedEventArgs e)
        {
            context.SearchGameType = txtGameType.Text;
        }

        public void PassEntry(Game inputGame)
        {
            if (inputGame.Id == 0)
            {
                data.AddEntry(inputGame);
            }
            else
            {
                data.UpdateEntry(inputGame);
            }
            context.UpdateDataGrid();
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            GameWindowView popup = new GameWindowView(new Game(), this);
            popup.ShowDialog();
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            GameWindowView popup = new GameWindowView((Game)dgvGameGrid.SelectedItem, this);
            popup.ShowDialog();
        }

        private void cboSearchMethod_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cboSearchMethod.SelectedIndex != -1 && context != null)
            {
                context.ContainsAny = cboSearchMethod.SelectedIndex == 0;
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dgvGameGrid.SelectedItem != null)
            {
                int id = ((Game)dgvGameGrid.SelectedItem).Id;
                if (MessageBox.Show($"You are about to remove the entry with the ID of {id}. \n Do you wish to continue?",
                                    "Delete confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    if (data.DeleteEntry<Game>(id))
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
    }
}
