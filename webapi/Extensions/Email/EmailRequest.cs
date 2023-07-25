// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.
// Licensed under the BSD 2-Clause License.

namespace BadgeBoard.Api.Extensions.Email
{
	public class EmailRequest
	{
		public string Receiver { get; set; } = string.Empty;
		public string Subject { get; set; } = string.Empty;
		public string Body { get; set; } = string.Empty;
	}

	public class EmailRequestBuilder
	{
		private EmailRequest request;

		public EmailRequestBuilder()
		{
			request = new EmailRequest();
		}

		public EmailRequestBuilder SetReceiver(string email)
		{
			request.Receiver = email;
			return this;
		}

		public EmailRequestBuilder SetSubject(string subject)
		{
			request.Subject = subject;
			return this;
		}

		public EmailRequestBuilder SetBody(string body)
		{
			request.Body = body;
			return this;
		}

		public EmailRequest Build()
		{
			return request;
		}
	}
}
