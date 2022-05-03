using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace PizzaGame
{
    internal class Program
    {
        public static int pizzas  { get; set; }
        public static int[] allbites = new int[3] { 1, 2, 3 };
        public static Random random = new Random();
        static void Main(string[] args)
        {
            Console.WriteLine($"Press any Key to play pizza game.");
            Console.ReadKey();
            while (true)
            {
                pizzas = new Random().Next(11, 500);
                Console.WriteLine($"are u playing with {pizzas} pizzas");
                PlayPizzaGame(allbites, null, 0);
                Console.WriteLine($"wanna play again?");
                Console.ReadKey();
            }
        }

        private static void PlayPizzaGame(int[] bites, int? bite, int player)
        {
            Thread.Sleep(100);
            //jump case
            if (bites.All(a => a > pizzas))
            {
                Console.WriteLine($"{PrintPlayer(player)} jump his turn, no bites availble");
                PlayPizzaGame(allbites, null, ++player);
            }
            else if (bites.Any(a => a == pizzas))
            {
                //try to avoid defeat case
                List<int> availableBites = AvailableBites(bite).ToList();
                int currentBite = 0;
                if (!availableBites.Any(a => a < pizzas))
                {
                    currentBite = availableBites.Where(a => a == pizzas).FirstOrDefault();
                    Console.WriteLine($"{PrintPlayer(player)} have eaten {currentBite} pizzas. {0} pizzas left on table");
                    Console.WriteLine($"{PrintPlayer(player)} GAME OVER");
                    return; //exit condition
                }
                //eat
                currentBite = availableBites.Where(a => a < pizzas).First();
                pizzas -= currentBite;
                Console.WriteLine($"{PrintPlayer(player)} have eaten {currentBite} pizzas. {pizzas} pizzas left on table(tried to avoid defeat)");
                PlayPizzaGame(AvailableBites(currentBite), currentBite, ++player);
            }
            else // eat case
            {
                int currentBite = GetBite(AvailableBites(bite), random);
                pizzas -= currentBite;
                Console.WriteLine($"{PrintPlayer(player)} have eaten {currentBite} pizzas. {pizzas} pizzas left on table");
                PlayPizzaGame(AvailableBites(currentBite), currentBite, ++player);
            }
        }
        #region utils
        public static int[] AvailableBites(int? toIgnore)
        {
            var bitesCopy = new int[allbites.Length];
            allbites.CopyTo(bitesCopy, 0);
            if (toIgnore.HasValue)
                bitesCopy = bitesCopy.Where(x => x != toIgnore && x <= pizzas).ToArray();
            int upperLimit = toIgnore.HasValue ? 1 : 2;
            return bitesCopy;
        }
        public static int GetBite(int[] bites, Random r)
        {
            return bites[r.Next(0, bites.Length)];
        }
        public static string PrintPlayer(int p)
        {
            return string.Format("{0}", p % 2 == 0 ? "player A" : "player B");
        }
        #endregion
    }
}