using System;
using System.Collections.Generic;
using System.Text;

namespace Data_Management.Models
{
    public class Team
    {
        public int Id { get; set; }
        public string TeamName { get; set; }
        public int Points { get; set; }

        public override string ToString()
        {
            return $"{Id},{TeamName},{Points}";
        }
    }
}
