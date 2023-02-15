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
    class EventsViewModel : ObservableObject
    {
        private DataAccess data = new DataAccess();
        private DataGrid dataGrid;
        public ObservableCollection<Event> eventList { get; set; }
        public ObservableCollection<Event> filteredEventList { get; set; }

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

        private string searchEventLocation;
        public string SearchEventLocation
        {
            get => searchEventLocation;
            set
            {
                searchEventLocation = value.ToString().ToUpper();
                SearchFieldsUpdated();
            }
        }

        public void SetDataGrid(ref DataGrid dgvInput)
        {
            dataGrid = dgvInput;
        }

        public void UpdateDataGrid()
        {
            eventList = new ObservableCollection<Event>(data.GetEntries<Event>());
            dataGrid.ItemsSource = eventList;
            dataGrid.Items.Refresh();
        }

        private void SearchFieldsUpdated()
        {
            filteredEventList = new ObservableCollection<Event>();
            if (string.IsNullOrWhiteSpace(searchEventName) &&
                string.IsNullOrWhiteSpace(searchEventLocation))
            {
                UpdateDataGrid();
            }
            else
            {
                foreach (var @event in eventList)
                {
                    // Two integer variables are created to keep a tally of how many fields have text in them
                    // and how many parameters in the current entry match their corresponding field
                    int expected = 0;
                    int contains = 0;

                    if (!string.IsNullOrWhiteSpace(searchEventName))
                    {
                        expected++;
                        if (@event.EventName.ToUpper().Contains(searchEventName))
                        {
                            contains++;
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(searchEventLocation))
                    {
                        expected++;
                        if (@event.EventLocation.ToUpper().Contains(searchEventLocation))
                        {
                            contains++;
                        }
                    }

                    if (containsAny)
                    {
                        if (contains > 0)
                        {
                            filteredEventList.Add(@event);
                        }
                    }
                    else
                    {
                        if (expected == contains)
                        {
                            filteredEventList.Add(@event);
                        }
                    }
                }
                dataGrid.ItemsSource = filteredEventList;
                dataGrid.Items.Refresh();
            }
        }
    }
}