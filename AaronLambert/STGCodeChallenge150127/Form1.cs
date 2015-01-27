using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace STGCodeChallenge150127
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            DoTheWork();
        }

        private void DoTheWork()
        {
            string CCNbr = txtInput.Text.Trim();
            ValidateCCNbr(CCNbr);
        }

        private void ValidateCCNbr(string CCNbr)
        {
            string OrigCCNbr = CCNbr;
            string CardType = "";
            int CCLen;

            if (string.IsNullOrWhiteSpace(CCNbr))
            {
                InvalidCCNbr(OrigCCNbr);
                return;
            }

            // Remove any dashes and spaces
            CCNbr = CCNbr.Replace("-", "");
            CCNbr = CCNbr.Replace(" ", "");
            CCLen = CCNbr.Length;

            // Determine Card Type and validate length
            if (CCNbr.Substring(0, 1) == "4")
            {
                CardType = "Visa";
                if (CCLen != 13 && CCLen != 16)
                {
                    InvalidCCNbr(OrigCCNbr, CardType);
                    return;
                }
            }
            else
            {
                int prefix = 0;
                if (int.TryParse(CCNbr.Substring(0, 2), out prefix))
                {
                    if (prefix >= 51 && prefix <= 55)
                    {
                        CardType = "Master Card";
                        if (CCLen != 16)
                        {
                            InvalidCCNbr(OrigCCNbr, CardType);
                            return;
                        }
                    }
                    else if (prefix == 37)
                    {
                        CardType = "American Express";
                        if (CCLen != 15)
                        {
                            InvalidCCNbr(OrigCCNbr, CardType);
                            return;
                        }
                    }
                    else
                    {
                        InvalidCCNbr(OrigCCNbr);
                        return;
                    }
                }
                else
                {
                    InvalidCCNbr(OrigCCNbr);
                    return;
                }

            }

            // Validate the CC Nbr using the mathematical checksum
            int checksum = 0;
            if (!CalculateChecksum(CCNbr, out checksum))
            {
                InvalidCCNbr(OrigCCNbr, CardType);
                return;
            }

            // The calculated value must be evenly divisible by 10
            ShowResults(OrigCCNbr, CardType, (checksum % 10) == 0);
        }

        private void InvalidCCNbr(string CCNbr, string CardType = "")
        {
            ShowResults(CCNbr, CardType, false);
        }

        private void ValidCCNbr(string CCNbr, string CardType)
        {
            ShowResults(CCNbr, CardType, true);
        }

        private void ShowResults(string CCNbr, string CardType, bool Valid)
        {
            MessageBox.Show(string.Format("'{0}' is {1}a valid {2}credit card number.", CCNbr, (Valid ? "" : "NOT "), (string.IsNullOrEmpty(CardType) ? "" : CardType + " ")));
        }

        private bool CalculateChecksum(string CCNbr, out int checksum)
        {
            // Validate the CC Nbr using the mathematical checksum
            checksum = 0;

            // Must be a valid number - make sure each char is a digit
            if (!CCNbr.All(Char.IsDigit))
                return false;

            // Reverse the CC Nbr so the checksum is easier to calculate
            string RevCCNbr = string.Concat(Enumerable.Reverse(CCNbr));

            for (int i = 0; i < RevCCNbr.Length; i++)
            {
                string c = RevCCNbr.Substring(i, 1);
                int d = 0;
                if (!int.TryParse(c, out d))
                    return false;
                
                // We double the value of the alternate digits starting with the second one
                if (i % 2 == 1)
                {
                    d *= 2;

                    // If the value is now >= 10, add the digits separately
                    if (d >= 10)
                        d -= 9;
                }

                checksum += d;
            }
            return true;
        }
    }
}
