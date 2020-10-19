using System;
using System.Collections.ObjectModel;
using System.Linq;
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
            IsBusy = true;

            try
            {
                var newItem = new Event(Guid.NewGuid().ToString(), EventType)
                {
                    Date = Date,
                    Users = new ObservableCollection<EventUser>
                    {
                        new EventUser(UserConstants.UserId) { IsAttending = true}
                    }
                };

                await _dataStore.AddAsync(newItem);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Save new event failed. Exception: " + ex);
            }
            finally
            {
                IsBusy = false;

                // This will pop the current page off the navigation stack
                await Shell.Current.GoToAsync("..");
            }

        }
    }
}
