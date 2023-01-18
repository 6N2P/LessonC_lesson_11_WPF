using System;
using System.Collections.Generic;
using System.Windows;

namespace Lesson11_new.Class
{
   public class BankManager : IWorker
    {
        string name = string.Empty;
        HandlerFile handlerFile;
        public string Name { get => name; set => name= value; }

        public BankManager() { }
        public BankManager(string name)
        {
            this.name = name;
        }

        public void CreateClient(string lastNameClient, string nameClient, string patronomikClient, decimal phoneClient, string seriesAndNamber)
        {
            #region Проверка входных данх
            if (string.IsNullOrEmpty(lastNameClient))
            {
                MessageBox.Show("Не заполнена фамилия");
                return;
            }
            if (string.IsNullOrEmpty(nameClient))
            {
                MessageBox.Show("Не заполненно имя");
                return ;
            }
            if (string.IsNullOrEmpty(patronomikClient))
            {
                MessageBox.Show("Не заполненно отчество");
                return;
            }
            if(phoneClient == 0)
            {
                MessageBox.Show("Не задан номер телефона");
                return;
            }
            if (string.IsNullOrEmpty(seriesAndNamber))
            {
                MessageBox.Show("Не заданы серия и номер паспорта");
                return;
            }
            #endregion Проверка входных данных

            DateTime dateTime = DateTime.Now;
            ClientBank clientBank = new ClientBank(lastNameClient, nameClient,
                patronomikClient, phoneClient, seriesAndNamber, "Менеджер " + Name, dateTime, "Создание клиента");

            List<ClientBank> clientBanks = new List<ClientBank>();
            handlerFile = new HandlerFile();
            clientBanks = handlerFile.LoadingDataFromFile();

            if (clientBanks != null && clientBank != null)
            {
                clientBanks.Add(clientBank);
                handlerFile.SeaveClientListFile(clientBanks);
            }
        }
    }
}
