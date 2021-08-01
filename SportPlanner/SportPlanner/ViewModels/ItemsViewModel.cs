using SportPlanner.Models;
using SportPlanner.Services;
using SportPlanner.Views;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SportPlanner.ViewModels
{
    public class ItemsViewModel : BaseViewModel
    {
        private Event _selectedItem;
        private readonly IEventDataStore _dataStore;

        public ObservableCollection<Event> Items { get; }
        public Command LoadItemsCommand { get; }
        public Command AddItemCommand { get; }
        public Command<Event> ItemTapped { get; }

        public ItemsViewModel(IEventDataStore dataStore)
        {
            Title = "Events";
            Items = new ObservableCollection<Event>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            ItemTapped = new Command<Event>(OnItemSelected);

            AddItemCommand = new Command(OnAddItem);
            _dataStore = dataStore;
        }

        private async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                Items.Clear();
                var items = await _dataStore.GetFromUserAsync(UserConstants.UserId, forceRefresh: true);
                foreach (var @event in items.OrderBy(i => i.Date))
                {
                    @event.CurrentUserIsAttending = UserIsAttendingEvent(@event);
                    Items.Add(@event);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public void OnAppearing()
        {
            IsBusy = true;
            SelectedItem = null;
        }

        public Event SelectedItem
        {
            get => _selectedItem;
            set
            {
                SetProperty(ref _selectedItem, value);
                OnItemSelected(value);
            }
        }

        private async void OnAddItem(object obj)
        {
            await Shell.Current.GoToAsync(nameof(NewItemPage));
        }

        private async void OnItemSelected(Event item)
        {
            if (item == null)
                return;

            // This will push the ItemDetailPage onto the navigation stack
            await Shell.Current.GoToAsync($"{nameof(ItemDetailPage)}?{nameof(ItemDetailViewModel.ItemId)}={item.Id}");
        }

        private bool UserIsAttendingEvent(Event @event)
        {
            var user = @event.Users
                .FirstOrDefault(u => u.UserId == UserConstants.UserId);

            return user != null && user.IsAttending;
        }
    }
}