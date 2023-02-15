using Data_Management;
using Data_Management.Models;
using KiddEsports.Core;
using System;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace KiddEsports.MVVM.ViewModel.WindowViewModels
{
    class GameWindowViewModel
    {
        public Game CurrentGame { get; set; }

        public void SetCurrentGame(Game inputGame)
        {
            CurrentGame = inputGame;
        }
    }
}

