using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Lesson11_new.Class;

namespace Lesson11_new.ViewModels
{
    public class WindowViewModel : INotifyPropertyChanged
    {
        #region Constructor
        public WindowViewModel()

        {
            SelectIndexWorcer = 0;  
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
        private ObservableCollection<ClientBank> _client;
        private HandlerFile _handlerFile; 
        private int _selectIndexWorcer;
        private string _nameBankWerker;

        public string NameBankWerker
        {
            get=> _nameBankWerker;
            set
            {
                _nameBankWerker = value;
                OnPropertyChanged("NameBankWerker");
            }
        }

        public int SelectIndexWorcer
        {
            get => _selectIndexWorcer;
            set
            {
                _selectIndexWorcer = value;
                OnPropertyChanged("SelectIndexWorcer");

                ClientBanksObs = new ObservableCollection<ClientBank>();
                if (SelectIndexWorcer == 0)
                {
                    ClientBanksObs = GetClientForConsultant();
                }
                else
                {
                    ClientBanksObs = GetClientForManedger();
                }
            }
        }

        public ObservableCollection<ClientBank> ClientBanksObs
        {
            get
            {
                return _client;
            }
            set
            {
                _client = value;
                OnPropertyChanged("ClientBanksObs");
            }
        }

        #endregion Property

        #region Comands
        DelegateCommand _showCreateClientWindow;
        /// <summary>
        /// Команда для вызова окна создания клиента
        /// </summary>
        public DelegateCommand ShowCreateClientWindow
        {
            get
            {
                return _showCreateClientWindow ??
                    (_showCreateClientWindow = new DelegateCommand(obj =>
                    {
                        if (SelectIndexWorcer != 0)
                        {
                            CreateClientWindow createClientWindow = new CreateClientWindow();
                            createClientWindow.Show();
                        }
                        else
                        {
                            MessageBox.Show("Создовать клиента может только менеджер");
                        }
                    }));
            }
        }

        DelegateCommand _updateDataGrid;
        /// <summary>
        /// Команда для обновления списка клиентов
        /// </summary>
        public DelegateCommand UpdateDataGrid
        {
            get
            {
                return _updateDataGrid ??
                    (_updateDataGrid = new DelegateCommand(obj =>
                    {
                        ClientBanksObs = new ObservableCollection<ClientBank>();
                        if (SelectIndexWorcer == 0)
                        {
                            ClientBanksObs = GetClientForConsultant();
                        }
                        else
                        {
                            ClientBanksObs = GetClientForManedger();
                        };
                    }));
            }
        }
        #endregion Comands

        #region Metods
        /// <summary>
        /// Метод отображения данных для консультанта
        /// </summary>
        /// <returns>Возвращает список клиентов</returns>
        private ObservableCollection<ClientBank> GetClientForConsultant()
        {
            _handlerFile = new HandlerFile();
            _client = new ObservableCollection<ClientBank>();

            List<ClientBank> clientBanks = new List<ClientBank>();
            clientBanks = _handlerFile.LoadingDataFromFile();

            foreach (ClientBank clientBank in clientBanks)
            {
                clientBank.SeriesAndNumberPassportClient = "«******************»";
                _client.Add(clientBank);
            }
            return _client;
        }
        /// <summary>
        /// Метод отображения данных для менеджара
        /// </summary>
        /// <returns>Возвращает список клиентов</returns>
        private ObservableCollection<ClientBank> GetClientForManedger()
        {
            _handlerFile = new HandlerFile();
            _client = new ObservableCollection<ClientBank>(_handlerFile.LoadingDataFromFile());

            return _client;
        }
        #endregion Metods
    }
}
