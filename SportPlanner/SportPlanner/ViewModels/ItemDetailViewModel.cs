using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using SportPlanner.Models;
using SportPlanner.Services;
using SportPlanner.Views;
using Xamarin.Forms;

namespace SportPlanner.ViewModels
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    public class ItemDetailViewModel : BaseViewModel
    {
        private string itemId;
        private string timeOfDay;
        private Event @event;
        private bool attendingBtnEnabled;
        private bool unAttendingBtnEnabled;
        private int attendingCount;
        private readonly IDataStore<Event> _dataStore;

        public Command EditItemCommand { get; }

        public ItemDetailViewModel(IDataStore<Event> dataStore)
        {
            AttendCommand = new Command(OnAttend);
            _dataStore = dataStore;
            EditItemCommand = new Command(OnEditItemTapped, CanEditItem);
        }

        private bool CanEditItem(object arg)
        {
            if (@event == null)
                return false;

            return @event.Users
                .Where(u => u.IsOwner)
                .Any(u => u.UserId == UserConstants.UserId);
        }

        private async void OnEditItemTapped(object obj)
        {
            await Shell.Current.GoToAsync($"{nameof(NewItemPage)}?{nameof(NewItemViewModel.ItemId)}={Id}");
        }

        private async void OnAttend(object obj)
        {
            try
            {
                IsBusy = true;

                if (!bool.TryParse(obj.ToString(), out var isAttending))
                    return;

                var eventUser = @event.Users.FirstOrDefault(u => u.UserId == UserConstants.UserId);
                if (eventUser == null)
                {
                    var newEventUser = new EventUser(UserConstants.UserId)
                    {
                        UserName = UserConstants.UserName,
                        IsAttending = true
                    };
                    @event.Users.Add(newEventUser);
                }
                else
                {
                    eventUser.IsAttending = isAttending;
                }

                await _dataStore.UpdateAsync(@event);
                UpdateProperties(@event);
            }
            catch (Exception e)
            {
                Debug.WriteLine("Failed to update event. " + e.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public string Id { get; set; }

        public ObservableCollection<EventUser> Users { get; set; } = new ObservableCollection<EventUser>();

        public Event Event
        {
            get => @event;
            set 
            { 
                SetProperty(ref @event, value);
                EditItemCommand.ChangeCanExecute();
            }
        }

        public string TimeOfDay
        {
            get => timeOfDay;
            set => SetProperty(ref timeOfDay, value);
        }

        public bool AttendingBtnEnabled
        {
            get => attendingBtnEnabled;
            set => SetProperty(ref attendingBtnEnabled, value);
        }

        public bool UnAttendingBtnEnabled
        {
            get => unAttendingBtnEnabled;
            set => SetProperty(ref unAttendingBtnEnabled, value);
        }

        public int AttendingCount
        {
            get => attendingCount;
            set => SetProperty(ref attendingCount, value);
        }

        public string ItemId
        {
            get
            {
                return itemId;
            }
            set
            {
                itemId = value;
                LoadItemId(value);
            }
        }

        public Command AttendCommand { get; }

        public async void LoadItemId(string itemId)
        {
            try
            {
                var @event = await _dataStore.GetAsync(itemId);
                Id = @event.Id;
                Title = @event.EventType.ToString();
                UpdateProperties(@event);
            }
            catch (Exception e)
            {
                Debug.WriteLine("Failed to Load Item. " + e.Message);
            }
        }

        private void UpdateProperties(Event @event)
        {
            Event = @event;
            TimeOfDay = @event.Date.TimeOfDay.ToString(@"hh\:mm");
            AttendingCount = @event.Users.Count(u => u.IsAttending);
            UpdateEventUserCollection(@event);

            var user = @event.Users.FirstOrDefault(u => u.UserId == UserConstants.UserId);
            var isAttending = user?.IsAttending ?? false;
            AttendingBtnEnabled = !isAttending;
            UnAttendingBtnEnabled = isAttending;
        }

        private void UpdateEventUserCollection(Event @event)
        {
            Users.Clear();
            foreach (var eventUser in @event.Users)
            {
                Users.Add(eventUser);
            }
        }
    }
}
