using Data_Management;
using Data_Management.Models;
using KiddEsports.Core;
using System;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace KiddEsports.MVVM.ViewModel.WindowViewModels
{
    class EventWindowViewModel
    {
        public Event CurrentEvent { get; set; }

        public void SetCurrentEvent(Event inputEvent)
        {
            CurrentEvent = inputEvent;
        }
    }
}

