// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.
// Licensed under the BSD 2-Clause License.

using BadgeBoard.Api.Extensions.Email;
using BadgeBoard.Api.Modules.BadgeAccount.Services.Utils;
using Microsoft.Extensions.Options;

namespace BadgeBoard.Api.Modules.BadgeUser.Services.Utils;

public static class EmailUtil
{
	private const string REGISTER_HTML = "./Templates/register.html";
	private const string RETRIEVE_HTML = "./Templates/retrieve.html";
	private const int DEFAULT_CODE_LENGTH = 6;

	public static async Task SendRegistrationEmailAsync(IServiceProvider provider, string email, string subject)
	{
		var builder = new EmailRequestBuilder()
			.SetReceiver(email)
			.SetSubject(subject);
		var code = CodeUtil.GenerateCode(DEFAULT_CODE_LENGTH);
		var html = File.ReadAllText(REGISTER_HTML);
		builder.SetBody(html.Replace("{{code}}", code));

		var request = builder.Build();
		var service = new EmailService(provider.GetRequiredService<IOptions<EmailOptions>>());

		await service.SendEmailAsync(request);
	}

	public static async void SendRetrievalEmailAsync(IServiceProvider provider, string email, string subject)
	{
		var builder = new EmailRequestBuilder()
			.SetReceiver(email)
			.SetSubject(subject);
		var code = CodeUtil.GenerateCode(DEFAULT_CODE_LENGTH);
		var html = File.ReadAllText(RETRIEVE_HTML);
		builder.SetBody(html.Replace("{{code}}", code));

		var request = builder.Build();
		var service = new EmailService(provider.GetRequiredService<IOptions<EmailOptions>>());

		await service.SendEmailAsync(request);
	}
}