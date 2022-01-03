using SportPlanner.Models;
using SportPlanner.Models.Constants;
using SportPlanner.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using Xamarin.Forms;

namespace SportPlanner.ViewModels
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    public class NewItemViewModel : BaseViewModel
    {
        private List<EventUser> _invitedUsers = new List<EventUser>();
        private ObservableCollection<EventType> _eventTypes = new ObservableCollection<EventType>();
        private ObservableCollection<TaskAddEventUser> _users = new ObservableCollection<TaskAddEventUser>();
        private EventType eventType;
        private DateTime date;
        private Address address;
        private readonly IDataStore<Event> _eventDataStore;
        private readonly IDataStore<User> _userDataStore;
        private readonly IUserLoginService _userLoginService;
        private string _itemId;
        private TimeSpan selectedTime;

        public Command SaveCommand { get; }
        public Command CancelCommand { get; }
        public string Id { get; set; }

        public NewItemViewModel(IDataStore<Event> dataStore, IDataStore<User> userDataStore, IUserLoginService userLoginService)
        {
            SaveCommand = new Command(OnSave, ValidateSave);
            CancelCommand = new Command(OnCancel);
            PropertyChanged += (_, __) => SaveCommand.ChangeCanExecute();

            Date = DateTime.Now;
            _eventDataStore = dataStore;
            _userDataStore = userDataStore;
            _userLoginService = userLoginService;
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

        public TimeSpan SelectedTime
        {
            get => selectedTime;
            set => SetProperty(ref selectedTime, value);
        }

        public ObservableCollection<EventType> EventTypes
        {
            get => _eventTypes;
            set => SetProperty(ref _eventTypes, value);
        }

        public Address Address
        {
            get => address;
            set => SetProperty(ref address, value);
        }

        public async void LoadItemId(string itemId)
        {
            try
            {
                if (itemId != null && itemId != default(int).ToString())
                {
                    Debug.WriteLine($"Loading event with ID: " + itemId);

                    var @event = await _eventDataStore.GetAsync(itemId);
                    Id = @event.Id.ToString();
                    Title = @event.EventType.ToString();
                    Date = @event.Date;
                    EventType = @event.EventType;
                    Address = @event.Address;
                    _invitedUsers = @event.Users.ToList();
                }

                Debug.WriteLine($"Loading users");
                var users = await _userDataStore.GetAsync(forceRefresh: true);
                var user = await _userLoginService.GetUserFromLocalDb();
                Users = CreateUsersList(users, _invitedUsers, user.Id);
            }
            catch (Exception e)
            {
                Debug.WriteLine("Failed to Load Item. " + e.Message);
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
            await Shell.Current.GoToAsync("..");
        }

        private async void OnSave()
        {
            IsBusy = true;

            var identifier = string.IsNullOrEmpty(Id)
                ? Guid.NewGuid()
                : Guid.Parse(Id);

            try
            {
                var user = await _userLoginService.GetUserFromLocalDb();

                var usersToInvite = Users.Where(u => u.Invited);
                var eventDate = new DateTime(Date.Year, Date.Month, Date.Day, SelectedTime.Hours, SelectedTime.Minutes, 0);
                var newItem = new Event(identifier, EventType)
                {
                    Date = eventDate,
                    Users = CreateEventUsers(usersToInvite, _invitedUsers, user),
                    Address = Address
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

        private static ObservableCollection<EventUser> CreateEventUsers(IEnumerable<TaskAddEventUser> usersToInvite, IEnumerable<EventUser> previouslyInvitedUsers, User user)
        {
            var collection = new ObservableCollection<EventUser>();
            foreach (var userToInvite in usersToInvite)
            {
                var eventUser = CreateEventUser(userToInvite.Id, userToInvite.Name, previouslyInvitedUsers);
                collection.Add(eventUser);
            }

            var currentUser = CreateEventUser(user.Id, user.Name, previouslyInvitedUsers);
            currentUser.IsOwner = true;
            collection.Add(currentUser);

            return collection;
        }

        private static EventUser CreateEventUser(Guid userId, string userName, IEnumerable<EventUser> previouslyInvitedUsers)
        {
            return new EventUser(userId)
            {
                UserReply = previouslyInvitedUsers.FirstOrDefault(u => u.UserId == userId)?.UserReply ?? EventReply.NotReplied,
                UserName = userName
            };
        }

        private static ObservableCollection<TaskAddEventUser> CreateUsersList(IEnumerable<User> usersInTeam, IEnumerable<EventUser> invitedUsers, Guid userId)
        {
            var users = new ObservableCollection<TaskAddEventUser>();
            if (!usersInTeam.Any())
                return users;

            var usersWithOwnerRemoved = usersInTeam.Where(u => u.Id != userId);
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
