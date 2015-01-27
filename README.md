# Challenge08
Credit Card Validator

This weeks challenge should be quite simple and goes along with the Amazon thing.  All you have to do is write a credit card number validator.

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
