using System.Net.Mail;
using TeamCityChangeNotifier.Helpers;

namespace TeamCityChangeNotifier.Email
{
	public class EmailSender
	{
		private static readonly Settings settings = new Settings();
		private const string Sender = "TeamcityChanges@7digital.com";

		public void SendNotification(ChangeSet changeSet)
		{
			MailMessage mail = new MailMessage(Sender, settings.DestinationEmail);
			mail.Subject = changeSet.Summary();
			mail.Body = changeSet.Details();

			SmtpClient client = new SmtpClient();
			client.Host = settings.SmtpHost;
			client.Send(mail);
		}
	}
}
