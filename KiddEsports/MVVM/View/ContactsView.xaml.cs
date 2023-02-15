using Data_Management;
using Data_Management.Models;
using KiddEsports.MVVM.View.WindowViews;
using KiddEsports.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for ResultsView.xaml
    /// </summary>
    public partial class ContactsView : UserControl
    {
        private ContactsViewModel context;
        private DataAccess data = new DataAccess();

        /// <summary>
        /// Main constructor for this class. 
        /// Runs the Initialization method from the base class as 
        /// as well as initializing the ViewModel as the data context
        /// </summary>
        public ContactsView()
        {
            InitializeComponent();
            context = DataContext as ContactsViewModel;
            context.SetDataGrid(ref dgvContactGrid);
            context.UpdateDataGrid();
        }

        private void txtName_TextChanged(object sender, TextChangedEventArgs e)
        {
            context.SearchName = txtName.Text;
        }

        private void txtPhone_TextChanged(object sender, TextChangedEventArgs e)
        {
            context.SearchPhone = txtPhone.Text;
        }

        private void txtEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            context.SearchEmail = txtEmail.Text;
        }

        private void txtTeamName_TextChanged(object sender, TextChangedEventArgs e)
        {
            context.SearchTeamName = txtTeamName.Text;
        }

        private void cboSearchMethod_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cboSearchMethod.SelectedIndex != -1 && context != null)
            {
                context.ContainsAny = cboSearchMethod.SelectedIndex == 0;
            }
        }

        public void PassEntry(Contact contact)
        {
            if (contact.Id == 0)
            {
                data.AddEntry(contact);
            }
            else
            {
                data.UpdateEntry(contact);
            }
            context.UpdateDataGrid();
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            ContactWindowView popup = new ContactWindowView((ContactView)dgvContactGrid.SelectedItem, this);
            popup.ShowDialog();
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            ContactWindowView popup = new ContactWindowView(new ContactView(), this);
            popup.ShowDialog();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dgvContactGrid.SelectedItem != null)
            {
                int id = ((ContactView)dgvContactGrid.SelectedItem).Id;
                if (MessageBox.Show($"You are about to remove the entry with the ID of {id}. \n Do you wish to continue?",
                                    "Delete confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    if (data.DeleteEntry<Contact>(id))
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
