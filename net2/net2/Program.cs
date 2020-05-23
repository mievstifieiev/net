using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace net2
{

    class Program
    {
        static EventWaitHandle wh_b1 = new AutoResetEvent(false);
        static EventWaitHandle wh_b2 = new AutoResetEvent(false);
        static EventWaitHandle whb2 = new AutoResetEvent(false);
        static EventWaitHandle whac4 = new AutoResetEvent(false);
        static EventWaitHandle wha21 = new AutoResetEvent(false);
        static EventWaitHandle wha22 = new AutoResetEvent(false);
        static EventWaitHandle whd = new AutoResetEvent(false);
        static EventWaitHandle whdsqrt1 = new AutoResetEvent(false);
        static EventWaitHandle whdsqrt2 = new AutoResetEvent(false);
        static EventWaitHandle whx1 = new AutoResetEvent(false);
        static EventWaitHandle whx2 = new AutoResetEvent(false);


        static double a = 1, b = 5, c = 6;
        static double _b, b2, ac4, d, dsqrt, a2, x1, x2;
        static void Main()
        {
            Thread t1 = new Thread(_B);
            Thread t2 = new Thread(B2);
            Thread t3 = new Thread(AC4);
            Thread t4 = new Thread(D);
            Thread t5 = new Thread(DSQRT);
            Thread t6 = new Thread(A2);
            Thread t7 = new Thread(X1);
            Thread t8 = new Thread(X2);
            t1.Start();
            t2.Start();
            t3.Start();
            t4.Start();
            t5.Start();
            t6.Start();
            t7.Start();
            t8.Start();


            whx1.WaitOne();
            whx2.WaitOne();
            Console.WriteLine("x1 = " + x1.ToString() + "; x2 = " + x2.ToString());
            Console.ReadLine();
        }
        static void _B()
        {
            _b = -b;
            wh_b1.Set();
            wh_b2.Set();
            Console.WriteLine("-b done " + _b);
        }
        static void B2()
        {
            b2 = b * b;
            whb2.Set();
            Console.WriteLine("b^2 done " + b2);
        }

        static void AC4()
        {
            ac4 = a * c * 4;
            whac4.Set();
            Console.WriteLine("a*c*4 done " + ac4);
        }
        
        static void D()
        {
            whb2.WaitOne();
            whac4.WaitOne();
            d = b2 - ac4;
            whd.Set();
            Console.WriteLine("D done" + d);
        }

        static void DSQRT()
        {
            whd.WaitOne();
            dsqrt = Math.Sqrt(d);
            whdsqrt1.Set();
            whdsqrt2.Set();
            Console.WriteLine("DSQRT done " + dsqrt);
        }

        static void A2()
        {
            a2 = a*2;
            wha21.Set();
            wha22.Set();
            Console.WriteLine("a^2 done " + a2);
        }

        static void X1()
        {
            wh_b1.WaitOne();
            whdsqrt1.WaitOne();
            wha21.WaitOne();
            x1 = (_b + dsqrt)/a2;
            whx1.Set();
            Console.WriteLine("x1 done");
        }

        static void X2()
        {
            wh_b2.WaitOne();
            whdsqrt2.WaitOne();
            wha22.WaitOne();
            x2 = (_b - dsqrt) / a2;
            whx2.Set();
            Console.WriteLine("x2 done");
        }
    }
}
