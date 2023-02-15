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
    /// Interaction logic for EventWindowView.xaml
    /// </summary>
    public partial class EventWindowView : Window
    {
        public EventsView WindowParent;
        private EventWindowViewModel context;
        DataAccess data = new DataAccess();

        /// <summary>
        /// Main constructor for this class. 
        /// Runs the Initialization method from the base class as 
        /// as well as initializing the ViewModel as the data context
        /// </summary>
        public EventWindowView(Event inputEvent, EventsView windowParent)
        {
            WindowParent = windowParent;
            InitializeComponent();
            context = DataContext as EventWindowViewModel;
            context.SetCurrentEvent(inputEvent);

            BindingOperations.SetBinding(txtEventName, TextBox.TextProperty, QuickBind("EventName"));
            BindingOperations.SetBinding(txtEventLocation, TextBox.TextProperty, QuickBind("EventLocation"));
            BindingOperations.SetBinding(datePicker, DatePicker.SelectedDateProperty, QuickBind("EventDate"));
        }

        private Binding QuickBind(string propPath)
        {
            Binding binding = new Binding(propPath);
            binding.Source = context.CurrentEvent;
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
            if (string.IsNullOrWhiteSpace(txtEventName.Text) ||
                string.IsNullOrWhiteSpace(txtEventLocation.Text) ||
                string.IsNullOrWhiteSpace(datePicker.Text))
            {
                MessageBox.Show("Please fill all fields correctly before proceeding", "Error", MessageBoxButton.OK);
            }
            else
            {
                WindowParent.PassEntry(context.CurrentEvent);
                this.Close();
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            txtEventName.Text = "";
            txtEventLocation.Text = "";
            datePicker.SelectedDate = null;
        }
    }
}
