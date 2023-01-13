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
            _initialClient = new ObservableCollection<ClientBank>();
            _initialClient = CurrentListClient();
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
        private ObservableCollection<ClientBank> _initialClient;
        private HandlerFile _handlerFile; 
        private int _selectIndexWorcer;
        private string _nameBankWerker;
        private int _columnChanged;

        public int ColumnChanged { get => _columnChanged; set => _columnChanged = value; }
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

        DelegateCommand _saveChanged;
        public DelegateCommand SaveChanged
        {
            get
            {
                return _saveChanged ??
                    (_saveChanged = new DelegateCommand(obj =>
                    {
                       
                            List<ClientBank> clientBanks = new List<ClientBank>();
                            ObservableCollection<ClientBank> ChangeClientBanksObs = new ObservableCollection<ClientBank>();
                            ChangeClientBanksObs = ChangeList(_initialClient, ClientBanksObs, NameBankWerker, SelectIndexWorcer);
                            clientBanks = ChangeClientBanksObs.ToList();
                            _handlerFile = new HandlerFile();
                            _handlerFile.SeaveClientListFile(clientBanks);
                        
                       
                        
                        _initialClient = CurrentListClient();
                    }));
            }
        }
        #endregion Comands

        #region Metods

        private ObservableCollection<ClientBank> CurrentListClient()
        {
            ObservableCollection<ClientBank> result = new ObservableCollection<ClientBank>();
            foreach (var client in ClientBanksObs)
            {
                ClientBank clientB = new ClientBank(client.LastnameClient, client.NameClient, client.PatronymicClient, client.NumberPhoneClient, client.SeriesAndNumberPassportClient
                    , client.WhoCangedFile, client.TimeOfChange, client.WhatDataHasChangedInFile);
                result.Add(clientB);
            }
            return result;
        }
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
        /// <summary>
        /// Изменяет коллекцию клиентов
        /// </summary>
        /// <param name="initialStait">Изначальная коллекция сотрудников</param>
        /// <param name="afterChanged">Коллекция после изменения в окне</param>
        /// <param name="nameWorker">Имя работника</param>
        /// <returns>Возвращает изменёную коллекцию клиентов для записи в файл</returns>
        private ObservableCollection<ClientBank> ChangeList(ObservableCollection<ClientBank> initialStait, ObservableCollection<ClientBank> afterChanged, String nameWorker, int workerChange)
        {
            ObservableCollection<ClientBank> result = new ObservableCollection<ClientBank>();
            string changeString = string.Empty;
            string nameWoker=string.Empty;
            string changeLastName = "Фамилия, ";
            string changeName = "Имя, ";
            string changePatranomic = "Отчество, ";
            string changeNumberPhon = "Телефон, ";
            string changeSeriesAndNamber = "Серия и номер паспорта, ";
            DateTime dateTime = DateTime.Now;
            int cauntCange = 0;
          
            for (int i = 0; i < initialStait.Count; i++)
            {
                changeString = initialStait[i].WhatDataHasChangedInFile;
                
                #region Изменение фамилии
                if (workerChange == 1)
                {
                    if (initialStait[i].LastnameClient != afterChanged[i].LastnameClient)
                    {
                        changeString += changeLastName;
                        cauntCange++;
                    }
                }
                else
                {
                    changeString += initialStait[i].LastnameClient;
                    MessageForConsultant();
                    break;
                }
                #endregion Изменение фамилии

                #region Изменение имени клиента
                if (workerChange == 1)
                {
                    if (initialStait[i].NameClient != afterChanged[i].NameClient)
                    {
                        changeString += changeName;
                        cauntCange++;
                    }
                }
                else
                {
                    changeString += initialStait[i].NameClient;
                    MessageForConsultant();
                    break;
                }
                #endregion Изменение имени клиента

                #region Изменение отчества
                if (workerChange == 1)
                {
                    if (initialStait[i].PatronymicClient != afterChanged[i].PatronymicClient)
                    {
                        changeString += changePatranomic;
                        cauntCange++;
                    }
                }
                else
                {
                    changeString += initialStait[i].PatronymicClient;
                    MessageForConsultant();
                    break;
                }
                #endregion Изменение отчества

                #region Изменение номера телефона
                if (initialStait[i].NumberPhoneClient != afterChanged[i].NumberPhoneClient)
                {
                    changeString += changeNumberPhon;
                    cauntCange++;
                }
                #endregion Изменение номера телефона 

                #region Изменение серии и номера паспрота
                if (workerChange == 1)
                {
                    if (initialStait[i].SeriesAndNumberPassportClient != afterChanged[i].SeriesAndNumberPassportClient)
                    {
                        changeString += changeSeriesAndNamber;
                        cauntCange++;
                    }
                }
                else
                {
                    changeString += initialStait[i].SeriesAndNumberPassportClient;
                    MessageForConsultant();
                }
                #endregion Изменение серии и номера паспрота

                //проверка былили изменения чтобы установить Имя изменявшего
                if (cauntCange>0)
                {
                    nameWoker = nameWorker;
                }
                else
                {
                    nameWoker = initialStait[i].WhoCangedFile;
                }

                result.Add(new ClientBank(afterChanged[i].LastnameClient, afterChanged[i].NameClient, afterChanged[i].PatronymicClient,
                    afterChanged[i].NumberPhoneClient, afterChanged[i].SeriesAndNumberPassportClient, nameWoker, dateTime, changeString));

                changeString = string.Empty;
                nameWoker = string.Empty;
                cauntCange = 0;
            }
            return result;

            void MessageForConsultant()
            {
                MessageBox.Show("Консультант может менять только номер телефона");
            }

        }
        #endregion Metods
        enum worker : int
        {
            Consultant,
            Manager
        }
    }
}
