using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3.Tests
{
    [TestClass]
    public class CreditAccountTests
    {
        [TestMethod]
        public void Constructor_Valid()
        {
            double balance = 0.0;
            string currency = "uah";
            double limit = 100.0;

            var account = new CreditAccount(balance, currency, limit);
            Assert.AreEqual(balance, account.Balance, "Баланс встановлено некоректно");
            Assert.AreEqual("UAH", account.Currency, "Валюта має бути у верх. регістрі");
            Assert.AreEqual(limit, account.CreditLimit, "Ліміт встановлено некоректно");
            Assert.IsTrue((DateTime.Now - account.OpenDate).TotalSeconds < 5, "Дата відкриття має бути поточною датою");
        }

        [TestMethod]
        public void Constructor_WithNegativeLimit()
        {
            Assert.ThrowsException<ArgumentException>(() => new CreditAccount(0.0, "UAH", -100.0));
        }

        [TestMethod]
        public void Constructor_DebtOverLimit()
        {
            Assert.ThrowsException<ArgumentException>(() => new CreditAccount(-200.0, "UAH", 100.0));
        }

        [TestMethod]
        public void Constructor_EmptyCurrency()
        {
            Assert.ThrowsException<ArgumentException>(() => new CreditAccount(0.0, " ", 100.0));
        }

        [TestMethod]
        public void Deposit_IncreaseBalance()
        {
            var account = new CreditAccount(50.0, "UAH", 100.0);
            account.Deposit(10.0);
            Assert.AreEqual(60.0, account.Balance, "Баланс після поповнення некоректний");
        }

        [TestMethod]
        public void Deposit_WithZeroAMount()
        {
            var account = new CreditAccount(50.0, "UAH", 100.0);
            Assert.ThrowsException<ArgumentException>(() => account.Deposit(0.0));
        }

        [TestMethod]
        public void WithDraw_WithinLimit_BalanceNegative()
        {
            var account = new CreditAccount(0.0, "UAH", 100.0);
            account.Withdraw(50.0);
            Assert.AreEqual(-50.0, account.Balance, "Баланс має піти в борг");
        }

        [TestMethod]
        public void WithDraw_ExactlyLimit()
        {
            var account = new CreditAccount(0.0, "UAH", 100.0);
            account.Withdraw(100.0);
            Assert.AreEqual(-100.0, account.Balance, "Знімати рівно в межах ліміту");
        }

        [TestMethod]
        public void CalculateRepayment_ReturnsDebth()
        {
            var account = new CreditAccount(0.0, "UAH", 100.0);
            account.Withdraw(10.0);
            Assert.AreEqual(10.0, account.CalculateRepayment(), "Сума погашення = боргу");
        }

        [TestMethod]
        public void CalculateRepayment_NoDebt()
        {
            var account = new CreditAccount(10.0, "UAH", 100.0);
            Assert.AreEqual(0.0, account.CalculateRepayment(), "Якщо боргу немає, то погашення = 0");
        }

        [TestMethod]
        public void Deposit_ReducesDebt()
        {
            var account = new CreditAccount(0.0, "UAH", 100.0);
            account.Withdraw(50.0);
            account.Deposit(20.0);
            Assert.AreEqual(30.0, account.CalculateRepayment(), "Після поповнення борг має зменшитися");
        }

        [TestMethod]
        public void Withdraw_ReturnsCorrectString()
        {
            var account = new CreditAccount(0.0, "UAH", 100.0);
            double amount = 40.0;
            string protocol = account.Withdraw(amount);

            StringAssert.Contains(protocol, $"{amount:N2}");
            StringAssert.Contains(protocol, "UAH");
            StringAssert.Contains(protocol, $"{-40.0:N2}");
        }
    }
}
