using System;

namespace Lesson11_new.Class;

 public class ClientBank
{

    string lastNameClient = string.Empty;
    string nameClient = string.Empty;
    string patronymicClient = string.Empty;
    decimal numberPhoneClient = 0000000000;
    string seriesAndNumberPassportClient = string.Empty;
    string whoCangedFile = string.Empty;
    DateTime timeOfChange = new DateTime();
    string whatDataHasChangedInFile = string.Empty;

    public string LastnameClient { get => lastNameClient; }
    public string NameClient { get => nameClient; }
    public string PatronymicClient { get => patronymicClient; }
    public decimal NumberPhoneClient { get => numberPhoneClient; set => numberPhoneClient = value; }
    public string SeriesAndNumberPassportClient { get => seriesAndNumberPassportClient; set => seriesAndNumberPassportClient = value; }
    public string WhoCangedFile { get => whoCangedFile; set => whoCangedFile = value; }
    public DateTime TimeOfChange { get => timeOfChange; set => timeOfChange = value; }
    public string WhatDataHasChangedInFile { get => whatDataHasChangedInFile; set => whatDataHasChangedInFile = value; }

    public ClientBank(string lastName, string name, string patronamic, decimal numberPhone, string seriesAndNumber, string whoCangedFile, DateTime dateTime, string whatDataHasChanged)
    {
        this.lastNameClient = lastName;
        this.nameClient = name;
        this.patronymicClient = patronamic;
        this.numberPhoneClient = numberPhone;
        this.SeriesAndNumberPassportClient = seriesAndNumber;
        this.whoCangedFile = whoCangedFile;
        this.TimeOfChange = dateTime;
        this.whatDataHasChangedInFile = whatDataHasChanged;
    }

    public override string ToString()

    {
        return $"{LastnameClient} {NameClient} {PatronymicClient} {NumberPhoneClient} '********' {WhoCangedFile} {this.TimeOfChange} {WhatDataHasChangedInFile}";
    }
}
