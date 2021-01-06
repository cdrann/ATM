using ATMproject;
using ATMProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ATMProject
{
    static class Validator
    {

        public static bool IsValidWithdrawal(CheckBox checkBoxL, CheckBox checkBoxS, TextBox txtTerminal, int accountAmount, int ATMsum)
        {
            if (checkBoxL.Checked && checkBoxS.Checked)
            {
                MessageBox.Show("Вы не можете выбрать сразу два способа выдачи.");
                return false;
            }
            else if (string.IsNullOrEmpty(txtTerminal.Text))
            {
                MessageBox.Show("Вы не ввели сумму для выдачи.");
                return false;
            }
            else if (!checkBoxS.Checked && !checkBoxL.Checked)
            {
                MessageBox.Show("Вы не выбрали купюрами какого достоинства произвести выдачу.");
                return false;
            }
            else if (int.Parse(txtTerminal.Text) < 0)
            {
                MessageBox.Show("Вы не можете снять отрицательную сумму.");
                return false;
            }
            else if (int.Parse(txtTerminal.Text) > accountAmount)
            {
                MessageBox.Show("На Вашем счету недостаточно средств.");
                return false;
            }
            else if (int.Parse(txtTerminal.Text) > ATMsum)
            {
                MessageBox.Show("В ATM недостаточно купюр, чтобы выдать эту сумму");
                return false;
            }

            return true;
        }


        //TODO void not Is...
        /* public static void isValidateNotesToDeposit(string amount, int N, ATMproject.ATM currATM, Account currAccount)
        { 
            if (!string.IsNullOrEmpty(amount))
            {
                int currN = int.Parse(amount);

                if (currN < 0)
                {
                    MessageBox.Show("Введена отрицательная сумма.");
                }
                else if (currATM.GetCurrNotesAmount() + currN > currATM.GetMaxNotesAmount())
                {
                    MessageBox.Show("Превышено ограничение по количеству хранимых в ATM купюр.");
                }
                else
                {
                    var currCashAmount = currATM.getCurrCashInATM();
                    
                    // будет ли так оно меняться вообще? ..
                    currCashAmount[N] += currN;

                    currATM.AddCurrNotesAmount(currN);
                    currATM.AddCurrNotesSum(currN * N);
                    currAccount.AddAmount(currN * N);
                    MessageBox.Show("На счет внесено " + currN * N);
                }
            }
            else
            {
                MessageBox.Show("Вы не ввели количество купюр номинала " + N +
                    ". Попробуйте операцию снова.");
            }    
        } */


        public static bool IsValidDeposit(GroupBox groupBoxModeDeposit)
        {
            foreach (GroupBox box in groupBoxModeDeposit.Controls.OfType<GroupBox>())
            {
                bool state = false;
                int name = 0;
                string curramount = "";

                for (int i = 0; i < box.Controls.Count; i++)
                {
                    if (box.Controls[i] is CheckBox)
                    {
                        state = ((CheckBox)box.Controls[i]).Checked;
                        name = int.Parse(((CheckBox)box.Controls[i]).Text);
                    }
                    if (box.Controls[i] is TextBox)
                    {
                        curramount = ((TextBox)box.Controls[i]).Text;
                    }
                }

                
                




                //updating boxes
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

            }


                return true;
        }

        internal static void ValidateCurrBox(KeyValuePair<string, bool> kvp)
        {
            if (kvp.Value == false && !string.IsNullOrEmpty(kvp.Key))
            {
                MessageBox.Show("Введено количество купюр, но не отмечена необходимость внесения, " +
                    "повторите операцию");
            }

            //throw new NotImplementedException();
        }
    }
}
