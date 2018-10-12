using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FitnessGuru_Main.Models;
using SendGrid;
using SendGrid.Helpers.Mail;
using SendGrid.Helpers.Mail.Model;
using WebGrease.Css.Extensions;

namespace FitnessGuru_Main.utils
{
    public class EmailSender
    {
        private const String API_KEY = "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";

        //public void Send2(String toEmail, String subject, String contents)
        //{
        //    var client = new SendGridClient(API_KEY);
        //    var from = new EmailAddress("noreply@fitnessguru.com", "Fitness Guru Customer Relationship Hub");
        //    var to = new EmailAddress(toEmail, "");
        //    var plainTextContent = contents;
        //    var htmlContent = "<p>" + contents + "</p>";
        //    var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
        //    var response = client.SendEmailAsync(msg);
        //}


        public void Send(String toEmail, String name, String type, Session sessionInfo)
        {
            var HtmlContent = "";
            var Content = "";
            var Subject = "";
;           switch (type)
            {
                case "RegistrationSuccess":
                    HtmlContent = "<div style=\"background-color:black; margin:10px; padding: 10px; width:100%; text-align:center\"> " +
                                  "<h2 style=\"color:white\">We are super excited to have you on board!!!</h2></div>" +
                                  "<p>Dear " + name + ", <br><br>" +
                                  EmailBody.WelcomeUser + "</p>";
                    Content = "Dear " + name +
                              ", " + EmailBody.WelcomeUser;

                    Subject = "Registration Successful";
                    break;

                case "JoinedSession":
                    var session = ((Session) sessionInfo);
                    var sessionHtml = "Session Name: " + session.SessionName + "<br>" +
                                      "Session At: " + session.SessionAt + "<br>" +
                                      "Trainer: " + session.GymMember.FirstName + "<br>" +
                                      "Session Desc: " + session.Desc + "<br>";

                    HtmlContent = "<div style=\"background-color:black; margin:10px; padding: 10px; width:100%; text-align:center\"> " +
                                  "<h2 style=\"color:white\">Session Join Successful</h2></div>" +
                                  "<p>Dear " + name + ", <br><br>" +
                                  EmailBody.SessionJoinNotification+ "</p>" +
                                  "<br><br><h3>Session Details</h3>" + 
                                  "<p>" + sessionHtml + "</p>" + "<br>Looking forward to meet you at session!!!";
                    Content = "Dear " + name +
                              ", " + EmailBody.SessionJoinNotification +
                              "<br>Session Details" +
                              "<br>" + sessionHtml +
                              "<br>Looking forward to meet you at session!!!";

                    Subject = "Successfully Joined Session " + session.SessionName;
                    break;

                case "OptOutSession":
                    session = ((Session)sessionInfo);
                    sessionHtml = "Session Name: " + session.SessionName + "<br>" +
                                      "Session At: " + session.SessionAt + "<br>" +
                                      "Trainer: " + session.GymMember.FirstName + "<br>" +
                                      "Session Desc: " + session.Desc + "<br>";

                    HtmlContent = "<div style=\"background-color:black; margin:10px; padding: 10px; width:100%; text-align:center\"> " +
                                  "<h2 style=\"color:white\">Opted Out from Session</h2></div>" +
                                  "<p>Dear " + name + ", <br><br>" +
                                  EmailBody.SessionOptOutNotification + "</p>" +
                                  "<br><br><h3>Session Details</h3>" +
                                  "<p>" + sessionHtml + "</p>" + "<br>";
                    Content = "Dear " + name +
                              ", " + EmailBody.SessionOptOutNotification +
                              "<br>Session Details" +
                              "<br>" + sessionHtml +
                              "<br>";

                    Subject = "Successfully Opted Out from Session " + session.SessionName;
                    break;


                case "SessionDelete":
                    session = ((Session)sessionInfo);
                    sessionHtml = "Session Name: " + session.SessionName + "<br>" +
                                  "Session At: " + session.SessionAt + "<br>" +
                                  "Trainer: " + session.GymMember.FirstName + "<br>" +
                                  "Session Desc: " + session.Desc + "<br>";

                    HtmlContent = "<div style=\"background-color:black; margin:10px; padding: 10px; width:100%; text-align:center\"> " +
                                  "<h2 style=\"color:white\">Session Cancelled</h2></div>" +
                                  "<p>Dear " + name + ", <br><br>" +
                                  EmailBody.SessionCancelledOrDeletedNotification + "</p>" +
                                  "<br><br><h3>Session Details</h3>" +
                                  "<p>" + sessionHtml + "</p>" + "<br>";
                    Content = "Dear " + name +
                              ", " + EmailBody.SessionCancelledOrDeletedNotification +
                              "<br>Session Details" +
                              "<br>" + sessionHtml +
                              "<br>";

                    Subject = "Session " + session.SessionName + " has been Cancelled/Removed";
                    break;

                case "SessionEdit":
                    session = ((Session)sessionInfo);
                    sessionHtml = "Session Name: " + session.SessionName + "<br>" +
                                  "Session At: " + session.SessionAt + "<br>" +
                                  "Trainer: " + session.GymMember.FirstName + "<br>" +
                                  "Session Desc: " + session.Desc + "<br>";

                    HtmlContent = "<div style=\"background-color:black; margin:10px; padding: 10px; width:100%; text-align:center\"> " +
                                  "<h2 style=\"color:white\">Session Updated</h2></div>" +
                                  "<p>Dear " + name + ", <br><br>" +
                                  EmailBody.SessionEditNotification + "</p>" +
                                  "<br><br><h3>Session Details</h3>" +
                                  "<p>" + sessionHtml + "</p>" + "<br>";
                    Content = "Dear " + name +
                              ", " + EmailBody.SessionEditNotification +
                              "<br>Updated Session Details" +
                              "<br>" + sessionHtml +
                              "<br>";

                    Subject = "Session " + session.SessionName + " has been Updated";
                    break;


                case "SessionCreate":
                    session = ((Session)sessionInfo);
                    sessionHtml = "Session Name: " + session.SessionName + "<br>" +
                                  "Session At: " + session.SessionAt + "<br>" +
                                  "Trainer: " + session.GymMember.FirstName + "<br>" +
                                  "Session Desc: " + session.Desc + "<br>";

                    HtmlContent = "<div style=\"background-color:black; margin:10px; padding: 10px; width:100%; text-align:center\"> " +
                                  "<h2 style=\"color:white\">Session Created</h2></div><br>" +
                                  "<p>Dear " + name + ", <br><br>" +
                                  EmailBody.SessionCreateNotification + "</p>" +
                                  "<br><br><h3>Session Details</h3>" +
                                  "<p>" + sessionHtml + "</p>" + "<br>";
                    Content = "<br>Dear " + name +
                              ", " + EmailBody.SessionCreateNotification +
                              "<br>New Session Details" +
                              "<br>" + sessionHtml +
                              "<br>";

                    Subject = "New Session created : " + session.SessionName ;
                    break;


            }
            var client = new SendGridClient(API_KEY);
            var from = new EmailAddress("noreply@fitnessguru.com", "Fitness Guru Customer Relationship Hub");
            var to = new EmailAddress(toEmail, "");
            var plainTextContent = Content;
            var msg = MailHelper.CreateSingleEmail(from, to, Subject, plainTextContent, HtmlContent);
            var response = client.SendEmailAsync(msg);

        }
    }

    public static class EmailBody
    {
        public const string WelcomeUser = "We are super excited to have you onboard. You can now access our website to plan your activities without any hazzle.. " +
                                           "Please feel free to reach out to us for any queries, suggestions or feedback. " +
                                           "Once again... welcome on board!";

        public const string SessionCreateNotification = "New session introduced at gym. Please find the details below.";

        public const string SessionJoinNotification = "You have successfully joined the session";

        public const string SessionOptOutNotification = "You have successfully opted out from the session";

        public const string SessionEditNotification = "Session updated. Please find the updated details below";

        public const string SessionCancelledOrDeletedNotification =
            "We regret to inform you that the session has been cancelled. Please plan accordingly.";


    }
}