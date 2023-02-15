using System;
using System.Collections.Generic;
using System.Text;

namespace Data_Management.Models
{
    public class ContactView
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string TeamName { get; set; }
    }
}