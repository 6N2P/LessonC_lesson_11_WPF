using System;

namespace Lesson11_new.Class;

 public class ClientBank
{

    private string _lastNameClient = string.Empty;
    private string _nameClient = string.Empty;
    private string _patronymicClient = string.Empty;
    private decimal _numberPhoneClient = 0000000000;
    private string _seriesAndNumberPassportClient = string.Empty;
    private string _whoCangedFile = string.Empty;
    private DateTime _timeOfChange = new DateTime();
    private string _whatDataHasChangedInFile = string.Empty;

    public string LastnameClient { get => _lastNameClient; set => _lastNameClient = value; }
    public string NameClient { get => _nameClient; set => _nameClient = value; }
    public string PatronymicClient { get => _patronymicClient; set => _patronymicClient = value;  }
    public decimal NumberPhoneClient { get => _numberPhoneClient; set => _numberPhoneClient = value; }
    public string SeriesAndNumberPassportClient { get => _seriesAndNumberPassportClient; set => _seriesAndNumberPassportClient = value; }
    public string WhoCangedFile { get => _whoCangedFile; set => _whoCangedFile = value; }
    public DateTime TimeOfChange { get => _timeOfChange; set => _timeOfChange = value; }
    public string WhatDataHasChangedInFile { get => _whatDataHasChangedInFile; set => _whatDataHasChangedInFile = value; }

    public ClientBank(string lastName, string name, string patronamic, decimal numberPhone, string seriesAndNumber, string whoCangedFile, DateTime dateTime, string whatDataHasChanged)
    {
        this._lastNameClient = lastName;
        this._nameClient = name;
        this._patronymicClient = patronamic;
        this._numberPhoneClient = numberPhone;
        this.SeriesAndNumberPassportClient = seriesAndNumber;
        this._whoCangedFile = whoCangedFile;
        this.TimeOfChange = dateTime;
        this._whatDataHasChangedInFile = whatDataHasChanged;
    }
   
}
