using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Challenge8 {
    public partial class frmMain : Form {
        public frmMain() {
            InitializeComponent();

            txtCC.Text = "4512113014643252";
        }

        private void cmdValidate_Click(object sender, EventArgs e) {

            lblResult.Text = ValidateCC(txtCC.Text);

        }

        private string ValidateCC(string ccNum) {

            System.Diagnostics.Debug.WriteLine("*************************************");

            string[] validDigits = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9"};

            string rtn = string.Empty;
            string checkCC = string.Empty;
            string prefix = string.Empty;

            foreach (char c in ccNum) {
                if (validDigits.Contains(c.ToString())) {
                    checkCC += c.ToString();
                }
            }

            switch (checkCC.Count()) {
                case 13:
                case 16:

                    switch (checkCC[0]) {
                        case '4':
                            rtn = string.Format("{0} - {1} -> {2}", "VA", checkCC, ParseCCNumber(checkCC));
                            break;
                        case '5':
                            prefix = checkCC.Substring(0, 2);

                            switch (prefix) {
                                case "51":
                                case "52":
                                case "53":
                                case "54":
                                case "55":
                                    rtn = string.Format("{0} - {1} -> {2}", "MC", checkCC, ParseCCNumber(checkCC));
                                    break;
                                default:
                                    rtn = "Invalid card number";
                                    break;
                            }

                            break;
                        default:
                            rtn = "Invalid card number";
                            break;
                    }

                    break;
                case 15:
                    prefix = checkCC.Substring(0, 2);

                    if (prefix == "37") {
                        rtn = string.Format("{0} - {1} -> {2}", "AX", checkCC, ParseCCNumber(checkCC));
                    } else {
                        rtn = "Invalid card number";
                    }

                    break;
                default:
                    rtn = "Invalid card number length";
                    break;
            }

            return rtn;
        }

        private string ParseCCNumber(string checkCC) {

            int ival = 0;
            int val = 0;

            for (int i = checkCC.Count() - 1; i >= 0; i--) {

                System.Diagnostics.Debug.WriteLine(string.Format("{0:00} - {1} = {2}", i, int.Parse(checkCC[i].ToString()), (i % 2 == 0) ? string.Format("{0}**", int.Parse(checkCC[i].ToString()) * 2) : int.Parse(checkCC[i].ToString()).ToString()));

                ival = (i % 2 == 0) ? int.Parse(checkCC[i].ToString()) * 2 : int.Parse(checkCC[i].ToString()) ;

                foreach(char c in ival.ToString()) {
                    val += int.Parse(c.ToString());
                }
            }

            return string.Format("{0} {1}", val,  (val % 10 == 0) ? "Valid" : "Invalid");
        }
    }
}
