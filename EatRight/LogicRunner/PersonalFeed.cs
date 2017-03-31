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
        };
        public static void SendEmail(RecommendationDB recommendationDB, IEnumerable<Meal> meals, string mealType) {
            meals = meals.OrderByDescending(m => m.Grade);
            var fromAddress = new MailAddress("alex_bihshtein@hotmail.com");
            string fromPassword = "99sozio#";
            string subject = DateTime.Now.DayOfWeek.ToString() + " " + mealType + " recommendation for "+ recommendationDB.UserProfile.Name;
            var graders = recommendationDB.GradersWeight.OrderByDescending(i => i.Value).ToList();


            string body = string.Format("<p>Recommendation priorities are :  ");
            for (int i = 0; i < 4; i++) 
                body += GradersGroupsNames[graders[i].Key] +" - ";
            body = body.Remove(body.Length - 3, 2);
            body += "</p>";
            if (recommendationDB.UserProfile.Restrictions != null) {
                body += string.Format("<p>Your priorities (and restictions)  are :     ");
                recommendationDB.UserProfile.Restrictions.ToList().ForEach(r => body += r.ToString() + " - ");
                body = body.Remove(body.Length - 3, 2);
                body += "</p>";
            }

            meals.ToList().ForEach(m => {
                var recipeLink = m.Recipe.Urn+ m.Recipe.OriginalID.ToString();
                var shortlink = new WebClient().DownloadString(string.Format("http://wasitviewed.com/index.php?href=http%3A%2F%2F{0}&email=alex_bihshtein%40hotmail.com&notes=&bitly=bitly&nobots=nobots&submit=Generate+Link", recipeLink));
                var parts = shortlink.Split(new string[] { "bit.ly" }, StringSplitOptions.None);
                var link = new String(parts[1].TakeWhile(c => c != '\"').ToArray());
                
                var nutritionScore =(int)((m.GradeInfo.GradersInfo[GraderType.MaxNutValuesMealGrader].Grade +
                    m.GradeInfo.GradersInfo[GraderType.MinNutValuesMealGrader].Grade +
                    m.GradeInfo.GradersInfo[GraderType.CaloriesCountMealGrader].Grade) / 3 * 100);
                var simplicityScore = (int)(m.GradeInfo.GradersInfo[GraderType.PrepTimeMealGrader].Grade * 100);

                int nutritionBarSize = nutritionScore < 60 ? 60 : nutritionScore;
                int simplicityBarSize = simplicityScore < 60 ? 60 : simplicityScore;
                nutritionBarSize *= 3;
                simplicityBarSize *= 3;

                var strRecipe = "<p><font style=\"background-color:{0};font-weight: bold;\">{1}</font></p>";
                var strImage = "<a href=\"{0}\"><img src=\"{1}\" style=\"width: 250; height: 250;\"></a>";
                var strScore = "<div class=\"chart\"><data ng-init=\"{0}\"/><div style =\"background-color:{1}; width:{0}px;\">{2} ({3}%)</div></div>";


                body += string.Format(strRecipe, "Beige", m.Recipe.Name+" Score : " + ((int)m.Grade).ToString());
                body += string.Format(strImage, "http://bit.ly" + link, m.Recipe.ImageUrl);
                body += string.Format(strScore, nutritionBarSize, GetColorByScore(nutritionScore), "Nutrition", nutritionScore);
                body += string.Format(strScore, simplicityBarSize, GetColorByScore(simplicityScore), "Simplicity", simplicityScore);
            });
            body += "<p>Press the picture to go to the recipe</p>";
            body += "<p>For any requests or concerns please reply to this address</p>";
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
            message.To.Add(new MailAddress(recommendationDB.UserProfile.Email));  

            {
                smtp.Send(message);
            }
        }

        private static string GetColorByScore(double score) {
            if (score > 85)
                return "LightGreen";
            else if (score > 60)
                return "PaleGreen";
            else
                return "MistyRose";

        }
    }
}
