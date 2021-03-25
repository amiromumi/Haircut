using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace project_haircut
{
    class Program
    {
        static DateTime startService;
        static Random random = new Random();
        static int[] countCustomer;
        static Queue<int> customerList = new Queue<int>();
        static object _lock = new object();
        static DateTime[] startTimeBarber;
        static DateTime[] endTimeBarber;
        static int[] totalWorkTimeBarber;
        static int barberCount;

        static void Main(string[] args)
        {
            startService = DateTime.Now;

            Console.WriteLine("Please enter the number of the barbers: ");
            barberCount = Convert.ToInt32(Console.ReadLine());

            startTimeBarber = new DateTime[barberCount];
            endTimeBarber = new DateTime[barberCount];
            totalWorkTimeBarber = new int[barberCount];
            countCustomer = new int[barberCount];

            Console.WriteLine("Please enter the number of customers: ");
            int customerCount = Convert.ToInt32(Console.ReadLine());

            for (int i = 0; i < customerCount; i++)
            {
                customerList.Enqueue(i);
            }

            
            Thread[] threads = new Thread[barberCount];        

            for (int i = 0; i < barberCount; ++i)
            {
                int barberId = i;
                Thread t = new Thread(() => stylist(barberId));
                threads[i] = t;
                t.Start();
            }

            foreach (Thread thread in threads)
            {
                thread.Join();
            }


            DateTime endService = DateTime.Now;
            int finalService = (Int32)(endService - startService).TotalMilliseconds;

            Console.WriteLine("Total system time : " + finalService + " Milli Second");
            
            PrintReport();

            Console.ReadKey();
        }

        static void stylist(int id)
        {
            startTimeBarber[id] = DateTime.Now;
            while (true)
            {
                lock (_lock)
                {
                    if (customerList.Count == 0)
                    {
                        break;
                    }
                    customerList.Dequeue();
                }

                int randomTime = random.Next(1, 5);
                totalWorkTimeBarber[id] += randomTime;
                Thread.Sleep(TimeSpan.FromMilliseconds(randomTime));
                countCustomer[id]++;
            }
            endTimeBarber[id] = DateTime.Now;
        }

        static void PrintReport()
        {
            for (int i = 0; i < barberCount; i++)
            {
                Console.WriteLine();

                Console.WriteLine($"Count Customers, barber {i} : " + countCustomer[i]);    
                
                int finalTimeBarber = (Int32)(endTimeBarber[i] - startTimeBarber[i]).TotalMilliseconds;
                Console.WriteLine($"Total Time, barber {i} : " + finalTimeBarber + " Milli Second");
                
                Console.WriteLine($"Total work Time, barber {i} : " + totalWorkTimeBarber[i] + " Milli Second");
            }
        }
    }
}
