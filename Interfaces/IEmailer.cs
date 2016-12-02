using GoodBytes.Infrastructure.Utils.Models;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace GoodBytes.Infrastructure.Utils.Interfaces
{
	public interface IEmailer
	{
		Task<HttpStatusCode> SendNotification(string email, string subject, string template);
		Task<HttpStatusCode> SendEmailsContacts(List<ContactsModel> contacts, string username);
	}
}