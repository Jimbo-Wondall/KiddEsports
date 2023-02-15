using KiddEsports.Core;
using System;
using System.Collections.Generic;
using System.Text;
using Data_Management;
using Data_Management.Models;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Data;

namespace KiddEsports.MVVM.ViewModel
{
    class TeamsViewModel : ObservableObject
    {
        private DataAccess data = new DataAccess();
        private DataGrid dataGrid;
        public ObservableCollection<Team> teamList { get; set; }
        public ObservableCollection<Team> filteredTeamList { get; set; }

        private string searchTeamName;
        public string SearchTeamName
        {
            get => searchTeamName;
            set
            {
                searchTeamName = value.ToString().ToUpper();
                SearchFieldsUpdated();
            }
        }

        /// <summary>
        /// Initializes a local datagrid variable to set and manipulate the data shown in it easier
        /// </summary>
        /// <param name="dgvInput"></param>
        public void SetDataGrid(ref DataGrid dgvInput)
        {
            dataGrid = dgvInput;
        }

        /// <summary>
        /// Sends a query to the database to get all entries and puts it into an 
        /// observable collection, which is then set as the datagrids itemsource
        /// </summary>
        public void UpdateDataGrid()
        {
            teamList = new ObservableCollection<Team>(data.GetEntries<Team>());
            dataGrid.ItemsSource = CollectionViewSource.GetDefaultView(teamList);
            dataGrid.Items.Refresh();
        }

        /// <summary>
        /// Creates a second list of entries and copies any entries that satisfy the search conditions over 
        /// from the original list, then sets the datagrids itemsource to this new filtered list.
        /// </summary>
        private void SearchFieldsUpdated()
        {
            // Creates a second team list that will only hold entries that match the seach condition
            filteredTeamList = new ObservableCollection<Team>();

            // Checks if the seach field is empty, if it is the datagrid will be reset, otherwise it will be filtered
            if (string.IsNullOrWhiteSpace(searchTeamName))
            {
                UpdateDataGrid();
            }
            else
            {
                foreach (var team in teamList)
                {
                    if (!string.IsNullOrWhiteSpace(searchTeamName))
                    {
                        if (team.TeamName.ToUpper().Contains(searchTeamName))
                        {
                            filteredTeamList.Add(team);
                        }
                    }
                }
                dataGrid.ItemsSource = CollectionViewSource.GetDefaultView(filteredTeamList);
                dataGrid.Items.Refresh();
            }
        }
    }
}

