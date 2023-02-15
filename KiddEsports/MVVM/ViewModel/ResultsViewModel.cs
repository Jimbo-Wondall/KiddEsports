using Data_Management;
using Data_Management.Models;
using KiddEsports.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Data;

namespace KiddEsports.MVVM.ViewModel
{
    class ResultsViewModel : ObservableObject
    {
        private DataAccess data = new DataAccess();
        private DataGrid dataGrid;
        public ObservableCollection<ResultView> resultList { get; set; }
        public ObservableCollection<ResultView> filteredResultList { get; set; }


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

        private string searchValue;
        public int ResultValue
        {
            get
            {
                switch (searchValue)
                {
                    case "Draw":
                        return 0;
                    case "Team 1 Won":
                        return 1;
                    case "Team 2 Won":
                        return 2;
                    default:
                        return -1;
                }
            }
            set
            {
                searchValue = value switch
                {
                    0 => "Draw",
                    1 => "Team 1 Won",
                    2 => "Team 2 Won",
                    _ => "",
                };
                SearchFieldsUpdated();
            }
        }

        private string searchTeam1Name;
        public string SearchTeam1Name
        {
            get => searchTeam1Name;
            set
            {
                searchTeam1Name = value.ToString().ToUpper();
                SearchFieldsUpdated();
            }
        }

        private string searchTeam2Name;
        public string SearchTeam2Name
        {
            get => searchTeam2Name;
            set
            {
                searchTeam2Name = value.ToString().ToUpper();
                SearchFieldsUpdated();
            }
        }

        private string searchEventName;
        public string SearchEventName
        {
            get => searchEventName;
            set
            {
                searchEventName = value.ToString().ToUpper();
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

        public void SetDataGrid(ref DataGrid dgvInput)
        {
            dataGrid = dgvInput;
        }

        public void UpdateDataGrid()
        {
            resultList = new ObservableCollection<ResultView>(data.GetEntries<Result, ResultView>());
            dataGrid.ItemsSource = CollectionViewSource.GetDefaultView(resultList);
            dataGrid.Items.Refresh();
        }

        /// <summary>
        /// Creates a second list of entries and copies any entries that satisfy the search conditions over 
        /// from the original list, then sets the datagrids itemsource to this new filtered list.
        /// </summary>
        private void SearchFieldsUpdated()
        {
            // Creates a second copy of the result list
            filteredResultList = new ObservableCollection<ResultView>();

            // Checks if all of the input fields are empty, if they are the datagrid is reset
            // if they aren't, it will be filtered based on what is there
            if (string.IsNullOrWhiteSpace(searchTeam1Name) &&
                string.IsNullOrWhiteSpace(searchTeam2Name) &&
                string.IsNullOrWhiteSpace(searchEventName) &&
                string.IsNullOrWhiteSpace(searchGameName) &&
                string.IsNullOrWhiteSpace(searchValue))
            {
                UpdateDataGrid();
            }
            else
            {
                foreach (var result in resultList)
                {
                    // Two integer variables are created to keep a tally of how many fields have text in them
                    // and how many parameters in the current entry match their corresponding field
                    int contains = 0;
                    int expected = 0;

                    // Checks if the field is not empty
                    if (!string.IsNullOrWhiteSpace(searchTeam1Name))
                    {
                        expected++;
                        // Checks if either of the team names in the current result contain what was searched for
                        if (result.Team1Name.ToUpper().Contains(searchTeam1Name) ||
                            result.Team2Name.ToUpper().Contains(searchTeam1Name))
                        {
                            contains++;
                            if (!string.IsNullOrWhiteSpace(searchTeam2Name))
                            {
                                expected++;
                                if (result.Team1Name.ToUpper().Contains(searchTeam2Name) ||
                                    result.Team2Name.ToUpper().Contains(searchTeam2Name))
                                {
                                    contains++;
                                }
                            }
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(searchValue))
                    {
                        expected++;
                        if (result.Result == searchValue)
                        {
                            contains++;
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(searchEventName))
                    {
                        expected++;
                        if (result.EventName.ToUpper().Contains(searchEventName))
                        {
                            contains++;
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(searchGameName))
                    {
                        expected++;
                        if (result.GameName.ToUpper().Contains(searchGameName))
                        {
                            contains++;
                        }
                    }

                    if (containsAny)
                    {
                        if (contains > 0)
                        {
                            filteredResultList.Add(result);
                        }
                    }
                    else
                    {
                        if (expected == contains)
                        {
                            filteredResultList.Add(result);
                        }
                    }
                }
                dataGrid.ItemsSource = CollectionViewSource.GetDefaultView(filteredResultList);
                dataGrid.Items.Refresh();
            }
        }
    }
}