// YDateCPP.cpp : This file contains the 'main' function. Program execution begins and ends there.
//
#define _CRT_SECURE_NO_WARNINGS

#include <iostream>
#include <string>
#include <vector>
#include <ctime>
#include <locale>
#include <sstream>
#include <iomanip>

struct Period
{
    std::tm date1;
    std::tm date2;
};

typedef std::vector<Period> vecPeriods;

static void ReturnError()
{
    std::cout << "0\n";
    std::cout << "\n";
}

static bool StartDateLessThanEnd(std::tm startDate, std::tm endDate)
{
    std::time_t start = std::mktime(&startDate);
    std::time_t end = std::mktime(&endDate);
    double aux = std::difftime(end, start);
    if (aux > 0)
    {
        return true;
    }
    return false;
}

static void CheckIfEndOfPeriodCorrect(Period& temp_pair, std::tm endDate)
{
    std::time_t start = std::mktime(&temp_pair.date2);
    std::time_t end = std::mktime(&endDate);
    double aux = std::difftime(start, end);
    if (aux >= 0)
    {
        temp_pair.date2 = endDate;
    }
}

static void Normalize(std::tm& curDate)
{
    time_t next = mktime(&curDate);
    std::tm* date = localtime(&next);
    auto stack = *date;
    curDate = stack;
}

static std::tm AddDays(std::tm curDate, int days)
{
    curDate.tm_mday += days;
    time_t next = mktime(&curDate);
    std::tm* date = localtime(&next);
    auto stack = *date;
    return stack;
}

static std::tm WEEKEndDate(std::tm curDate)
{
    int DaysTillSunday = 7 - (int)curDate.tm_wday;
    if (DaysTillSunday == 7)
    {
        DaysTillSunday = 0;
    }
    
    std::tm date = AddDays(curDate, DaysTillSunday);
    return date;
}

typedef std::tm (*PeriodProcessingDelegate)(std::tm curDate);

static void ProcessPeriod(vecPeriods& listOfPeriods, std::tm startDate, std::tm endDate, PeriodProcessingDelegate func)
{

    std::tm curDate;

    curDate = startDate;

    Period temp_pair;
    temp_pair.date1 = curDate;
    temp_pair.date2 = func(curDate);

    CheckIfEndOfPeriodCorrect(temp_pair, endDate);

    listOfPeriods.push_back(temp_pair);
    curDate = temp_pair.date2;

    while (StartDateLessThanEnd(curDate, endDate))
    {
        Period temp_pair;
        temp_pair.date1 = AddDays(curDate, 1);
        temp_pair.date2 = func(temp_pair.date1);
        listOfPeriods.push_back(temp_pair);
        curDate = temp_pair.date2;
    }
    if (listOfPeriods.size() > 1)
    {
        //trim last period
        CheckIfEndOfPeriodCorrect(listOfPeriods.back(), endDate);
    }
}

static void MainProcess(std::string sPeriodType, std::string sPeriod)
{
    size_t pos = 0;
    std::string arDates[2];
    std::string sEndDate;
    for(int i = 0; i < 2; i++)
    {
        if ((pos = sPeriod.find(' ')) == std::string::npos)
        {
            ReturnError();
            return;
        }
        
        arDates[0] = sPeriod.substr(0, pos);
        arDates[1] = sPeriod.substr(pos + 1);
        
    }


    vecPeriods listOfPeriods;

    //2020-01-10 2020-03-25
    std::string format = " %Y-%m-%d";
    std::tm startDate = { 0 };
    std::tm endDate = { 0 };
    try
    {
        std::istringstream ss(arDates[0]);
        ss >> std::get_time(&startDate, format.c_str());
        std::istringstream ss1(arDates[1]);
        ss1 >> std::get_time(&endDate, format.c_str());
    }
    catch (std::exception)
    {
        ReturnError();
        return;
    }

    Normalize(startDate);
    Normalize(endDate);
    if (sPeriodType == "WEEK")
    {
        ProcessPeriod(listOfPeriods, startDate, endDate, WEEKEndDate);
    }
    //if (sPeriodType == "MONTH")
    //{
    //    listOfPeriods = ProcessPeriod(startDate, endDate, MONTHEndDate);
    //}
    //if (sPeriodType == "REVIEW")
    //{
    //    listOfPeriods = ProcessPeriod(startDate, endDate, REVIEWEndDate);
    //}
    //if (sPeriodType == "YEAR")
    //{
    //    listOfPeriods = ProcessPeriod(startDate, endDate, YEAREndDate);
    //}
    //if (sPeriodType == "QUARTER")
    //{
    //    listOfPeriods = ProcessPeriod(startDate, endDate, QUARTEREndDate);
    //}

    std::cout << listOfPeriods.size();
    std::cout << "\n";
    for (auto i = 0; i < listOfPeriods.size(); i++)
    {
        auto d1 = listOfPeriods[i].date1;
        auto d2 = listOfPeriods[i].date2;
        std::ostringstream oss;
        oss << std::put_time(&listOfPeriods[i].date1, format.c_str());

        std::ostringstream oss1;
        oss1 << std::put_time(&listOfPeriods[i].date2, format.c_str());
        //format = "yyyy-MM-dd";
        std::cout << oss.str() << " " << oss1.str() << "\n";
    }
}
int main()
{
    
    std::string sPeriodType;
    std::string sPeriod;

    /*std::getline(std::cin, sPeriodType);
    std::getline(std::cin, sPeriod);*/

    //string sPeriodType = "MONTH";
    //string sPeriod = "2020-01-10 2020-03-25";

    sPeriodType = "WEEK";
    sPeriod = "2020-01-26 2020-03-23";


    //string sPeriodType = "REVIEW";
    //string sPeriod = "2016-09-20 2022-11-30";

    //string sPeriodType = "YEAR";
    //string sPeriod = "2016-09-20 2022-11-30";

    //string sPeriodType = "QUARTER";
    //string sPeriod = "2016-09-20 2022-11-30";

    MainProcess(sPeriodType, sPeriod);
}

// Run program: Ctrl + F5 or Debug > Start Without Debugging menu
// Debug program: F5 or Debug > Start Debugging menu

// Tips for Getting Started: 
//   1. Use the Solution Explorer window to add/manage files
//   2. Use the Team Explorer window to connect to source control
//   3. Use the Output window to see build output and other messages
//   4. Use the Error List window to view errors
//   5. Go to Project > Add New Item to create new code files, or Project > Add Existing Item to add existing code files to the project
//   6. In the future, to open this project again, go to File > Open > Project and select the .sln file
