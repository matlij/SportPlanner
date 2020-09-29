using System;
using System.Diagnostics;
using SportPlanner.Extensions;
using SportPlanner.Models;
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

        public ItemDetailViewModel()
        {
            AttendCommand = new Command(OnAttend);
        }

        private async void OnAttend(object obj)
        {
            try
            {
                if (@event.UsersAttending.Contains(UserConstants.UserName))
                {
                    @event.UsersAttending.Remove(UserConstants.UserName);
                }
                else
                {
                    @event.UsersAttending.Add(UserConstants.UserName);
                }

                await DataStore.UpdateItemAsync(@event);
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
                var @event = await DataStore.GetItemAsync(itemId);
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
            var userIsAttending = @event.UsersAttending.ContainsValue(UserConstants.UserName);
            AttendingBtnEnabled = !userIsAttending;
            UnAttendingBtnEnabled = userIsAttending;
            AttendingCount = this.@event.UsersAttending.Count;
        }
    }
}
