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
    class ContactWindowViewModel
    {
        public Contact CurrentContact { get; set; }

        private DataAccess data = new DataAccess();
        public ComboBox cboTeam;

        public ObservableCollection<Team> teamList { get; set; }
        public ObservableCollection<Team> filteredTeamList { get; set; }

        private string searchTeamName;
        public string SearchTeamName
        {
            get => searchTeamName;
            set
            {
                searchTeamName = value.ToString().ToUpper();
                TeamFieldUpdated();
            }
        }

        public void SetCurrentContact(Contact inputContact)
        {
            CurrentContact = inputContact;
        }

        public void SetComboBoxes(ref ComboBox cboBox)
        {
            cboTeam = cboBox;
        }

        public void UpdateComboBoxes()
        {
            teamList = new ObservableCollection<Team>(data.GetEntries<Team>());
            ResetTeamBox();
        }
        private void ResetTeamBox()
        {
            cboTeam.ItemsSource = teamList;
            cboTeam.Items.Refresh();
        }

        /// <summary>
        /// This method is triggered whenever the team combo box is edited 
        /// and sets the itemsource to a filtered verson of the team list based 
        /// on what entries match what is in the combo box
        /// </summary>
        private void TeamFieldUpdated()
        {
            filteredTeamList = new ObservableCollection<Team>();
            if (string.IsNullOrWhiteSpace(searchTeamName))
            {
                ResetTeamBox();
            }
            else
            {
                foreach (var team in teamList)
                {
                    if (team.TeamName.ToUpper().Contains(searchTeamName))
                    {
                        filteredTeamList.Add(team);
                    }
                }
                cboTeam.ItemsSource = filteredTeamList;
                cboTeam.Items.Refresh();
                if (cboTeam.SelectedItem == null)
                {
                    cboTeam.SelectedIndex = 0;
                }
                CurrentContact.fkTeam_Id = ((Team)cboTeam.SelectedItem).Id;
            }
        }
    }
}

