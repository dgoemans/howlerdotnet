
using System;
using Jayrock.Json;

namespace twitster
{
	
	
	public class Helpers
	{
		
        public static DateTime ParseDateTime(string date)
        {
            //string dayOfWeek = date.Substring(0, 3).Trim();
            string month = date.Substring(4, 3).Trim();
            string dayInMonth = date.Substring(8, 2).Trim();
            string time = date.Substring(11, 9).Trim();
            //string offset = date.Substring(20, 5).Trim();
            string year = date.Substring(25, 5).Trim();
            string dateTime = string.Format("{0}-{1}-{2} {3}", dayInMonth, month, year, time);
            DateTime ret = DateTime.Parse(dateTime);
            return ret;
        }
		
		public static void NullCheck( object o )
		{
			if( o == null )
				throw new NullReferenceException("Object passed is null: " + o.ToString());
		}
		
		public static bool ParseJSONBool( object toParse, out bool result )
		{
			try
			{
				result = ((JsonBoolean) toParse ).Equals( JsonBoolean.TrueText );
				return true;
			}
			catch( Exception e )
			{
				Console.WriteLine("Could not parse value from JSON: ", e.Message );
				result = false;
				return false;
			}
		}
		
		public static bool ParseJSONLong( object toParse, out long result )
		{
			try
			{
				result = ((JsonNumber) toParse ).ToInt64();
				return true;
			}
			catch( Exception e )
			{
				Console.WriteLine("Could not parse value from JSON: ", e.Message );
				result = 0;
				return false;
			}
		}

		public static bool ParseJSONInt( object toParse, out int result )
		{
			try
			{
				result = ((JsonNumber) toParse ).ToInt32();
				return true;
			}
			catch( Exception e )
			{
				Console.WriteLine("Could not parse value from JSON: ", e.Message );
				result = 0;
				return false;
			}
		}
		
	}
}
