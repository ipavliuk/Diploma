using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendsCommon.Types
{
	public class EntitiesSchema : IEnumerable<KeyValuePair<string, string>>
	{
		public string Id { get; set; }

		public string AppId { get; set; }

		public Dictionary<string, string> ColumnTypeMapping { get; set; }

		public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
		{
			return ColumnTypeMapping.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}
	}
}
