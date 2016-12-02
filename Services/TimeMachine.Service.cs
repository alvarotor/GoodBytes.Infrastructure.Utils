//using System;
//using GoodBytes.Infrastructure.Utils.Interfaces;
//using NodaTime;

//namespace GoodBytes.Infrastructure.Utils.Services
//{
//    public class TimeMachine : ITimeMachine
//    {
//        private static string _timeZone;

//        public TimeMachine(string timeZone)
//        {
//            _timeZone = timeZone;
//        }

//        public DateTime ConvertFromUtc(DateTime dateTime, int offsetInMinutes)
//        {
//            Offset offset = Offset.FromMilliseconds(Milliseconds(offsetInMinutes));
//            Instant instant = Instant.FromDateTimeUtc(dateTime);
//            return instant.WithOffset(offset)
//                .LocalDateTime
//                .ToDateTimeUnspecified();
//        }

//        public DateTime ConvertToUtc(DateTime dateTime, int offsetInMinutes)
//        {
//            Offset offset = Offset.FromMilliseconds(Milliseconds(offsetInMinutes));
//            LocalDateTime localDateTime = LocalDateTime.FromDateTime(dateTime);
//            return new OffsetDateTime(localDateTime, offset).ToInstant().ToDateTimeUtc();
//        }

//        public DateTime ConvertToTimeZoneFromUtc(DateTime utcDateTime)
//        {
//            DateTimeZone timeZone = DateTimeZoneProviders.Tzdb[_timeZone];C:\Users\Alvaro\Documents\Visual Studio 2015\Projects\GoodBytes\Dlls\GoodBytes.Infrastructure.Utils\Services\TimeMachine.Service.cs
//            return Instant.FromDateTimeUtc(utcDateTime).InZone(timeZone).ToDateTimeUnspecified();
//        }

//        //public DateTime ConvertToUtcFromLocalTime(DateTime localTime)
//        //{
//        //	DateTimeZone timeZone = DateTimeZoneProviders.Tzdb[_timeZone];
//        //	Instant instant = Instant.from(dateTime);

//        //	return Instant.FromDateTimeUtc(utcDateTime).InZone(timeZone).ToDateTimeUnspecified();
//        //}

//        private int Milliseconds(int offsetInMinutes)
//        {
//            return offsetInMinutes*60*1000;
//        }
//    }
//}