using GoodBytes.Infrastructure.Utils.Interfaces;
using GoodBytes.Infrastructure.Utils.Models;
using SendGrid.Helpers.Mail;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Threading.Tasks;

namespace GoodBytes.Infrastructure.Utils.Services
{
	public class Emailer : IEmailer
	{
		private string GetKey()
		{
			var ENV = ConfigurationManager.AppSettings["ENV"];
			if (ENV == "DEV" || ENV == null)
				return ConfigurationManager.AppSettings["SendGridApiKeyDev"];
			return ConfigurationManager.AppSettings["SendGridApiKey"];
		}

		public async Task<HttpStatusCode> SendEmailsContacts(List<ContactsModel> contacts, string username)
		{
			dynamic sg = new SendGrid.SendGridAPIClient(GetKey());

			var mail = new Mail();
			Personalization personalization;

			foreach (var contact in contacts)
			{
				personalization = new Personalization();
				Email to = new Email();

				if (!string.IsNullOrEmpty(contact.Name))
					to.Name = contact.Name;
				else
				{
					var atIndex = contact.Email.IndexOf('@');
					to.Name = contact.Email.Substring(0, atIndex);
				}
				to.Address = contact.Email;
				personalization.AddTo(to);
				personalization.AddSubstitution("%invited%", to.Name);
				personalization.AddSubstitution("%username%", username);
				//personalization.AddCustomArgs("marketing", "false");
				//personalization.AddCustomArgs("transactional", "true");
				mail.AddPersonalization(personalization);
			}

			var body = @"
			<p>
			Hello %invited%,
			</p>
			<p>&nbsp;</p>
			<p>
			Your friend %username%, has invited you to join him on our community.
			</p>";

			mail.AddContent(new Content("text/html", body));
			mail.From = new Email(ConfigurationManager.AppSettings["DomainEmail"], ConfigurationManager.AppSettings["DomainName"]);
			mail.Subject = "%invited%, special Invitation to join";

			dynamic response = await sg.client.mail.send.post(requestBody: mail.Get());
			return (HttpStatusCode)response.StatusCode;
			//System.Diagnostics.Trace.WriteLine(response.Body.ReadAsStringAsync().Result);
			//System.Diagnostics.Trace.WriteLine(response.Headers.ToString());
			//System.Diagnostics.Trace.WriteLine(mail.Get());
		}

		public async Task<HttpStatusCode> SendNotification(string email, string subject, string template)
		{
			dynamic sg = new SendGrid.SendGridAPIClient(GetKey());

			Email from = new Email(ConfigurationManager.AppSettings["DomainEmail"], ConfigurationManager.AppSettings["DomainName"]);
			Email to = new Email(email);
			Content content = new Content("text/html", template);
			Mail mail = new Mail(from, subject, to, content);

			dynamic response = await sg.client.mail.send.post(requestBody: mail.Get());
			return (HttpStatusCode)response.StatusCode;
		}
	}
}
