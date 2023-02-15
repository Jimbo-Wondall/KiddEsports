using KiddEsports.Core;
using System;
using System.Collections.Generic;
using System.Text;
using Data_Management;
using Data_Management.Models;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace KiddEsports.MVVM.ViewModel
{
    class GamesViewModel : ObservableObject
    {
        private DataAccess data = new DataAccess();
        private DataGrid dataGrid;
        public ObservableCollection<Game> gameList { get; set; }
        public ObservableCollection<Game> filteredGameList { get; set; }

        private bool containsAny;
        public bool ContainsAny
        {
            get => containsAny;
            set
            {
                containsAny = value;
                SearchFieldsUpdated();
            }
        }

        private string searchGameName;
        public string SearchGameName
        {
            get => searchGameName;
            set
            {
                searchGameName = value.ToString().ToUpper();
                SearchFieldsUpdated();
            }
        }

        private string searchGameType;
        public string SearchGameType
        {
            get => searchGameType;
            set
            {
                searchGameType = value.ToString().ToUpper();
                SearchFieldsUpdated();
            }
        }

        public void SetDataGrid(ref DataGrid dgvInput)
        {
            dataGrid = dgvInput;
        }

        public void UpdateDataGrid()
        {
            gameList = new ObservableCollection<Game>(data.GetEntries<Game>());
            dataGrid.ItemsSource = gameList;
            dataGrid.Items.Refresh();
        }

        private void SearchFieldsUpdated()
        {
            filteredGameList = new ObservableCollection<Game>();
            if (string.IsNullOrWhiteSpace(searchGameName) &&
                string.IsNullOrWhiteSpace(searchGameType))
            {
                UpdateDataGrid();
            }
            else
            {
                foreach (var game in gameList)
                {
                    // Two integer variables are created to keep a tally of how many fields have text in them
                    // and how many parameters in the current entry match their corresponding field
                    int expected = 0;
                    int contains = 0;

                    if (!string.IsNullOrWhiteSpace(searchGameName))
                    {
                        expected++;
                        if (game.GameName.ToUpper().Contains(searchGameName))
                        {
                            contains++;
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(searchGameType))
                    {
                        expected++;
                        if (game.GameType.ToUpper().Contains(searchGameType))
                        {
                            contains++;
                        }
                    }

                    if (contains == expected)
                    {
                        filteredGameList.Add(game);
                    }
                }
                dataGrid.ItemsSource = filteredGameList;
                dataGrid.Items.Refresh();
            }
        }
    }
}

