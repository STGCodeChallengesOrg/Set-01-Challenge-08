using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace STGCodeChallenge8
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //txtTextToProcess.Text = "4512113014843252"; //invalid
            txtTextToProcess.Text = "4512113014643252"; //valid visa
        }

        private void btnValidate_Click_1(object sender, RoutedEventArgs e)
        {
            string creditCardType = string.Empty;
            if (validateCreditCardNumber(txtTextToProcess.Text, out creditCardType))
            {
                lblValid.Foreground = Brushes.Green;
                lblValid.Content = "Valid " + creditCardType;
            }
            else
            {
                lblValid.Foreground = Brushes.Red;
                lblValid.Content = "Invalid";
            }
        }

        /// <summary>
        /// Determine whether or not a provided credit card number is valid.
        /// </summary>
        /// <param name="creditCardNumber">The credit card number to validate</param>
        /// <param name="creditCardType">A variable to hold the card type of a valid credit card to return to the user</param>
        /// <returns>A bool true if a valid credit card number and false if it is invalid.  If valid also sets the value of the credit card type to display to the user.</returns>
        private bool validateCreditCardNumber(string creditCardNumber, out string creditCardType)
        {
            creditCardNumber = Regex.Replace(creditCardNumber, @"\s", ""); //remove whitespace
            if (hasValidPrefixAndLength(creditCardNumber, out creditCardType))
            {
                return checkSum(creditCardNumber, creditCardType);
            }
            return false;
        }

        /// <summary>
        /// Determine the credit card type based on the prefix. Validate the card prefix and length.
        /// </summary>
        /// <param name="creditCardNumber">The Credit card number to validate</param>
        /// <param name="creditCardType">A variable to hold the card type of a valid credit card to return to the user</param>
        /// <returns>A bool true if the previx and length are valid; otherwise, returns false.  Sets the card type if the number is valid</returns>
        private bool hasValidPrefixAndLength(string creditCardNumber, out string creditCardType)
        {
            string validVisaPattern = @"4[0-9]{12}|4[0-9]{15}";
            string validMasterCardPattern = @"5[1-5][0-9]{14}";
            string validAmericanExpressPattern = @"37[0-9]{13}";
            if (Regex.Match(creditCardNumber, validVisaPattern).Success)
            {
                creditCardType = "Visa";
                return true;
            }
            else if (Regex.Match(creditCardNumber, validMasterCardPattern).Success)
            {
                creditCardType = "Master Card";
                return true;
            }
            else if (Regex.Match(creditCardNumber, validAmericanExpressPattern).Success)
            {
                creditCardType = "American Express";
                return true;
            }
            creditCardType = "??";
            return false;
        }

        /// <summary>
        /// Last step of validation.  Determine if the credit card number passes the check sum test.
        /// </summary>
        /// <param name="creditCardNumber">Credit Card number to validate</param>
        /// <param name="creditCardType">Type of credit card</param>
        /// <returns>True if the credit card number is valid; otherwise, returns false</returns>
        private bool checkSum(string creditCardNumber, string creditCardType)
        {
            int sum = 0;
            int secondToLastIndex = creditCardNumber.Length - 2;
            char[] digitsInCreditCardNumber = creditCardNumber.ToCharArray();
            for (int index = 0; index < creditCardNumber.Length; index++)
            {
                if (index <= secondToLastIndex && ((secondToLastIndex - index) % 2 == 0))
                {
                    int doubledValue = int.Parse(digitsInCreditCardNumber[index].ToString()) * 2;
                    if (doubledValue > 9)
                    {
                        foreach (string digit in Regex.Split("" + doubledValue, @"(\d)"))
                        {
                            if (Regex.Match(digit, @"\d").Success)
                            {
                                sum += int.Parse(digit);
                            }
                        }
                    }
                    else
                    {
                        sum += doubledValue;
                    }
                }
                else
                {
                    sum += int.Parse(digitsInCreditCardNumber[index].ToString());
                }
            }
            return sum % 10 == 0;
        }
    }
}
