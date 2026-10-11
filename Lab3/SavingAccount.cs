using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3
{
    public class SavingAccount : FinancialAccount, IInterestBearing
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

        double minBalance;
        public double MinBalance
        {
            get { return minBalance; }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Мін. залишок не може бути від'ємним!", nameof(value));
                }
                minBalance = value;
            }
        }

        public SavingAccount(double balance, string currency, double interestRate, double minBalance) : base(balance, currency)
        {
            this.InterestRate = interestRate;
            this.MinBalance = minBalance;
            if(balance < minBalance)
            {
                throw new ArgumentException("Баланс не може бути меншим за мін. залишок!", nameof (balance));
            }
        }

        public override string Withdraw(double amount)
        {
            CheckAmount(amount);
            if(Balance-amount < minBalance)
            {
                throw new InvalidOperationException("Не можна знімати нижче мін. залишку!");
            }
            Balance -= amount;
            return $"Знято {amount:N2} {Currency}. Баланс: {Balance:N2} {Currency}";
        }

        public double CalculateInterest(int months)
        {
            if (months < 0)
            {
                throw new ArgumentException("К-сть місяців не може бути від'ємною!",nameof(months));
            }
            return Math.Round(Balance * InterestRate / 100 * months / 12, 2);
        }
    }
}
