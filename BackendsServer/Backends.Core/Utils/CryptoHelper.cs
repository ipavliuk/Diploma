using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Backends.Core.Utils
{
	public static class CryptoHelper
	{
		public static string CreateMD5Hash(this string value, string salt = null)
		{
			string saltedValue = (salt ?? string.Empty) + value;

			byte[] buffer = Encoding.UTF8.GetBytes(saltedValue);

			using (var md5 = new MD5CryptoServiceProvider())
			{
				return string.Join("", md5.ComputeHash(buffer)
										.Select(b => b.ToString("x2")));
			}

		}
	}
}
