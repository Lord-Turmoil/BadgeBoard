// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.
// Licensed under the BSD 2-Clause License.

namespace BadgeBoard.Api.Extensions.Email;

public interface IEmailService
{
    Task SendEmailAsync(EmailRequest request);
}