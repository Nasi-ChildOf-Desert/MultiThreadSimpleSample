using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ThreadTrain
{
    class Program
    {
        static List<int> basket;
        static List<int> basketHis;
        static readonly object basketLock = new object();
        static readonly object basketHisLock = new object();
        static bool runProducer = true;
        static bool runHisMaker = true;
        static bool runDisplayer = true;
        static void Main(string[] args)
        {
            basket = new List<int>();
            basketHis = new List<int>();
            // Initialize basketHis with 10 inside lists
           
            Thread NumGenThread = new Thread(RandomGenerator);
            Thread HisThread = new Thread(HistoMaker);
            Thread DisThread = new Thread(DisplayHisto);
            NumGenThread.Start();
            HisThread.Start();
            DisThread.Start();

            NumGenThread.Join();
            HisThread.Join();
            DisThread.Join();
            Console.WriteLine("this is test");
            Console.ReadKey();
        }
        private static void RandomGenerator()
        {
            Random rng = new Random();
            while (runProducer)
            {
                lock (basketLock)
                    basket.Add(rng.Next(100));
                Thread.Sleep(10);
            }

        }
        private static void HistoMaker()
        {
            List<int> temp = new List<int>();

            while (runHisMaker)
            {
                if (basket.Count <= 0)
                {
                    Thread.Sleep(10);
                    continue;
                }
                lock (basketLock)
                {
                    temp = new List<int>(basket); //This solution only works for primitive types
                    //  temp = basket; // clone or refrence? So, from this point I would like to keep adding element
                    //  or making changes in name_list2 without affecting name_list1. How do I do that?
                    // ? 2 : its better to add in temp or new every time ? "The processing time is longer than the time to enter new data."
                    basket.Clear();
                }
                // When initializing your collections

                lock (basketHisLock)//"This loop may take too long and cause a delay."
                {
                    if (basketHis.Count == 0)
                    {
                        for (int k = 0; k < 10; k++)
                        {
                            basketHis.Add(0);
                        }
                    }
                    for (int i = 0; i < temp.Count; i++)// do i need create new for basketHis ? how to define it?
                    {
                        if (temp[i] < 10)
                            basketHis[0]++;
                        else if (temp[i] < 20)
                            basketHis[1]++;
                        else if (temp[i] < 30)
                            basketHis[2]++;
                        else if (temp[i] < 40)
                            basketHis[3]++;
                        else if (temp[i] < 50)
                            basketHis[4]++;
                        else if (temp[i] < 60)
                            basketHis[5]++;
                        else if (temp[i] < 70)
                            basketHis[6]++;
                        else if (temp[i] < 80)
                            basketHis[7]++;
                        else if (temp[i] < 90)
                            basketHis[8]++;
                        else
                            basketHis[9]++;

                    }
                }
                    
               
            }


        }
        private static void DisplayHisto()
        {
            List<int> temp = new List<int>();
            while (runDisplayer)
            {
                if (basketHis.Count <= 0)
                {
                    Thread.Sleep(10);
                    continue;
                }
                lock (basketHisLock)// imagine they are difrent object of difrernt class do i need exact locker of class a for a bascket?
                {
                    temp = new List<int>(basketHis); // how to check each row is for lastSecond? timer?                
                  //  basketHis.Clear();
                }

                for (int i = 0; i < temp.Count; i++)// do i need create new for basketHis ? how to define it?
                {
                    Console.WriteLine("the hist of bin {0}   is  {1}",i, temp[i]);
                }
            }
        }

    }
}
