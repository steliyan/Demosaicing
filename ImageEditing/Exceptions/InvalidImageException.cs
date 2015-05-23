using System;

namespace ImageEditing.Exceptions
{
	public class InvalidImageException : ArgumentException
	{
		public InvalidImageException()
		{ }

		public InvalidImageException(string message)
			: base(message)
		{ }

		public InvalidImageException(string message, string paramName)
			: base(message, paramName)
		{ }
	}
}
