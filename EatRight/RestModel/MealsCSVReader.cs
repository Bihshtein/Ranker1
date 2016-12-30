using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;

namespace RestModel
{
    public class MealsCSVReader
    {
        public static string csvPath = Assembly.GetExecutingAssembly().Location + @"..\..\..\..\..\RestModel\FixedMeals.csv";

        public static void CreateFixedMealsList(RestDBInterface unit)
        {
            unit.Recipes.Empty();
            int curID = 0;

            var reader = new StreamReader(File.OpenRead(csvPath));

            while (!reader.EndOfStream)
            {
                var productsIndex = new Dictionary<int, string>();

                string name = "";
                var productsWeight = new Dictionary<string, double>();
                var types = new HashSet<MealType>();
                var categories = new HashSet<MealCategory>();
                TimeSpan prepTime = new TimeSpan(0);

                var line = reader.ReadLine();
                while (line != null && line.Length > 1)
                {
                    var tokens = line.Split(',');
                    tokens = tokens.Where(x => !string.IsNullOrEmpty(x)).ToArray();
                    if (tokens.Count() == 0)
                    {
                        break;
                    }

                    if (line.Length <= 1)
                    {
                        continue;
                    }
                    
                    var att = tokens[0];
                    switch (att)
                    {
                        case "name":
                            name = tokens[1];
                            break;
                        case "types":
                            for (int idx = 1; idx < tokens.Length; idx++)
                            {
                                types.Add(GetType(tokens[idx]));
                            }
                            break;
                        case "categories":
                            for (int idx = 1; idx < tokens.Length; idx++)
                            {
                                categories.Add(GetCategory(tokens[idx]));
                            }
                            break;
                        case "products":
                            for (int idx = 1; idx < tokens.Length; idx++)
                            {
                                if (Queries<Product>.GetMatchingProductsForIngredient(tokens[idx]).Count == 0)
                                {
                                    throw new Exception("No such product: " + tokens[idx]);
                                }
                                productsWeight[tokens[idx]] = 0;
                                productsIndex[idx] = tokens[idx];
                            }
                            break;
                        case "weights":
                            for (int idx = 1; idx < tokens.Length; idx++)
                            {
                                productsWeight[productsIndex[idx]] = Convert.ToDouble(tokens[idx]);
                            }
                            break;
                        case "prep_time":
                            prepTime = new TimeSpan(0,(int) Convert.ToDouble(tokens[1]),0);
                            break;
                    }

                    line = reader.ReadLine();
                }

                if (name.Length > 0 && productsWeight.Count > 0)
                {
                    Recipe meal = new Recipe() {
                        Name = name,
                        ProductsWeight = productsWeight,
                        Types = types,
                        Categories = categories,
                        PrepTime = prepTime,
                        ID = curID++
                    };

                    meal.CalculateNutValuesAndCalories();

                    unit.Recipes.Add(meal);
                }
            }
        }

        public static MealType GetType(string typeStr)
        {
            foreach (MealType type in Enum.GetValues(typeof(MealType)))
            {
                if (type.ToString().ToLower() == typeStr.ToLower())
                {
                    return type;
                }
            }

            System.Console.WriteLine("***ERROR*** No such type: " + typeStr);
            Environment.Exit(1);

            return MealType.Breakfast;
        }

        public static MealCategory GetCategory(string categoryStr)
        {
            foreach (MealCategory category in Enum.GetValues(typeof(MealCategory)))
            {
                if (category.ToString().ToLower() == categoryStr.ToLower())
                {
                    return category;
                }
            }

            System.Console.WriteLine("***ERROR*** No such category: " + categoryStr);
            Environment.Exit(1);

            return MealCategory.Chinese;
        }
    }
}
