using System;
using System.Diagnostics;
using System.Text;
using Microsoft.EntityFrameworkCore;
using ptmk_task.EntityModels;
using ptmk_task.Models;

namespace ptmk_task
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("No arguments!");
                return;
            }

            switch (args[0])
            {
                case "1":
                    using (ApplicationContext context = new ApplicationContext())
                    {
                    }
                    break;
                case "2":
                    OptionTwo(args[1], args[2], args[3]);
                    break;
                case "3":
                    OptionThree();
                    break;
                case "4":
                    OptionFour();
                    break;
                case "5":
                    OptionFive();
                    break;
            }
        }

        private static void OptionTwo(string name, string birthDate, string gender)
        {
            DateTime dtBirthDate = DateTime.ParseExact(birthDate, "dd.MM.yyyy",
                System.Globalization.CultureInfo.InvariantCulture);
            int intGender = Convert.ToInt32(gender);
            using (ApplicationContext db = new ApplicationContext())
            {
                User user = new User(name, dtBirthDate, intGender);
                db.Users.Add(user);
                db.SaveChanges();
            }
        }

        private static void OptionThree()
        {
            const string query = "SELECT u.Id, u.Name, u.DateOfBirth, u.Gender " +
                                 "FROM Users as u " +
                                 "WHERE(u.Name, u.DateOfBirth) IN (" +
                                 "SELECT u1.Name, u1.DateOfBirth " +
                                 "FROM Users as u1 " +
                                 "GROUP BY u1.Name, u1.DateOfBirth " +
                                 "HAVING COUNT(*) = 1" +
                                 ")";
            using (ApplicationContext db = new ApplicationContext())
            {
                var uniqUsers = db.Users.FromSqlRaw(query).ToList();
                foreach (var user in uniqUsers)
                {
                    Console.WriteLine($"{user.Name} - {user.DateOfBirth} - {user.Gender}");
                }
            }
        }

        private static void OptionFour()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                for (int i = 0; i < 100; i++)
                {
                    var user = new User("Fname", DateTime.Today, 1);
                    db.Add(user);
                }

                Random random = new Random();
                for (int i = 0; i < 1000000 - 100; i++)
                {
                    db.Add(GenerateUser(random));
                }

                db.SaveChanges();
            }
        }

        private static User GenerateUser(Random random)
        {
            int year = random.Next(2000, 2010);
            int month = random.Next(1, 13);
            int day = random.Next(1, 28);
            var birthDate = new DateTime(year, month, day);
            int gender = random.Next(0, 2);
            string upperChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string lowerChars = "abcdefghijklmnopqrstuvwxyz";
            StringBuilder sb = new StringBuilder();
            sb.Append(upperChars[random.Next(upperChars.Length)]);
            for (int i = 0; i < 8; i++)
            {
                sb.Append(lowerChars[random.Next(lowerChars.Length)]);
            }

            return new User(sb.ToString(), birthDate, gender);
        }

        private static void OptionFive()
        {
            var swq = new Stopwatch();
            var sw = new Stopwatch();
            sw.Start();
            string query = "SELECT u.Id, u.Name, u.DateOfBirth, u.Gender " +
                           "FROM Users as u " +
                           "where u.Name LIKE 'F%'";
            using (ApplicationContext db = new ApplicationContext())
            {
                swq.Start();
                var fUsers = db.Users.FromSqlRaw(query).ToList();
                swq.Stop();
                foreach (var user in fUsers)
                {
                    Console.WriteLine($"{user.Name} - {user.DateOfBirth} - {user.Gender}");
                }
            }
            sw.Stop();
            Console.WriteLine($"Общее время выполнения - {sw.Elapsed}");
            Console.WriteLine($"Время выполнения только запроса - {swq.Elapsed}");
        }
    }
}