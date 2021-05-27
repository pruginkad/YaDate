using System;
using System.Collections.Generic;

namespace YaDate
{
    class Program
    {
        static void ReturnError()
        {
            Console.WriteLine("0");
            Console.WriteLine("");
        }
        class Period
        {
            public DateTime date1 = new DateTime();
            public DateTime date2 = new DateTime();
        }

        static void MainProcess(string sPeriodType, string sPeriod)
        {
            var arDates = sPeriod.Split(' ');
            if (arDates.Length < 2)
            {
                ReturnError();
                return;
            }


            List<Period> listOfPeriods = new List<Period>();

            //2020-01-10 2020-03-25
            string format = "yyyy-MM-dd";
            DateTime startDate;
            DateTime endDate;
            try
            {
                startDate = DateTime.ParseExact(arDates[0], format, null);
                endDate = DateTime.ParseExact(arDates[1], format, null);

            }
            catch (Exception)
            {
                ReturnError();
                return;
            }

            if (sPeriodType == "WEEK")
            {
                listOfPeriods = ProcessWEEK(startDate, endDate);
            }
            if (sPeriodType == "MONTH")
            {
                listOfPeriods = ProcessMONTH(startDate, endDate);
            }

            Console.WriteLine(listOfPeriods.Count);
            for (var i = 0; i < listOfPeriods.Count; i++)
            {
                var d1 = listOfPeriods[i].date1;
                var d2 = listOfPeriods[i].date2;
                Console.WriteLine($"{d1.ToString(format)} {d2.ToString(format)}");
            }
        }

        static void CheckIfEndOfPeriodCorrect(ref Period temp_pair, DateTime endDate)
        {
            if (temp_pair.date2 >= endDate)
            {
                temp_pair.date2 = endDate;
            }
        }
        static List<Period> ProcessMONTH(DateTime startDate, DateTime endDate)
        {
            DateTime curDate = new DateTime();

            curDate = startDate;
            List<Period> listOfPeriods = new List<Period>();
            //Find last day of first month
            Period temp_pair = new Period();
            temp_pair.date1 = curDate;
            temp_pair.date2 = new DateTime(curDate.Year, curDate.Month, DateTime.DaysInMonth(curDate.Year, curDate.Month));
            CheckIfEndOfPeriodCorrect(ref temp_pair, endDate);

            listOfPeriods.Add(temp_pair);
            curDate = temp_pair.date2;
            while (curDate < endDate)
            {
                temp_pair = new Period();
                temp_pair.date1 = curDate.AddDays(1);
                temp_pair.date2 = curDate.AddMonths(1);
                
                CheckIfEndOfPeriodCorrect(ref temp_pair, endDate);
                
                listOfPeriods.Add(temp_pair);
                curDate = temp_pair.date2;
            }
            return listOfPeriods;
        }
        static List<Period> ProcessWEEK(DateTime startDate, DateTime endDate)
        {
            DateTime curDate = new DateTime();

            curDate = startDate;
            List<Period> listOfPeriods = new List<Period>();
            //Find last day of week
            
            Period temp_pair = new Period();
            temp_pair.date1 = curDate;
            var DaysTillSunday = 7 - (int)temp_pair.date1.DayOfWeek;
            if(DaysTillSunday == 7)
            {
                DaysTillSunday = 0;
            }
            temp_pair.date2 = temp_pair.date1.AddDays(DaysTillSunday);
            CheckIfEndOfPeriodCorrect(ref temp_pair, endDate);

            listOfPeriods.Add(temp_pair);
            curDate = temp_pair.date2;

            while (curDate < endDate)
            {
                temp_pair = new Period();
                temp_pair.date1 = curDate.AddDays(1);
                temp_pair.date2 = curDate.AddDays(7);
                CheckIfEndOfPeriodCorrect(ref temp_pair, endDate);
                listOfPeriods.Add(temp_pair);
                curDate = temp_pair.date2;
            }
            return listOfPeriods;
        }
        static void Main(string[] args)
        {
            //sPeriodType = Console.ReadLine();
            //sPeriod = Console.ReadLine();

            //string sPeriodType = "MONTH";
            //string sPeriod = "2020-01-10 2020-03-25";
            
            string sPeriodType = "WEEK";
            string sPeriod = "2020-01-26 2020-03-23";

            MainProcess(sPeriodType, sPeriod);
        }
    }
}
