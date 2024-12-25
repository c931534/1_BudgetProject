namespace BudgetAPIProject;

public class BudgetService(IBudgetRepo repo)
{
    public decimal Query(DateTime startDate, DateTime endDate)
    {
        var allData = repo.GetAll();
        var getRange = GetYearMonthsInRange(startDate, endDate);
        //[202410,202411]
        var rangeData = allData.Where(x => getRange.Contains(x.YearMonth));
        
        var monthDayDic = GetDaysInEachMonth(startDate, endDate);

        var summaryAmount = 0.0m;
        foreach (var data in getRange)
        {
            var daysContain = monthDayDic[data];
            
            var monthOfDay = CountDay(startDate);
            // all mon amount
            var monthlyAmount = rangeData.FirstOrDefault(x => x.YearMonth == data).Amount;
            
            var dayAmount = (decimal)(monthlyAmount / monthOfDay * daysContain);
            
            summaryAmount += dayAmount;
        }
        
        return summaryAmount;
    }

    private static List<string> GetYearMonthsInRange(DateTime startTime, DateTime endTime)
    {
        var yearMonths = new List<string>();

        var current = new DateTime(startTime.Year, startTime.Month, 1);
        var end = new DateTime(endTime.Year, endTime.Month, 1);

        while (current <= end)
        {
            var yearMonth = current.ToString("yyyyMM");
            yearMonths.Add(yearMonth);
            current = current.AddMonths(1);
        }

        return yearMonths;
    }

    private int CountDay(DateTime time)
    {
        var daysInMonth = DateTime.DaysInMonth(time.Year, time.Month);
        return daysInMonth;
    }
    
    static Dictionary<string, int> GetDaysInEachMonth(DateTime startTime, DateTime endTime)
    {
        var monthDays = new Dictionary<string, int>();

        var current = new DateTime(startTime.Year, startTime.Month, 1);
        var end = new DateTime(endTime.Year, endTime.Month, 1);

        while (current <= end)
        {
            var yearMonth = current.ToString("yyyyMM");

            // 計算該月的開始和結束範圍
            var firstDayOfMonth = current;
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            // 計算有效區間的天數
            var actualStart = (current.Month == startTime.Month && current.Year == startTime.Year) 
                ? startTime 
                : firstDayOfMonth;

            var actualEnd = (current.Month == endTime.Month && current.Year == endTime.Year) 
                ? endTime 
                : lastDayOfMonth;

            var days = (actualEnd - actualStart).Days + 1;
            monthDays[yearMonth] = days;

            // 移動到下一個月
            current = current.AddMonths(1);
        }

        return monthDays;
    }
}