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

    }
}
