using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ATMproject;

namespace ATM
{
    public class Deposit : Transaction
    {
        Dictionary<int, int> billsToDeposit;

        public Deposit(ATMproject.ATM _currATM, Account _currAccount, Dictionary<int, int> _billsToDeposit) : base(_currATM, _currAccount)
        {
            billsToDeposit = _billsToDeposit;
        }

        public override void Execute()
        {
            int amount = CurrATM.ExecuteDeposit(billsToDeposit, CurrAccount);

            CurrAccount.AddAmount(amount);
            
            // добавить манипуляции с базой данных итд
        }

        


    }
}
