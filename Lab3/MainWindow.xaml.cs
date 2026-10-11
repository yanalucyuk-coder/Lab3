using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Lab3
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            AccountTypeComboBox.SelectedIndex = 0;
            SetInitialUiState();
        }

        private void AccountTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Змінюється коли користувач міняє вибір
            SetInitialUiState();
        }

        private void SetInitialUiState()
        {
            if(AccountTypeComboBox.SelectedIndex == 0)
            {
                DepositFields.Visibility = Visibility.Visible;
                InterestRateTextBox.Visibility = Visibility.Visible;

                CreditFields.Visibility = Visibility.Collapsed;
                CreditLimitTextBox.Visibility = Visibility.Collapsed;

                SavingFields.Visibility = Visibility.Collapsed;
                MinBalanceTextBox.Visibility = Visibility.Collapsed;
            }
            else if(AccountTypeComboBox.SelectedIndex == 1) 
            {
                DepositFields.Visibility = Visibility.Collapsed;
                InterestRateTextBox.Visibility = Visibility.Collapsed;

                CreditFields.Visibility = Visibility.Visible;
                CreditLimitTextBox.Visibility = Visibility.Visible;

                SavingFields.Visibility = Visibility.Collapsed;
                MinBalanceTextBox.Visibility = Visibility.Collapsed;
            }
            else
            {
                DepositFields.Visibility = Visibility.Collapsed;

                CreditFields.Visibility = Visibility.Collapsed;
                CreditLimitTextBox.Visibility = Visibility.Collapsed;

                SavingFields.Visibility = Visibility.Visible;
                InterestRateTextBox.Visibility = Visibility.Visible;
                MinBalanceTextBox.Visibility = Visibility.Visible;
            }
        }

        //всі рахунки
        private List<FinancialAccount> _accounts = new List<FinancialAccount>();

        //загальні обрахунки
        private AccountCalculator _calculator = new AccountCalculator();

        private void CreateAccountButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if(!double.TryParse(BalanceTextBox.Text, out var balance))
                {
                    MessageBox.Show("Будь ласка, введіть коректний баланс.", "Помилка введення", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                string currency = CurrencyTextBox.Text;
                FinancialAccount account;

                if (AccountTypeComboBox.SelectedIndex == 0)
                {
                    if (!double.TryParse(InterestRateTextBox.Text, out double rate))
                    {
                        MessageBox.Show("Будь ласка, введіть коректну відсоткову ставку.", "Помилка введення", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    account = new DepositAccount(balance, currency, rate);
                }
                else if (AccountTypeComboBox.SelectedIndex == 1)
                {
                    if (!double.TryParse(CreditLimitTextBox.Text, out double limit))
                    {
                        MessageBox.Show("Будь ласка, введіть коректний кредитний ліміт.", "Помилка введення", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    account = new CreditAccount(balance, currency, limit);
                }
                else 
                {
                    if (!double.TryParse(InterestRateTextBox.Text, out double rate))
                    {
                        MessageBox.Show("Будь ласка, введіть коректну відсоткову ставку.", "Помилка введення", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    if (!double.TryParse(MinBalanceTextBox.Text, out double minBalance))
                    {
                        MessageBox.Show("Будь ласка, введіть коректний мінімальний залишок.", "Помилка введення", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    account = new SavingAccount(balance, currency, rate, minBalance);
                }

                _accounts.Add(account);
                AccountsListBox.Items.Add(GetAccountInfo(account));
                UpdateAnalytics();
            }

            catch(ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Помилка валідації", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (FormatException ex)
            {
                MessageBox.Show(ex.Message, "Помилка формату", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Сталася неочікувана помилка: " + ex.Message, "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DepositButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                FinancialAccount? account = GetSelectedAccount();
                if (account == null) return;

                if (!double.TryParse(AmountTextBox.Text, out double amount))
                {
                    MessageBox.Show("Будь ласка, введіть коректну суму.", "Помилка введення", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                string protocol = account.Deposit(amount);
                RefreshSelectedItem(account);
                UpdateAnalytics();
                MessageBox.Show(protocol, "Операцію виконано", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Помилка валідації", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "Помилка операції", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Сталася неочікувана помилка: " + ex.Message, "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void WithdrawButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                FinancialAccount? account = GetSelectedAccount();
                if (account == null) return;

                if (!double.TryParse(AmountTextBox.Text, out double amount))
                {
                    MessageBox.Show("Будь ласка, введіть коректну суму.", "Помилка введення", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                string protocol = account.Withdraw(amount);
                RefreshSelectedItem(account);
                UpdateAnalytics();
                MessageBox.Show(protocol, "Операцію виконано", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Помилка валідації", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "Помилка операції", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Сталася неочікувана помилка: " + ex.Message, "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CalculateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                FinancialAccount? account = GetSelectedAccount();
                if (account == null) return;

                if (account is not IInterestBearing interestAccount)
                {
                    MessageBox.Show("Розрахунок відсотків недоступний для цього типу рахунку.", "Неможливо виконати", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!int.TryParse(MonthsTextBox.Text, out int months) || months <= 0)
                {
                    MessageBox.Show("Введіть кількість місяців: ціле число більше нуля.","Помилка введення", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                double interest = interestAccount.CalculateInterest(months);
                MessageBox.Show($"Відсотки за {months} міс.: {interest:N2} {account.Currency}","Розрахунок відсотків", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Помилка валідації", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Сталася неочікувана помилка: " + ex.Message, "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RepaymentButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                FinancialAccount? account = GetSelectedAccount();
                if (account == null) return;

                if (account is not IRepayable repayable)
                {
                    MessageBox.Show("Сума погашення розраховується лише для кредитного рахунку.", "Неможливо виконати", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }


                double repayment = repayable.CalculateRepayment();
                if (repayment == 0)
                {
                    MessageBox.Show("Боргу немає, погашення не потрібне.","Розрахунок погашення", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                MessageBox.Show($"Сума погашення: {repayment:N2} {account.Currency}","Розрахунок погашення", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Помилка валідації", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Сталася неочікувана помилка: " + ex.Message, "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private FinancialAccount? GetSelectedAccount()
        {
            if (AccountsListBox.SelectedIndex < 0)
            {
                MessageBox.Show("Спочатку оберіть рахунок у списку.", "Помилка вибору", MessageBoxButton.OK, MessageBoxImage.Warning);
                return null;
            }
            return _accounts[AccountsListBox.SelectedIndex];
        }

        private string GetAccountInfo(FinancialAccount account)
        {
            string type;
            string extra;
            if (account is DepositAccount deposit)
            {
                type = "Депозитний";
                extra = $"Ставка: {deposit.InterestRate}%";
            }
            else if (account is CreditAccount credit)
            {
                type = "Кредитний";
                extra = $"Ліміт: {credit.CreditLimit:N2}";
            }
            else if (account is SavingAccount saving)
            {
                type = "Накопичувальний";
                extra = $"Ставка: {saving.InterestRate}% | Мін. залишок: {saving.MinBalance:N2}";
            }
            else
            {
                type = "Рахунок";
                extra = "";
            }
            return $"{type} | Баланс: {account.Balance:N2} {account.Currency} | {extra} | Відкрито: {account.OpenDate:dd.MM.yyyy HH:mm}";
        }

        private void RefreshSelectedItem(FinancialAccount account)
        {
            int index = AccountsListBox.SelectedIndex;
            AccountsListBox.Items[index] = GetAccountInfo(account);
            AccountsListBox.SelectedIndex = index;
        }

        private void UpdateAnalytics()
        {
            if (_accounts.Count == 0)
            {
                MaxBalanceTextBlock.Text = "0.00";
                TotalInterestTextBlock.Text = "0.00";
                TotalDebtTextBlock.Text = "0.00";
                return;
            }

            FinancialAccount best = _calculator.GetAccountWithMaxBalance(_accounts);
            MaxBalanceTextBlock.Text = $"{best.Balance:N2} {best.Currency} ({best.OpenDate:dd.MM.yyyy HH:mm})";

            // Збираємо рахунки за інтерфейсами
            var interestAccounts = new List<IInterestBearing>();
            var repayableAccounts = new List<IRepayable>();
            for (int i = 0; i < _accounts.Count; i++)
            {
                if (_accounts[i] is IInterestBearing interest)
                {
                    interestAccounts.Add(interest);
                }
                if (_accounts[i] is IRepayable repayable)
                {
                    repayableAccounts.Add(repayable);
                }
            }

            TotalInterestTextBlock.Text = $"{_calculator.GetTotalInterest(interestAccounts, 12):N2}";
            TotalDebtTextBlock.Text = $"{_calculator.GetTotalDebt(repayableAccounts):N2}";
        }
    }
}