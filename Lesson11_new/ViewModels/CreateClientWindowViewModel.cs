using Lesson11_new.Class;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Lesson11_new.ViewModels
{
    public class CreateClientWindowViewModel : INotifyPropertyChanged
    {

        #region Constructor
        public CreateClientWindowViewModel(CreateClientWindow createClientWindow)
        {
            CreateClientWindow = createClientWindow;
        }
        #endregion Constructor

        #region PropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        #endregion PropertyChanged

        #region Property
        
        CreateClientWindow CreateClientWindow { get; set; }
        private string _lastNameClient;
        public string LastNameClient
        {
            get { return _lastNameClient; }
            set
            {
                _lastNameClient = value;
                OnPropertyChanged("LastNameClient");
            }
        }

        private string _nameClient;
        public string NameClient
        {
            get { return _nameClient; }
            set
            {
                _nameClient = value;
                OnPropertyChanged("NameClient");
            }
        }

        private string _patronymicClient;
        public string PatronymicClient
        {
            get { return _patronymicClient; }
            set
            {
                _patronymicClient = value;
                OnPropertyChanged("PatronymicClient");
            }

        }
        private decimal _numberPhoneClient;
        public decimal NumberPhoneClient
        {
            get { return _numberPhoneClient; }
            set
            {
                _numberPhoneClient = value;
                OnPropertyChanged("NumberPhoneClient");
            }
        }
        private string _seriesAndNumberPassport;
        public string SeriesAndNumberPassport
        {
            get { return _seriesAndNumberPassport; }
            set
            {
                _seriesAndNumberPassport = value;
                OnPropertyChanged("SeriesAndNumberPassport");
            }
        }
        private string _nameMeneger;
        public string NameMeneger
        {
            get { return _nameMeneger; }
            set
            {
                _nameMeneger = value;
                OnPropertyChanged("NameMeneger");
            }
        }
        #endregion Property

        #region Commands
        DelegateCommand _createClient;
        public DelegateCommand CreatecreateClient
        {
            get
            {
                return _createClient ??
                    (_createClient = new DelegateCommand(obj =>
                    {
                        BankManager bankManager = new BankManager(NameMeneger);
                        bankManager.CreateClient(LastNameClient, NameClient, PatronymicClient, NumberPhoneClient, SeriesAndNumberPassport);
                        CreateClientWindow.Close();
                    }));
            }
        }
        #endregion Commands

    }
}
