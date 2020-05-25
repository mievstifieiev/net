using System;
using System.Collections.Generic;
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
            Array.Sort(part);
            Console.WriteLine("Часть №" + num + " закончила сортировку.");
        }
    }
    class Program
    {
        static int n, p;
        static int[] mas = new int[n];

        static void Main()
        {
            int[,] Indexes;
            Console.Write("Введите размер массива: ");
            n = Convert.ToInt32(Console.ReadLine());
            Console.Write("Введите количество потоков: ");
            p = Convert.ToInt32(Console.ReadLine());
            mas = MAS(n);      
            Indexes = PartIdexes(p, n);
            MyPart[] Parts = new MyPart[p];
            for (int i = 0; i < p; i++)
            {
                int[] mp = new int[p];
                for (int j = Indexes[i, 0]; j <= Indexes[i,1]; j++)
                {
                    mp[j] = mas[j];
                }
                Parts[i] = new MyPart(mp, i);
            }
            Console.ReadLine();
        }

        static int[] MAS(int n)
        {
            Random rnd = new Random();
            int[] M = new int[n];
            for (int i = 0; i < n; i++)
            {
                M[i] = rnd.Next(-100, 1000);
            }
            return M;
        }
        static int[,] PartIdexes(int p, int n)
        {
            int part = n / p, i = 0;
            int[,] Indexes = new int[p, 2];
            for (int j = 0; j < p; j++)
            {
                Indexes[j, 0] = 0;
                Indexes[j, 1] = 0;
            }
            do
            {
                Indexes[i, 0] = part * i;
                Indexes[i, 1] = (part * (i + 1))-1;
                i++;
            } while (i < p - 1);
            Indexes[i, 0] = part * i;
            Indexes[i, 1] = n - 1;
            return Indexes;
        }
    }
}
