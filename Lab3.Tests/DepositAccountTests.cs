using System.Security.AccessControl;

namespace Lab3.Tests
{
    [TestClass]
    public class DepositAccountTests
    {
        [TestMethod]
        public void Constructor_ValidData()
        {
            double balance = 100.0;
            string currence = "uah";
            double rate = 12.0;

            var account = new DepositAccount(balance, currence, rate);
            Assert.AreEqual(balance, account.Balance, "Баланс встановлено некоректно");
            Assert.AreEqual("UAH", account.Currency, "Валюта має бути у верх. регістрі");
            Assert.AreEqual(rate, account.InterestRate, "Ставку встановлено некоректно");
            Assert.IsTrue((DateTime.Now - account.OpenDate).TotalSeconds < 5, "Дата відкриття має бути поточною датою");
        }

        [TestMethod]
        public void Constructor_WithNegativeBalance()
        {
            Assert.ThrowsException<ArgumentException>(() => new DepositAccount(-5.0, "UAH", 10));
        }

        [TestMethod]
        public void Constructor_WithNegativeInterestRate()
        {
            Assert.ThrowsException<ArgumentException>(() => new DepositAccount(5.0, "UAH", -10));
        }

        [TestMethod]
        public void Constructor_WithEmptyValue()
        {
            Assert.ThrowsException<ArgumentException>(() => new DepositAccount(5.0, "  ", 10));
        }

        [TestMethod]
        public void Deposit_IncreaseBalance()
        {
            var account = new DepositAccount(100.0, "UAH", 10);
            account.Deposit(100.0);
            Assert.AreEqual(200.0, account.Balance, "Баланс після поповнення некоректний");
        }

        [TestMethod]
        public void Deposit_WithZeroAmount()
        {
            var account = new DepositAccount(100.0, "UAH", 10.0);
            Assert.ThrowsException<ArgumentException>(()=> account.Deposit(0));
        }

        [TestMethod]
        public void WithDraw_DecreaseBalance()
        {
            var account = new DepositAccount(100.0, "UAH", 10.0);
            account.Withdraw(30.0);
            Assert.AreEqual(70.0, account.Balance, "Баланс після зняття некоректний");
        }

        [TestMethod]
        public void WithDraw_MoreThanBalance()
        {
            var account = new DepositAccount(100.0, "UAH", 10.0);
            Assert.ThrowsException<InvalidOperationException>(() => account.Withdraw(110.0));
        }

        [TestMethod]
        public void Withdraw_ExactBalance()
        {
            var account = new DepositAccount(100.0, "UAH", 10.0);
            account.Withdraw(100.0);
            Assert.AreEqual(0.0, account.Balance, "Після зняття всієї суми баланс має дорівнювати нулю");
        }

        [TestMethod]
        public void CalculateInterest_Correct()
        {
            var account = new DepositAccount(100.0, "UAH", 10.0);
            //100*0,1*6/12
            double expected = 5.0;
            double actual = account.CalculateInterest(6);
            Assert.AreEqual(expected, actual, "Відсотки розраховано некоректно");
        }

        [TestMethod]
        public void CalculateInterest_NegativeMonths()
        {
            var account = new DepositAccount(100.0, "UAH", 10.0);
            Assert.ThrowsException<ArgumentException>(()=>account.CalculateInterest(-3));
        }

        [TestMethod]
        public void Deposit_ReturnCorrectString()
        {
            var account = new DepositAccount(100.0, "UAH", 10.0);
            double amount = 40.0;
            string protocol = account.Deposit(amount);

            StringAssert.Contains(protocol, $"{amount:N2}");
            StringAssert.Contains(protocol, "UAH");
            StringAssert.Contains(protocol, $"{140.0:N2}");
        }
    }
}