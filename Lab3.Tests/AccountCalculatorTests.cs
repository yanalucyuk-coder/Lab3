using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3.Tests
{
    [TestClass]
    public class AccountCalculatorTests
    {
        [TestMethod]
        public void GetMaxBalance_MixedAccounts_Number()
        {
            var calculator = new AccountCalculator();
            var accounts = new List<FinancialAccount>
            {
                new DepositAccount(1000.0,"UAH",10.0),
                new DepositAccount(3400.0,"UAH",2.0),
                new CreditAccount(-100.0,"UAH",500.0),
                new CreditAccount(1000.0,"UAH",1440.0),
            };

            double actual = calculator.GetMaxBalance(accounts);
            Assert.AreEqual(3400.0, actual, "Макс. баланс визначено некоректно");
        }

        [TestMethod]
        public void GetAccountWithMaxBalance()
        {
            var calculator = new AccountCalculator();
            var account1 = new DepositAccount(900.0, "UAH", 4.0);
            var accounts = new List<FinancialAccount>
            {
                account1,
                new CreditAccount (0.0, "UAH", 100.0),
                new DepositAccount(100.0,"UAH", 5.0)
            };

            var actual = calculator.GetAccountWithMaxBalance(accounts);
            Assert.AreSame(account1, actual, "Має повернутися рахунок, де найбільше баланс");
        }

        [TestMethod]
        public void GetMaxBalance_AllNegative_Number()
        {
            var calculator = new AccountCalculator();
            var accounts = new List<FinancialAccount>
            {
                new CreditAccount(-500.0,"UAH",1500.0),
                new CreditAccount(-100.0,"UAH",1500.0),
                new CreditAccount(-700.0,"UAH",1440.0),
            };

            double actual = calculator.GetMaxBalance(accounts);
            Assert.AreEqual(-100.0, actual, "Серед боргів максимум це найменший борг");
        }

        [TestMethod]
        public void GetMaxBalance_SingleAccount_Number()
        {
            var calculator = new AccountCalculator();
            var accounts = new List<FinancialAccount>
            {
                new DepositAccount(450.0,"UAH",15.0)
            };

            double actual = calculator.GetMaxBalance(accounts);
            Assert.AreEqual(450.0, actual, "Для одного рахунку максимум = балансу");
        }

        [TestMethod]
        public void GetMaxBalance_MaxIsFirst()
        {
            var calculator = new AccountCalculator();
            var accounts = new List<FinancialAccount>
            {
                new DepositAccount(1200.0,"UAH",1500.0),
                new DepositAccount(100.0,"UAH",1500.0),
                new DepositAccount(600.0,"UAH",1440.0),
            };

            double actual = calculator.GetMaxBalance(accounts);
            Assert.AreEqual(1200.0, actual, "Макс. має знаходитися на першому місці");
        }

        [TestMethod]
        public void GetMaxBalance_MaxIsLast()
        {
            var calculator = new AccountCalculator();
            var accounts = new List<FinancialAccount>
            {
                new DepositAccount(1200.0,"UAH",1500.0),
                new DepositAccount(100.0,"UAH",1500.0),
                new DepositAccount(1400.0,"UAH",1440.0)
            };

            double actual = calculator.GetMaxBalance(accounts);
            Assert.AreEqual(1400.0, actual, "Макс. має знаходитися на останньому місці");
        }

        [TestMethod]
        public void GetAccountMaxBalance_EqualBalancest()
        {
            var calculator = new AccountCalculator();
            var first = new DepositAccount(500.0, "UAH", 5.0);
            var second = new CreditAccount(500.0, "UAH", 100.0);
            var accounts = new List<FinancialAccount> { first,second };
            var actual = calculator.GetAccountWithMaxBalance(accounts);
            Assert.AreSame(first, actual, "При однакових балансах повертається перший");
        }

        [TestMethod]
        public void GetMaxBalance_EmptyList()
        {
            var calculator = new AccountCalculator();
            var accounts = new List<FinancialAccount>();

            Assert.ThrowsException<InvalidOperationException>(() => calculator.GetMaxBalance(accounts));
        }

        [TestMethod]
        public void GetMaxBalance_Null()
        {
            var calculator = new AccountCalculator();

            Assert.ThrowsException<ArgumentNullException>(() => calculator.GetMaxBalance(null!));
        }

        //тести лаби 4
        [TestMethod]
        public void GetTotalInterest_MixedInterestAccounts()
        {
            var calculator = new AccountCalculator();
            var accounts = new List<IInterestBearing>
            {
                new DepositAccount(100.0, "UAH", 10.0), //100*0.1*6/12 = 5
                new SavingAccount(200.0, "UAH", 5.0, 100.0) //200*0.05*6/12 = 5
            };
            Assert.AreEqual(10.0, calculator.GetTotalInterest(accounts, 6));
        }

        [TestMethod]
        public void GetTotalInterest_EmptyList()
        {
            var calculator = new AccountCalculator();
            Assert.AreEqual(0.0, calculator.GetTotalInterest(new List<IInterestBearing>(), 6), "Список є пустим!");
        }

        [TestMethod]
        public void GetTotalInterest_Null()
        {
            var calculator = new AccountCalculator();
            Assert.ThrowsException<ArgumentNullException>(() => calculator.GetTotalInterest(null!,6));
        }

        [TestMethod]
        public void GetTotalDebt_OnlyNegativeBalancesCount()
        {
            var calculator = new AccountCalculator();
            var accounts = new List<IRepayable>
            {
                new CreditAccount(-100.0, "UAH", 400.0),
                new CreditAccount(-400.0, "UAH", 900.0),
                new CreditAccount(300.0, "UAH", 700.0)
            };
            Assert.AreEqual(500.0, calculator.GetTotalDebt(accounts));
        }

        [TestMethod]
        public void GetTotalDebt_Null()
        {
            var calculator = new AccountCalculator();
            Assert.ThrowsException<ArgumentNullException>(() => calculator.GetTotalDebt(null!));
        }

        [TestMethod]
        public void AllAccounts_ITransactional()
        {
            var accounts = new List<ITransactional>
            {
                new DepositAccount(100.0, "UAH", 400.0),
                new CreditAccount(300.0, "UAH", 500.0),
                new SavingAccount(100.0, "UAH", 10.0, 50.0)
            };

            foreach (var account in accounts)
            {
                string protocol = account.Deposit(10.0);
                StringAssert.Contains(protocol, "UAH");
            }
        }
    }
}
