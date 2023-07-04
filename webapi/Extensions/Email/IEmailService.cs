namespace BadgeBoard.Api.Extensions.Email
{
	public interface IEmailService
	{
		Task SendEmailAsync(EmailRequest request);
	}
}
