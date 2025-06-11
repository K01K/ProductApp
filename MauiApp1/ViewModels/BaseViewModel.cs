using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ProductApp.ViewModels
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        private bool _isBusy;
        private string _title = string.Empty;

        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T backingStore, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        protected async Task HandleErrorAsync(string message, Exception? exception = null)
        {
            System.Diagnostics.Debug.WriteLine($"Error: {message} - {exception?.Message}");

            if (Application.Current?.MainPage != null)
            {
                await Application.Current.MainPage.DisplayAlert("Błąd", message, "OK");
            }
        }
        protected async Task ExecuteSafeAsync(Func<Task> operation, string? errorMessage = null)
        {
            try
            {
                await operation();
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(errorMessage ?? "Wystąpił nieoczekiwany błąd", ex);
            }
        }
    }
}