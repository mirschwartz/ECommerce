using ECommerce.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _5_17classwork;


namespace LoginAdmin
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("ADMIN SIGNIN");
            Console.WriteLine("Enter First Name:");
            string firstName = Console.ReadLine();

            Console.WriteLine("Enter Last Name:");
            string lastName = Console.ReadLine();

            Console.WriteLine("Enter Email:");
            string email = Console.ReadLine();

            Console.WriteLine("Enter Password:");
            string password = Console.ReadLine();

            UserManager manager = new UserManager(Properties.Settings.Default.ConStr);
            manager.AddUser(firstName, lastName, email, password);

            Console.ReadKey(true);
        }
    }
}