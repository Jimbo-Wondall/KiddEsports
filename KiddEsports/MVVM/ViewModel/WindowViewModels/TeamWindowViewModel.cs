using Data_Management;
using Data_Management.Models;
using KiddEsports.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Controls;

namespace KiddEsports.MVVM.ViewModel.WindowViewModels
{
    class TeamWindowViewModel
    {
        public Team CurrentTeam { get; set; }

        public void SetCurrentTeam(Team inputTeam)
        {
            CurrentTeam = inputTeam;
        }
    }
}

