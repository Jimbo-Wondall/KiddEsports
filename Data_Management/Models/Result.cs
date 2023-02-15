using System;
using System.Collections.Generic;
using System.Text;

namespace Data_Management.Models
{
    public class Result
    {
        public int Id { get; set; }
        public byte ResultValue { get; set; }
        public int fkGameType_Id { get; set; }
        public int fkTeam1_Id { get; set; }
        public int fkTeam2_Id { get; set; }
        public int fkEvent_Id { get; set; }
    }
}
