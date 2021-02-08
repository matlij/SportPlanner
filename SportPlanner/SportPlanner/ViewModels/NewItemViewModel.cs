using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using SportPlanner.Models;
using SportPlanner.Services;
using Xamarin.Forms;

namespace SportPlanner.ViewModels
{
    public class NewItemViewModel : BaseViewModel
    {
        private ObservableCollection<EventType> _eventTypes = new ObservableCollection<EventType>();
        private ObservableCollection<TaskAddEventUser> _users = new ObservableCollection<TaskAddEventUser>();
        private EventType eventType;
        private DateTime date;
        private readonly IDataStore<Event> _eventDataStore;
        private readonly IDataStore<User> _userDataStore;

        public string Id { get; set; }
        public Command SaveCommand { get; }
        public Command CancelCommand { get; }

        public ObservableCollection<TaskAddEventUser> Users
        {
            get => _users;
            set => SetProperty(ref _users, value);
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

        public NewItemViewModel(IDataStore<Event> dataStore, IDataStore<User> userDataStore)
        {
            SaveCommand = new Command(OnSave, ValidateSave);
            CancelCommand = new Command(OnCancel);
            this.PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();

            Date = DateTime.Now;
            _eventDataStore = dataStore;
            _userDataStore = userDataStore;
            PopulateEventTypes();
        }

        public async Task LoadUsers()
        {
            IsBusy = true;

            try
            {
                Users.Clear();
                var users = await _userDataStore.GetAsync(forceRefresh: true);
                foreach (var user in users.Where(u => u.Id != UserConstants.UserId))
                {
                    var taskAddEventUser = new TaskAddEventUser(user.Id)
                    {
                        Name = user.Name,
                        Invited = true
                    };
                    Users.Add(taskAddEventUser);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Load users failed: " + e);
            }
            finally
            { 
                IsBusy = false;
            }
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
            get => _eventTypes;
            set => SetProperty(ref _eventTypes, value);
        }

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
                    Users = CreateEventUsers(_users)
                };

                await _eventDataStore.AddAsync(newItem);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Save new event failed. Exception: " + ex);
            }
            finally
            {
                IsBusy = false;

                // This will pop the current page off the navigation stack
                await Shell.Current.GoToAsync("..");
            }

        }

        private static ObservableCollection<EventUser> CreateEventUsers(IEnumerable<TaskAddEventUser> users)
        {
            var collection = new ObservableCollection<EventUser>();
            var invitedUsers = users.Where(u => u.Invited);
            foreach (var user in invitedUsers)
            {
                var eventUser = new EventUser(user.Id)
                {
                    IsAttending = false,
                    UserName = user.Name
                };
                collection.Add(eventUser);
            }

            var currentUser = new EventUser(UserConstants.UserId);
            collection.Add(currentUser);

            return collection;
        }
    }
}
