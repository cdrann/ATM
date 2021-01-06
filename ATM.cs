using ATMProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ATMproject
{
    public class ATM
    {
        public CurrencyBill bills50s;
        public CurrencyBill bills100s;
        public CurrencyBill bills200s;
        public CurrencyBill bills500s;
        public CurrencyBill bills1000s;
        public CurrencyBill bills2000s;
        public CurrencyBill bills5000s;

        private string mode;

        private int maxNotesAmount;

        private int currNotesAmount;


        public ATM()
        {
            //creating handlers, setting initial state of ATM
            bills50s = new CurrencyBill(50, 1);
            bills100s = new CurrencyBill(100, 2);
            bills200s = new CurrencyBill(200, 3);
            bills500s = new CurrencyBill(500, 2);
            bills1000s = new CurrencyBill(1000, 4);
            bills2000s = new CurrencyBill(2000, 1);
            bills5000s = new CurrencyBill(5000, 1);

            bills50s
                .RegisterNext(bills100s)
                  .RegisterNext(bills200s)
                  .RegisterNext(bills500s)
                  .RegisterNext(bills1000s)
                  .RegisterNext(bills2000s)
                  .RegisterNext(bills5000s);

             mode = "authorization";

             maxNotesAmount = 15;

             currNotesAmount = 0;
        }




        public Dictionary<int, int> getCurrCashInATM()
        {
            return new Dictionary<int, int>
            {
                [50] = bills50s.GetQuantity(),
                [100] = bills100s.GetQuantity(),
                [200] = bills200s.GetQuantity(),
                [500] = bills500s.GetQuantity(),
                [1000] = bills1000s.GetQuantity(),
                [2000] = bills2000s.GetQuantity(),
                [5000] = bills5000s.GetQuantity()
            };
        }

        



        public string GetMode()
        {
            return mode;
        }

        //TODO : то что тут PUBLIC -- плохо?
        public void SetMode(string newMode)
        {
            mode = newMode;
        }

        public int GetMaxNotesAmount()
        {
            return maxNotesAmount;
        }

        public int GetCurrNotesAmount()
        {
            return currNotesAmount;
        }


        //TODO : то что тут PUBLIC -- плохо?
        public void SetCurrNotesAmount(int amount)
        {
            currNotesAmount += amount;
        }

        public int GetCurrNotesSum()
        {
            Dictionary<int, int> currState = getCurrCashInATM();
            int sum = 0;

            foreach (KeyValuePair<int, int> keyValue in currState)
            {
                sum += keyValue.Key * keyValue.Value;
            }

            return sum;
        }





        public string GetStateReadable()
        {
            string state = "";

            var currCashState = getCurrCashInATM();

            foreach (KeyValuePair<int, int> kvp in currCashState)
            {
                state += string.Format("{0}: {1} купюр\n",
                    kvp.Key, kvp.Value);
            }

            return state;
        }





        // another class??
        public static Dictionary<int, int> getInfoAboutBillsToDeposit(GroupBox groupBoxModeDeposit)
        {
            Dictionary<int, int> BillsToDeposit = new Dictionary<int, int>(); ;

            foreach (GroupBox box in groupBoxModeDeposit.Controls.OfType<GroupBox>())
            {
                KeyValuePair<int, int> denomination_quantity_pair = ExecuteGetNumBills_Iteration(box); 
                
                BillsToDeposit.Add(denomination_quantity_pair.Key, denomination_quantity_pair.Value);
            }

            return BillsToDeposit;
        }

        public static KeyValuePair<int, int> ExecuteGetNumBills_Iteration(GroupBox box)
        {
            bool state = false;
            int denomination = 0; //denomination
            
            string curramount = ""; //quantity
            int quantity;
       
            for (int i = 0; i < box.Controls.Count; i++)
            {
                if (box.Controls[i] is CheckBox)
                {
                    state = ((CheckBox)box.Controls[i]).Checked;
                    denomination = int.Parse(((CheckBox)box.Controls[i]).Text);
                }
                if (box.Controls[i] is TextBox)
                {
                    curramount = ((TextBox)box.Controls[i]).Text;
                }
            }

            KeyValuePair<string, bool> boxVals = new KeyValuePair<string, bool> (curramount, state);
            Validator.ValidateCurrBox(boxVals);

            quantity = int.Parse(curramount);
            KeyValuePair<int, int> denomination_quantity_pair = new KeyValuePair<int, int>(denomination, quantity);

            //TODO ref????????????????????????????????///
            for (int i = 0; i < box.Controls.Count; i++)
            {
                 if (box.Controls[i] is CheckBox)
                 {
                     ((CheckBox)box.Controls[i]).Checked = false;
                 }
                 if (box.Controls[i] is TextBox)
                 {
                     ((TextBox)box.Controls[i]).Text = string.Empty;
                 }
            } 
            

            return denomination_quantity_pair;
        }













        public int ExecuteWithdraw(bool isSmallMode, bool isLargeMode, int amount)
        {
            var currCashState = getCurrCashInATM();
            Dictionary<int, int> source = new Dictionary<int, int>(currCashState);

            CurrencyBill firstHandler = null;

            if (isSmallMode)
            {
                //set proper handlers pipeline
                bills50s
                    .RegisterNext(bills100s)
                    .RegisterNext(bills200s)
                    .RegisterNext(bills500s)
                    .RegisterNext(bills1000s)
                    .RegisterNext(bills2000s)
                    .RegisterNext(bills5000s);

                firstHandler = bills50s;
            }
            else if (isLargeMode)
            {
                //set proper handlers pipeline
                bills5000s
                    .RegisterNext(bills2000s)
                    .RegisterNext(bills1000s)
                    .RegisterNext(bills500s)
                    .RegisterNext(bills200s)
                    .RegisterNext(bills100s)
                    .RegisterNext(bills50s);

                firstHandler = bills5000s;
            }

            int sum_dispense = 0;
            doWithdraw(amount, firstHandler, ref sum_dispense);

            return sum_dispense; 
        }

        public void doWithdraw(int amount, CurrencyBill firstHandler, ref int sum_dispense)
        {
            while (true)
            {
                //sender pass the request to first handler in the pipeline
                
                var isDepensible = firstHandler.DispenseRequest(amount, this, ref sum_dispense);
                if (!isDepensible)
                {
                    MessageBox.Show($"Failed to dispense ${amount}!");
                }
            }
        }






        public int ExecuteDeposit(Dictionary<int, int> billsToDeposit, Account currAccount)
        {
            int sum_deposit = 0;
            foreach (KeyValuePair<int, int> keyValue in billsToDeposit)
            {
                doDeposit(keyValue.Value, keyValue.Key, currAccount, ref sum_deposit);
            }

            return sum_deposit; //sum that was deposited
        }

        public void doDeposit(int amount, int denomination, Account currAccount, ref int sum_deposit)
        {
            while (true)
            {
                //sender pass the request to first handler in the pipeline
                var isDeposible = bills50s.DepositRequest(amount, denomination, currAccount, this, ref sum_deposit);
                if (!isDeposible)
                {
                    MessageBox.Show($"Failed to deposit ${amount}!");
                    //привышено количество максимальное купюр в банкомате
                }
            }
        }

    }





}
