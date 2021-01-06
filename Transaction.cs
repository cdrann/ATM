using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATMproject;

namespace ATM
{
    public abstract class Transaction
    {
        private ATMproject.ATM currATM;
        private Account currAccount;

        public Transaction(ATMproject.ATM _currATM, Account _currAccount)
        {
            currATM = _currATM;
            currAccount = _currAccount;
        }

        public ATMproject.ATM CurrATM
        {
            get
            {
                return currATM;
            }
        }

        public ATMproject.Account CurrAccount
        {
            get
            {
                return currAccount;
            }
        }

        public abstract void Execute();
    }
}
