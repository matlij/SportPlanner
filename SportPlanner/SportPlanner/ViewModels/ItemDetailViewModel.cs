using System;
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
        private EventType eventType;
        private DateTime date;
        public string Id { get; set; }

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

        public async void LoadItemId(string itemId)
        {
            try
            {
                var item = await DataStore.GetItemAsync(itemId);
                Id = item.Id;
                EventType = item.EventType;
            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to Load Item");
            }
        }
    }
}
