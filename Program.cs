using System;
using System.Collections.Generic;
using System.Linq;

namespace GymManagementSystem
{
    enum SubscriptionType
    {
        Monthly = 1,
        ThreeMonths = 2,
        Yearly = 3
    }

    class Member
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public DateTime JoinDate { get; set; }
        public DateTime SubscriptionEnd { get; set; }
        public SubscriptionType SubscriptionType { get; set; }
        public decimal AmountPaid { get; set; }

        public bool IsActive()
        {
            return SubscriptionEnd >= DateTime.Now;
        }

        public void RenewSubscription(SubscriptionType type, decimal price)
        {
            SubscriptionType = type;
            AmountPaid += price;

            if (SubscriptionEnd < DateTime.Now)
                SubscriptionEnd = DateTime.Now;

            switch (type)
            {
                case SubscriptionType.Monthly:
                    SubscriptionEnd = SubscriptionEnd.AddMonths(1);
                    break;
                case SubscriptionType.ThreeMonths:
                    SubscriptionEnd = SubscriptionEnd.AddMonths(3);
                    break;
                case SubscriptionType.Yearly:
                    SubscriptionEnd = SubscriptionEnd.AddYears(1);
                    break;
            }
        }

        public void Display()
        {
            Console.WriteLine("================================");
            Console.WriteLine($"ID: {Id}");
            Console.WriteLine($"Name: {Name}");
            Console.WriteLine($"Phone: {Phone}");
            Console.WriteLine($"Join Date: {JoinDate.ToShortDateString()}");
            Console.WriteLine($"Subscription Ends: {SubscriptionEnd.ToShortDateString()}");
            Console.WriteLine($"Type: {SubscriptionType}");
            Console.WriteLine($"Total Paid: {AmountPaid}");
            Console.WriteLine($"Status: {(IsActive() ? "Active" : "Expired")}");
        }
    }

    class Program
    {
        static List<Member> members = new List<Member>();
        static int idCounter = 1;

        static void Main(string[] args)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("===== Gym Management System =====");
                Console.WriteLine("1- Add Member");
                Console.WriteLine("2- Renew Subscription");
                Console.WriteLine("3- Show All Members");
                Console.WriteLine("4- Show Expired Members");
                Console.WriteLine("5- Show Total Income");
                Console.WriteLine("6- Exit");

                Console.Write("Choose option: ");
                int choice;
                while (!int.TryParse(Console.ReadLine(), out choice) || choice < 1 || choice > 6)
                {
                    Console.Write("Invalid choice, enter a number between 1 and 6: ");
                }
                switch (choice)
                {
                    case 1: AddMember(); break;
                    case 2: RenewMember(); break;
                    case 3: ShowAll(); break;
                    case 4: ShowExpired(); break;
                    case 5: ShowIncome(); break;
                    case 6: return;
                }

                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
            }
        }

        static void AddMember()
        {
            Console.Write("Enter Name: ");
            string name = Console.ReadLine();

            Console.Write("Enter Phone: ");
            string phone = Console.ReadLine();

            SubscriptionType type = ChooseSubscription();
            decimal price = GetPrice(type);

            Member m = new Member
            {
                Id = idCounter++,
                Name = name,
                Phone = phone,
                JoinDate = DateTime.Now,
                SubscriptionEnd = DateTime.Now,
                AmountPaid = 0
            };

            m.RenewSubscription(type, price);
            members.Add(m);

            Console.WriteLine("Member Added Successfully!");
        }

        static void RenewMember()
        {
            Console.Write("Enter Member ID: ");
            int id;
            while (!int.TryParse(Console.ReadLine(), out id))
            {
                Console.Write("Invalid ID, enter a number: ");
            }
            var member = members.FirstOrDefault(m => m.Id == id);

            if (member == null)
            {
                Console.WriteLine("Member Not Found!");
                return;
            }

            SubscriptionType type = ChooseSubscription();
            decimal price = GetPrice(type);

            member.RenewSubscription(type, price);
            Console.WriteLine("Subscription Renewed Successfully!");
        }

        static void ShowAll()
        {
            foreach (var m in members)
                m.Display();
        }

        static void ShowExpired()
        {
            var expired = members.Where(m => !m.IsActive());

            foreach (var m in expired)
                m.Display();
        }

        static void ShowIncome()
        {
            decimal total = members.Sum(m => m.AmountPaid);
            Console.WriteLine($"Total Income: {total}");
        }

        static SubscriptionType ChooseSubscription()
        {
            Console.WriteLine("Choose Subscription Type:");
            Console.WriteLine("1- Monthly (300)");
            Console.WriteLine("2- Three Months (800)");
            Console.WriteLine("3- Yearly (3000)");

            int choice;
            while (!int.TryParse(Console.ReadLine(), out choice))
            {
                Console.Write("Invalid choice, enter a number: ");
            }
            return (SubscriptionType)choice;
        }

        static decimal GetPrice(SubscriptionType type)
        {
            switch (type)
            {
                case SubscriptionType.Monthly: return 300;
                case SubscriptionType.ThreeMonths: return 800;
                case SubscriptionType.Yearly: return 3000;
                default: return 0;
            }
        }
    }
}