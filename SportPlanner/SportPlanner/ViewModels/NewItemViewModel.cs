using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using SportPlanner.Models;
using Xamarin.Forms;

namespace SportPlanner.ViewModels
{
    public class NewItemViewModel : BaseViewModel
    {
        private EventType eventType;
        private DateTime date;
        public string Id { get; set; }

        public NewItemViewModel()
        {
            SaveCommand = new Command(OnSave, ValidateSave);
            CancelCommand = new Command(OnCancel);
            this.PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();
        }

        private bool ValidateSave()
        {
            return 
                date != null && 
                date != DateTime.MinValue && 
                eventType != EventType.Undefined;
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

        public Command SaveCommand { get; }
        public Command CancelCommand { get; }

        private async void OnCancel()
        {
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }

        private async void OnSave()
        {
            var newItem = new Event(Guid.NewGuid().ToString())
            {
                Date = Date,
                EventType = EventType
            };

            await DataStore.AddItemAsync(newItem);

            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }
    }
}
