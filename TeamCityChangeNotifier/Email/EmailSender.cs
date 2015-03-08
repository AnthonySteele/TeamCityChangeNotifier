using System;
using System.Net.Mail;
using TeamCityChangeNotifier.Helpers;
using TeamCityChangeNotifier.Models;

namespace TeamCityChangeNotifier.Email
{
	public class EmailSender
	{
		private readonly ConfigSettings _settings = new ConfigSettings();

		public void SendNotification(ChangeSet changeSet)
		{
			MailMessage mail = new MailMessage(_settings.SenderEmail, _settings.DestinationEmail);
			mail.Subject = changeSet.Summary();
			mail.Body = changeSet.Details();

			SmtpClient client = new SmtpClient();
			client.Host = _settings.SmtpHost;
			client.Send(mail);

			var summaryOutput = string.Format("Sent notification to {0} from {1}", _settings.DestinationEmail, _settings.SenderEmail);
			Console.WriteLine(summaryOutput);
		}
	}
}
