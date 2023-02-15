using Data_Management;
using Data_Management.Models;
using KiddEsports.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace KiddEsports.MVVM.ViewModel
{
    class ContactsViewModel : ObservableObject
    {
        private DataAccess data = new DataAccess();
        private DataGrid dataGrid;
        public ObservableCollection<ContactView> contactList { get; set; }
        public ObservableCollection<ContactView> filteredContactList { get; set; }

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

        private string searchFirstName;
        private string searchLastName;
        public string SearchName
        {
            get => searchFirstName + " " + searchLastName;
            set
            {
                string[] temp = new string[2];
                if (value.Contains(" "))
                {
                    temp = value.Split(" ");
                    temp[0] = temp[0].ToUpper();
                    temp[1] = temp[1].ToUpper();
                }
                else
                {
                    temp[0] = value.ToUpper();
                }
                searchFirstName = temp[0];
                searchLastName = temp[1];
                SearchFieldsUpdated();
            }
        }

        private string searchPhone;
        public string SearchPhone
        {
            get => searchPhone;
            set
            {
                searchPhone = value.ToString().ToUpper();
                SearchFieldsUpdated();
            }
        }

        private string searchEmail;
        public string SearchEmail
        {
            get => searchEmail;
            set
            {
                searchEmail = value.ToString().ToUpper();
                SearchFieldsUpdated();
            }
        }

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

        public void SetDataGrid(ref DataGrid dgvInput)
        {
            dataGrid = dgvInput;
        }

        public void UpdateDataGrid()
        {
            contactList = new ObservableCollection<ContactView>(data.GetEntries<Contact, ContactView>());
            dataGrid.ItemsSource = contactList;
            dataGrid.Items.Refresh();
        }

        public void SearchFieldsUpdated()
        {
            filteredContactList = new ObservableCollection<ContactView>();
            if (string.IsNullOrWhiteSpace(searchFirstName) &&
                string.IsNullOrWhiteSpace(searchLastName) &&
                string.IsNullOrWhiteSpace(searchPhone) &&
                string.IsNullOrWhiteSpace(searchEmail) &&
                string.IsNullOrWhiteSpace(searchTeamName))
            {
                UpdateDataGrid();
            }
            else
            {
                foreach (var contact in contactList)
                {
                    // Two integer variables are created to keep a tally of how many fields have text in them
                    // and how many parameters in the current entry match their corresponding field
                    int contains = 0;
                    int expected = 0;
                    if (!string.IsNullOrWhiteSpace(searchFirstName))
                    {
                        expected++;
                        if (contact.FirstName.ToUpper().Contains(searchFirstName) || 
                            contact.LastName.ToUpper().Contains(searchFirstName))
                        {
                            contains++;
                            if (!string.IsNullOrWhiteSpace(searchLastName))
                            {
                                expected++;
                                if (contact.LastName.ToUpper().Contains(searchLastName) ||
                                contact.FirstName.ToUpper().Contains(searchLastName))
                                {
                                    contains++;
                                }
                            }
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(searchPhone))
                    {
                        expected++;
                        if (contact.Phone.ToUpper().Contains(searchPhone))
                        {
                            contains++;
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(searchEmail))
                    {
                        expected++;
                        if (contact.Email.ToUpper().Contains(searchEmail))
                        {
                            contains++;
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(searchTeamName))
                    {
                        expected++;
                        if (contact.TeamName.ToUpper().Contains(searchTeamName))
                        {
                            contains++;
                        }
                    }

                    if (containsAny)
                    {
                        if (contains > 0)
                        {
                            filteredContactList.Add(contact);
                        }
                    }
                    else
                    {
                        if (expected == contains)
                        {
                            filteredContactList.Add(contact);
                        }
                    }


                }
                dataGrid.ItemsSource = filteredContactList;
                dataGrid.Items.Refresh();


                //filteredContactList = contactList.Where(c => c.FirstName.ToUpper().Contains(SearchName));
            }
        }
    }
}
