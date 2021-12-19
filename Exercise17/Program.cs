using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

namespace Exercise17
{
    class Program
    {
        static string str = string.Empty;
        static object locker = new object();
        static void Main(string[] args)
        {
            //Задание 1
            List<Car> cars = new List<Car>();
            for (int i = 1; i <= 100000; i++)
            {                
                Car car = new Car();
                car.Age = i;
                cars.Add(car);
            }

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            foreach (var item in cars)
            {
                item.Age = (343 * 34 * DateTime.Now.Millisecond * 77) / DateTime.Now.Minute;                
            }
            stopwatch.Stop();
            Console.Write("Simple Foreach: ");
            elapsedTime(stopwatch.Elapsed);

            Stopwatch stopwatch2 = new Stopwatch();
            stopwatch2.Start();
            for (int i = 0; i < cars.Count; i++)
            {
                cars[i].Age = (343 * 34 * DateTime.Now.Millisecond * 77) / DateTime.Now.Minute;
            }
            stopwatch2.Stop();
            Console.Write("Simple For: ");
            elapsedTime(stopwatch2.Elapsed);

            Stopwatch stopwatch3 = new Stopwatch();
            stopwatch3.Start();
            //Parallel.ForEach(cars, car => car.Age = (343 * 34 * DateTime.Now.Millisecond * 77) / DateTime.Now.Minute);
            Parallel.ForEach(cars, car=> ValueChange(car));
            stopwatch3.Stop();
            Console.Write("Parallel Foreach: ");
            elapsedTime(stopwatch3.Elapsed);

            Stopwatch stopwatch4 = new Stopwatch();
            stopwatch4.Start();
            Action<int> transform = (int i) => { cars[i].Age = (343 * 34 * DateTime.Now.Millisecond * 77) / DateTime.Now.Minute; };
            Parallel.For(0, cars.Count, transform);
            stopwatch4.Stop();
            Console.Write("Parallel For: ");
            elapsedTime(stopwatch4.Elapsed);


            //Задание 2
            Task first = new Task(Action1);
            first.Start();
            Task second = new Task(Action2);
            second.Start();

            Task.WaitAll(first, second);
            using (var stream = new FileStream(@"Q:\ItAcademy\Homework\Tasks\Занятие 17 (Многопоточность)\Test\третий.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                using (var writer = new StreamWriter(stream))
                {
                    writer.Write(str);
                    writer.Close();
                }
            }
        }
        
        public static void ValueChange(Car car)
        {            
            object locker = new object();
            lock (locker)
            {
                car.Age = (343 * 34 * DateTime.Now.Millisecond * 77) / DateTime.Now.Minute;
            }
        }
        public static void elapsedTime(TimeSpan ts)
        {
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            ts.Hours, ts.Minutes, ts.Seconds,
            ts.Milliseconds / 10);
            Console.WriteLine(elapsedTime);
        }
        static void Action1()
        {
            lock (locker)
            {
                string path1 = @"Q:\ItAcademy\Homework\Tasks\Занятие 17 (Многопоточность)\Test\первый.txt";
                using (StreamReader sr = new StreamReader(path1))
                {
                    str += sr.ReadToEnd();
                }
            }
        }
        static void Action2()
        {
            lock (locker)
            {
                string path1 = @"Q:\ItAcademy\Homework\Tasks\Занятие 17 (Многопоточность)\Test\второй.txt";
                using (StreamReader sr = new StreamReader(path1))
                {
                    str += sr.ReadToEnd();
                }
            }
        }
    }
}
