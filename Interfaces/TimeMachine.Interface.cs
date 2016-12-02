using System;

namespace GoodBytes.Infrastructure.Utils.Interfaces
{
	public interface ITimeMachine
	{
		DateTime ConvertFromUtc(DateTime dateTime, int offsetInMinutes);
		DateTime ConvertToUtc(DateTime dateTime, int offsetInMinutes);
		DateTime ConvertToTimeZoneFromUtc(DateTime utcDateTime);
	}
}
