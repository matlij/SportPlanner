using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
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
        private string attendButtonText;

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
                AttendButtonText = GetAttendButtonText();
            }
            catch (Exception e)
            {
                Debug.WriteLine("Failed to update event. " + e.Message);
            }
        }

        private string GetAttendButtonText()
        {
            return @event.UsersAttending.Contains(UserConstants.UserName)
                ? "UnAttend"
                : "Attend";
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

        public string AttendButtonText
        {
            get => attendButtonText;
            set => SetProperty(ref attendButtonText, value);
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
                var item = await DataStore.GetItemAsync(itemId);
                Id = item.Id;
                Event = item;
                TimeOfDay = item.Date.TimeOfDay.ToString(@"hh\:mm");
                AttendButtonText = GetAttendButtonText();
                Title = $"{item.EventType} {item.Date.ToShortDateString()}";
            }
            catch (Exception e)
            {
                Debug.WriteLine("Failed to Load Item. " + e.Message);
            }
        }
    }
}
