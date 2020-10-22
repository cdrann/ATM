namespace ATMproject
{
    public class Account
    {
        private string pin;
        public string name { get; set; }
        public string surname { get; set; }
        private int amount;

        internal void generateTestAccount()
        {
            pin = "1234";
            name = "Anna";
            surname = "Redko"; 
            amount = 10175;
        }

        public int GetAmount()
        {
            return amount;
        }

        public void AddAmount(int amountAdded)
        {
            amount += amountAdded;
        }

        public string GetPassword()
        {
            return pin;
        }
    }
}
