using System;

namespace ImageEditing.Exceptions
{
	public class InvalidPixelFormatException : ArgumentException
	{
		public InvalidPixelFormatException()
		{ }

		public InvalidPixelFormatException(string message)
			: base(message)
		{ }

		public InvalidPixelFormatException(string message, string paramName)
			: base(message, paramName)
		{ }
	}
}
