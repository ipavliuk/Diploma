using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backends.Core.Model
{
	/*internal enum BacksDataType
	{
		BString = 1,
		BInt = 2,
		BTime = 3,
		BObj = 4
	}*/
	public static class BacksDataType
	{
		public const string BString = "string";
		public const string BInt = "integer";
		public const string BTime = "time";
		public const string BObject = "object";
		public const string BPointer = "pointer";
		public const string BBoolean = "boolean";

		/*public static string ToBacksType(Type type)
		{
			string sType = null;
			switch (type.Name)
			{
				/*case System.String:
					sType = BString;
				case System.Int32:
					sType = BInt;

				case System.Boolean:
					sType = BBoolean;
				
			}
		}*/
	}

	
}
