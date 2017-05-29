using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace BackendsCommon.Types
{
	
	public class EntitiesSchema : ICollection<KeyValuePair<string, string>>//: IEnumerable<KeyValuePair<string, string>>
    {
        public EntitiesSchema()
        {
            ColumnTypeMapping = new Dictionary<string, string>();
        }

		public Dictionary<string, string> ColumnTypeMapping { get; set; }

        public int Count
        {
            get
            {
                return ColumnTypeMapping.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public void Add(KeyValuePair<string, string> item)
        {
            ColumnTypeMapping.Add(item.Key, item.Value);
        }

        public void Clear()
        {
            ColumnTypeMapping.Clear();
        }

        public bool Contains(KeyValuePair<string, string> item)
        {
            return ColumnTypeMapping.ContainsKey(item.Key);
        }

        public void CopyTo(KeyValuePair<string, string>[] array, int arrayIndex)
        {
            foreach (var kvp in ColumnTypeMapping)
            {
                array[arrayIndex] = new KeyValuePair<string, string>(kvp.Key, kvp.Value);
                arrayIndex++;
            }
        }

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
		{
			return ColumnTypeMapping.GetEnumerator();
		}

        public bool Remove(KeyValuePair<string, string> item)
        {
            return ColumnTypeMapping.Remove(item.Key);
        }

        IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}
	}
}
