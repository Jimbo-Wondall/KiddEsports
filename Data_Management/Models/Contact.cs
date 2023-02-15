using System;
using System.Collections.Generic;
using System.Text;

namespace Data_Management.Models
{
    public class Contact
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int fkTeam_Id { get; set; }

        public Contact()
        {

        }
        public Contact(ContactView inputView)
        {
            Id = inputView.Id;
            FirstName = inputView.FirstName;
            LastName = inputView.LastName;
            Phone = inputView.Phone;
            Email = inputView.Email;
        }
        public Contact(ContactView inputView, int teamID)
        {
            Id = inputView.Id;
            FirstName = inputView.FirstName;
            LastName = inputView.LastName;
            Phone = inputView.Phone;
            Email = inputView.Email;
            fkTeam_Id = teamID;
        }
    }
}
