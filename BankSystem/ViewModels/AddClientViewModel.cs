using BankSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.ViewModels
{
    internal class AddClientViewModel : ViewModel
    {
        private string _FirstName;
        private string _LastName;
        private string _Phone;
        private string _Passport;
        private string _Town;

        public string FirstName { get { return _FirstName; } set { Set(ref _FirstName, value); } }
        public string LastName { get { return _LastName; } set { Set(ref _LastName, value); } }
        public string Phone { get { return _Phone; } set { Set(ref _Phone, value); } }
        public string Passport { get { return _Passport; } set { Set(ref _Passport, value); } }
        public string Town { get { return _Town; } set { Set(ref _Town, value); } }
    }
}
