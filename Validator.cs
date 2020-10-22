using ATMproject;
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


        public static void CheckingNotes(string amount, int N, ATM currATM, Account currAccount)
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
                    currATM.Cash[N] += currN;
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
        }


    }
}
