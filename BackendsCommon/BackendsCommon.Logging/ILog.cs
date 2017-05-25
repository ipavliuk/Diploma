using System;
using JetBrains.Annotations;

namespace BackendsCommon.Logging
{
	public interface ILog
	{
		bool IsDebugEnabled { get; }


		bool IsInfoEnabled { get; }


		bool IsWarnEnabled { get; }


		bool IsErrorEnabled { get; }


		bool IsFatalEnabled { get; }


		void Debug(object message);


		void Debug(object message, Exception exception);

		[StringFormatMethod("format")]
		void DebugFormat(string format, params object[] args);

		[StringFormatMethod("format")]
		void DebugFormat(string format, object arg0);

		[StringFormatMethod("format")]
		void DebugFormat(string format, object arg0, object arg1);

		[StringFormatMethod("format")]
		void DebugFormat(string format, object arg0, object arg1, object arg2);

		[StringFormatMethod("format")]
		void DebugFormat(IFormatProvider provider, string format, params object[] args);


		void Info(object message);


		void Info(object message, Exception exception);


		[StringFormatMethod("format")]
		void InfoFormat(string format, params object[] args);

		[StringFormatMethod("format")]
		void InfoFormat(string format, object arg0);

		[StringFormatMethod("format")]
		void InfoFormat(string format, object arg0, object arg1);

		[StringFormatMethod("format")]
		void InfoFormat(string format, object arg0, object arg1, object arg2);

		[StringFormatMethod("format")]
		void InfoFormat(IFormatProvider provider, string format, params object[] args);


		void Warn(object message);


		void Warn(object message, Exception exception);


		[StringFormatMethod("format")]
		void WarnFormat(string format, params object[] args);

		[StringFormatMethod("format")]
		void WarnFormat(string format, object arg0);

		[StringFormatMethod("format")]
		void WarnFormat(string format, object arg0, object arg1);

		[StringFormatMethod("format")]
		void WarnFormat(string format, object arg0, object arg1, object arg2);

		[StringFormatMethod("format")]
		void WarnFormat(IFormatProvider provider, string format, params object[] args);


		void Error(object message);


		void Error(object message, Exception exception);

		[StringFormatMethod("format")]
		void ErrorFormat(string format, params object[] args);

		[StringFormatMethod("format")]
		void ErrorFormat(string format, object arg0);

		[StringFormatMethod("format")]
		void ErrorFormat(string format, object arg0, object arg1);

		[StringFormatMethod("format")]
		void ErrorFormat(string format, object arg0, object arg1, object arg2);

		[StringFormatMethod("format")]
		void ErrorFormat(IFormatProvider provider, string format, params object[] args);


		void Fatal(object message);


		void Fatal(object message, Exception exception);

		[StringFormatMethod("format")]
		void FatalFormat(string format, params object[] args);

		[StringFormatMethod("format")]
		void FatalFormat(string format, object arg0);

		[StringFormatMethod("format")]
		void FatalFormat(string format, object arg0, object arg1);

		[StringFormatMethod("format")]
		void FatalFormat(string format, object arg0, object arg1, object arg2);

		[StringFormatMethod("format")]
		void FatalFormat(IFormatProvider provider, string format, params object[] args);
	}
}
