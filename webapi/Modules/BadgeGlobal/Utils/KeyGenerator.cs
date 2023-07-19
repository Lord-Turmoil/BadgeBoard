namespace BadgeBoard.Api.Modules.BadgeGlobal.Utils
{
	public static class KeyGenerator
	{
		private const int MinUserId = 1000000000;
		private const int MaxUserId = 2000000000;

		public static int GenerateKey()
		{
			return (int)Random.Shared.NextInt64(MinUserId, MaxUserId + 1);
		}
	}
}
