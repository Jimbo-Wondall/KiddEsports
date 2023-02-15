using Data_Management;
using Data_Management.Models;
using KiddEsports.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Controls;

namespace KiddEsports.MVVM.ViewModel.WindowViewModels
{
    class ResultsWindowViewModel
    {
        public Result CurrentResult { get; set; }

        private DataAccess data = new DataAccess();
        public ComboBox cboTeam1;
        public ComboBox cboTeam2;
        public ComboBox cboGameType;
        public ComboBox cboEvent;

        public ObservableCollection<Team> teamList { get; set; }
        public ObservableCollection<Team> filteredResultList1 { get; set; }
        public ObservableCollection<Team> filteredResultList2 { get; set; }

        public ObservableCollection<Game> gameList { get; set; }
        public ObservableCollection<Game> filteredGameList { get; set; }

        public ObservableCollection<Event> eventList { get; set; }
        public ObservableCollection<Event> filteredEventList { get; set; }

        private string searchTeam1Name;
        public string SearchTeam1Name
        {
            get => searchTeam1Name;
            set
            {
                searchTeam1Name = value == null ? "" : value.ToString().ToUpper();
                Team1FieldUpdated();
            }
        }

        private string searchTeam2Name;
        public string SearchTeam2Name
        {
            get => searchTeam2Name;
            set
            {
                searchTeam2Name = value == null ? "" : value.ToString().ToUpper();
                Team2FieldUpdated();
            }
        }

        private string searchGameType;
        public string SearchGameType
        {
            get => searchGameType;
            set
            {
                searchGameType = value == null ? "" : value.ToString().ToUpper();
                GameNameFieldUpdated();
            }
        }

        private string searchEventName;
        public string SearchEventName
        {
            get => searchEventName;
            set
            {
                searchEventName = value == null ? "" : value.ToString().ToUpper();
                EventNameFieldUpdated();
            }
        }

        /// <summary>
        /// Sets the combo boxes to a reference of the real ones on the screen 
        /// so they can be easily referenced within this class
        /// </summary>
        /// <param name="cboTeam1Input"></param>
        /// <param name="cboTeam2Input"></param>
        /// <param name="cboGameTypeInput"></param>
        /// <param name="cboEventInput"></param>
        public void SetComboBoxes(
            ref ComboBox cboTeam1Input, ref ComboBox cboTeam2Input,
            ref ComboBox cboGameTypeInput, ref ComboBox cboEventInput)
        {
            cboTeam1 = cboTeam1Input;
            cboTeam2 = cboTeam2Input;
            cboGameType = cboGameTypeInput;
            cboEvent = cboEventInput;
        }

        public void UpdateComboBoxes()
        {
            teamList = new ObservableCollection<Team>(data.GetEntries<Team>());
            gameList = new ObservableCollection<Game>(data.GetEntries<Game>());
            eventList = new ObservableCollection<Event>(data.GetEntries<Event>());
            ResetBox1();
            ResetBox2();
            ResetBox3();
            ResetBox4();
        }

        private void ResetBox1()
        {
            cboTeam1.ItemsSource = teamList;
            cboTeam1.Items.Refresh();
        }
        private void ResetBox2()
        {
            cboTeam2.ItemsSource = teamList;
            cboTeam2.Items.Refresh();
        }
        private void ResetBox3()
        {
            cboGameType.ItemsSource = gameList;
            cboGameType.Items.Refresh();
        }
        private void ResetBox4()
        {
            cboEvent.ItemsSource = eventList;
            cboEvent.Items.Refresh();
        }

        /// <summary>
        /// This method is triggered whenever the team 1 combo box is edited 
        /// and sets the itemsource to a filtered verson of the team list based 
        /// on what entries match what is in the combo box
        /// </summary>
        private void Team1FieldUpdated()
        {
            filteredResultList1 = new ObservableCollection<Team>();
            if (string.IsNullOrWhiteSpace(searchTeam1Name))
            {
                ResetBox1();
            }
            else
            {
                foreach (var team in teamList)
                {
                    if (team.TeamName.ToUpper().Contains(searchTeam1Name))
                    {
                        filteredResultList1.Add(team);
                    }
                }
                cboTeam1.ItemsSource = filteredResultList1;
                cboTeam1.Items.Refresh();
                if (cboTeam1.SelectedItem == null)
                {
                    cboTeam1.SelectedIndex = 0;
                }
                CurrentResult.fkTeam1_Id = ((Team)cboTeam1.SelectedItem).Id;
            }
        }

        /// <summary>
        /// This method is triggered whenever the team 2 combo box is edited        
        /// and sets the itemsource to a filtered verson of the team list based 
        /// on what entries match what is in the combo box
        /// </summary>
        private void Team2FieldUpdated()
        {
            filteredResultList2 = new ObservableCollection<Team>();
            if (string.IsNullOrWhiteSpace(searchTeam2Name))
            {
                ResetBox2();
            }
            else
            {
                foreach (var team in teamList)
                {
                    if (team.TeamName.ToUpper().Contains(searchTeam2Name))
                    {
                        filteredResultList2.Add(team);
                    }
                }
                cboTeam2.ItemsSource = filteredResultList2;
                cboTeam2.Items.Refresh();
                if (cboTeam2.SelectedItem == null)
                {
                    cboTeam2.SelectedIndex = 0;
                }
                CurrentResult.fkTeam2_Id = ((Team)cboTeam2.SelectedItem).Id;
            }
        }

        /// <summary>
        /// This method is triggered whenever the game type combo box is edited
        /// and sets the itemsource to a filtered verson of the game list based 
        /// on what entries match what is in the combo box
        /// </summary>
        private void GameNameFieldUpdated()
        {
            filteredGameList = new ObservableCollection<Game>();
            if (string.IsNullOrWhiteSpace(searchGameType))
            {
                ResetBox3();
            }
            else
            {
                foreach (var game in gameList)
                {
                    if (game.GameType.ToUpper().Contains(searchGameType) ||
                        game.GameName.ToUpper().Contains(searchGameType))
                    {
                        filteredGameList.Add(game);
                    }
                }
                cboGameType.ItemsSource = filteredGameList;
                cboGameType.Items.Refresh();

                if (cboGameType.SelectedItem == null)
                {
                    cboGameType.SelectedIndex = 0;
                }
                CurrentResult.fkGameType_Id = ((Game)cboGameType.SelectedItem).Id;
            }
        }

        /// <summary>
        /// This method is triggered whenever the event combo box is edited
        /// and sets the itemsource to a filtered verson of the event list based 
        /// on what entries match what is in the combo box
        /// </summary>
        private void EventNameFieldUpdated()
        {
            filteredEventList = new ObservableCollection<Event>();
            if (string.IsNullOrWhiteSpace(searchEventName))
            {
                ResetBox4();
            }
            else
            {
                foreach (var @event in eventList)
                {
                    if (@event.EventName.ToUpper().Contains(searchEventName) ||
                        @event.EventLocation.ToUpper().Contains(searchEventName))
                    {
                        filteredEventList.Add(@event);
                    }
                }
                cboEvent.ItemsSource = filteredEventList;
                cboEvent.Items.Refresh();

                if (cboEvent.SelectedItem == null)
                {
                    cboEvent.SelectedIndex = 0;
                }
                CurrentResult.fkEvent_Id = ((Event)cboEvent.SelectedItem).Id;
            }
        }
    }
}

