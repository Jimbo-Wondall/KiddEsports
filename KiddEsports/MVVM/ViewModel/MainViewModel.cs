using KiddEsports.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace KiddEsports.MVVM.ViewModel
{
    class MainViewModel : ObservableObject
    {

        public RelayCommand TeamViewCommand { get; set; }
        public RelayCommand EventViewCommand { get; set; }
        public RelayCommand GameViewCommand { get; set; }
        public RelayCommand ContactViewCommand { get; set; }
        public RelayCommand ResultViewCommand { get; set; }

        public EventsViewModel EventVM { get; set; }
        public TeamsViewModel TeamVM { get; set; }
        public GamesViewModel GameVM { get; set; }
        public ContactsViewModel ContactVM { get; set; }
        public ResultsViewModel ResultVM { get; set; }

        private object _currentView;

        public object CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }

        public MainViewModel()
        {
            TeamVM = new TeamsViewModel();
            EventVM = new EventsViewModel();
            GameVM = new GamesViewModel();
            ContactVM = new ContactsViewModel();
            ResultVM = new ResultsViewModel();

            TeamViewCommand = new RelayCommand(o =>
            {
                CurrentView = TeamVM;
            });

            EventViewCommand = new RelayCommand(o =>
            {
                CurrentView = EventVM;
            });

            GameViewCommand = new RelayCommand(o =>
            {
                CurrentView = GameVM;
            });

            ContactViewCommand = new RelayCommand(o =>
            {
                CurrentView = ContactVM;
            });

            ResultViewCommand = new RelayCommand(o =>
            {
                CurrentView = ResultVM;
            });
        }
    }
}
