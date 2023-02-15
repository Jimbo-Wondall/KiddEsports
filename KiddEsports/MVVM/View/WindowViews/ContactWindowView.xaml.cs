using Data_Management;
using Data_Management.Models;
using KiddEsports.MVVM.ViewModel;
using KiddEsports.MVVM.ViewModel.WindowViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace KiddEsports.MVVM.View.WindowViews
{
    /// <summary>
    /// Interaction logic for ContactWindowView.xaml
    /// </summary>
    public partial class ContactWindowView : Window
    {
        public ContactsView WindowParent;
        private ContactWindowViewModel context;
        DataAccess data = new DataAccess();

        /// <summary>
        /// Main constructor for this class. 
        /// Runs the Initialization method from the base class as 
        /// as well as initializing the ViewModel as the data context
        /// </summary>
        public ContactWindowView(ContactView inputContactView, ContactsView windowParent)
        {
            WindowParent = windowParent;
            InitializeComponent();
            context = DataContext as ContactWindowViewModel;
            context.SetCurrentContact(new Contact(inputContactView));

            BindingOperations.SetBinding(txtFirstName, TextBox.TextProperty, QuickBind("FirstName"));
            BindingOperations.SetBinding(txtLastName, TextBox.TextProperty, QuickBind("LastName"));
            BindingOperations.SetBinding(txtPhone, TextBox.TextProperty, QuickBind("Phone"));
            BindingOperations.SetBinding(txtEmail, TextBox.TextProperty, QuickBind("Email"));
            cboTeam.Text = inputContactView.TeamName;

            context.SetComboBoxes(ref cboTeam);
            context.UpdateComboBoxes();
        }

        private Binding QuickBind(string propPath)
        {
            Binding binding = new Binding(propPath);
            binding.Source = context.CurrentContact;
            return binding;
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

        private void cboTeam_TextChanged(object sender, TextChangedEventArgs e)
        {
            context.SearchTeamName = cboTeam.Text;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFirstName.Text) ||
                string.IsNullOrWhiteSpace(txtLastName.Text) ||
                string.IsNullOrWhiteSpace(txtPhone.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text) ||
                string.IsNullOrWhiteSpace(cboTeam.Text) ||
                cboTeam.SelectedItem == null)
            {
                MessageBox.Show("Please fill all fields correctly before proceeding", "Error", MessageBoxButton.OK);
            }
            else
            {
                WindowParent.PassEntry(context.CurrentContact);
                this.Close();
            }
        }
    }
}
