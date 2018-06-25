using System;

namespace Muflone.CommonDomain
{
    internal static class StringExtensions
	{
		public static Guid ToGuid(this string value)
		{
		    Guid.TryParse(value, out var guid);
			return guid;
		}
	}
}