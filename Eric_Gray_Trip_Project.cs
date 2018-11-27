using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp3
{
    public class StartHere
    {
        public static void Main()
        {
            while (true)
            {
                Trip cart = new Trip();
                while (cart.State.Execute()) ;

                Console.WriteLine();
                Console.WriteLine("Enter [New Booking] to make another Trip.");
                string continueShopping = Console.ReadLine();
                if (!continueShopping.Equals("New Booking")) break;
            }
        }
    }

    public enum TripStateType
    {
        Agent,

        Travelers,
        Reservations,
        Payment,
        CashPayment,
        CheckPayment,
        CardPayment,
        VerifyPayment,
        PersonalMessage,
        Itinerary


    }

    public class Trip
    {
        public int numberOfPeople { get; set; }
        public int costOfTrip = 0;
        public int moneyPaid = 0;
        public int checkNumber;
        public int cardNumber;
        public String note;
        public String people="";
        public String master;
        public String agentSignature;
        public Trip()
        {
            numberOfPeople = 0;
            TripStateFactory.Set(TripStateType.Agent, this);

        }
        public IState State { get; set; }


    }



    public interface IState
    {
        bool Execute();
    }

    public class TripStateFactory
    {
        public static void Set(TripStateType stateType, Trip context)
        {
            switch (stateType)
            {
                case TripStateType.Agent:
                    context.State = new TripStateAddAgent(context);
                    break;

                case TripStateType.Travelers:
                    context.State = new TripStateAddTravelers(context);
                    break;
                case TripStateType.Reservations:
                    context.State = new TripStateAddReservations(context);
                    break;

                case TripStateType.Payment:
                    context.State = new TripStateChoosePaymentMethod(context);
                    break;
                case TripStateType.CashPayment:
                    context.State = new AcceptPaymentCash(context);
                    break;
                case TripStateType.CardPayment:
                    context.State = new AcceptPaymentCard(context);
                    break;
                case TripStateType.CheckPayment:
                    context.State = new AcceptPaymentCheck(context);
                    break;
                case TripStateType.VerifyPayment:
                    context.State = new VerifyPayment(context);
                    break;

                case TripStateType.PersonalMessage:
                    context.State = new TripStatePersonalMessage(context);
                    break;
                case TripStateType.Itinerary:
                    context.State = new TripStateShowReceipt(context);
                    break;
                default:
                    throw new InvalidOperationException("Unknown state " + context.State);
            }
            Console.WriteLine();
            Console.WriteLine($"*** STATE CHANGED TO [{stateType}] ***");
            Console.WriteLine();
        }
    }

    public abstract class TripState : IState
    {
        protected TripState(Trip context)
        {
            _context = context;
        }
        private Trip _context;
        public abstract bool Execute();
        protected Trip Trip => _context;
    }



  

    public class TripStateShowReceipt : TripState
    {
        public TripStateShowReceipt(Trip context) : base(context) {
            
        }

        public override bool Execute()
        {
           
            ReceiptComponent decorator = new ReceiptGeneratorConcreteComponent(Trip);
            decorator = new ReceiptGeneratorConcreteComponentSeparator(decorator);
 
            Console.WriteLine(Trip.note);
            Console.WriteLine("-" + Trip.agentSignature);
            decorator = new ReceiptGeneratorConcreteComponentSeparator(decorator);
            decorator = new ReceiptGeneratorConcreteComponentLineItems(decorator);
            Console.WriteLine(Trip.people);
            decorator = new ReceiptGeneratorConcreteComponentSeparator(decorator);
            Console.WriteLine(Trip.master);

            decorator = new ReceiptGeneratorConcreteComponentSeparator(decorator);
    

            
            return false;
        }
    }


    public class TripStateAddAgent : TripState
    {
        public TripStateAddAgent(Trip context) : base(context) { }

        public override bool Execute()
        {
            while (true)
            {

                Console.WriteLine("Enter your agent Name");
                string name = Console.ReadLine();
                Trip.agentSignature = name;

                //are we done? ensure there is at least 1 item in cart if attempted
               // if (name.Equals("done", StringComparison.CurrentCultureIgnoreCase))
               switch(name)
                {
                    case "Peter":
                        TripStateFactory.Set(TripStateType.Travelers, Trip);
                        return true;
                        
                    case "Laura":
                        TripStateFactory.Set(TripStateType.Travelers, Trip);
                        return true;


                    case "Samantha":
                        TripStateFactory.Set(TripStateType.Travelers, Trip);
                        return true;


                    case "Jeff":
                        TripStateFactory.Set(TripStateType.Travelers, Trip);
                        return true;




                      


                }


                Console.WriteLine("That is not a known agent, try again");


            }
        }
    }







    public class TripStateAddTravelers : TripState
    {
        public TripStateAddTravelers(Trip context) : base(context)
        {
           
        }

        public override bool Execute()
        {


            Console.WriteLine("Enter name of a Traveler, or [done] to finish");
            while (true)
            {


                string name = Console.ReadLine();
             
                //are we done? ensure there is at least 1 item in cart if attempted
                if (name.Equals("done", StringComparison.CurrentCultureIgnoreCase))
                {

                    //done... change state

                    TripStateFactory.Set(TripStateType.Reservations, Trip);

                    return true;

                }
                else
                {
                    Trip.people += name + "  ";
                    Trip.numberOfPeople++;
                    Console.WriteLine("Enter the name of Another Traveler");
                }


            }
        }
    }


    public class TripStateAddReservations : TripState
    {
        public TripStateAddReservations(Trip context) : base(context) { }
       
        public override bool Execute()
        {
            Console.WriteLine("Select destination: [mazu][luna lake][detroit][ashland][itza]");
            while (true)
            {
              
                string newLocation = Console.ReadLine();


                switch (newLocation)
                {

                    case ("mazu"):
                        Trip.master += "Going to mazu in a ";
                        Console.WriteLine("What mode of Transport? [Plane][Car][Boat]");
                        String transport = Console.ReadLine();
                        if ( transport== "Plane")
                        {
                            Trip.master += " Plane, ";
                            Console.Write("Which date for Arrival?");
                            Trip.master += "arriving on " + Console.ReadLine()+"  |";
                      

                            Trip.costOfTrip += 1000 * Trip.numberOfPeople;
                        }
                        else if (transport == "Car")
                        {
                            Trip.master += "Car, ";
                            Console.Write("Which date for Arrival?");
                            Trip.master += "arriving on " + Console.ReadLine()+"  |";

                            Trip.costOfTrip += 800 * Trip.numberOfPeople;
                        }
                        else if (transport == "Boat")
                        {
                            Trip.master += " Boat, ";
                            Console.Write("Which date for Arrival?");
                            Trip.master += "arriving on " + Console.ReadLine()+"  |";
                   
                            Trip.costOfTrip += 9000 * Trip.numberOfPeople;
                        }
                        else
                        {
                            Console.WriteLine("Invalid Transport Type, try again.");
                            return true;
                        }
                        Console.WriteLine("The cost so far is " + Trip.costOfTrip);
                        Console.WriteLine("Do you wish to enter an additional destination?");
                        return true;

                    case ("luna lake"):
                        Trip.master += "Going to luna lake in a ";
                        Console.WriteLine("What mode of Transport? [Plane][Car][Boat]");
                        transport = Console.ReadLine();
                        if (transport == "Plane")
                        {
                            Trip.master += " Plane, ";
                            Console.Write("Which date for Arrival?");
                            Trip.master += "arriving on " + Console.ReadLine()+"  |";
                           
                            Trip.costOfTrip += 500 * Trip.numberOfPeople;
                        }
                        else if (transport == "Car")
                        {
                            Trip.master += " Car,";
                            Console.Write("Which date for Arrival?");
                            Trip.master += "arriving on " + Console.ReadLine()+"  |";
                           
                            Trip.costOfTrip +=500 * Trip.numberOfPeople;
                        }
                        else if (transport == "Boat")
                        {
                            Trip.master += " Boat, ";
                            Console.Write("Which date for Arrival?");
                            Trip.master += "arriving on " + Console.ReadLine()+"  |";
                         
                            Trip.costOfTrip += 500 * Trip.numberOfPeople;
                        }
                        else
                        {
                            Console.WriteLine("Invalid Transport Type, try again.");
                            return true;
                        }
                        Console.WriteLine("The cost so far is " + Trip.costOfTrip);
                        Console.WriteLine("Do you wish to enter an additional destination?");
                        return true;

                    case ("detroit"):
                        Trip.master += "Going to detroit in a ";
                        Console.WriteLine("What mode of Transport? [Plane][Car][Boat]");
                        transport = Console.ReadLine();
                        if (transport == "Plane")
                        {
                            Trip.master += "Plane ";
                            Console.Write("Which date for Arrival?");
                            Trip.master += "arriving on " + Console.ReadLine()+"  |";
                         
                            Trip.costOfTrip +=100 * Trip.numberOfPeople;
                        }
                        else if (transport == "Car")
                        {
                            Trip.master += "Car ";
                            Console.Write("Which date for Arrival?");
                            Trip.master += "arriving on " + Console.ReadLine()+"  |";
                           
                            Trip.costOfTrip += 200 * Trip.numberOfPeople;
                        }
                        else if (transport == "Boat")
                        {
                            Trip.master += "Boat ";
                            Console.Write("Which date for Arrival?");
                            Trip.master += "arriving on " + Console.ReadLine()+"  |";
                           
                            Trip.costOfTrip += 5400 * Trip.numberOfPeople;
                        }
                        else
                        {
                            Console.WriteLine("Invalid Transport Type, try again.");
                            return true;
                        }
                        Console.WriteLine("The cost so far is " + Trip.costOfTrip);
                        Console.WriteLine("Do you wish to enter an additional destination?");
                        return true;

                    case ("ashland"):
                        Trip.master += "Going to ashland in a ";
                        Console.WriteLine("What mode of Transport? [Plane][Car][Boat]");
                        transport = Console.ReadLine();
                        if (transport == "Plane")
                        {
                            Trip.master += "Plane ";
                            Console.Write("Which date for Arrival?");
                            Trip.master += "arriving on " + Console.ReadLine()+"  |";
                     
                            Trip.costOfTrip += 7000 * Trip.numberOfPeople;
                        }
                        else if (transport == "Car")
                        {
                            Trip.master += "Car ";
                            Console.Write("Which date for Arrival?");
                            Trip.master += "arriving on " + Console.ReadLine()+"  |";
                          
                            Trip.costOfTrip += 540 * Trip.numberOfPeople;
                        }
                        else if (transport == "Boat")
                        {
                            Trip.master += "Boat ";
                            Console.Write("Which date for Arrival?");
                            Trip.master += "arriving on " + Console.ReadLine()+"  |";
                           
                            Trip.costOfTrip += 9200 * Trip.numberOfPeople;
                        }
                        else
                        {
                            Console.WriteLine("Invalid Transport Type, try again.");
                            return true;
                        }
                        Console.WriteLine("The cost so far is " + Trip.costOfTrip);
                        Console.WriteLine("Do you wish to enter an additional destination?");
                        return true;

                    case ("itza"):
                        Trip.master += "Going to itza in a ";
                        Console.WriteLine("What mode of Transport? [Plane][Car][Boat]");
                        transport = Console.ReadLine();
                        if (transport == "Plane")
                        {
                            Trip.master += "Plane";
                            Console.Write("Which date for Arrival?");
                            Trip.master += "arriving on " + Console.ReadLine()+"  |";
                            Trip.costOfTrip += 1040 * Trip.numberOfPeople;
                        }
                        else if (transport == "Car")
                        {

                            Trip.master += "Car";
                            Console.Write("Which date for Arrival?");
                            Trip.master += "arriving on " + Console.ReadLine()+"  |";
                            Trip.costOfTrip += 1200 * Trip.numberOfPeople;
                        }
                        else if (transport == "Boat")
                        {
                            Trip.master += "Boat";
                            Console.Write("Which date for Arrival?");
                            Trip.master += "arriving on " + Console.ReadLine()+"  |";
                            Trip.costOfTrip += 600 * Trip.numberOfPeople;
                        }
                        else
                        {
                            Console.WriteLine("Invalid Transport Type, try again.");
                            return true;
                        }
                    
                        Console.WriteLine("The cost so far is " + Trip.costOfTrip);
                        Console.WriteLine("Do you wish to enter an additional destination?");
                        return true;
                    default:
                        TripStateFactory.Set(TripStateType.Payment, Trip);
                        return true;


                }




                
                
               
                String answer = Console.ReadLine();
                if (answer.Equals("yes", StringComparison.CurrentCultureIgnoreCase))
                {
                    return true;
                }
                else
                {
                    TripStateFactory.Set(TripStateType.Payment,Trip);
                    return true;
                }
            }
            
        }
    }



    public class TripStateChoosePaymentMethod : TripState
    {
        public TripStateChoosePaymentMethod(Trip context) : base(context) { }

        public override bool Execute()
        {
            
            while (true)
            {
                Console.WriteLine("Pay with [cash] or [check] or [card]?");
                string newItem = Console.ReadLine();
                if (newItem.Equals("cash", StringComparison.CurrentCultureIgnoreCase))
                {
                    TripStateFactory.Set(TripStateType.CashPayment, Trip);
                    return true;
                }
                else if (newItem.Equals("check", StringComparison.CurrentCultureIgnoreCase))
                {
                    TripStateFactory.Set(TripStateType.CheckPayment, Trip);
                    return true;
                }
                else if (newItem.Equals("card", StringComparison.CurrentCultureIgnoreCase))
                {
                    TripStateFactory.Set(TripStateType.CardPayment, Trip);
                    return true;
                }
                else
                {
                    Console.WriteLine("ERROR: You must enter [cash] or [check] or [card]");
                }
            }
        }
    }



    public class AcceptPaymentCash : TripState
    {
        public AcceptPaymentCash(Trip context) : base(context) { }

        public override bool Execute()
        {
            Console.WriteLine("Your Total is "+ Trip.costOfTrip+"$... please insert That amount of cash.");
            Trip.moneyPaid += Convert.ToInt32(Console.ReadLine());
            TripStateFactory.Set(TripStateType.VerifyPayment, Trip);
            return true;
        }
    }

       
    public class AcceptPaymentCheck : TripState
    {
        public AcceptPaymentCheck(Trip context) : base(context)
        { }
        public override bool Execute()
        {
            Console.WriteLine("Enter a Check number please");
            Trip.checkNumber = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Enter the amount of the check");
            Trip.moneyPaid+= Convert.ToInt32(Console.ReadLine());
            TripStateFactory.Set(TripStateType.VerifyPayment, Trip);
            return true;
        }

        
    }

    public class AcceptPaymentCard : TripState
    {
        public AcceptPaymentCard(Trip context) : base(context)
        { }
        public override bool Execute()
        {
            while (true) {
                Console.WriteLine("Enter a Card Number please");
                if (Convert.ToInt32(Console.ReadLine()) > 0)
                {
                    Console.WriteLine("Enter the Security Code");
                    Console.ReadLine();
                    Trip.cardNumber = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine("Enter the amount to be charged to this card");
                    Trip.moneyPaid += Convert.ToInt32(Console.ReadLine());
                    TripStateFactory.Set(TripStateType.VerifyPayment, Trip);
                    return true;
                }
            }
             
        }


    }




    public  class VerifyPayment : TripState
    {
       

        public VerifyPayment(Trip context) : base(context)
        {
            
        }
        public override bool Execute()
        {
            if (Trip.moneyPaid >= Trip.costOfTrip)
            {
                Console.WriteLine("Payment Accpeted");
                TripStateFactory.Set(TripStateType.PersonalMessage, Trip);
                return true;
            }
            else
            {
                Console.WriteLine("There is still an outstanding balance of " + (Trip.costOfTrip - Trip.moneyPaid));
                TripStateFactory.Set(TripStateType.Payment, Trip);
                return true;            }
        }




    }

    public class TripStatePersonalMessage : TripState
    {
        public TripStatePersonalMessage(Trip context) : base(context)
        { }
        public override bool Execute()
        {
            Console.WriteLine("Enter a Personal Message for the client's Itenerary");
            Trip.note = Console.ReadLine();
            TripStateFactory.Set(TripStateType.Itinerary, Trip);
            return true;
        }


    }



    public abstract class ReceiptComponent
    {
        public Trip Trip { get; set; }
   
    }

    public class ReceiptGeneratorConcreteComponent : ReceiptComponent
    {
        public ReceiptGeneratorConcreteComponent(Trip trip)
        {
            Trip = Trip;
        }

       
    }

    public abstract class ReceiptGeneratorDecorator : ReceiptComponent
    {
        protected ReceiptComponent _receiptComponent;

        public ReceiptGeneratorDecorator(ReceiptComponent receiptComponent)
        {
            _receiptComponent = receiptComponent;
            Trip = _receiptComponent.Trip;
        }


    }

    public class ReceiptGeneratorConcreteComponentLineItems : ReceiptGeneratorDecorator
    {
        public ReceiptGeneratorConcreteComponentLineItems(ReceiptComponent receiptComponent) : base(receiptComponent)
        {

            Console.WriteLine("People going on the trip- ");
        } }

   

    public class ReceiptGeneratorConcreteComponentSeparator : ReceiptGeneratorDecorator
    {
        public ReceiptGeneratorConcreteComponentSeparator(ReceiptComponent receiptComponent) :base(receiptComponent)
        {
            Console.WriteLine("*****************************************************************");
        }

       
    }

    public class ReceiptGeneratorConcreteComponentPaymentDetails : ReceiptGeneratorDecorator
    {
        public ReceiptGeneratorConcreteComponentPaymentDetails(ReceiptComponent receiptComponent) :
        base(receiptComponent)
        { }

      
    }

    public class ReceiptGeneratorConcreteComponentReturnPolicy : ReceiptGeneratorDecorator
    {
        public ReceiptGeneratorConcreteComponentReturnPolicy(ReceiptComponent receiptComponent) :
        base(receiptComponent)
        { }

     
    }

    public class ReceiptGeneratorConcreteComponentThankYou : ReceiptGeneratorDecorator
    {
        public ReceiptGeneratorConcreteComponentThankYou(ReceiptComponent receiptComponent) :
        base(receiptComponent)
        { }

        
    }
}
