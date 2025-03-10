using System.Diagnostics.Metrics;

public class PaymentService
{
    private static void WriteCentered(string text)
    {
        int windowWidth = Console.WindowWidth;
        int textLength = text.Length;
        int spaces = (windowWidth - textLength) / 2;
        Console.WriteLine(new string(' ', spaces) + text);
    }
    private static string ReadCentered(string prompt)
    {
        int windowWidth = Console.WindowWidth;
        int textLength = prompt.Length;
        int spaces = (windowWidth - textLength) / 2;
        Console.Write(new string(' ', spaces) + prompt);
        return Console.ReadLine();
    }
    public static bool PaymentGateway(Order o)
    {
        if (o.PaymentMethod == PaymentMode.Card)
        {
        //Console.Write("");
        card:
            long cardno = long.Parse(ReadCentered("Enter card number: "));
            if (cardno == 0)
            {
                return false;
            }
            if (cardno.ToString().Length != 16)
            {
                WriteCentered("Invalid card number!");
                goto card;
            }

        //Console.Write();
        carddate:
            string? expiry = ReadCentered("Enter card expiry MM/YYYY : ");
            if (expiry == "0")
            {
                return false;
            }
            if (!DateTime.TryParseExact(expiry, "MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime parsedTime))
            {
                WriteCentered("Invalid expiry date!");
                goto carddate;
            }

        cvv:
            //Console.Write();
            string cvv = ReadCentered("Enter CVV : ");
            if (cvv == "0")
            {
                return false;
            }
            if (cvv.Length != 3)
            {
                WriteCentered("Invalid CVV!");
                goto cvv;
            }

            string opt = ReadCentered($"Confirm payment of {o.TotalAmount} ? (y/n) : ");
            if (opt == "y")
            {
                WriteCentered("Payment successful!");
                return true;
            }
            else if (opt == "n")
            {
                WriteCentered("Payment failed!");
                return false;
            }

        }
        else if (o.PaymentMethod == PaymentMode.UPI)
        {
        start:
            WriteCentered("Choose Payment method:");
            WriteCentered("1. Google Pay");
            WriteCentered("2. PayTm");
            WriteCentered("3. PhonePe");
            //Console.Write();
            string? choice = ReadCentered("Enter your choice:");
            UPI paymentmethod = UPI.GooglePay;
            if(string.IsNullOrEmpty(choice))
            {
                WriteCentered("Invalid choice!");
                goto start;
            }
            if (choice == "0")
            {
                return false;
            }
            if (choice == "1")
            {
                paymentmethod = UPI.GooglePay;//"Google Pay";
            }
            else if (choice == "2")
            {
                paymentmethod = UPI.PayTM;// "PayTm";
            }
            else if (choice == "3")
            {
                paymentmethod = UPI.PhonePe;// "PhonePe";
            }


            User? u = UserService.users.FirstOrDefault(u => u.UserId == o.UserId);
            if (u != null)
            { }
            int tries = 3;
            WriteCentered($"Redirecting to {paymentmethod.ToString()}....");
            string opt = ReadCentered($"Total Price : {o.TotalAmount}\tProceed to pay? (y/n) : ");
            if (opt == "0")
            {
                Console.Clear();
                goto start;
            }
            if (opt == "y")
            {
                pin:
                if (paymentmethod == UPI.GooglePay)
                {
                    WriteCentered($"Your UPI Id : {u.Upiid = u.Username + "@okaxis"}");
                }
                else if (paymentmethod == UPI.PayTM)
                {
                    WriteCentered($"Your UPI Id : {u.Upiid = u.Username + "@paytm"}");
                }
                else if (paymentmethod == UPI.PhonePe)
                {
                    WriteCentered($"Your UPI Id : {u.Upiid = u.Username + "@ybl"}");
                }


                string password = ReadCentered($"Enter your {paymentmethod} upi PIN : ");
                if (password == "0")
                {
                    Console.Clear();
                    goto start;
                }
                if(string.IsNullOrEmpty(password))
                {
                    WriteCentered("Invalid pin!");
                    goto pin;
                }
                if (u.Upipin[paymentmethod].Contains(password))
                {
                    tries--;
                    if (tries == 0)
                    {
                        WriteCentered("Payment failed!");
                        return false;
                    }
                    WriteCentered("Wrong upi pin! Enter correct pin.");
                    goto pin;
                }
                WriteCentered("Payment successful!");
                o.UPIMethod = paymentmethod;
                return true;
            }
            else if (opt == "n")
            {
                WriteCentered("Payment cancelled!");
                return false;
            }



        }
        return false;


    }
        //return false;
    
}