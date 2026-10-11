using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3
{
    public class AccountCalculator
    {
        public FinancialAccount GetAccountWithMaxBalance(IList<FinancialAccount> accounts)
        {
            if (accounts == null)
            {
                throw new ArgumentNullException(nameof(accounts), "Список рахунків не передано!");
            }

            if (accounts.Count == 0)
            {
                throw new InvalidOperationException("Список рахунків порожній!");
            }

            FinancialAccount best = accounts[0];

            for (int i = 0; i < accounts.Count; i++)
            {
                if (accounts[i].Balance > best.Balance)
                {
                    best = accounts[i];
                }
            }
            return best;
        }

        public double GetMaxBalance(IList<FinancialAccount> accounts)
        {
            return GetAccountWithMaxBalance(accounts).Balance;
        }

        public double GetTotalInterest(IEnumerable<IInterestBearing> accounts, int months)
        {
            if (accounts == null)
            {
                throw new ArgumentNullException(nameof(accounts), "Список рахунків не передано!");
            }
            double total = 0;
            foreach (IInterestBearing account in accounts)
            {
                total += account.CalculateInterest(months);
            }
            return total;

        }

        public double GetTotalDebt(IEnumerable<IRepayable> accounts)
        {
            if(accounts == null)
            {
                throw new ArgumentNullException(nameof(accounts), "Список рахунків не передано!");
            }
            double total = 0;
            foreach (IRepayable account in accounts)
            {
                total += account.CalculateRepayment();
            }
            return total;
        }

    }
}
