using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace net3
{
    class MyPart //класс-обёртка для потока
    {
        int[] part;
        public Thread Thrd;
        int num;

        public MyPart(List<int> PART, int NUM)
        {
            num = NUM;
            part = new int[PART.Count()];
            for (int i = 0; i < PART.Count(); i++)
            {
                part[i] = PART[i];
            }
            Thrd = new Thread(this.Run);
            Thrd.Start();
        }

        void Run()
        {
            //Console.WriteLine("Часть №" + num + " начала сортировку.");
            int temp;
            for (int i = 0; i < part.Length- 1; i++)
            {
                for (int j = 0; j < part.Length - i - 1; j++)
                {
                    if (part[j + 1] < part[j])
                    {
                        temp = part[j + 1];
                        part[j + 1] = part[j];
                        part[j] = temp;
                    }
                }
            }
            //foreach (var item in part)
            //{
            //    Console.Write(item+" ");

            //}
            //Console.WriteLine();
            //Console.WriteLine("Часть №" + num + " закончила сортировку.");

        }

        public int[] REt() //метод, возвращающий массив
        {
            return part;
        }
    }
    class Program
    {
        static int n, p;
        static int[] mas = new int[n];
        static Stopwatch sm = new Stopwatch();

        static void Main()
        {
            int[,] Indexes;
            Console.Write("Введите размер массива: ");
            n = Convert.ToInt32(Console.ReadLine());
            Console.Write("Введите количество потоков: ");
            p = Convert.ToInt32(Console.ReadLine());
            int sp = 1;
            Console.WriteLine("Введите спрособ заполнения массива: 1 - рандомное заполнение (по умолчанию), 2 - неравномерное заполнение");
            sp = Convert.ToInt32(Console.ReadLine());
            if (sp == 2)
                mas = MAS1();
            if (sp == 1)
                mas = MAS();
            int max = mas.Max<int>(), min = mas.Min<int>();
            //foreach (var item in mas)
            //{
            //    Console.WriteLine(item);
            //}
            Indexes = PartIdexes(min, max);
            sm.Start();
            MyPart[] Parts = new MyPart[p]; //массив потоков
            List<int> mp = new List<int>();
            for (int i = 0; i < p-1; i++)
            {
                for (int j = 0; j < mas.Length; j++)
                {
                    if ((mas[j]>=Indexes[i, 0])&&(mas[j] < Indexes[i, 1]))
                    {
                        mp.Add(mas[j]);
                    }
                }
                Console.WriteLine("В потоке №"+ i +" обрабатывается "+ mp.Count()+" элементов");
                Parts[i] = new MyPart(mp, i);

                mp.Clear();
            }
            for (int j = 0; j < mas.Length; j++)
            {
                if ((mas[j] >= Indexes[p-1, 0]) && (mas[j] <= Indexes[p-1, 1]))
                {
                    mp.Add(mas[j]);
                }
            }
            Console.WriteLine("В потоке №" + (p-1) + " обрабатывается " + mp.Count() + " элементов");
            Parts[p-1] = new MyPart(mp, p-1);
            foreach (var item in Parts)
            {
                item.Thrd.Join();
            }
            List<int> SMas;
            SMas = SortMas(Parts);
            sm.Stop();
            //Console.WriteLine("Выводим отсортированный массив:");
            //foreach (var item in SMas)
            //{
            //    Console.WriteLine(item);
            //}
            long time = sm.ElapsedMilliseconds;
            Console.WriteLine("Время затраченное на сортировку массива (в миллисекундах): " + time);
            Console.ReadLine();
        }

        static List<int> SortMas(MyPart[] parts) //соединяю отсортированные части массива
        {
            List<int> newMas = new List<int>();
            for (int i = 0; i < p - 1; i++)
            {
                for (int j = 0; j < parts[i].REt().Length; j++)
                {
                    newMas.Add(parts[i].REt()[j]);
                }
            }
            return newMas;
        }

        static int[] MAS() //Рандомно заполняю массив
        {
            Random rnd = new Random();
            int[] M = new int[n];
            for (int i = 0; i < n; i++)
            {
                M[i] = rnd.Next(-100, 1000);
            }
            return M;
        }

        static int[] MAS1()
        {
            Random rnd = new Random();
            int[] M = new int[n];
            int i = 0;
            while (i < n)
            {
                int temp = rnd.Next(0, 101);
                if (temp <= 10)
                {
                    M[i] = 1000;
                }
                else
                {
                    M[i] = 1;
                }
                i++;

            }

            return M;
        }
        static int[,] PartIdexes(int min, int max) //Прописываю массив индексов 
        {
            int part = (max-min)/p, i = 0;
            int[,] Indexes = new int[p, 2];
            for (int j = 0; j < p; j++)
            {
                Indexes[j, 0] = 0;
                Indexes[j, 1] = 0;
            }
            while (i < p - 1)
            {
                Indexes[i, 0] = min + part * i;
                Indexes[i, 1] = min + (part * (i + 1));
                i++;
            }
            Indexes[p-1, 0] = min + part * (p-1);
            Indexes[p-1, 1] = max;
            return Indexes;
        }
    }
}
