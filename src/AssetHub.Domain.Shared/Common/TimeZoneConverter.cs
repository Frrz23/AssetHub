using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetHub.Common
{
    public interface ITimeZoneConverter
    {
        DateTime ConvertToUserTime(DateTime utcTime);
    }

    public class TimeZoneConverter : ITimeZoneConverter
    {
        private readonly TimeZoneInfo _userTimeZone;

        public TimeZoneConverter()
        {
            // Windows: "Nepal Standard Time", Linux: "Asia/Kathmandu"
            _userTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Asia/Kathmandu");
        }

        public DateTime ConvertToUserTime(DateTime utcTime)
        {
            return TimeZoneInfo.ConvertTimeFromUtc(DateTime.SpecifyKind(utcTime, DateTimeKind.Utc), _userTimeZone);
        }
    }
}
