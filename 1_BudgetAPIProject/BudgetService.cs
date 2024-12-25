namespace BudgetAPIProject;

public class BudgetService
{
    private readonly IBudgetRepo _repo;

    public BudgetService(IBudgetRepo repo)
    {
        _repo = repo;
    }
    public decimal Query(DateTime StartDate, DateTime EndDate)
    {
        var allData = _repo.GetAll();
        var getRange = GetYearMonthsInRange(StartDate, EndDate);
        //[202410,202411]
        var rangeData = allData.Where(x => getRange.Contains(x.YearMonth));
        
        var MonthDayDic = GetDaysInEachMonth(StartDate, EndDate);

        decimal summaryAmount = 0.0m;
        foreach (var data in getRange)
        {
            var daysContain = MonthDayDic[data];
            
            var monthOfDay = CountDay(StartDate);
            // all mon amount
            var monthlyAmount = rangeData.FirstOrDefault(x => x.YearMonth == data).Amount;
            
            var dayAmount = (decimal)(monthlyAmount / monthOfDay * daysContain);
            
            summaryAmount += dayAmount;
        }
        
        return summaryAmount;
    }

    public static List<string> GetYearMonthsInRange(DateTime startTime, DateTime endTime)
    {
        List<string> yearMonths = new List<string>();

        DateTime current = new DateTime(startTime.Year, startTime.Month, 1);
        DateTime end = new DateTime(endTime.Year, endTime.Month, 1);

        while (current <= end)
        {
            string yearMonth = current.ToString("yyyyMM"); // 格式化為 YYYYMM
            yearMonths.Add(yearMonth);
            current = current.AddMonths(1);
        }

        return yearMonths;
    }
    public int CountDay(DateTime time)
    {
        var daysInMonth = DateTime.DaysInMonth(time.Year, time.Month);
        return daysInMonth;
        
    }
    
    static Dictionary<string, int> GetDaysInEachMonth(DateTime startTime, DateTime endTime)
    {
        Dictionary<string, int> monthDays = new Dictionary<string, int>();

        DateTime current = new DateTime(startTime.Year, startTime.Month, 1);
        DateTime end = new DateTime(endTime.Year, endTime.Month, 1);

        while (current <= end)
        {
            string yearMonth = current.ToString("yyyyMM");

            // 計算該月的開始和結束範圍
            DateTime firstDayOfMonth = current;
            DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            // 計算有效區間的天數
            DateTime actualStart = (current.Month == startTime.Month && current.Year == startTime.Year) 
                ? startTime 
                : firstDayOfMonth;

            DateTime actualEnd = (current.Month == endTime.Month && current.Year == endTime.Year) 
                ? endTime 
                : lastDayOfMonth;

            int days = (actualEnd - actualStart).Days + 1;
            monthDays[yearMonth] = days;

            // 移動到下一個月
            current = current.AddMonths(1);
        }

        return monthDays;
    }
}