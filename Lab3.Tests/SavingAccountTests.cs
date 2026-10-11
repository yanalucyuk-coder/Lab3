using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3.Tests
{
    [TestClass]
    public class SavingAccountTests
    {
        [TestMethod]
        public void Constructor_ValidData()
        {
            double balance = 100.0;
            string currency = "uah";
            double rate = 7.0;
            double minbalance = 20.0;
            var account = new SavingAccount(balance, currency, rate, minbalance);
            Assert.AreEqual(balance, account.Balance, "Баланс встановлено некоректно");
            Assert.AreEqual("UAH", account.Currency, "Валюта має бути у верх. регістрі");
            Assert.AreEqual(rate, account.InterestRate, "Ставку встановлено некоректно");
            Assert.AreEqual(minbalance, account.MinBalance, "Мін. залишок встановлено некоректно");
        }

        [TestMethod]
        public void Constructor_NegativeBalance()
        {
            Assert.ThrowsException<ArgumentException>(() => new SavingAccount(100.0, "UAH", 10.0, 300.0));
        }

        [TestMethod]
        public void Constructor_NegativeRate()
        {
            Assert.ThrowsException<ArgumentException>(() => new SavingAccount(600.0, "UAH", -10.0, 300.0));
        }

        [TestMethod]
        public void Constructor_EmptyCurrency()
        {
            Assert.ThrowsException<ArgumentException>(() => new SavingAccount(600.0, " ", 10.0, 300.0));
        }

        [TestMethod]
        public void Constructor_NegativeMinBalance()
        {
            Assert.ThrowsException<ArgumentException>(() => new SavingAccount(600.0, " ", 10.0, -300.0));
        }

        [TestMethod]
        public void Withdraw_AboveMinimum()
        {
            var account = new SavingAccount(500.0, "UAH", 10.0, 100.0);
            account.Withdraw(200.0);
            Assert.AreEqual(300.0, account.Balance, "Неправильне зняття при достатній сумі!");
        }

        [TestMethod]
        public void Withdraw_ExactlyMinimum()
        {
            var account = new SavingAccount(500.0, "UAH", 10.0, 100.0);
            account.Withdraw(400.0);
            Assert.AreEqual(100.0, account.Balance, "Неправильне зняття при достатній сумі!");
        }

        [TestMethod]
        public void Withdraw_BelowMinimum()
        {
            var account = new SavingAccount(500.0, "UAH", 10.0, 100.0);
            Assert.ThrowsException<InvalidOperationException>(() => account.Withdraw(500.0));
        }

        [TestMethod]
        public void CalculateInterest_Correct()
        {
            var account = new SavingAccount(500.0, "UAH", 10.0, 0.0);
            //500 * 0.1 * 6/12 = 25
            Assert.AreEqual(25, account.CalculateInterest(6), "Неправильно підрахована відсоткова ставка!");
        }

        [TestMethod]
        public void CalculateInterest_NegativeMonth()
        {
            var account = new SavingAccount(500.0, "UAH", 10.0, 0.0);
            //500 * 0.1 * 6/12 = 25
            Assert.ThrowsException<ArgumentException>(() => account.CalculateInterest(-6));
        }
    }
}
