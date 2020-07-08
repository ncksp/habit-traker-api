using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace Abc.HabitTracker.Api.Tracker.Service
{
    public class Scheduler
    {
        private static Timer timer;
        /*
         * Service ini hanya melakukan orkresta terharap log habit dan snapshot badge, 
         * Service ini melakukan check tiap jam 00.00 dan melakukan checking untuk hari sebelumnya
         * 
         * 
         * Service ini dipake untuk ngecek secara otomatis saat user gak melakukan log pada habit 
         * kemudian service ini juga dipakai untuk update data log dari habit dan melakukan snapshot terhadap badge
         * 
         * Service ini masih memiliki kelemahan saat aplikasi sudah mati lama, misal:
         * 
         * Pada jam 23.00 - 01.00 aplikasi ini mati, maka service tidak bisa melakukan check. 
         * 
         * Kendala tersebut harus diperbaiki kedepannya.
         * 
         */
         

        public static void Run()
        {
            DateTime nowTime = DateTime.Now;
            DateTime scheduledTime = new DateTime(nowTime.Year, nowTime.Month, (nowTime.Day+1), 0, 0, 0, 0);
            if (nowTime > scheduledTime)
            {
                scheduledTime = scheduledTime.AddDays(1);
            }

            double tickTime = (double)(scheduledTime - DateTime.Now).TotalMilliseconds;
            timer = new Timer(tickTime);
            timer.Elapsed += new ElapsedEventHandler(Timer_Elapsed);
            timer.Start();
        }

        private static void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            timer.Stop();
            Run();
        }
    }
}
