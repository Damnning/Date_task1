using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Date_task1.Models
{
    public enum DayOfWeekEnum
    {
        Sunday, Monday, Tuesday, Wednesday, Thursday, Friday, Saturday
    }

    public class DateModel
    {
        public int Day { get; private set; }
        public int Month { get; private set; }
        public int Year { get; private set; }
        public DayOfWeekEnum DayOfWeek { get; private set; }

        private static readonly int[] DaysInMonth = { 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };

        public DateModel(int day, int month, int year)
        {
            if (!IsValidDate(day, month, year))
                throw new ArgumentException("Invalid date!");

            Day = day;
            Month = month;
            Year = year;
            DayOfWeek = CalculateDayOfWeek(day, month, year);
        }

        public static bool IsValidDate(int day, int month, int year)
        {
            if (month < 1 || month > 12 || day < 1 || year < 1)
                return false;

            int daysInMonth = GetDaysInMonth(month, year);
            return day <= daysInMonth;
        }

        public static int GetDaysInMonth(int month, int year)
        {
            if (month == 2 && IsLeapYear(year))
                return 29;
            return DaysInMonth[month - 1];
        }

        public void AddDays(int days)
        {
            while (days > 0)
            {
                int daysInCurrentMonth = GetDaysInMonth(Month, Year);
                if (Day + days > daysInCurrentMonth)
                {
                    days -= (daysInCurrentMonth - Day + 1);
                    Day = 1;
                    AddMonths(1);
                }
                else
                {
                    Day += days;
                    days = 0;
                }
            }
            DayOfWeek = CalculateDayOfWeek(Day, Month, Year);
        }

        public void AddMonths(int months)
        {
            int newMonth = Month + months;
            Year += (newMonth - 1) / 12;
            Month = (newMonth - 1) % 12 + 1;

            int maxDay = GetDaysInMonth(Month, Year);
            if (Day > maxDay)
                Day = maxDay;

            DayOfWeek = CalculateDayOfWeek(Day, Month, Year);
        }

        public void AddYears(int years)
        {
            Year += years;

            int maxDay = GetDaysInMonth(Month, Year);
            if (Day > maxDay)
                Day = maxDay;

            DayOfWeek = CalculateDayOfWeek(Day, Month, Year);
        }

        private static bool IsLeapYear(int year)
        {
            return (year % 4 == 0 && year % 100 != 0) || (year % 400 == 0);
        }

        private static DayOfWeekEnum CalculateDayOfWeek(int day, int month, int year)
        {
            int[] monthCodes = { 0, 3, 3, 6, 1, 4, 6, 2, 5, 0, 3, 5 };
            int centuryCode = (year / 100) % 4 * 2;
            int yearCode = (year % 100 + (year % 100) / 4) % 7;
            int leapYearCorrection = (month < 3 && IsLeapYear(year)) ? -1 : 0;

            int dayOfWeek = (day + monthCodes[month - 1] + yearCode + centuryCode + leapYearCorrection) % 7;
            return (DayOfWeekEnum)dayOfWeek;
        }
    }


}
