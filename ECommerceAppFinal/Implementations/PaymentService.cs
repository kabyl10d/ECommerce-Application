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
            if(cardno.ToString().Length != 16)
            {
                WriteCentered("Invalid card number!");
                goto card;
            }

            //Console.Write();
            carddate:
            string? expiry = ReadCentered("Enter card expiry MM/YYYY : ");
            if (!DateTime.TryParseExact(expiry, "MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime parsedTime))
            {
                WriteCentered("Invalid expiry date!");
                goto carddate;
            }

            cvv:
            //Console.Write();
            int cvv = int.Parse(ReadCentered("Enter CVV : "));
            if (cvv.ToString().Length != 3)
            {
                WriteCentered("Invalid CVV!");
                goto cvv;
            }

            Console.Write($"Confirm payment of {o.TotalAmount} ? (y/n) : ");
            if (Console.ReadLine() == "y")
            {
                WriteCentered("Payment successful!");
                return true;
            }
            else
            {
                WriteCentered("Payment failed!");
                return false;
            }

        }
        else if(o.PaymentMethod == PaymentMode.UPI)
        {
            WriteCentered("Choose Payment method:");
            WriteCentered("1. Google Pay");
            WriteCentered("2. PayTm");
            WriteCentered("3. PhonePe");
            //Console.Write();
            string? choice = ReadCentered("Enter your choice:");
            string? paymentmethod;
            if (choice == "1")
            {
                paymentmethod = "Google Pay";
            }
            else if (choice == "2")
            {
                paymentmethod = "PayTm";
            }
            else if (choice == "3")
            {
                paymentmethod = "PhonePe";
            }
            else
            {
                paymentmethod = null;
            }

            User u = UserService.users.FirstOrDefault(u => u.UserId == o.UserId);
            if (u != null)
            {
                int tries = 3;
                WriteCentered($"\nRedirecting to {paymentmethod}....");
                WriteCentered($"Total Price : {o.TotalAmount}\nProceed to pay? (y/n) : ");
                if (Console.ReadLine() == "y")
                {
                id:

                    string upiidnum = ReadCentered($"Enter your {paymentmethod} upi id / number : ");
                    if (upiidnum != u.Upiid && upiidnum != u.Phone)
                    {
                        WriteCentered("Wrong upi id or number!");
                        goto id;
                    }

                pin:
                    
                    int password = int.Parse(ReadCentered($"Enter your {paymentmethod} upi PIN : "));
                    if (password != u.Upipin)
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
                    return true;
                }
                else if(Console.ReadLine() == "n")
                {
                    WriteCentered("Payment cancelled!");
                    return false;
                }

                
                
            }
            return false;


        }
        return false;
    }
} 