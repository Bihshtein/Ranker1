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
        public static void SendEmail(RecommendationDB recommendationDB, IEnumerable<Meal> meals, string mealType, DayOfWeek dayOfWeek, bool debug =false) {
            meals = meals.OrderByDescending(m => m.Grade);
            var fromAddress = new MailAddress("alex_bihshtein@hotmail.com");
            string fromPassword = "99sozio$";
            string subject = dayOfWeek.ToString() + " " + mealType + " recommendation for "+ recommendationDB.UserProfile.Name;
            var graders = recommendationDB.GradersWeight.OrderByDescending(i => i.Value).ToList();


            string body = "";            
            body += "<p><b>Press the picture to enter the recipe</b></p>";
            body += "<table style=\"width:100%\">";
         
            var list = meals.ToList();
            while (list.Count > 0) {
                body += "<tr>";
                var count = 3;
                if (list.Count < count)
                    count = list.Count;
                var row = list.Take(count);
                row.ToList().ForEach(m => {
                    AddMeal(ref body, m);
                });
                body += "</tr>";
                list.RemoveRange(0,count);
            }
            body += "</table>";
            body += "<br></br>";
              if (recommendationDB.UserProfile.Restrictions.Count > 0) {
                body += string.Format("<div>Your restictions are :     ");
                recommendationDB.UserProfile.Restrictions.ToList().ForEach(r => body += r.ToString() + " - ");
                body = body.Remove(body.Length - 3, 2);
                body += "</div>";
            }
            body +=string.Format("<div>Your priorities are :  ");
            for (int i = 0; i < GradersGroupsNames.Count; i++)
                body += GradersGroupsNames[graders[i].Key] + " - ";
            body = body.Remove(body.Length - 3, 2);
            body += "</div>";
          
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

        public static void AddMeal(ref string body,Meal m) {
            body += "<td>";
            var recipeLink = m.Recipe.Urn + m.Recipe.OriginalID.ToString();
            var shortlink = new WebClient().DownloadString(string.Format("http://wasitviewed.com/index.php?href=http%3A%2F%2F{0}&email=alex_bihshtein%40hotmail.com&notes=&bitly=bitly&nobots=nobots&submit=Generate+Link", recipeLink));
            var parts = shortlink.Split(new string[] { "bit.ly" }, StringSplitOptions.None);
            var link = new String(parts[1].TakeWhile(c => c != '\"').ToArray());

            var nutritionScore = (int)(
                ((m.GradeInfo.GradersInfo[GraderType.MaxNutValuesMealGrader].Grade * 2) +
                (m.GradeInfo.GradersInfo[GraderType.MinNutValuesMealGrader].Grade * 3) +
                m.GradeInfo.GradersInfo[GraderType.CaloriesCountMealGrader].Grade) / 6 * 100);
            var strImage = "<a href=\"{0}\"><img src=\"{1}\"  height=\"300\" width=\"300\"></a>";

            var format = "<div>{0}</div>";
            var boldFormat = "<div><font style=\"font-weight:bold;\">{0}</font></div>";
            var scoreFormat = "<div><font style=\"font-weight: bold;font-size:16px;\" color=\"#3090C7\">{0}</font></div>";
            var resMax = m.GradeInfo.MaxNutrientGrades.OrderBy(e => e.Value).ToList();
            var resMin = m.GradeInfo.MinNutrientGrades.OrderByDescending(e => e.Key).ToList();
            resMin.RemoveAll(i => i.Key.Contains("Carbohydrate") || i.Key.Contains("(fat)") || i.Key.Contains("Fiber"));
            resMin.RemoveAll(i => i.Value < 1);
            var rnd = new Random();
            var richWith = new HashSet<string>();
            while (richWith.Count < 2 && richWith.Count < resMin.Count) {
                var num = rnd.Next(0, resMin.Count);
                richWith.Add(GetNutrientPrettyName(resMin[num].Key));
            }
            body += string.Format(format, String.Join(" ", m.Recipe.Name.Split(' ').ToList().Take(6).ToList()));
            body += string.Format(strImage, "http://bit.ly" + link, m.Recipe.ImageUrl);
            body += string.Format(scoreFormat, "Score : " + nutritionScore.ToString());
            if (resMax[0].Value < 1)
                body += string.Format(boldFormat, "Too much : " + resMax[0].Key.Split(',')[0]);

            else
                body += string.Format(boldFormat, "All nutrients are in range!");
            body += string.Format(boldFormat, "Rich with : " + String.Join(",", richWith.ToList()));

            body += "</td>";
        }

        public static string GetNutrientPrettyName(string nutrient) {
            return nutrient.Split(',')[0].Split('(')[0];
        }
    }
}
