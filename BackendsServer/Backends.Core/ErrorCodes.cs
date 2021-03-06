﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backends.Core
{
	public enum BacksErrorCodes
	{
		Ok = 0,
		SystemError = 1,
		AuthFailed = 2,
		OperationIsNotSupported = 3,
		InvalidCredentials = 4,
		SessionIsNotFound = 5,
		NotAllMandatFields = 6,
		DuplicateLogin = 7,
		ProjectCreationFailed =8,
		SignUpError = 9,
		UserIsNotFound = 10,
		EntityNotFound = 11

	}
}
