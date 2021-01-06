using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATM;
using ATMproject;
using ATMProject;

namespace ATMproject
{

    public class CurrencyBill
    {
        private CurrencyBill next = CurrencyBill.Zero; //sets default handler instead of null object
        private static readonly CurrencyBill Zero;

        public int Denomination { get; } //номинал

        private int Quantity;

        public int GetQuantity()
        {
            return Quantity;
        }

        private void AddQuantity(int q)
        {
            Quantity += q;
        }

        public void addNewBills(int q)
        {
            AddQuantity(q);
        }


        //A static constructor that initializes static Zero property
        //This property is used as default next handler instead of a null object
        static CurrencyBill()
        {
            Zero = new ZeroCurrencyBill();
        }

        //Use to set static Zero property
        //Will always return false at it cannot process any given amount.
        public class ZeroCurrencyBill : CurrencyBill
        {
            public ZeroCurrencyBill() : base(0, 0)
            {
            }

            public override bool DispenseRequest(int amount, ATM currATM, ref int sum_dispense)
            {
                return false;
            }

            public override bool DepositRequest(int amount, int denom, Account currAccount, ATM currATM, ref int sum_deposit)
            {
                return false;
            }
        }

        //CurrencyBill constructor that set the denomination value and quantity
        public CurrencyBill(int denomination, int quantity)
        {
            Denomination = denomination;
            Quantity = quantity;
        }

        //Method that set next handler in the pipeline
        public CurrencyBill RegisterNext(CurrencyBill currencyBill)
        {
            next = currencyBill;
            return next;
        }

        //Method that processes the request or passes it to the next handler
        public virtual bool DispenseRequest(int amount, ATM currATM, ref int sum_dispense)
        {
            if (amount >= Denomination)
            {
                var num = GetQuantity();
                var remainder = amount;
                while (remainder >= Denomination && num > 0 
                    && currATM.GetCurrNotesAmount() + 1 <= currATM.GetMaxNotesAmount())
                {
                    remainder -= Denomination;
                    num--; 
                }

                //added
                sum_dispense += Denomination * (GetQuantity() - num);
                Quantity -= num;
                //added

                if (remainder != 0)
                {
                    return next.DispenseRequest(remainder, currATM, ref sum_dispense);
                }

                return true;
            }
            else
            {
                return next.DispenseRequest(amount, currATM, ref sum_dispense);
            }

        }
       
        //Method that processes the request or passes it to the next handler
        public virtual bool DepositRequest(int amount, int denom, Account currAccount, ATM currATM, ref int sum_deposit)
        {

            if (amount + currATM.GetCurrNotesAmount() > currATM.GetMaxNotesAmount())
            {
                return false;
            }
            else if (denom == Denomination)
            {
                //то добавляем в ATM -> новое количество купюр
                addNewBills(amount);

                //добавляем к текущему количеству купюр 
                currATM.SetCurrNotesAmount(amount);

                //это сумма денег, выполняем транзакцию
                int currTransactionSum = amount * denom;

                sum_deposit += currTransactionSum;
                
                return true;
            }
            else
            {
                return next.DepositRequest(amount, denom, currAccount, currATM, ref sum_deposit);
            }
        }

        

    }
}
