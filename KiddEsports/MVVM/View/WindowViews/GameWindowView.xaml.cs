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
    public partial class GameWindowView : Window
    {
        public GamesView WindowParent;
        private GameWindowViewModel context;
        DataAccess data = new DataAccess();

        /// <summary>
        /// Main constructor for this class. 
        /// Runs the Initialization method from the base class as 
        /// as well as initializing the ViewModel as the data context
        /// </summary>
        public GameWindowView(Game inputGame, GamesView windowParent)
        {
            WindowParent = windowParent;
            InitializeComponent();
            context = DataContext as GameWindowViewModel;
            context.SetCurrentGame(inputGame);

            BindingOperations.SetBinding(txtGameName, TextBox.TextProperty, QuickBind("GameName"));
            BindingOperations.SetBinding(txtGameType, TextBox.TextProperty, QuickBind("GameType"));
            BindingOperations.SetBinding(txtGameDescription, TextBox.TextProperty, QuickBind("Description"));
        }
        private Binding QuickBind(string propPath)
        {
            Binding binding = new Binding(propPath);
            binding.Source = context.CurrentGame;
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
            if (string.IsNullOrWhiteSpace(txtGameName.Text))
            {
                MessageBox.Show("Please fill all fields correctly before proceeding", "Error", MessageBoxButton.OK);
            }
            else
            {
                WindowParent.PassEntry(context.CurrentGame);
                this.Close();
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}