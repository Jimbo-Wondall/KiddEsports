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
    /// Interaction logic for TeamsView.xaml
    /// </summary>
    public partial class EventsView : UserControl
    {
        DataAccess data = new DataAccess();
        private EventsViewModel context;

        /// <summary>
        /// Main constructor for this class. 
        /// Runs the Initialization method from the base class as 
        /// as well as initializing the ViewModel as the data context
        /// </summary>
        public EventsView()
        {
            InitializeComponent();
            context = DataContext as EventsViewModel;
            context.SetDataGrid(ref dgvEventGrid);
            context.UpdateDataGrid();
        }

        private void txtEventName_TextChanged(object sender, TextChangedEventArgs e)
        {
            context.SearchEventName = txtEventName.Text;
        }

        private void txtEventLocation_TextChanged(object sender, TextChangedEventArgs e)
        {
            context.SearchEventLocation = txtEventLocation.Text;
        }
        private void cboSearchMethod_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cboSearchMethod.SelectedIndex != -1 && context != null)
            {
                context.ContainsAny = cboSearchMethod.SelectedIndex == 0;
            }
        }

        public void PassEntry(Event inputEvent)
        {
            if (inputEvent.Id == 0)
            {
                data.AddEntry(inputEvent);
            }
            else
            {
                data.UpdateEntry(inputEvent);
            }
            context.UpdateDataGrid();
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            EventWindowView popup = new EventWindowView(new Event(), this);
            popup.ShowDialog();

        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            EventWindowView popup = new EventWindowView((Event)dgvEventGrid.SelectedItem, this);
            popup.ShowDialog();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dgvEventGrid.SelectedItem != null)
            {
                int id = ((Event)dgvEventGrid.SelectedItem).Id;
                if (MessageBox.Show($"You are about to remove the entry with the ID of {id}. \n Do you wish to continue?",
                                    "Delete confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    if (data.DeleteEntry<Event>(id))
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
