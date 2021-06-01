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
                listOfPeriods = ProcessPeriod(startDate, endDate, WEEKEndDate);
            }
            if (sPeriodType == "MONTH")
            {
                listOfPeriods = ProcessPeriod(startDate, endDate, MONTHEndDate);
            }
            if (sPeriodType == "REVIEW")
            {
                listOfPeriods = ProcessPeriod(startDate, endDate, REVIEWEndDate);
            }
            if (sPeriodType == "YEAR")
            {
                listOfPeriods = ProcessPeriod(startDate, endDate, YEAREndDate);
            }
            if (sPeriodType == "QUARTER")
            {
                listOfPeriods = ProcessPeriod(startDate, endDate, QUARTEREndDate);
            }
            
            Console.WriteLine(listOfPeriods.Count);
            for (var i = 0; i < listOfPeriods.Count; i++)
            {
                var d1 = listOfPeriods[i].date1;
                var d2 = listOfPeriods[i].date2;
                //format = "yyyy-MM-dd";
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

        static DateTime MONTHEndDate(DateTime curDate)
        {
            DateTime date = new DateTime(curDate.Year, curDate.Month, DateTime.DaysInMonth(curDate.Year, curDate.Month));
            return date;
        }

        static DateTime WEEKEndDate(DateTime curDate)
        {
            var DaysTillSunday = 7 - (int)curDate.DayOfWeek;
            if (DaysTillSunday == 7)
            {
                DaysTillSunday = 0;
            }
            DateTime date = curDate.AddDays(DaysTillSunday);
            return date;
        }
 
        static DateTime REVIEWEndDate(DateTime curDate)
        {
            //REVIEW — периоды, за которые оцениваются достижения сотрудников Яндекса. 
            //Летний период длится с 1 апреля по 30 сентября, зимний — с 1 октября по 31 марта.
            DateTime date2;
            if (curDate.Month >= 4 && curDate.Month <= 9)
            {//summer time
                date2 = new DateTime(curDate.Year, 9, 30);//September
            }
            else
            { //Winter time
                int year = curDate.Month <= 12 ? curDate.Year + 1 : curDate.Year;
                date2 = new DateTime(year, 3, 31);//March
            }
            return date2;
        }

        static DateTime YEAREndDate(DateTime curDate)
        {
            //YEAR — год c 1 января по 31 декабря.
            DateTime date = new DateTime(curDate.Year, 12, 31); ;
            return date;
        }

        static DateTime QUARTEREndDate(DateTime curDate)
        {
            //интервалы в три месяца: январь — март, апрель — июнь, июль — сентябрь, октябрь — декабрь.
            //Спионерил в интернете
            int quarterNumber = (curDate.Month - 1) / 3 + 1;
            DateTime firstDayOfQuarter = new DateTime(curDate.Year, (quarterNumber - 1) * 3 + 1, 1);
            DateTime lastDayOfQuarter = firstDayOfQuarter.AddMonths(3).AddDays(-1);
            DateTime date = lastDayOfQuarter;
            return date;
        }

        delegate DateTime PeriodProcessingDelegate(DateTime curDate);
        static List<Period> ProcessPeriod(DateTime startDate, DateTime endDate, PeriodProcessingDelegate func)
        {
            
            DateTime curDate = new DateTime();

            curDate = startDate;
            List<Period> listOfPeriods = new List<Period>();

            Period temp_pair = new Period();
            temp_pair.date1 = curDate;
            temp_pair.date2 = func(curDate);

            CheckIfEndOfPeriodCorrect(ref temp_pair, endDate);

            listOfPeriods.Add(temp_pair);
            curDate = temp_pair.date2;

            while (curDate < endDate)
            {
                temp_pair = new Period();
                temp_pair.date1 = curDate.AddDays(1);
                temp_pair.date2 = func(temp_pair.date1);
                listOfPeriods.Add(temp_pair);
                curDate = temp_pair.date2;
            }
            if(listOfPeriods.Count > 1)
            {
                //trim last period
                CheckIfEndOfPeriodCorrect(ref temp_pair, endDate);
            }
            

            return listOfPeriods;
        }
        static void Main(string[] args)
        {
            //sPeriodType = Console.ReadLine();
            //sPeriod = Console.ReadLine();

            //string sPeriodType = "MONTH";
            //string sPeriod = "2020-01-10 2020-03-25";

            //string sPeriodType = "WEEK";
            //string sPeriod = "2020-01-26 2020-03-23";
            
            //string sPeriodType = "REVIEW";
            //string sPeriod = "2016-09-20 2022-11-30";
            
            //string sPeriodType = "YEAR";
            //string sPeriod = "2016-09-20 2022-11-30";

            string sPeriodType = "QUARTER";
            string sPeriod = "2016-09-20 2022-11-30";

            MainProcess(sPeriodType, sPeriod);
        }
    }
}
