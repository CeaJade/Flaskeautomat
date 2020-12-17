using System;
using System.Collections.Generic;
using System.Threading;

namespace Flaskeautomat
{
    class Program
    {
        static Queue<string> beverages = new Queue<string>();
        static Queue<string> sodas = new Queue<string>();
        static Queue<string> beers = new Queue<string>();
        static Random random = new Random();

        static void Main(string[] args)
        {
            while (true)
            {
                Thread producer = new Thread(ProduceDrinks);
                producer.Start();
                Thread.Sleep(50);
                Thread drinkSorter = new Thread(SortDrinks);
                drinkSorter.Start();
                Thread.Sleep(50);
                Thread soda = new Thread(GetSoda);
                soda.Start();
                Thread beer = new Thread(Getbeer);
                beer.Start();
            }
            

        }

        public static void ProduceDrinks()
        {
            
            int drink = random.Next(0, 2);
            if (drink == 1) {
                beverages.Enqueue("Beer");

            } else {
                beverages.Enqueue("Soda");

            }
        }

        public static void SortDrinks()
        {
            lock (beverages)
            {
                while(beverages.Count == 0)
                {
                    Monitor.Wait(beverages);
                }

                string drink = beverages.Peek();

                if (drink == "Soda")
                {
                    sodas.Enqueue(drink);
                }
                if (drink == "Beer")
                {
                    beers.Enqueue(drink);
                }

                beverages.Dequeue();
            }
        }

        public static void GetSoda()
        {
            lock (sodas)
            {
                while (sodas.Count == 0)
                {
                    Monitor.Wait(sodas);
                }

                Console.WriteLine("2 " + sodas.Dequeue());
                
            }
        }

        public static void Getbeer()
        {
            lock (beers)
            {
                while (beers.Count == 0)
                {
                    Monitor.Wait(beers);
                }

                Console.WriteLine("1 " + beers.Dequeue());
                
            }
        }

    }
}
