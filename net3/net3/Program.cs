using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace net3
{
    class MyPart
    {
        int[] part;
        public Thread Thrd;
        int num;

        public MyPart(int[] PART, int NUM)
        {
            part = PART;
            num = NUM;
            Thrd = new Thread(this.Run);
            Thrd.Start();
        }

        void Run()
        {
            Console.WriteLine("Часть №" + num + " начала сортировку.");
            int temp;
            for (int i = 0; i < part.Length - 1; i++)
            {
                for (int j = 0; j < part.Length - i - 1; j++)
                {
                    if (part[j + 1] > part[j])
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
            Console.WriteLine("Часть №" + num + " закончила сортировку.");

        }

        public int[] REt()
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
            mas = MAS();
            //foreach (var item in mas)
            //{
            //    Console.WriteLine(item);
            //}
            Indexes = PartIdexes();
            sm.Start();
            MyPart[] Parts = new MyPart[p];
            for (int i = 0; i < p; i++)
            {
                int[] mp = new int[Indexes[i,1] - Indexes[i, 0]+1];
                int u = 0;
                for (int j = Indexes[i, 0]; j <= Indexes[i, 1]; j++)
                {
                    mp[u] = mas[j];
                    u++;
                }
                Parts[i] = new MyPart(mp, i);
                mp = null;
            }
            foreach (var item in Parts)
            {
                item.Thrd.Join();
            }
            int[] SMas = new int[n];
            SMas = SortMas(Parts);
            sm.Stop();
            //Console.WriteLine("Выводим отсортированный массив:");
            //foreach (var item in SMas)
            //{
            //    Console.WriteLine(item);
            //}
            long time = sm.ElapsedMilliseconds;
            Console.WriteLine("Время затраченное на сортировку массива (в миллисекундах): "+time);
            Console.ReadLine();
        }

        static int[] SortMas(MyPart[] parts)
        {
            int[] newMas = new int[n];
            int[] IndMas = new int[p];
            for (int i = 0; i < p; i++)
            {
                IndMas[i] = 0;
            }
            int j = 0;
            do
            {
                int max = System.Int32.MinValue;
                int pInd = -1;
                for (int i = 0; i < p; i++)
                {
                    if (IndMas[i]< parts[i].REt().Length)
                    {
                        if (parts[i].REt()[IndMas[i]] >= max)
                        {
                            max = parts[i].REt()[IndMas[i]];
                            pInd = i;
                        }
                    }
                }
                if (pInd > -1)
                {
                    newMas[j] = max;
                    IndMas[pInd]++;
                    j++;
                     
                }
            }
            while (j != n);
            return newMas;
        }

        static int[] MAS()
        {
            Random rnd = new Random();
            int[] M = new int[n];
            for (int i = 0; i < n; i++)
            {
                M[i] = rnd.Next(-100, 1000);
            }
            return M;
        }
        static int[,] PartIdexes()
        {
            int part = n / p, i = 0;
            int[,] Indexes = new int[p, 2];
            for (int j = 0; j < p; j++)
            {
                Indexes[j, 0] = 0;
                Indexes[j, 1] = 0;
            }
            while (i < p - 1)
            {
                Indexes[i, 0] = part * i;
                Indexes[i, 1] = (part * (i + 1))-1;
                i++;
            }
            Indexes[i, 0] = part * i;
            Indexes[i, 1] = n - 1;
            return Indexes;
        }
    }
}
