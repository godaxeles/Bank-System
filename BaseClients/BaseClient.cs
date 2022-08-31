using BaseClients.Account;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace BaseClients
{
    public class BaseClient : INotifyPropertyChanged
    {
        private string _FirstName;
        private string _LastName;
        private string _Phone;
        private string _Passport;
        private string _Town;
        private bool _IsActive;

        private ObservableCollection<BaseAccount<BaseClient>> _Account = new ObservableCollection<BaseAccount<BaseClient>>();
        public ObservableCollection<BaseAccount<BaseClient>> Account  { get => _Account; set => _Account = value; }

        protected Action<string> _messageAction;
        public event PropertyChangedEventHandler? PropertyChanged;

        public Guid IdClient { get; set; }
        public string FirstName { get => _FirstName; set => Set(ref _FirstName, value); }
        public string LastName { get => _LastName; set => Set(ref _LastName, value); }
        public string Phone { get => _Phone; set => Set(ref _Phone, value); }
        public string Passport { get => _Passport; set => Set(ref _Passport, value); }
        public string Town { get => _Town; set => Set(ref _Town, value); }
        public bool IsActive { get => _IsActive; set => Set(ref _IsActive, value); }
        public virtual bool IsCanChange { get; set; } = true;
        [JsonIgnore]        
        
        public Action<string> MessageAction
        {
            get => _messageAction;
            set => _messageAction = value;
        }

        public void AddAccount(BaseAccount<BaseClient> account)
        {
            _Account.Add(account);
            MessageAction?.Invoke($"Открыт счет {account.NumAccount} для клиента {this.IdClient}");
        }


        protected virtual void OnPropertyChanged([CallerMemberName] string PropertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
            MessageAction?.Invoke("Изменения успешно выполнены!");
        }

        protected virtual bool Set<T>(ref T field, T value, [CallerMemberName] string PropertyName = null)
        {
            if (Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(PropertyName);
            return true;
        }

        public BaseClient() { }

        public BaseClient(string FirstName, string LastName, string Phone, string Passport, string Town) :
           this(Guid.NewGuid(), FirstName, LastName, Phone, Passport, Town,  true)
        {


        }

        public BaseClient(Guid IdClient, string FirstName, string LastName, string Phone, string Passport, string Town, bool IsActive)
        {
            this.IdClient = IdClient;
            this.FirstName = FirstName;
            this.LastName = LastName;
            this.Phone = Phone;
            this.Passport = Passport;
            this.Town = Town;
            this.IsActive = IsActive;
        }
    }
}