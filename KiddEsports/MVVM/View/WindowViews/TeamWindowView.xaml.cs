using Data_Management;
using Data_Management.Models;
using KiddEsports.MVVM.ViewModel.WindowViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace KiddEsports.MVVM.View.WindowViews
{
    /// <summary>
    /// Interaction logic for GameWindowView.xaml
    /// </summary>
    public partial class TeamWindowView : Window
    {
        public TeamsView WindowParent;
        private TeamWindowViewModel context;
        DataAccess data = new DataAccess();

        /// <summary>
        /// Main constructor for this class. 
        /// Runs the Initialization method from the base class as 
        /// as well as initializing the ViewModel as the data context
        /// </summary>
        public TeamWindowView(Team inputTeam, TeamsView windowParent)
        {
            WindowParent = windowParent;
            InitializeComponent();
            context = DataContext as TeamWindowViewModel;
            context.SetCurrentTeam(inputTeam);

            BindingOperations.SetBinding(txtTeamName, TextBox.TextProperty, QuickBind("TeamName"));
        }
        private Binding QuickBind(string propPath)
        {
            Binding binding = new Binding(propPath);
            binding.Source = context.CurrentTeam;
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

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTeamName.Text))
            {
                MessageBox.Show("Please fill all fields correctly before proceeding", "Error", MessageBoxButton.OK);
            }
            else
            {
                WindowParent.PassEntry(context.CurrentTeam);
                this.Close();
            }
        }
    }
}