using System;

namespace Lesson11_new.Class
{
    public class BankConsultant : IWorker
    {
        string name = string.Empty;
        public string Name { get => name; set => name = value; }

        public BankConsultant(string nameWorkerBank)
        {
            this.name = nameWorkerBank;
          
        }
               
    }
}
