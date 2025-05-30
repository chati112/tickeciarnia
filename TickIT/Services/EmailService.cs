﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TickIT.Services
{
    internal class EmailService
    {
        public static void SendNewPasswordEmail(string recipientEmail, string newPassword)
        {
            string smtpHost = "smtp.gmail.com"; // Adres serwera SMTP
            int smtpPort = 587; // Port serwera SMTP 
            string smtpUsername = "Przychodnia2137@gmail.com"; //  e-mail
            string smtpPassword = "lsjp ajxr rmqf iovh"; // Hasło do  e-mail

            using (SmtpClient client = new SmtpClient(smtpHost, smtpPort))
            {
                client.EnableSsl = true; // Włączenie SSL
                client.Credentials = new NetworkCredential(smtpUsername, smtpPassword);

                using (MailMessage message = new MailMessage(smtpUsername, recipientEmail))
                {
                    message.Subject = "Twoje konto w TickIT już tu jest !";
                    message.Body = $"Twoje konto w aplikacji TickIT zostało utworzone !\n By w pełni korzystać z aplikacji zaloguj się używając swojego służbowego adresu e-mail i tymczasowego hasła znajdującego się poniżej." +
                        $"" +
                        $" Twoje tymczasowe hasło to: {newPassword}\nZalecamy zmianę hasła po zalogowaniu.";

                    try
                    {
                        client.Send(message);
                        Console.WriteLine("E-mail z tymczasowym hasłem został wysłany.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Wystąpił błąd podczas wysyłania e-maila: {ex.Message}");
                    }
                }
            }
        }

        public static void SendTicketResolvedEmail(string recipientEmail, string userName, string ticketTitle)
        {
            string smtpHost = "smtp.gmail.com";
            int smtpPort = 587;
            string smtpUsername = "Przychodnia2137@gmail.com";
            string smtpPassword = "lsjp ajxr rmqf iovh";

            using (SmtpClient client = new SmtpClient(smtpHost, smtpPort))
            {
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential(smtpUsername, smtpPassword);

                using (MailMessage message = new MailMessage(smtpUsername, recipientEmail))
                {
                    message.Subject = "Zgłoszenie zostało rozwiązane - TickIT";
                    message.Body = $"Dzień dobry {userName},\n\n" +
                                   $"Twoje zgłoszenie o tytule: \"{ticketTitle}\" zostało oznaczone jako rozwiązane.\n" +
                                   
                                   $"Dziękujemy za skorzystanie z naszego systemu wsparcia.\n\nZespół TickIT.";

                    try
                    {
                        client.Send(message);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Błąd podczas wysyłania maila: {ex.Message}");
                    }
                }
            }
        }



    }
}
