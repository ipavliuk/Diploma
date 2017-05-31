using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backends.Core.Extension
{
	public static class DictionaryExtensions
	{
		public static void CreateNewOrUpdateExisting<TKey, TValue>(this IDictionary<TKey, TValue> map, TKey key, TValue value)
		{
			map[key] = value;
		}
	}
}
