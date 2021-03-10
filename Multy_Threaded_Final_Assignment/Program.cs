using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Multy_Threaded_Final_Assignment
{
    class Program
    {
        // im using all the functions in here in main 

        // 3)
        public static Func<double, double, double> f;
        static double Add(double num1, double num2)
        {
            return num1 + num2;
        }

        // 9)
        public static void LongOperation()
        {
            for (int i = 0; i < 1_000_000_000; i++)
            {

            }
            Console.WriteLine("done");
        }

        // 14)
        public static void DownloadFile()
        {
            Console.WriteLine("downloading file...");
            Thread.Sleep(10_000);
            Console.WriteLine("completed");
        }
        public static void Multiply(int num1, int num2)
        {
            Console.WriteLine(num1 * num2);
        }

        // 19)
        static object key = new object();
        public static void DoctorTreatment()
        {
            lock(key)
            {
                Console.WriteLine("waiting for my turn");
                Monitor.Wait(key);
                Console.WriteLine("getting treatment");
                NurseCheck();
            }
            
        }
        public static void NurseCheck()
        {
            lock(key)
            {
                Console.WriteLine("nurse is checking");
                Thread.Sleep(5000);
                Console.WriteLine("next patient please!");
                Monitor.Pulse(key);
            }
          
        }

        // 22)
        public static ManualResetEvent manualHost = new ManualResetEvent(false);
        public static void EnterClubManual()
        {
            Console.WriteLine("waiting to enter...");
            manualHost.WaitOne();
            Console.WriteLine("im finally in");
        }
        public static AutoResetEvent autoHost2 = new AutoResetEvent(true);
        public static void EnterClubAuto()
        {
            Console.WriteLine("waiting to enter...");
            autoHost2.WaitOne();
            Console.WriteLine("im finally in");
            autoHost2.Set();
        }

        static void Main(string[] args)
        {
            // 1)
            List<int> numbers = new List<int>();
            Random r = new Random();
            for (int i = 0; i < 100; i++)
            {
                numbers.Add(r.Next(51));
            }

            // a)
            var nums = from num in numbers
                       where num == 10
                       select num;
            // b)
            nums = from num in numbers
                   where num % 3 == 0
                   select num;
            // c)
            nums = from num in numbers
                   where num > 20 && num % 2 == 0
                   select num;
            // d)
            nums = from num in numbers
                   orderby num descending
                   select num;

            // 2)
            List<string> names = new List<string>
            {
                "bob", "jack", "josh", "robin", "jake", "don",
                "lucifer", "chloe", "mike", "harvey"
            };

            // a)
            var selectedNames = from name in names
                                where name.Length > 4
                                select name;

            // b)
            selectedNames = from name in names
                            where name.Contains("a")
                            select name;

            // c)
            selectedNames = from name in names
                            orderby name
                            select name;
            // 3)
            f = Add;
            Console.WriteLine(f(1, 2));

            // 9)

            // a)
            Stopwatch s1 = new Stopwatch();
            s1.Start();
            LongOperation();
            LongOperation();
            LongOperation();
            LongOperation();
            LongOperation();
            s1.Stop();
            Console.WriteLine(s1.Elapsed); // 8.5 sec

            // b)
            Stopwatch s2 = new Stopwatch();
            s2.Start();
            Thread t1 = new Thread(() =>
            {
                LongOperation();
            });
            Thread t2 = new Thread(() =>
            {
                LongOperation();
            });
            Thread t3 = new Thread(() =>
            {
                LongOperation();
            });
            Thread t4 = new Thread(() =>
            {
                LongOperation();
            });
            Thread t5 = new Thread(() =>
            {
                LongOperation();
                s2.Stop();
                Console.WriteLine(s2.Elapsed);
            });
            t1.Start();
            t2.Start();
            t3.Start();
            t4.Start();
            t5.Start(); // 1.9 sec
                        // option b is faster bacause there are 5 threads doing 1/5 of the work at the same time

            // 12)

            // a)
            ThreadPool.QueueUserWorkItem((o) =>
            {
                Console.WriteLine("hello world");
            });

            // b)
            Thread tt = new Thread((o) =>
            {
                var numberss = from n in numbers
                               where n == (int)o
                               select n;
            });
            tt.Start(8);

            // 14)
            Thread ttt = new Thread(DownloadFile);
            ttt.Start();
            ThreadPool.QueueUserWorkItem((o) => Multiply(3, 6));

            // 19)
            Thread th = new Thread(() =>
            {
                DoctorTreatment();
            });
            Thread th2 = new Thread(() =>
            {
                DoctorTreatment();
            });
            Thread th3 = new Thread(() =>
            {
                DoctorTreatment();
            });
            Thread thh = new Thread(() =>
            {
                NurseCheck();
            });
            th.IsBackground = false;
            th2.IsBackground = false;
            th3.IsBackground = false;
            thh.IsBackground = false;
            th.Start();
            th2.Start();
            th3.Start();
            Thread.Sleep(500);
            thh.Start();

            // 22)

            // a)
            for (int i = 0; i < 50; i++)
            {
                new Thread(() => EnterClubManual()).Start();
            }
            Thread.Sleep(3000);
            manualHost.Set();

            // b)
            for (int i = 0; i < 50; i++)
            {
                new Thread(() => EnterClubAuto()).Start();
            }

        }
    }
}
