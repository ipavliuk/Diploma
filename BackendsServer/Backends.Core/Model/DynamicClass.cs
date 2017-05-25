using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Backends.Core.Model
{
	public class Field
	{
		public string FieldName;
		public string FieldType;
	}

	public class DynamicClass : DynamicObject
	{
		private Dictionary<string, KeyValuePair<Type, object>> _fields;

		public DynamicClass(List<Field> fields)
		{
			_fields = new Dictionary<string, KeyValuePair<Type, object>>();
			fields.ForEach(x => _fields.Add(x.FieldName,
				new KeyValuePair<Type, object>(TypeDescriptor.GetConverter(x.FieldType).GetType(), null)));
			//TypeDescriptor.GetConverter(x.FieldType).ConvertFromString(inputValue);
		}

		public override IEnumerable<string> GetDynamicMemberNames()
		{
			return _fields.Keys;
		}
		public override bool TrySetMember(SetMemberBinder binder, object value)
		{
			if (_fields.ContainsKey(binder.Name))
			{
				var type = _fields[binder.Name].Key;
				if (value.GetType() == type)
				{
					_fields[binder.Name] = new KeyValuePair<Type, object>(type, value);
					return true;
				}
				else throw new Exception("Value " + value + " is not of type " + type.Name);
			}
			return false;
		}

		public override bool TryGetMember(GetMemberBinder binder, out object result)
		{
			result = _fields[binder.Name].Value;
			return true;
		}

		//public DynamicMetaObject GetMetaObject(Expression parameter)
		//{
		//    return new DynamicEntryMetaObject(parameter, this);
		//}

		private class DynamicEntryMetaObject : DynamicMetaObject
		{
			internal DynamicEntryMetaObject(
				Expression parameter,
				DynamicClass value)
				: base(parameter, BindingRestrictions.Empty, value)
			{
			}

			public override DynamicMetaObject BindGetMember(GetMemberBinder binder)
			{
				var methodInfo = typeof(DynamicClass).GetMethod("GetEntryValue", BindingFlags.Instance | BindingFlags.NonPublic);
				var arguments = new Expression[]
				{
					Expression.Constant(binder.Name)
				};
				Expression objectExpression = Expression.Call(
					Expression.Convert(Expression, LimitType),
					methodInfo, arguments);

				return new DynamicMetaObject(
					objectExpression,
					BindingRestrictions.GetTypeRestriction(Expression, this.RuntimeType));
			}
		}
	}
}
