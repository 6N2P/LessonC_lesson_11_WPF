using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;
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
            NameBankWerker = "Консультант";
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
        private ObservableCollection<ClientBank> _actualClientFail;
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
                if (string.IsNullOrEmpty(value))
                {
                    MessageBox.Show("Поле имени сотрудника не должлно быть пустым");
                }
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

        
        /// <summary>
        /// Список клиентов банка из DataGeid
        /// </summary>
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
        /// <summary>
        /// Команда для сохронения изменений 
        /// </summary>
        public DelegateCommand SaveChanged
        {
            get
            {
                return _saveChanged ??
                    (_saveChanged = new DelegateCommand(obj =>
                    {
                       
                            List<ClientBank> clientBanks = new List<ClientBank>();
                            ObservableCollection<ClientBank> ChangeClientBanksObs = new ObservableCollection<ClientBank>();
                            ObservableCollection<ClientBank> ClientFail = GetActualClientFail();
                        
                            ChangeClientBanksObs = ChangeList(ClientFail, NameBankWerker, SelectIndexWorcer);
                            clientBanks = ChangeClientBanksObs.ToList();
                            _handlerFile = new HandlerFile();
                            _handlerFile.SeaveClientListFile(clientBanks);

                            _initialClient = CurrentListClient();
                        
                      
                    }));
            }
        }
        #endregion Comands

        #region Metods
        /// <summary>
        /// Метод возвращает список текущих клиентов
        /// </summary>
        /// <returns>Возвращает список для DataGrid</returns>
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
        /// Метод для получелния данных из файла со скрытием мерии и номера паспорта
        /// </summary>
        /// <returns>Возвращает список клиентов, не отображает серию и номер паспорта</returns>
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
        /// Метод для получения данных из Файла для Менеджера
        /// </summary>
        /// <returns>Возвращает список клиентов</returns>
        private ObservableCollection<ClientBank> GetClientForManedger()
        {
            _handlerFile = new HandlerFile();
            _client = new ObservableCollection<ClientBank>(_handlerFile.LoadingDataFromFile());

            return _client;
        }
        private ObservableCollection<ClientBank> GetActualClientFail()
        {
            _handlerFile = new HandlerFile();
            _actualClientFail = new ObservableCollection<ClientBank>(_handlerFile.LoadingDataFromFile());
            return _actualClientFail;
        }
        /// <summary>
        /// Изменяет коллекцию клиентов
        /// </summary>
        /// <param name="initialStait">Изначальная коллекция сотрудников</param>
        /// <param name="afterChanged">Коллекция после изменения в окне</param>
        /// <param name="nameWorker">Имя работника</param>
        /// <param name="workerChange">Значение выбранное в окне</param>
        /// <returns>Возвращает изменёную коллекцию клиентов для записи в файл</returns>
        private ObservableCollection<ClientBank> ChangeList(ObservableCollection<ClientBank> initialStait
            , String nameWorker, int workerChange)
        {
          

            ObservableCollection<ClientBank> result = new ObservableCollection<ClientBank>();
            string changeString = string.Empty;
            string _nameWoker=string.Empty;
            int initWorkerChange = workerChange;
            string changeLastName = "Фамилия, ";
            string afterChangeLastName = string.Empty;
            string changeName = "Имя, ";
            string afterChangeName=string.Empty;
            string changePatranomic = "Отчество, ";
            string afterChangePatranomic = string.Empty;
            string changeNumberPhon = "Телефон, ";
            decimal afterChangeNamberPhon = 0;
            string changeSeriesAndNamber = "Серия и номер паспорта, ";
            string afterChfngeSeriesAndNamber = string.Empty;
            DateTime dateTime = DateTime.Now;
            int cauntCange = 0;

            bool flagMessage=false;

            for (int i = 0; i < initialStait.Count; i++)
            {
                changeString = initialStait[i].WhatDataHasChangedInFile;

                #region Изменение фамилии

                if (initialStait[i].LastnameClient != ClientBanksObs[i].LastnameClient)
                {
                    if (initWorkerChange == 1)
                    {
                        afterChangeLastName = ClientBanksObs[i].LastnameClient;
                        changeString += changeLastName;
                        cauntCange++;                                            
                    }
                    else
                    {
                        afterChangeLastName = initialStait[i].LastnameClient;
                        flagMessage = true;                                             
                    }
                }
                else
                {
                    afterChangeLastName = initialStait[i].LastnameClient;                    
                }
                //проверка былили изменения чтобы установить Имя изменявшего
                if (cauntCange > 0)
                {
                    _nameWoker = workerChange == 1 ? "Менеджер " + nameWorker : "Консультант " + nameWorker;
                }
                else
                {
                    _nameWoker = initialStait[i].WhoCangedFile;
                }

                #endregion Изменение фамилии

                #region Изменение имени клиента

                if (initialStait[i].NameClient != ClientBanksObs[i].NameClient)
                {
                    if (initWorkerChange == 1)
                    {
                        afterChangeName = ClientBanksObs[i].NameClient;
                        changeString += changeName;
                        cauntCange++;                                          
                    }
                    else
                    {
                        afterChangeName = initialStait[i].NameClient;
                        flagMessage = true;                                             
                    }
                }
                else
                {
                    afterChangeName = initialStait[i].NameClient;                  
                }
                //проверка былили изменения чтобы установить Имя изменявшего
                if (cauntCange > 0)
                {
                    _nameWoker = workerChange == 1 ? "Менеджер " + nameWorker : "Консультант " + nameWorker;
                }
                else
                {
                    _nameWoker = initialStait[i].WhoCangedFile;
                }

                #endregion Изменение имени клиента

                #region Изменение отчества

                if (initialStait[i].PatronymicClient != ClientBanksObs[i].PatronymicClient)
                {
                    if (initWorkerChange == 1)
                    {
                        afterChangePatranomic = ClientBanksObs[i].PatronymicClient;
                        changeString += changePatranomic;
                        cauntCange++;                                          
                    }
                    else
                    {
                        afterChangePatranomic = initialStait[i].PatronymicClient;
                        flagMessage = true;                                               
                    }
                }
                else
                {
                    afterChangePatranomic = initialStait[i].PatronymicClient;                    
                }
                //проверка былили изменения чтобы установить Имя изменявшего
                if (cauntCange > 0)
                {
                    _nameWoker = workerChange == 1 ? "Менеджер " + nameWorker : "Консультант " + nameWorker;
                }
                else
                {
                    _nameWoker = initialStait[i].WhoCangedFile;
                }

                #endregion Изменение отчества

                #region Изменение номера телефона
                if (initialStait[i].NumberPhoneClient != ClientBanksObs[i].NumberPhoneClient)
                {
                    afterChangeNamberPhon = ClientBanksObs[i].NumberPhoneClient;
                    changeString += changeNumberPhon;
                    cauntCange++;                                
                }
                else
                {
                    afterChangeNamberPhon = initialStait[i].NumberPhoneClient;                    
                }
                //проверка былили изменения чтобы установить Имя изменявшего
                if (cauntCange > 0)
                {
                    _nameWoker = workerChange == 1 ? "Менеджер " + nameWorker : "Консультант " + nameWorker;
                }
                else
                {
                    _nameWoker = initialStait[i].WhoCangedFile;
                }
                #endregion Изменение номера телефона 

                #region Изменение серии и номера паспрота

                if (initialStait[i].SeriesAndNumberPassportClient != ClientBanksObs[i].SeriesAndNumberPassportClient)
                {
                    if (initWorkerChange == 1)
                    {
                        afterChfngeSeriesAndNamber = ClientBanksObs[i].SeriesAndNumberPassportClient;
                        changeString += changeSeriesAndNamber;
                        cauntCange++;                                           
                    }
                    else
                    {
                        afterChfngeSeriesAndNamber = initialStait[i].SeriesAndNumberPassportClient;
                        flagMessage = true;                                             
                    }
                }
                else
                {
                    afterChfngeSeriesAndNamber = initialStait[i].SeriesAndNumberPassportClient;                 
                }
                //проверка былили изменения чтобы установить Имя изменявшего
                if (cauntCange > 0)
                {
                    _nameWoker = workerChange == 1 ? "Менеджер " + nameWorker : "Консультант " + nameWorker;
                }
                else
                {
                    _nameWoker = initialStait[i].WhoCangedFile;
                }
                #endregion Изменение серии и номера паспрота
                if (flagMessage) MessageForConsultant();

                result.Add(new ClientBank(afterChangeLastName, afterChangeName, afterChangePatranomic,
                    afterChangeNamberPhon, afterChfngeSeriesAndNamber, _nameWoker, dateTime, changeString));

                changeString = string.Empty;
                _nameWoker = string.Empty;
                cauntCange = 0;
                flagMessage= false;
            }
            return result;        

        }
        void MessageForConsultant()
        {
            MessageBox.Show("Консультант может менять только номер телефона");
        }
        #endregion Metods
        enum worker 
        {
            Consultant = 0,
            Manager = 1
        }
    }
}
