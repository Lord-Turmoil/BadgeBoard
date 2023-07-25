// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.
// Licensed under the BSD 2-Clause License.

namespace BadgeBoard.Api.Modules.BadgeAccount.Services.Utils;

public static class CodeUtil
{
    private const string CodeCharSet = "AaBbCcDdEeFfGgHhIiJjKkLlMmNnOoPpQqRrSsTtUuVvWwXxYyZz0123456789";
    private const string LowerAlphaSet = "abcdefghijklmnopqrstuvwxyz";
    private const string UpperAlphaSet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private const string DigitSet = "0123456789";


    public static string GenerateCode(int length)
    {
        return _GenerateCode(length, CodeCharSet);
    }


    public static string GenerateCode(int length, int options)
    {
        var charset = string.Empty;
        if ((options & Options.Upper) != 0)
            charset += UpperAlphaSet;
        else if ((options & Options.Lower) != 0)
            charset += LowerAlphaSet;
        else if ((options & Options.Digit) != 0) charset += DigitSet;

        return _GenerateCode(length, charset);
    }


    private static string _GenerateCode(int length, string charset)
    {
        var upper = charset.Length;
        if (upper == 0) return string.Empty;

        var code = string.Empty;
        for (var i = 0; i < length; i++) code += CodeCharSet[(int)Random.Shared.NextInt64(upper)];

        return code;
    }


    public static class Options
    {
        public const int Upper = 1;
        public const int Lower = 2;
        public const int Digit = 4;
    }
}