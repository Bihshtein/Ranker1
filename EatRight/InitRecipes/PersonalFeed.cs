using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;


namespace InitRecipes {
    public class PersonalFeed {

        public static void SendEmail(IEnumerable<Meal> meals, string mealType) {
            var fromAddress = new MailAddress("alex_bihshtein@hotmail.com");
            string fromPassword = "99sozio#";
            string subject = mealType + " Recommendation, " + DateTime.Now.ToShortDateString();
            string body = "";
            meals.ToList().ForEach(m => body +=
            string.Format("<p><font style=\"background-color:{0};font-weight: bold;\">{1}</font></p><a href=\"http://allrecipes.com/recipe/{3}\"><img src=\"{2}\" style=\"width: 250; height: 250;\"></a>", "skyblue", m.Recipe.Name.Replace("Recipe", "Score : ") + ((int)m.Grade).ToString(), m.Recipe.ImageUrl, m.Recipe.ID.ToString()));
            body = "<!DOCTYPE html><html><body>" + body + "</html></body>";
            var smtp = new SmtpClient {
                Host = "smtp.live.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword

               )
            };
            var message = new MailMessage() {
                From = fromAddress,
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };
            message.To.Add(new MailAddress("alexbihsh@gmail.com"));
            message.To.Add(new MailAddress("uriberger@mail.tau.ac.il"));
            message.To.Add(new MailAddress(" liran.madjar@gmail.com"));

            {
                smtp.Send(message);
            }
        }
    }
}
