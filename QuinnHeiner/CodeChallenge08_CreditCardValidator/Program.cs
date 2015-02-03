using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

/*Code Challenge #8
This weeks challenge should be quite simple and goes along with the Amazon thing.  All you have to do is write a credit card number validation routine..
 
Credit Card Number Validation Criteria:
The following table identifies the validation rules that must be applied.
The program considers only the below to be valid credit cards.

 
Card Type
Card Number Prefix
Number of Digits (Length)
Checksum* Digit
VA
4
13 or 16
10
MC
51,52,53,54 or 55
16
10
AX
37
15
10
VA - Visa 
MC - Master Card 
AX - American Express

The checksum is performed on the credit card number (including the prefix)

*Checksum Explained
Double the value of alternate digits beginning with the second last digit from the right
Each doubled value becomes individual digits (16 becomes a 1 and a 6)
Add the individual digits comprising the products in step 1 to each of the values of the other digits
Divide the total by the checksum digit
If the checksum digit divides evenly into the total, the card number is valid
Examples (Visa card number):
4512113014843252
8+5+2+2+2+1+6+0+2+4+1+6+4+6+2+1+0+2
Sum=54
54/10 leaves a remainder. Thus the card number is invalid.
4512113014643252
8+5+2+2+2+1+6+0+2+4+1+2+4+6+2+1+0+2
Sum=50
50/10 leaves no remainder. Thus the card number is valid.
Recommendation for implementation:
Validate the card number prefix specific to the card type
Validate the length of the credit card number based upon the card type
perform the checksum digit to finalize the validation process
Notes:
The output to the screen must indicate whether or not the credit card that was entered is valid or not (echoing the card type and the card number). 
 
Spaces may be entered pre or post the card type and/or the card number.
 
The challenge is to validate a number that is passed in as well as return the type of card that the number applies to.
 */

namespace CodeChallenge08_CreditCardValidator
{
    class Program
    {
        public static void Main(string[] args)
        {
            string input;
            var creditCards = GetInheritedTypes<CreditCard>();
            do
            {
                Console.WriteLine("\n\nEnter the type of credit card.  Valid values are VA (Visa), MC (Master Card), or AX (American Express) (q to quit): ");
                input = Console.ReadLine().Trim().ToUpper();
                if (creditCards.ContainsKey(input))
                {
                    var card = creditCards[input];
                    Console.WriteLine("Enter the credit card number to validate (non-numeric characters will be ignored): ");
                    card.CardNumber = Console.ReadLine();
                    Console.WriteLine("Validation result for {0} #{1} : {2}", card.CardName, card.CardNumberDigits, card.isValid());
                }
                else
                {
                    Console.WriteLine("Invalid credit card type.  Valid values are VA, MC, or AX");
                }
            } while (input != "Q");
        }

        public static Dictionary<string, T> GetInheritedTypes<T>(params object[] constructorArgs) where T : class
        {
            var objects = new Dictionary<string, T>();
            foreach (Type type in
                Assembly.GetAssembly(typeof(T)).GetTypes()
                .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(T))))
            {
                T obj = (T)Activator.CreateInstance(type, constructorArgs);
                objects.Add(obj.GetType().GetProperty("CardType").GetValue(obj, null).ToString(), obj);
            }
            return objects;
        }

        public abstract class CreditCard
        {
            public string CardType { get; protected set; }
            public string CardName { get; protected set; }
            public string[] CardNumberPrefixes { get; protected set; }
            public int[] NumberOfDigits { get; protected set; }
            public int ChecksumDigit { get; protected set; }
            public string CardNumber { get; set; } // can include non-numeric characters
            public string CardNumberDigits // read-only field that extracts only digits from CardNumber property
            { 
                get 
                {
                    var cardNumberDigitsOnly = Regex.Replace(this.CardNumber, @"[^0-9]+", "");
                    return cardNumberDigitsOnly; 
                }
            }
            public virtual bool isValid()
            {
                return isValidCardPrefix() && isValidCardLength() && isValidChecksum();
            }

            private bool isValidCardPrefix()
            {
                foreach(var prefix in this.CardNumberPrefixes)
                {
                    if (this.CardNumberDigits.StartsWith(prefix))
                    {
                        return true;
                    }
                }
                return false;
            }

            private bool isValidCardLength()
            {
                foreach (var length in this.NumberOfDigits)
                {
                    if (this.CardNumberDigits.Length == length)
                    {
                        return true;
                    }
                }
                return false;
            }

            private bool isValidChecksum()
            {
                var sum = 0;
                for (var i = 0; i < CardNumberDigits.Length; i++)
                {
                    int digitNum;
                    var digit = CardNumberDigits[i];
                    if (!Int32.TryParse(digit.ToString(), out digitNum))
                    {
                        return false;
                    }
                    var digitValue = i % 2 == 0 ? digitNum * 2 : digitNum;
                    if (digitValue > 9)
                    {
                        var firstDigit = Convert.ToInt32(digitValue.ToString()[0].ToString());
                        var secondDigit = Convert.ToInt32(digitValue.ToString()[1].ToString());
                        digitValue = firstDigit + secondDigit;
                    }
                    sum += digitValue;
                }
                return sum > 0 &&  sum % this.ChecksumDigit == 0;
            }
        }

        public class Visa : CreditCard
        {
            public Visa()
            {
                this.CardType = "VA";
                this.CardName = "Visa";
                this.CardNumberPrefixes = new string[] { "4" };
                this.NumberOfDigits = new int[] { 13, 16 };
                this.ChecksumDigit = 10;
           }
        }

        public class MasterCard : CreditCard
        {
            public MasterCard()
            {
                this.CardType = "MC";
                this.CardName = "Master Card";
                this.CardNumberPrefixes = new string[] { "51", "52", "53", "54", "55" };
                this.NumberOfDigits = new int[] { 16 };
                this.ChecksumDigit = 10;
            }
        }

        public class AmericanExpress : CreditCard
        {
            public AmericanExpress()
            {
                this.CardType = "AX";
                this.CardName = "American Express";
                this.CardNumberPrefixes = new string[] { "37" };
                this.NumberOfDigits = new int[] { 15 };
                this.ChecksumDigit = 10;
            }
        }
    }
}
