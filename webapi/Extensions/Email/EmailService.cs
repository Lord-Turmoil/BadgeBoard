// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.
// Licensed under the BSD 2-Clause License.

using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace BadgeBoard.Api.Extensions.Email
{
	public class EmailService : IEmailService
	{
		private readonly EmailOptions _settings;

		public EmailService(IOptions<EmailOptions> settings)
		{
			_settings = settings.Value;
		}

		public async Task SendEmailAsync(EmailRequest request)
		{
			var email = new MimeMessage();

			email.From.Add(MailboxAddress.Parse(_settings.Email));
			email.Sender = MailboxAddress.Parse(_settings.Email);
			email.To.Add(MailboxAddress.Parse(request.Receiver));
			email.Subject = request.Subject;

			var builder = new BodyBuilder {
				HtmlBody = request.Body
			};
			email.Body = builder.ToMessageBody();
			using var smtp = new SmtpClient();
			await smtp.ConnectAsync(_settings.Host, _settings.Port, SecureSocketOptions.StartTls);
			await smtp.AuthenticateAsync(_settings.Email, _settings.Password);
			await smtp.SendAsync(email);
			await smtp.DisconnectAsync(true);
		}
	}
}
