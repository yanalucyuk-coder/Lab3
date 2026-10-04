using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3
{
    public class CreditAccount : FinancialAccount
    {
        double creditLimit;
        public double CreditLimit
        {
            get { return creditLimit; }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Кредитний ліміт не може бути від'ємним!", nameof(value));
                }
                creditLimit = value;
            }
        }
        public CreditAccount (double balance, string currency, double creditLimit) : base(balance, currency)
        {
            this.CreditLimit = creditLimit;
            if(balance < -creditLimit)
            {
                throw new ArgumentException ("Борг перевищує кредитний ліміт!", nameof(balance));
            }
        }

        public override string Withdraw(double amount)
        {
            CheckAmount(amount);
            if(Balance - amount < -CreditLimit)
            {
                throw new InvalidOperationException("Перевищено кредитний ліміт!");
            }
            Balance -= amount;
            return $"Знято {amount:N2} {Currency}. Баланс: {Balance:N2} {Currency}";
        }

        public virtual double CalculateRepayment()
        {
            return Balance < 0 ? -Balance : 0;
        }
    }
}
