using ATMProject;
using System;
using System.Windows.Forms;

namespace ATMproject
{
    public partial class Form1 : Form
    {
        private Account currAccount = new Account(); 
        private ATM currATM = new ATM();

        public Form1()
        {
            InitializeComponent();
            currAccount.generateTestAccount();

            txtboxDialogBox.Clear();
            txtboxDialogBox.AppendText("Введите пароль и нажмите ENTER.");
            txtboxProgramInfo.AppendText("Имя пользователя ATM: " + currAccount.name + " " + currAccount.surname
                + ". Сгенерированный PIN: " + currAccount.GetPassword()
                + ". Сгенерированный баланс: " + currAccount.GetAmount() + " руб.");
        }

        private void btnOne_Click(object sender, EventArgs e)
        {
            txtTerminal.Text = txtTerminal.Text + "1";
        }

        private void btnTwo_Click(object sender, EventArgs e)
        {
            txtTerminal.Text = txtTerminal.Text + "2";
        }

        private void btnThree_Click(object sender, EventArgs e)
        {
            txtTerminal.Text = txtTerminal.Text + "3";
        }

        private void btnFour_Click(object sender, EventArgs e)
        {
            txtTerminal.Text = txtTerminal.Text + "4";
        }

        private void btnFive_Click(object sender, EventArgs e)
        {
            txtTerminal.Text = txtTerminal.Text + "5";
        }

        private void btnSix_Click(object sender, EventArgs e)
        {
            txtTerminal.Text = txtTerminal.Text + "6";
        }

        private void btnSeven_Click(object sender, EventArgs e)
        {
            txtTerminal.Text = txtTerminal.Text + "7";
        }

        private void btnEight_Click(object sender, EventArgs e)
        {
            txtTerminal.Text = txtTerminal.Text + "8";
        }

        private void btnNine_Click(object sender, EventArgs e)
        {
            txtTerminal.Text = txtTerminal.Text + "9";
        }

        private void btnZero_Click(object sender, EventArgs e)
        {
            txtTerminal.Text = txtTerminal.Text + "0";
        }

        private void btnC_Click(object sender, EventArgs e)
        {
            if (txtTerminal.Text.Length > 0)
            {
                txtTerminal.Text = txtTerminal.Text.Remove(txtTerminal.Text.Length - 1, 1);
            }
        }

        private void btn_Clear(object sender, EventArgs e)
        {
            txtTerminal.Text = "";
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult iExit;

            iExit = MessageBox.Show("Confirm you want to exit", "ATM System", MessageBoxButtons.YesNo, 
                MessageBoxIcon.Question);

            if(iExit == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void ATM_Load(object sender, EventArgs e)
        {
            btnBalance.Enabled = false;
            btnDeposit.Enabled = false;
            btnWithdraw.Enabled = false;
            btnState.Enabled = false;

            groupBoxModeDeposit.Visible = false;
            groupBoxModeWithdrawal.Visible = false;
        }

        private void btnEnter_Click(object sender, EventArgs e)
        {
            if (currATM.GetMode() == "authorization")
            {
                string pin1 = string.Format(txtTerminal.Text);

                if (pin1.Equals(currAccount.GetPassword()))
                { 
                    btnBalance.Enabled = true;
                    btnDeposit.Enabled = true;
                    btnWithdraw.Enabled = true;
                    btnState.Enabled = true;
                    txtTerminal.Clear();
                    txtboxDialogBox.Clear();
                    txtboxDialogBox.AppendText("Добро пожаловать. Выберите действие, которое хотите выполнить");
                    currATM.SetMode("");
                }
                else
                {
                    txtboxDialogBox.Clear();
                    txtboxDialogBox.AppendText("Неверный PIN! Попробуйте снова.");
                    txtTerminal.Clear();
                }
            }
            else if (currATM.GetMode() == "withdraw")
            {
                if(Validator.IsValidWithdrawal(checkBoxL, checkBoxS, txtTerminal, 
                    currAccount.GetAmount(), currATM.GetCurrNotesSum()))
                {
                    currATM.ExecuteModeWithdrawal(txtTerminal, checkBoxS, checkBoxL, currAccount);
                }

                groupBoxModeWithdrawal.Visible = false;
                txtboxDialogBox.Clear();
                txtTerminal.Clear();
                checkBoxL.Checked = false;
                checkBoxS.Checked = false;
                currATM.SetMode("");
            }
            else if (currATM.GetMode() == "deposit")
            {
                currATM.ExecuteModeDeposit(groupBoxModeDeposit, currAccount);
                
                groupBoxModeDeposit.Visible = false;
                txtboxDialogBox.Clear();
                txtTerminal.Clear();
                currATM.SetMode("");
            }
        }
        
        private void btnBalance_Click(object sender, EventArgs e)
        {
            groupBoxModeDeposit.Visible = false;
            groupBoxModeWithdrawal.Visible = false;

            txtTerminal.Text = "";
            txtboxDialogBox.Clear();
            txtboxDialogBox.AppendText("Ваш текущий баланс составляет " + currAccount.GetAmount() + " рублей.");
        }

        private void btnDeposit_Click(object sender, EventArgs e)
        {
            groupBoxModeWithdrawal.Visible = false;

            txtTerminal.Text = "";

            txtboxDialogBox.Clear();
            txtboxDialogBox.AppendText("Выберите купюры и их количество в контейнере \"Купюры\" " +
                "и нажмите ENTER.");

            groupBoxModeDeposit.Visible = true;
            currATM.SetMode("deposit");
        }

        private void btnWithdraw_Click(object sender, EventArgs e)
        {
            groupBoxModeDeposit.Visible = false;

            txtTerminal.Text = "";
            txtboxDialogBox.Clear();
            txtboxDialogBox.AppendText("Введите необходимую сумму на клавиатуре. Выберите желаемое " +
                "достоинство купюр. Нажмите ENTER.");
 
            groupBoxModeWithdrawal.Visible = true;
            currATM.SetMode("withdraw");
        }

        private void btnState_Click(object sender, EventArgs e) 
        {
            MessageBox.Show(currATM.GetState(), "Банкомат сейчас содержит:");
        }
    }
}
