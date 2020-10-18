using System;
using System.Diagnostics;
using System.Linq;
using SportPlanner.Extensions;
using SportPlanner.Models;
using SportPlanner.Services;
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

        public ItemDetailViewModel(IDataStore<Event> dataStore)
        {
            AttendCommand = new Command(OnAttend);
            _dataStore = dataStore;
        }

        private async void OnAttend(object obj)
        {
            try
            {
                var eventUser = @event.Users.FirstOrDefault(u => u.UserId == UserConstants.UserId);
                if (eventUser != null)
                {
                    @event.Users.Remove(eventUser);
                }
                else
                {
                    var newEventUser = new EventUser(UserConstants.UserId) { IsAttending = true };
                    @event.Users.Add(newEventUser);
                }

                await _dataStore.UpdateAsync(@event);
                UpdateProperties(@event);
            }
            catch (Exception e)
            {
                Debug.WriteLine("Failed to update event. " + e.Message);
            }
        }

        public string Id { get; set; }

        public Event Event
        {
            get => @event;
            set => SetProperty(ref @event, value);
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
                Title = $"{@event.EventType} {@event.Date.ToShortDateString()}";
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
            var userIsAttending = @event.Users.ContainsValue(new EventUser(UserConstants.UserId));
            AttendingBtnEnabled = !userIsAttending;
            UnAttendingBtnEnabled = userIsAttending;
            AttendingCount = @event.Users.Count;
        }
    }
}
