using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace Lesson11_new.Class
{/// <summary>
/// Класс для работы с файлом
/// </summary>
     class HandlerFile
    {
        static string path = @"C:\Users\Ivanovsv\Desktop\Lessons\Lesson11_new\client.txt";
        //static string path = Path.GetFullPath(Lesson_11.Properties.Resources.client).ToString();
        /// <summary>
        /// Считывает из файла и создаёт лист клиентов
        /// </summary>
        /// <returns></returns>
        public  List<ClientBank> LoadingDataFromFile()
        {
            List<ClientBank> clientList = new List<ClientBank>();
            string stringData = File.ReadAllText(path);
            stringData = Regex.Replace(stringData, "\r", "");
            stringData = Regex.Replace(stringData, "\n", "");
            stringData = Regex.Replace(stringData, "\t", "");
            string[] data = stringData.Split('#');
            
            int countData = data.Length;
            int numberOfValuesInRow = 8;
            int countClient = countData / numberOfValuesInRow;

            for (int i = 0; i < countClient; i++)
            {
                ClientBank clientBank = new ClientBank(data[i * numberOfValuesInRow + 0],
                    data[i * numberOfValuesInRow + 1],
                    data[i * numberOfValuesInRow + 2],
                    Convert.ToDecimal(data[i * numberOfValuesInRow + 3]),
                    data[i * numberOfValuesInRow + 4],
                    data[i * numberOfValuesInRow + 5],
                    Convert.ToDateTime(data[i * numberOfValuesInRow + 6]),
                    data[i * numberOfValuesInRow + 7]);

                clientList.Add(clientBank);

            }

            return clientList;
        }

        /// <summary>
        /// сохроняет в файл список из клиентов
        /// </summary>
        /// <param name="clienList"></param>
        public void SeaveClientListFile(List<ClientBank> clienList)
        {
            string[] newLineMasive;
            newLineMasive = new string[clienList.Count];

            for (int i = 0; i < clienList.Count; i++)
            {
                newLineMasive[i] = HandlerFile.ClientToString(clienList[i]);
            }
            File.WriteAllLines(path, newLineMasive);
        }
        /// <summary>
        /// переводит клиента в строку для записи в файл
        /// </summary>
        /// <param name="clientBank"></param>
        /// <returns></returns>
        static string ClientToString(ClientBank clientBank)
        {
            string clientString = $"{clientBank.LastnameClient}#{clientBank.NameClient}#" +
                $"{clientBank.PatronymicClient}#{clientBank.NumberPhoneClient}#{clientBank.SeriesAndNumberPassportClient}" +
                $"#{clientBank.WhoCangedFile}#{clientBank.TimeOfChange}#{clientBank.WhatDataHasChangedInFile}#";
            return clientString;
        }
    }
}

