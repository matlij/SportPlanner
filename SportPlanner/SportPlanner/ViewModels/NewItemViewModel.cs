using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SportPlanner.Models;
using SportPlanner.Services;
using Xamarin.Forms;

namespace SportPlanner.ViewModels
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    public class NewItemViewModel : BaseViewModel
    {
        private static readonly SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1, 1);

        private List<EventUser> _invitedUsers = new List<EventUser>();
        private ObservableCollection<EventType> _eventTypes = new ObservableCollection<EventType>();
        private ObservableCollection<TaskAddEventUser> _users = new ObservableCollection<TaskAddEventUser>();
        private EventType eventType;
        private DateTime date;
        private readonly IDataStore<Event> _eventDataStore;
        private readonly IDataStore<User> _userDataStore;
        private string _itemId;

        public Command SaveCommand { get; }
        public Command CancelCommand { get; }
        public string Id { get; set; }

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

        public ObservableCollection<TaskAddEventUser> Users
        {
            get => _users;
            set => SetProperty(ref _users, value);
        }

        public string ItemId
        {
            get
            {
                return _itemId;
            }
            set
            {
                _itemId = value;
                LoadItemId(value);
            }
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

        public ObservableCollection<EventType> EventTypes
        {
            get => _eventTypes;
            set => SetProperty(ref _eventTypes, value);
        }

        public async Task LoadUsers()
        {
            Debug.WriteLine($"Loading users");

            IsBusy = true;

            try
            {
                Users.Clear();
                var users = await _userDataStore.GetAsync(forceRefresh: true);

                await _semaphoreSlim.WaitAsync();
                Debug.WriteLine($"Populating users list");
                Users = CreateUsersList(users, _invitedUsers);
            }
            catch (Exception e)
            {
                Debug.WriteLine("Load users failed: " + e);
            }
            finally
            {
                IsBusy = false;
                _semaphoreSlim.Release();
            }
        }

        public async void LoadItemId(string itemId)
        {
            Debug.WriteLine($"Loading event with ID: " + itemId);

            try
            {
                await _semaphoreSlim.WaitAsync();

                var @event = await _eventDataStore.GetAsync(itemId);
                _invitedUsers = @event.Users.ToList();
                Id = @event.Id;
                Title = @event.EventType.ToString();
                Date = @event.Date;
                EventType = @event.EventType;
            }
            catch (Exception e)
            {
                Debug.WriteLine("Failed to Load Item. " + e.Message);
            }
            finally
            {
                _semaphoreSlim.Release();
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

        private async void OnCancel()
        {
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }

        private async void OnSave()
        {
            IsBusy = true;

            var identifier = string.IsNullOrEmpty(Id)
                ? Guid.NewGuid().ToString()
                : Id;

            try
            {
                var usersToInvite = Users.Where(u => u.Invited);
                var newItem = new Event(identifier, EventType)
                {
                    Date = Date,
                    Users = CreateEventUsers(usersToInvite, _invitedUsers)
                };

                await _eventDataStore.UpdateAsync(newItem);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Save or update event failed. Exception: " + ex);
            }
            finally
            {
                IsBusy = false;

                await Shell.Current.GoToAsync($"..?{nameof(ItemDetailViewModel.ItemId)}={identifier}");
            }

        }

        private static ObservableCollection<EventUser> CreateEventUsers(IEnumerable<TaskAddEventUser> usersToInvite, IEnumerable<EventUser> currentlyInvitedUsers)
        {
            var collection = new ObservableCollection<EventUser>();
            foreach (var userToInvite in usersToInvite)
            {
                var eventUser = new EventUser(userToInvite.Id)
                {
                    IsAttending = IsAttencding(currentlyInvitedUsers, userToInvite.Id),
                    UserName = userToInvite.Name
                };
                collection.Add(eventUser);
            }

            var currentUser = new EventUser(UserConstants.UserId)
            {
                IsAttending = IsAttencding(currentlyInvitedUsers, UserConstants.UserId),
                IsOwner = true
            };
            collection.Add(currentUser);

            return collection;
        }

        private static bool IsAttencding(IEnumerable<EventUser> currentlyInvitedUsers, string userIdToInvite)
        {
            return currentlyInvitedUsers
                .SingleOrDefault(eu => eu.UserId == userIdToInvite)?
                .IsAttending ?? false;
        }

        private static ObservableCollection<TaskAddEventUser> CreateUsersList(IEnumerable<User> usersInTeam, IEnumerable<EventUser> invitedUsers)
        {
            var users = new ObservableCollection<TaskAddEventUser>();
            if (usersInTeam.Count() == 0)
                return users;

            var usersWithOwnerRemoved = usersInTeam.Where(u => u.Id != UserConstants.UserId);
            foreach (var user in usersWithOwnerRemoved)
            {
                var isInvited = invitedUsers.Any(eu => eu.UserId == user.Id);
                var taskAddEventUser = new TaskAddEventUser(user.Id)
                {
                    Name = user.Name,
                    Invited = isInvited
                };
                users.Add(taskAddEventUser);
            }

            return users;
        }
    }
}
