using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ATMproject;

namespace ATM
{
    class Withdrawal : Transaction
    {
        int amount;
        bool isSmallMode;
        bool isLargeMode;

        public Withdrawal(ATMproject.ATM _currATM, Account _currAccount, 
            int _amount, bool _isSmallMode, bool _isLargeMode) : base(_currATM, _currAccount)
        {
            amount = _amount;
            isSmallMode = _isSmallMode;
            isLargeMode = _isLargeMode;
        }

        public override void Execute()
        {
            int sum_withdrawal = CurrATM.ExecuteWithdraw(isSmallMode, isLargeMode, amount);

            CurrAccount.AddAmount(-sum_withdrawal);
        }

        

    }
}
