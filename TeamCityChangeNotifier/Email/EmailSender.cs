﻿using System;
using System.Net.Mail;
using TeamCityChangeNotifier.Helpers;
using TeamCityChangeNotifier.Models;

namespace TeamCityChangeNotifier.Email
{
	public class EmailSender
	{
		private static readonly ConfigSettings settings = new ConfigSettings();
		private const string Sender = "TeamcityChanges@7digital.com";

		public void SendNotification(ChangeSet changeSet)
		{
			MailMessage mail = new MailMessage(Sender, settings.DestinationEmail);
			mail.Subject = changeSet.Summary();
			mail.Body = changeSet.Details();

			SmtpClient client = new SmtpClient();
			client.Host = settings.SmtpHost;
			client.Send(mail);

			Console.WriteLine("Sent notification to " + settings.DestinationEmail);
		}
	}
}
