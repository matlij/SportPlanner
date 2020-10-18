﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using SportPlanner.Models;
using SportPlanner.Services;
using Xamarin.Forms;

namespace SportPlanner.ViewModels
{
    public class NewItemViewModel : BaseViewModel
    {
        private ObservableCollection<EventType> eventTypes = new ObservableCollection<EventType>();
        private EventType eventType;
        private DateTime date;
        private readonly IDataStore<Event> _dataStore;

        public string Id { get; set; }

        public NewItemViewModel(IDataStore<Event> dataStore)
        {
            SaveCommand = new Command(OnSave, ValidateSave);
            CancelCommand = new Command(OnCancel);
            this.PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();

            Date = DateTime.Now;
            PopulateEventTypes();
            _dataStore = dataStore;
        }

        private void PopulateEventTypes()
        {
            var eventTypes = Enum.GetValues(typeof(EventType)).Cast<EventType>().Skip(1);
            foreach (var @event in eventTypes)
            {
                EventTypes.Add(@event);
            }
        }

        private bool ValidateSave()
        {
            return
                date != null &&
                date != DateTime.MinValue &&
                eventType != EventType.Undefined;
        }

        public ObservableCollection<EventType> EventTypes
        {
            get => eventTypes;
            set => SetProperty(ref eventTypes, value);
        }

        public EventType EventType
        {
            get => eventType;
            set => SetProperty(ref eventType, value);
        }

        public DateTime Date
        {
            get => date;
            set => SetProperty(ref date, value);
        }

        public DateTime MinDate
        {
            get => DateTime.Now;
        }

        public DateTime MaxDate
        {
            get => DateTime.Now.AddDays(365);
        }

        public Command SaveCommand { get; }
        public Command CancelCommand { get; }

        private async void OnCancel()
        {
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }

        private async void OnSave()
        {
            var newItem = new Event(Guid.NewGuid().ToString(), EventType)
            {
                Date = Date,
            };

            await _dataStore.AddAsync(newItem);

            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }
    }
}
