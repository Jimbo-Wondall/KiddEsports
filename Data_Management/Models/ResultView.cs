using System;
using System.Collections.Generic;
using System.Text;

namespace Data_Management.Models
{
    public class ResultView
    {
        public int Id { get; set; }
        public string Result { get; set; }
        public string GameName { get; set; }
        public string Team1Name { get; set; }
        public string Team2Name { get; set; }
        public string EventName { get; set; }

        public override string ToString()
        {
            return $"{Id},{Result},{GameName},{Team1Name},{Team2Name},{EventName}";
        }
    }
}
