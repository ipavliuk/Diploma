using System;
using System.Globalization;
using log4net;
using log4net.Core;
using log4net.Util;

namespace BackendsCommon.Logging
{
	public class Log : ILog
	{
		private static readonly Type ThisDeclaringType = typeof(Log);
		private static readonly Level LevelDebug = Level.Debug;
		private static readonly Level LevelError = Level.Error;
		private static readonly Level LevelFatal = Level.Fatal;
		private static readonly Level LevelInfo = Level.Info;
		private static readonly Level LevelWarn = Level.Warn;
		private readonly ILogger _logger;

		static Log()
		{
			//force old system to XmlConfigurator.Configure
			Logger.Instance.GetHashCode();
		}

		public Log(string fullName)
		{
			_logger = LogManager.GetLogger(fullName).Logger;
		}

		public Log(Type type)
		{
			_logger = LogManager.GetLogger(type).Logger;
		}

		#region ILog Members

		public bool IsDebugEnabled
		{
			get { return _logger.IsEnabledFor(LevelDebug); }
		}


		public bool IsInfoEnabled
		{
			get { return _logger.IsEnabledFor(LevelInfo); }
		}

		public bool IsWarnEnabled
		{
			get { return _logger.IsEnabledFor(LevelWarn); }
		}


		public bool IsErrorEnabled
		{
			get { return _logger.IsEnabledFor(LevelError); }
		}

		public bool IsFatalEnabled
		{
			get { return _logger.IsEnabledFor(LevelFatal); }
		}

		public void Debug(object message)
		{
			_logger.Log(ThisDeclaringType, LevelDebug, message, null);
		}

		public void Debug(object message, Exception exception)
		{
			_logger.Log(ThisDeclaringType, LevelDebug, message, exception);
		}

		public void DebugFormat(string format, params object[] args)
		{
			if (!IsDebugEnabled)
				return;
			_logger.Log(ThisDeclaringType, LevelDebug, new SystemStringFormat(CultureInfo.InvariantCulture, format, args), null);
		}

		public void DebugFormat(string format, object arg0)
		{
			if (!IsDebugEnabled)
				return;
			_logger.Log(ThisDeclaringType, LevelDebug, new SystemStringFormat(CultureInfo.InvariantCulture, format, new[]
			{
				arg0
			}), null);
		}

		public void DebugFormat(string format, object arg0, object arg1)
		{
			if (!IsDebugEnabled)
				return;
			_logger.Log(ThisDeclaringType, LevelDebug, new SystemStringFormat(CultureInfo.InvariantCulture, format, new[]
			{
				arg0,
				arg1
			}), null);
		}

		public void DebugFormat(string format, object arg0, object arg1, object arg2)
		{
			if (!IsDebugEnabled)
				return;
			_logger.Log(ThisDeclaringType, LevelDebug, new SystemStringFormat(CultureInfo.InvariantCulture, format, new[]
			{
				arg0,
				arg1,
				arg2
			}), null);
		}

		public void DebugFormat(IFormatProvider provider, string format, params object[] args)
		{
			if (!IsDebugEnabled)
				return;
			_logger.Log(ThisDeclaringType, LevelDebug, new SystemStringFormat(provider, format, args), null);
		}

		public void Info(object message)
		{
			_logger.Log(ThisDeclaringType, LevelInfo, message, null);
		}

		public void Info(object message, Exception exception)
		{
			_logger.Log(ThisDeclaringType, LevelInfo, message, exception);
		}

		public void InfoFormat(string format, params object[] args)
		{
			if (!IsInfoEnabled)
				return;
			_logger.Log(ThisDeclaringType, LevelInfo, new SystemStringFormat(CultureInfo.InvariantCulture, format, args), null);
		}

		public void InfoFormat(string format, object arg0)
		{
			if (!IsInfoEnabled)
				return;
			_logger.Log(ThisDeclaringType, LevelInfo, new SystemStringFormat(CultureInfo.InvariantCulture, format, new[]
			{
				arg0
			}), null);
		}

		public void InfoFormat(string format, object arg0, object arg1)
		{
			if (!IsInfoEnabled)
				return;
			_logger.Log(ThisDeclaringType, LevelInfo, new SystemStringFormat(CultureInfo.InvariantCulture, format, new[]
			{
				arg0,
				arg1
			}), null);
		}

		public void InfoFormat(string format, object arg0, object arg1, object arg2)
		{
			if (!IsInfoEnabled)
				return;
			_logger.Log(ThisDeclaringType, LevelInfo, new SystemStringFormat(CultureInfo.InvariantCulture, format, new[]
			{
				arg0,
				arg1,
				arg2
			}), null);
		}

		public void InfoFormat(IFormatProvider provider, string format, params object[] args)
		{
			if (!IsInfoEnabled)
				return;
			_logger.Log(ThisDeclaringType, LevelInfo, new SystemStringFormat(provider, format, args), null);
		}

		public void Warn(object message)
		{
			_logger.Log(ThisDeclaringType, LevelWarn, message, null);
		}

		public void Warn(object message, Exception exception)
		{
			_logger.Log(ThisDeclaringType, LevelWarn, message, exception);
		}

		public void WarnFormat(string format, params object[] args)
		{
			if (!IsWarnEnabled)
				return;
			_logger.Log(ThisDeclaringType, LevelWarn, new SystemStringFormat(CultureInfo.InvariantCulture, format, args), null);
		}

		public void WarnFormat(string format, object arg0)
		{
			if (!IsWarnEnabled)
				return;
			_logger.Log(ThisDeclaringType, LevelWarn, new SystemStringFormat(CultureInfo.InvariantCulture, format, new[]
			{
				arg0
			}), null);
		}

		public void WarnFormat(string format, object arg0, object arg1)
		{
			if (!IsWarnEnabled)
				return;
			_logger.Log(ThisDeclaringType, LevelWarn, new SystemStringFormat(CultureInfo.InvariantCulture, format, new[]
			{
				arg0,
				arg1
			}), null);
		}

		public void WarnFormat(string format, object arg0, object arg1, object arg2)
		{
			if (!IsWarnEnabled)
				return;
			_logger.Log(ThisDeclaringType, LevelWarn, new SystemStringFormat(CultureInfo.InvariantCulture, format, new[]
			{
				arg0,
				arg1,
				arg2
			}), null);
		}

		public void WarnFormat(IFormatProvider provider, string format, params object[] args)
		{
			if (!IsWarnEnabled)
				return;
			_logger.Log(ThisDeclaringType, LevelWarn, new SystemStringFormat(provider, format, args), null);
		}

		public void Error(object message)
		{
			_logger.Log(ThisDeclaringType, LevelError, message, null);
		}

		public void Error(object message, Exception exception)
		{
			_logger.Log(ThisDeclaringType, LevelError, message, exception);
		}

		public void ErrorFormat(string format, params object[] args)
		{
			if (!IsErrorEnabled)
				return;
			_logger.Log(ThisDeclaringType, LevelError, new SystemStringFormat(CultureInfo.InvariantCulture, format, args), null);
		}

		public void ErrorFormat(string format, object arg0)
		{
			if (!IsErrorEnabled)
				return;
			_logger.Log(ThisDeclaringType, LevelError, new SystemStringFormat(CultureInfo.InvariantCulture, format, new[]
			{
				arg0
			}), null);
		}

		public void ErrorFormat(string format, object arg0, object arg1)
		{
			if (!IsErrorEnabled)
				return;
			_logger.Log(ThisDeclaringType, LevelError, new SystemStringFormat(CultureInfo.InvariantCulture, format, new[]
			{
				arg0,
				arg1
			}), null);
		}

		public void ErrorFormat(string format, object arg0, object arg1, object arg2)
		{
			if (!IsErrorEnabled)
				return;
			_logger.Log(ThisDeclaringType, LevelError, new SystemStringFormat(CultureInfo.InvariantCulture, format, new[]
			{
				arg0,
				arg1,
				arg2
			}), null);
		}

		public void ErrorFormat(IFormatProvider provider, string format, params object[] args)
		{
			if (!IsErrorEnabled)
				return;
			_logger.Log(ThisDeclaringType, LevelError, new SystemStringFormat(provider, format, args), null);
		}

		public void Fatal(object message)
		{
			_logger.Log(ThisDeclaringType, LevelFatal, message, null);
		}

		public void Fatal(object message, Exception exception)
		{
			_logger.Log(ThisDeclaringType, LevelFatal, message, exception);
		}

		public void FatalFormat(string format, params object[] args)
		{
			if (!IsFatalEnabled)
				return;
			_logger.Log(ThisDeclaringType, LevelFatal, new SystemStringFormat(CultureInfo.InvariantCulture, format, args), null);
		}

		public void FatalFormat(string format, object arg0)
		{
			if (!IsFatalEnabled)
				return;
			_logger.Log(ThisDeclaringType, LevelFatal, new SystemStringFormat(CultureInfo.InvariantCulture, format, new[]
			{
				arg0
			}), null);
		}

		public void FatalFormat(string format, object arg0, object arg1)
		{
			if (!IsFatalEnabled)
				return;
			_logger.Log(ThisDeclaringType, LevelFatal, new SystemStringFormat(CultureInfo.InvariantCulture, format, new[]
			{
				arg0,
				arg1
			}), null);
		}

		public void FatalFormat(string format, object arg0, object arg1, object arg2)
		{
			if (!IsFatalEnabled)
				return;
			_logger.Log(ThisDeclaringType, LevelFatal, new SystemStringFormat(CultureInfo.InvariantCulture, format, new[]
			{
				arg0,
				arg1,
				arg2
			}), null);
		}

		public void FatalFormat(IFormatProvider provider, string format, params object[] args)
		{
			if (!IsFatalEnabled)
				return;
			_logger.Log(ThisDeclaringType, LevelFatal, new SystemStringFormat(provider, format, args), null);
		}

		#endregion
	}
}
