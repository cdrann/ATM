using ATMProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;


namespace ATMproject
{
    public class ATM
    {
        public Dictionary<int, int> Cash = new Dictionary<int, int> //DO PRIVATE + getter setter
        {
            [50] = 0,
            [100] = 0,
            [200] = 0,
            [500] = 0,
            [1000] = 0,
            [2000] = 0,
            [5000] = 0
        };

        private string mode = "authorization";

        private int maxNotesAmount = 10;

        private int currNotesAmount = 0;

        private int currNotesSum = 0;

        public string GetMode()
        {
            return mode;
        }

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

        public void AddCurrNotesAmount(int val)
        {
            currNotesAmount += val;
        }

        public int GetCurrNotesSum()
        {
            return currNotesSum;
        }

        public void AddCurrNotesSum(int val)
        {
            currNotesSum += val;
        }

        public string GetState()
        {
            string state = "";
            foreach (KeyValuePair<int, int> kvp in Cash)
            {
                state += string.Format("{0}: {1} купюр\n",
                    kvp.Key, kvp.Value);
            }

            return state;
        }

        public void ExecuteModeDeposit_Iteration(GroupBox box, Account currAccount)
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

            if (state == false && !string.IsNullOrEmpty(curramount))
            {
                MessageBox.Show("Введено количество купюр, но не отмечена необходимость внесения, " +
                    "повторите операцию");
            }
            else if (state)
            {
                Validator.CheckingNotes(curramount, name, this, currAccount);
            }

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

        public void ExecuteModeDeposit(GroupBox groupBoxModeDeposit, Account currAccount)
        {
            foreach (GroupBox box in groupBoxModeDeposit.Controls.OfType<GroupBox>())
            {
                ExecuteModeDeposit_Iteration(box, currAccount);
            }
        }

        public void ExecuteModeWithdrawal(TextBox txtTerminal, CheckBox checkBoxS, CheckBox checkBoxL, Account currAccount)
        {
            bool isImpossible = false;
            int withdrawal = int.Parse(txtTerminal.Text); 

            Dictionary<int, int> result = Withdraw(withdrawal, ref isImpossible,
                checkBoxS, checkBoxL, txtTerminal);

            if (!isImpossible)
            {
                int withdrawnNotes = 0;
                var oldState = new Dictionary<int, int>(Cash);
                foreach (var pair in oldState)
                {
                    if (result.ContainsKey(pair.Key))
                    {
                        Cash[pair.Key] = oldState[pair.Key] - result[pair.Key];
                        withdrawnNotes += result[pair.Key];
                    }
                }

                MessageBox.Show(withdrawal + " успешно выдано.");

                currAccount.AddAmount(-withdrawal);
                AddCurrNotesSum(-withdrawal);
                AddCurrNotesAmount(-withdrawnNotes);
            }
        }


        public Dictionary<int, int> Withdraw(int amount, ref bool fl, 
            CheckBox checkBoxS, CheckBox checkBoxL, TextBox txtTerminal)
        {
            Dictionary<int, int> r = new Dictionary<int, int>();
            Dictionary<int, int> source = new Dictionary<int, int>(Cash);

            if (!checkBoxL.Checked && txtTerminal != null)
            {
                WithdrawSmall(amount, source, r, ref fl);
            }
            else if (!checkBoxS.Checked && txtTerminal != null)
            {
                WithdrawLarge(amount, source, r, ref fl);
            }

            r = r.Where(kvp => kvp.Value != 0).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            return r;
        }


        public void WithdrawLarge(int sumNeeded, Dictionary<int, int> source, Dictionary<int, int> result, ref bool fl)
        {
            try
            {
                int change;
                int k = 0;

                if (source.Count > result.Count)
                {
                    k = source.Where(kvp => !result.ContainsKey(kvp.Key)).ToDictionary(kvp => kvp.Key,
                        kvp => kvp.Value).Keys.Max();
                }
                else
                {
                    fl = true;
                    throw new Exception("Требуемую сумму невозможно выдать");
                }

                KeyValuePair<int, int> sel = new KeyValuePair<int, int>(k, sumNeeded / k);
                if (sel.Value > source[sel.Key])
                {
                    change = sumNeeded - sel.Key * source[sel.Key];
                    sel = new KeyValuePair<int, int>(sel.Key, source[sel.Key]);
                }
                else
                {
                    change = sumNeeded - sel.Key * sel.Value;
                }

                if (change == 0)
                {
                    result.Add(sel.Key, sel.Value);
                    return;
                }

                if (change < source.Keys.Min())
                {
                    sel = new KeyValuePair<int, int>(sel.Key, sel.Value - 1);
                    result.Add(sel.Key, sel.Value);
                    WithdrawLarge(sumNeeded - sel.Key * sel.Value, source, result, ref fl);
                    return;
                }

                source[sel.Key] -= sel.Value;
                result.Add(sel.Key, sel.Value);

                WithdrawLarge(change, source, result, ref fl);
            }
            catch (Exception ex)
            {
                if (ex.Source != null)
                    MessageBox.Show("Ошибка при попытке выдать крупными купюрами, попробуйте другой способ "
                        + ex.Source);
                return;
            }
        }


        public void WithdrawSmall(int sumNeeded, Dictionary<int, int> source, Dictionary<int, int> result, ref bool fl)
        {
            try
            {
                int change;
                int k = 0;

                if (source.Count > result.Count)
                {
                    k = source.Where(kvp => !result.ContainsKey(kvp.Key)).ToDictionary(kvp => kvp.Key,
                        kvp => kvp.Value).Keys.Min();
                }
                else
                {
                    fl = true;
                    throw new Exception("Требуемую сумму невозможно выдать");
                }

                KeyValuePair<int, int> sel = new KeyValuePair<int, int>(k, sumNeeded / k);
                if (sel.Value > source[sel.Key])
                {
                    change = sumNeeded - sel.Key * source[sel.Key];
                    sel = new KeyValuePair<int, int>(sel.Key, source[sel.Key]);
                }
                else
                {
                    change = sumNeeded - sel.Key * sel.Value;
                }

                if (change == 0)
                {
                    result.Add(sel.Key, sel.Value);
                    return;
                }

                if (change > source.Keys.Max())
                {
                    sel = new KeyValuePair<int, int>(sel.Key, sel.Value + 1);
                    result.Add(sel.Key, sel.Value);
                    WithdrawSmall(sumNeeded - sel.Key * sel.Value, source, result, ref fl);
                    return;
                }

                source[sel.Key] -= sel.Value;
                result.Add(sel.Key, sel.Value);

                WithdrawSmall(change, source, result, ref fl);
            }
            catch (Exception ex)
            {
                if (ex.Source != null)
                    MessageBox.Show("Ошибка при попытке выдать мелкими купюрами, попробуйте другой способ "
                        + ex.Source);
                return;
            }
        }
    }
}
