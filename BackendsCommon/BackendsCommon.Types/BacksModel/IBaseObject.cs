using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendsCommon.Types
{
	interface IBaseObject : IEnumerable<KeyValuePair<string, object>>
	{
		string Name { get;}
		string Id { get; }
		DateTime? CreatedAt { get; }
		DateTime? UpdatedAt { get; }
		string AppId { get;}

		object this[string key] { get; }
		bool ContainsKey(string key);
	}
}
