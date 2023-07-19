using System.Runtime.Serialization;

namespace BadgeBoard.Api.Modules.BadgeGlobal.Exceptions
{
	public class MissingReferenceException : Exception
	{
		public MissingReferenceException(string message) : base(message) { }
	}
}
