using KiddEsports.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data_Management.Models
{
    public class Event : ObsObject
    {
        public int Id { get; set; }
        public string EventName { get; set; }
        public DateTime EventDate { get; set; }
        public string EventLocation { get; set; }
    }
}
