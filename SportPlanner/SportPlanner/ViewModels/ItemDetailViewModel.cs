using SportPlanner.Models;
using SportPlanner.Models.Constants;
using SportPlanner.Services;
using SportPlanner.Views;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using Xamarin.Forms;

namespace SportPlanner.ViewModels
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    public class ItemDetailViewModel : BaseViewModel
    {
        private Event @event;
        private string itemId;
        private string timeOfDay;
        private string attendBtnText;
        private bool isAttending;

        private readonly IDataStore<Event> _dataStore;

        public Command EditItemCommand { get; }
        public Command DeleteCommand { get; }

        public ItemDetailViewModel(IDataStore<Event> dataStore)
        {
            _dataStore = dataStore;
            AttendCommand = new Command(OnAttend);
            EditItemCommand = new Command(OnEditItemTapped, CanEditItem);
            DeleteCommand = new Command(OnDelete, CanEditItem);
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

                var eventUser = @event.Users.FirstOrDefault(u => u.UserId == UserConstants.UserId);
                if (eventUser == null)
                {
                    var newEventUser = new EventUser(UserConstants.UserId)
                    {
                        UserName = UserConstants.UserName,
                        UserReply = EventReply.Attending
                    };
                    @event.Users.Add(newEventUser);
                }
                else
                {
                    eventUser.UserReply = eventUser.IsAttending
                        ? EventReply.NotAttending
                        : EventReply.Attending;
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
                DeleteCommand.ChangeCanExecute();
            }
        }

        public string TimeOfDay
        {
            get => timeOfDay;
            set => SetProperty(ref timeOfDay, value);
        }

        public string AttendBtnText
        {
            get => attendBtnText;
            set => SetProperty(ref attendBtnText, value);
        }

        public bool IsAttending
        {
            get => isAttending;
            set => SetProperty(ref isAttending, value);
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

        private async void OnDelete(object obj)
        {
            try
            {
                if (string.IsNullOrEmpty(ItemId))
                    return;

                var shouldDelete = await Application.Current.MainPage.DisplayAlert("Delete event", "Are you sure you want to delete?", "Yes", "Cancel");
                if (!shouldDelete)
                    return;

                IsBusy = true;

                await _dataStore.DeleteAsync(ItemId);

                await Shell.Current.GoToAsync($"../..");
            }
            catch (Exception)
            {
                Debug.WriteLine($"Delete event '{ItemId}' failed");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void UpdateProperties(Event @event)
        {
            Event = @event;
            TimeOfDay = @event.Date.TimeOfDay.ToString(@"hh\:mm");
            UpdateEventUserCollection(@event);

            var user = @event.Users.FirstOrDefault(u => u.UserId == UserConstants.UserId);
            IsAttending = user?.IsAttending ?? false;
            AttendBtnText = IsAttending
                ? "UnAttend"
                : "Attend";
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
