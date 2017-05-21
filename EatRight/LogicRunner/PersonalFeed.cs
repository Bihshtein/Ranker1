using RecommendationBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using RestModel;


namespace LogicRunner {
    public class PersonalFeed {
        public static Dictionary<GraderType, string> GradersGroupsNames = new Dictionary<GraderType, string>() {
            {GraderType.CaloriesCountMealGrader, "exact calories" },
            {GraderType.MaxNutValuesMealGrader, "less unhealthy " },
            {GraderType.MinNutValuesMealGrader, "more nutritious " },
            {GraderType.PrepTimeMealGrader, "less preparation time" },
            {GraderType.StepsNumMealGrader, "less complicated" },
        };
        public static void SendEmail(RecommendationDB recommendationDB, IEnumerable<Meal> meals, string mealType, bool debug =false) {
            meals = meals.OrderByDescending(m => m.Grade);
            var fromAddress = new MailAddress("alex_bihshtein@hotmail.com");
            string fromPassword = "99sozio#";
            string subject = DateTime.Now.DayOfWeek.ToString() + " " + mealType + " recommendation for "+ recommendationDB.UserProfile.Name;
            var graders = recommendationDB.GradersWeight.OrderByDescending(i => i.Value).ToList();


            string body = string.Format("<div><b>Recommendation priorities are :  ");
            for (int i = 0; i < 4; i++) 
                body += GradersGroupsNames[graders[i].Key] +" - ";
            body = body.Remove(body.Length - 3, 2);
            body += "</b></div>";
            if (recommendationDB.UserProfile.Restrictions.Count > 0) {
                body += string.Format("<div><b>Your restictions are :     ");
                recommendationDB.UserProfile.Restrictions.ToList().ForEach(r => body += r.ToString() + " - ");
                body = body.Remove(body.Length - 3, 2);
                body += "<b></div>";
            }
            body += "<p><i>Press the picture to go to the recipe</i></p>";
            body += "<table style=\"width:100%\">";
            body += "<tr>";
            meals.ToList().ForEach(m => {
                body += "<td>";
                var recipeLink = m.Recipe.Urn+ m.Recipe.OriginalID.ToString();
                var shortlink = new WebClient().DownloadString(string.Format("http://wasitviewed.com/index.php?href=http%3A%2F%2F{0}&email=alex_bihshtein%40hotmail.com&notes=&bitly=bitly&nobots=nobots&submit=Generate+Link", recipeLink));
                var parts = shortlink.Split(new string[] { "bit.ly" }, StringSplitOptions.None);
                var link = new String(parts[1].TakeWhile(c => c != '\"').ToArray());
                
                var nutritionScore =(int)(
                    ((m.GradeInfo.GradersInfo[GraderType.MaxNutValuesMealGrader].Grade *2)+
                    (m.GradeInfo.GradersInfo[GraderType.MinNutValuesMealGrader].Grade *3) +
                    m.GradeInfo.GradersInfo[GraderType.CaloriesCountMealGrader].Grade) / 6 * 100);

                var strRecipe = "<div><font style=\"background-color:{0};font-weight: bold;\">{1}</font></div>";
                var strImage = "<a href=\"{0}\"><img src=\"{1}\"  height=\"300\" width=\"300\"></a>";
                var format = "<div><font style=\"font-weight: bold;\">{0}</font></div>";
                var resMax = m.GradeInfo.MaxNutrientGrades.OrderBy(e => e.Value).ToList();
                var resMin = m.GradeInfo.MinNutrientGrades.OrderByDescending(e => e.Value).ToList().Take(3).ToList();
                body += string.Format(strRecipe, "Beige", m.Recipe.Name+", Score : " + nutritionScore.ToString());
                body += string.Format(strImage, "http://bit.ly" + link, m.Recipe.ImageUrl);
                body += string.Format(format,  "Rich with : "+ resMin[0].Key.Split(',')[0] + ", "+ resMin[1].Key.Split(',')[0] + ", " +resMin[2].Key.Split(',')[0]);
                if (resMax[0].Value < 1) {
                    body += string.Format(format, "Too much : " + resMax[0].Key.Split(',')[0]);
                }
                body += "</td>";
            });
            body += "</tr>";
            body += "</table>";

            body += "<div>For any requests or concerns please reply to this address</div>";
            body = "<!DOCTYPE html><html><body>" + body + "</html></body>";
            var address = recommendationDB.UserProfile.Email;
            if (debug)
                address = "alexbihsh@gmail.com";
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
            message.To.Add(new MailAddress(address));  

            {
                smtp.Send(message);
            }
        }
    }
}
