using JetBrains.Annotations;
using log4net;
using log4net.Appender;
using log4net.Repository;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace BackendsCommon.Logging
{
	public class Logger
	{
		private log4net.ILog _Logger = null;
		private const string LOGGER_CONFIG_FILE = "log4net.config";

		private static readonly Logger _Instance = new Logger();
		public static Logger Instance
		{
			get
			{
				return _Instance;
			}
		}

		public bool IsInfoEnabled
		{
			get { return _Logger.IsInfoEnabled; }
		}


		private Logger()
		{
			string configPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), LOGGER_CONFIG_FILE);
			if (File.Exists(configPath))
			{
				log4net.Config.XmlConfigurator.Configure(new FileInfo(configPath));
			}
			else
			{
				log4net.Config.XmlConfigurator.Configure();
			}
			_Logger = LogManager.GetLogger(typeof(LogManager).FullName);
			_Logger.Info("Logger initialized");
			AppDomain.CurrentDomain.UnhandledException += LogUnhandledException;
		}

		/// <summary>
		/// Log simple information to a logger storage
		/// </summary>
		/// <param name="message">Message to be logged</param>
		public void Info(string message)
		{
			_Logger.Info(message);
		}


		/// <summary>
		/// Log simple information to a logger storage
		/// </summary>
		/// <param name="message">Message to be logged</param>
		public void Info(object message)
		{
			_Logger.Info(message);
		}

		/// <summary>
		/// Log information with specified formatting to a logger storage
		/// </summary>
		/// <param name="msg">Message to be logged</param>
		/// <param name="args">argument for string formatting</param>
		[StringFormatMethod("msg")]
		public void Info(string msg, params object[] args)
		{
			_Logger.InfoFormat(msg, args);
		}

		/// <summary>
		/// Log simple warning  to a logger storage
		/// </summary>
		/// <param name="message">Message to be logged</param>
		public void Warn(string message)
		{
			_Logger.Warn(message);
		}

		public void Warn(object message)
		{
			_Logger.Warn(message);
		}

		/// <summary>
		/// Log warning with specified formatting to a logger storage
		/// </summary>
		/// <param name="message">Message to be logged</param>
		/// <param name="args">argument for string formatting</param>
		[StringFormatMethod("message")]
		public void Warn(string message, params object[] args)
		{
			_Logger.WarnFormat(message, args);
		}

		/// <summary>
		/// Log fatal error to a logger storage
		/// </summary>
		/// <param name="message">Message to be logged</param>
		/// <param name="exc">Exception to be logged</param>
		public void Fatal(string message, Exception exc)
		{
			_Logger.Fatal(message, exc);
		}

		/// <summary>
		/// Log fatal error to a logger storage
		/// </summary>
		/// <param name="message">Message to be logged</param>
		public void Fatal(string message)
		{
			_Logger.Fatal(message, null);
		}

		/// <summary>
		/// Log fatal error to a logger storage
		/// </summary>
		/// <param name="message">Message to be logged</param>
		/// <param name="args">argument for string formatting</param>
		[StringFormatMethod("message")]
		public void Fatal(string message, params object[] args)
		{
			_Logger.FatalFormat(message, args);
		}

		/// <summary>
		/// Log error to a logger storage
		/// </summary>
		/// <param name="message">Message to be logged</param>
		/// <param name="exc">Exception to be logged</param>
		public void Error(string msg, Exception exc)
		{
			_Logger.Error(msg, exc);
		}

		/// <summary>
		/// Log error to a logger storage
		/// </summary>
		/// <param name="message">Message to be logged</param>
		/// <param name="exc">Exception to be logged</param>
		[StringFormatMethod("msg")]
		public void Error(string msg, params object[] msgs)
		{
			_Logger.ErrorFormat(msg, msgs);
		}

		/// <summary>
		/// Log error to a logger storage
		/// </summary>
		/// <param name="message">Message to be logged</param>
		/// <param name="exc">Exception to be logged</param>
		public void Error(Exception exc)
		{
			_Logger.Error(string.Empty, exc);
		}

		/// <summary>
		/// Log error to a logger storage
		/// </summary>
		/// <param name="message">Message to be logged</param>
		public void Error(string msg)
		{
			_Logger.Error(msg, null);
		}

		/// <summary>
		/// Log player session details to a logger storage
		/// </summary>
		/// <param name="message">Message to be logged</param>
		/// <param name="exc">Exception to be logged</param>
		public void LogPlayerDetails(string UUID, long playerId, long gameSessionId, long gameId)
		{
			this._Logger.InfoFormat("ID={0}, P1={1}, GS= {2}, G={3}",
									string.IsNullOrEmpty(UUID) ? "	" : UUID,
									playerId == 0 ? "	" : playerId.ToString(),
									gameSessionId == 0 ? "	" : gameSessionId.ToString(),
									gameId == 0 ? "	" : gameId.ToString());
		}

		private void FlushBuffers()
		{
			ILoggerRepository rep = LogManager.GetRepository();
			foreach (IAppender appender in rep.GetAppenders())
			{
				var buffered = appender as BufferingAppenderSkeleton;
				if (buffered != null)
				{
					buffered.Flush();
				}
			}
		}

		private void LogUnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			Exception exception = e.ExceptionObject as Exception;
			Fatal("Service raised unhandled exception", exception);
			var process = Process.GetCurrentProcess();
			Fatal("Memory info:{0}", new
			{
				WorkingSet64Mb = process.WorkingSet64 >> 20,
				PeakWorkingSet64Mb = process.PeakWorkingSet64 >> 20,
				PagedMemorySize64Mb = process.PagedMemorySize64 >> 20,
				PeakPagedMemorySize64Mb = process.PeakPagedMemorySize64 >> 20
			});
			if (e.IsTerminating)
			{
				Fatal("Service is terminating");
			}
			FlushBuffers();
		}
	}
}
