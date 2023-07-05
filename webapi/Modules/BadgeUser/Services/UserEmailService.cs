using BadgeBoard.Api.Extensions.Email;
using BadgeBoard.Api.Modules.BadgeAccount.Services;
using Microsoft.Extensions.Options;

namespace BadgeBoard.Api.Modules.BadgeUser.Services
{
	public class UserEmailService
	{
		private const string REGISTER_HTML = "./Templates/register.html";
		private const string RETRIEVE_HTML = "./Templates/retrieve.html";
		private const int DEFAULT_CODE_LENGTH = 6;

		private readonly IServiceProvider _provider;

		public UserEmailService(IServiceProvider provider)
		{
			_provider = provider;
		}

		public async Task SendRegistrationEmailAsync(string email, string subject)
		{
			var builder = new EmailRequestBuilder()
				.SetReceiver(email)
				.SetSubject(subject);
			var code = CodeGenerator.GenerateCode(DEFAULT_CODE_LENGTH);
			var html = File.ReadAllText(REGISTER_HTML);
			builder.SetBody(html.Replace("{{code}}", code));

			var request = builder.Build();
			var service = new EmailService(_provider.GetRequiredService<IOptions<EmailOptions>>());

			await service.SendEmailAsync(request);
		}

		public async void SendRetrieveEmail(string email, string subject)
		{
			var builder = new EmailRequestBuilder()
				.SetReceiver(email)
				.SetSubject(subject);
			var code = CodeGenerator.GenerateCode(DEFAULT_CODE_LENGTH);
			var html = File.ReadAllText(RETRIEVE_HTML);
			builder.SetBody(html.Replace("{{code}}", code));

			var request = builder.Build();
			var service = new EmailService(_provider.GetRequiredService<IOptions<EmailOptions>>());

			await service.SendEmailAsync(request);
		}
	}
}
