using RecommendationBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;


namespace LogicRunner {
    public class PersonalFeed {

        public static void SendEmail(IEnumerable<Meal> meals, string mealType) {
            var fromAddress = new MailAddress("alex_bihshtein@hotmail.com");
            string fromPassword = "99sozio#";
            string subject = mealType + " Recommendation, " + DateTime.Now.ToShortDateString();
            string body = "";
            meals.ToList().ForEach(m => {
                var recipeLink = "allrecipes.com/recipe/" + m.Recipe.ID.ToString();
                var shortlink = new WebClient().DownloadString(string.Format("http://wasitviewed.com/index.php?href=http%3A%2F%2F{0}&email=alex_bihshtein%40hotmail.com&notes=&bitly=bitly&nobots=nobots&submit=Generate+Link", recipeLink));
                var parts = shortlink.Split(new string[] { "bit.ly" }, StringSplitOptions.None);
                var link = new String(parts[1].TakeWhile(c => c != '\"').ToArray());
                body +=

                    string.Format(
                        "<p><font style=\"background-color:{0};font-weight: bold;\">{1}</font></p><a href=\"{3}\"><img src=\"{2}\" style=\"width: 250; height: 250;\"></a>",
                        "Beige", m.Recipe.Name.Replace("Recipe", "Score : ") + ((int)m.Grade).ToString(), m.Recipe.ImageUrl,  "http://bit.ly"+ link);
                });
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
            message.To.Add(new MailAddress("liran.madjar@gmail.com"));
            message.To.Add(new MailAddress("siukeicheung184@gmail.com"));

            {
                smtp.Send(message);
            }
        }
    }
}
