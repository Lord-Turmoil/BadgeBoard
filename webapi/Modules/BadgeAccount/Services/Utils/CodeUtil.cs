﻿namespace BadgeBoard.Api.Modules.BadgeAccount.Services.Utils
{
    public static class CodeUtil
    {
        private static readonly string CODE_CHAR_SET = "AaBbCcDdEeFfGgHhIiJjKkLlMmNnOoPpQqRrSsTtUuVvWwXxYyZz0123456789";

        public static string GenerateCode(int length)
        {
            string code = string.Empty;
            int upper = CODE_CHAR_SET.Length;

            for (int i = 0; i < length; i++)
            {
                code += CODE_CHAR_SET[(int)Random.Shared.NextInt64(upper)];
            }

            return code;
        }
    }
}