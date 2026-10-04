using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3
{
    public abstract class FinancialAccount
    {
        double balance;
        public double Balance
        {
            get { return balance; }
            protected set { balance = value; }
        }

        string currency = "";
        public string Currency
        {
            get { return currency; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Валюта не може дорівнювати = 0 або бути порожньою!", nameof(value));
                }
                currency = value.Trim().ToUpper(); //забирає зайві пробіли
            }
        }

        public DateTime OpenDate;

        public FinancialAccount(double balance, string currency)
        {
            this.Balance = balance;
            this.Currency = currency;
            this.OpenDate = DateTime.Now;
        }

        protected void CheckAmount (double amount)
        {
            if(amount <= 0)
            {
                throw new ArgumentException ("Сума має бути не менше нуля!", nameof(amount));
            }

        }

        public virtual string Deposit(double amount)
        {
            CheckAmount(amount);
            Balance += amount;
            return $"Рахунок поповнено на {amount:N2} {Currency}. Баланс: {Balance:N2} {Currency}";
        }

        public abstract string Withdraw(double amount);
    }
}
