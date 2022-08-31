using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using BankSystem;
using BankSystem.Command;
using BankSystem.ViewModels;
using BaseClients;
using BaseClients.Account;

namespace BankSystem.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {
        Repository r = new("./DB.json");
        private ObservableCollection<BaseClient> _Clients = new ObservableCollection<BaseClient>();
        private ObservableCollection<BaseAccount<BaseClient>> _Accounts = new ObservableCollection<BaseAccount<BaseClient>>();
        private string _MessageText;
        private BaseAccount<BaseClient> _AccountFrom;
        private BaseAccount<BaseClient> _AccountTo;
        private BaseClient _SelectedClientForm;
        private BaseClient _SelectedClientTo;
        private float _ReplenishSum;
        private float _TransSum;
        
        public ICommand ReplanishAccount { get; }
        public ICommand OpenDeposite { get; }
        public ICommand OpenNoDeposite { get; }
        public ICommand CloseAccount { get; }
        public ICommand MoneyTransfer { get; }
        public ICommand AddClient { get; }
        public ICommand DelClient { get; }
        public ICommand SaveChange { get; }

        public float ReplenishSum { get { return _ReplenishSum; } set { Set(ref _ReplenishSum, value); } }
        public string MessageText { get { return _MessageText; } set { Set(ref _MessageText, value); } }
        public BaseAccount<BaseClient> AccountFrom { get { return _AccountFrom; } set { Set(ref _AccountFrom, value); } }
        public BaseAccount<BaseClient> AccountTo { get { return _AccountTo; } set { Set(ref _AccountTo, value); } }
        public ObservableCollection<BaseClient> Clients { get { return _Clients; } set { Set(ref _Clients, value); } }
        public ObservableCollection<BaseAccount<BaseClient>> Accounts { get { return _Accounts; } set { Set(ref _Accounts, value); } }
        public float TransSum { get { return _TransSum; } set { Set(ref _TransSum, value); } }
        public BaseClient SelectedClientFrom 
        { 
            get { return _SelectedClientForm; }
            set { Set(ref _SelectedClientForm, value); }
        }
        public BaseClient SelectedClientTo
        {
            get { return _SelectedClientTo; }
            set { Set(ref _SelectedClientTo, value); }
        }

        public void GetClients()
        {
            foreach (var client in r.GetClient().Where(e => e.IsActive))
            {
                client.MessageAction = ShowMessage;
                foreach (var bankAccount in client.Account)
                {
                    bankAccount.MessageAction = ShowMessage;
                }
                _Clients.Add((BaseClient)client);
            }
        }

        private void OnSaveChange(object p)
        {
            r.SaveBase();
        }

        private bool CanSaveChange(object p)
        {
            if (SelectedClientFrom != null) return true;
            else return false;
        }

        private void OnMoneyTransfer(object p)
        {
            try
            {
                (AccountFrom as IAccountContrvariant<BaseClient, BaseAccount<BaseClient>>).TransAccountToAccount((BaseAccount<BaseClient>)AccountTo, TransSum);
            }
            catch (MoneyExeption e)
            {
                MessageBox.Show(e.Message);
            }
            TransSum = 0.0F;
            r.SaveBase();
        }

        private void OnAddClient(object p)
        {
            AddClient window = new();
            window.ShowDialog();
            
            if (window.DialogResult ?? false)
            {
                r.AddClient(
                    (window.DataContext as AddClientViewModel).FirstName,
                    (window.DataContext as AddClientViewModel).LastName,
                    (window.DataContext as AddClientViewModel).Phone,
                    (window.DataContext as AddClientViewModel).Passport,
                    (window.DataContext as AddClientViewModel).Town);
            }
        }

        private bool CanAddClient(object p) => true;

        private void OnDelClient(object p)
        {
            _Clients[_Clients.IndexOf((BaseClient)SelectedClientFrom)].IsActive = false;
            r.SaveBase();
        }

        private bool CanDelClient(object p)
        {
            if (SelectedClientFrom != null) return true;
            else return false;
        }

        private bool CanMoneyTransfer(object p)
        {
            if (AccountFrom != null && AccountTo != null && TransSum != 0) return true;
            else return false;
        }

        private void OnCloseAccount(object p)
        {
            SelectedClientFrom.Account.Remove((BaseAccount<BaseClient>)p);
        }

        private bool CanCloseAccount(object p)
        {
            if (_SelectedClientForm != null && AccountFrom?.Balance == 0) return true;
            else return false;
        }

        private void OpenNoDepositeAccount(object p)
        {
            SelectedClientFrom.AddAccount(new NoDeposite<BaseClient>(r.GenId()));
            r.SaveBase();
        }

        private bool CanOpenNoDepositeAccount(object p)
        {
            if(SelectedClientFrom != null)
            {
                if (SelectedClientFrom.Account.Count(e => e is NoDeposite<BaseClient>) == 0) return true;
            }
            return false;
        }

        private void OpenDepositeAccount(object p)
        {
            SelectedClientFrom.AddAccount(new Deposite<BaseClient>(r.GenId()));
            r.SaveBase();
        }

        private bool CanOpenDeposite(object p)
        {
            if (SelectedClientFrom != null)
            {
                if (SelectedClientFrom.Account.Count(e => e is Deposite<BaseClient>) == 0) return true;
            }
            return false;
        }

        private void OnReplanishAccount(object p)
        {

            if (p as IAccountCovariant<BaseClient, BaseAccount<BaseClient>> != null)
            {
                (p as IAccountCovariant<BaseClient, BaseAccount<BaseClient>>).PutMoney(ReplenishSum);
                ReplenishSum = 0.0F;
            }
            r.SaveBase();
        }

        private bool CanReplanishAccount(object p)
        {
            if (ReplenishSum > 0 && AccountFrom != null) return true;
            else return false;
        }

        private void ShowMessage(string obj)
        {
            MessageText = obj;
        }

        public MainWindowViewModel()
        {
            GetClients();
            ReplanishAccount = new RelayCommand(OnReplanishAccount, CanReplanishAccount);
            OpenDeposite = new RelayCommand(OpenDepositeAccount, CanOpenDeposite);
            OpenNoDeposite = new RelayCommand(OpenNoDepositeAccount, CanOpenNoDepositeAccount);
            CloseAccount = new RelayCommand(OnCloseAccount, CanCloseAccount);
            MoneyTransfer = new RelayCommand(OnMoneyTransfer, CanMoneyTransfer);
            AddClient = new RelayCommand(OnAddClient, CanAddClient);
            DelClient = new RelayCommand(OnDelClient, CanDelClient);
            SaveChange = new RelayCommand(OnSaveChange, CanSaveChange);
        }
    }
}

