using SportPlanner.Models;
using SportPlanner.Models.Constants;
using SportPlanner.Repository.Interfaces;
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
        private readonly IUserLoginService _userLoginService;

        public ObservableCollection<Event> Items { get; }
        public Command LoadItemsCommand { get; }
        public Command AddItemCommand { get; }
        public Command<Event> ItemTapped { get; }

        public ItemsViewModel(IEventDataStore dataStore, IUserLoginService userLoginService)
        {
            Title = "Aktiviteter";
            Items = new ObservableCollection<Event>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            ItemTapped = new Command<Event>(OnItemSelected);

            AddItemCommand = new Command(OnAddItem);
            _dataStore = dataStore;
            _userLoginService = userLoginService;
        }

        private async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                var user = await _userLoginService.GetUserFromLocalDb();
                if (user is null)
                {
                    Debug.WriteLine("No user found in local DB");
                }

                Items.Clear();
                var items = await _dataStore.GetFromUserAsync(user.Id, forceRefresh: true);
                foreach (var @event in items.OrderBy(i => i.Date))
                {
                    @event.CurrentUserIsAttending = UserIsAttendingEvent(@event, user.Id);
                    Items.Add(@event);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Load events faild: " + ex);
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
            await Shell.Current.GoToAsync($"{nameof(NewItemPage)}?{nameof(NewItemViewModel.ItemId)}=0");
        }

        private async void OnItemSelected(Event item)
        {
            if (item == null)
                return;

            // This will push the ItemDetailPage onto the navigation stack
            await Shell.Current.GoToAsync($"{nameof(ItemDetailPage)}?{nameof(ItemDetailViewModel.ItemId)}={item.Id}");
        }

        private bool UserIsAttendingEvent(Event @event, Guid userId)
        {
            var user = @event.Users
                .FirstOrDefault(u => u.UserId == userId);

            return user != null && user.IsAttending;
        }
    }
}