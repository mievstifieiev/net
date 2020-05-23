using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace net1
{
    class SumArray
    {
        int sum;
        object lockOn = new object();//закрытый объект доступный для последующей блокировки

        public int Sum(int[] nums)
        {
            lock (lockOn)
            {//заблокировать весь метод
                sum = 0; //установить исходное значение суммы
                for(int i=0; i<nums.Length; i++)
                {
                    sum += nums[i];
                    Console.WriteLine("Текущая сумма для потока " + Thread.CurrentThread.Name + " равна " + sum);
                    Thread.Sleep(10); // разрешить переключение задач
                }
                return sum;
            }
        }
    }

    class MyThread
    {
        public Thread Thrd;
        int[] a;
        int answer;
        //Создать один объект типа SumArray для всех экземпляров класса MyThread.
        static SumArray sa = new SumArray();
        //Сконструировать новый поток.

        public MyThread(string name, int[] nums)
        {

            a = nums;
            Thrd = new Thread(this.Run);
            Thrd.Name = name;
            Thrd.Start(); //выполнение нового потока
        }
        void Run()
        {
            Console.WriteLine(Thrd.Name + " начат. ");
            answer = sa.Sum(a);
            Console.WriteLine("Сумма для потока " + Thrd.Name + " равна " + answer);
            Console.WriteLine(Thrd.Name + " завершен. ");
        }
    }
    class Program
    {
        static void Main()
        {
            int[] a = { 1, 2, 3, 4, 5 };
            MyThread mt1 = new MyThread("Поток №1", a);
            MyThread mt2 = new MyThread("Поток №2", a);
            Console.ReadLine();
        }
    }
}
