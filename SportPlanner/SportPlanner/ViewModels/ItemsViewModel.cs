using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;
using SportPlanner.Models;
using SportPlanner.Views;
using System.Linq;
using SportPlanner.Services;

namespace SportPlanner.ViewModels
{
    public class ItemsViewModel : BaseViewModel
    {
        private Event _selectedItem;
        private readonly IDataStore<Event> _dataStore;

        public ObservableCollection<Event> Items { get; }
        public Command LoadItemsCommand { get; }
        public Command AddItemCommand { get; }
        public Command<Event> ItemTapped { get; }

        public ItemsViewModel(IDataStore<Event> dataStore)
        {
            Title = "Events";
            Items = new ObservableCollection<Event>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            ItemTapped = new Command<Event>(OnItemSelected);

            AddItemCommand = new Command(OnAddItem);
            _dataStore = dataStore;
        }

        async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                Items.Clear();
                var items = await _dataStore.GetAsync(true);
                foreach (var @event in items.OrderBy(i => i.Date))
                {
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

        async void OnItemSelected(Event item)
        {
            if (item == null)
                return;

            // This will push the ItemDetailPage onto the navigation stack
            await Shell.Current.GoToAsync($"{nameof(ItemDetailPage)}?{nameof(ItemDetailViewModel.ItemId)}={item.Id}");
        }
    }
}