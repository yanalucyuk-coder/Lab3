using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3
{
    public class DepositAccount : FinancialAccount
    {
        double interestRate;
        public double InterestRate
        {
            get { return interestRate; }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Відсоткова ставка не може бути від'ємною!", nameof(value));
                }
                interestRate = value;
            }
        }

        public DepositAccount (double balance, string currency, double interestRate) : base (balance, currency)
        {
            if (balance < 0)
            {
                throw new ArgumentException ("Баланс депозиту не може бути від'ємним!", nameof(balance));
            }
            this.InterestRate = interestRate;
        }

        public override string Withdraw(double amount)
        {
            CheckAmount (amount);
            if(amount > Balance)
            {
                throw new InvalidOperationException("Недостатньо коштів для зняття на депозитному рахунку!");
            }
            Balance-=amount;
            return $"Знято {amount:N2} {Currency}. Баланс: {Balance:N2} {Currency}";
        }

        public virtual double CalculateInterest (int months)
        {
            if (months <0)
            {
                throw new ArgumentException("К-сть місяців не можу бути від'ємною!", nameof(months));
            }
            return Math.Round(Balance * InterestRate / 100 * months / 12, 2);
        }
    }
}
