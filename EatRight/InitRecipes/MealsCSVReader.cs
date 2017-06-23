using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;
using RestModel;
using Logic;
using System.Globalization;
using System.Threading;

namespace InitRecipes {
    public class MealsCSVReader
    {
        public static string csvPath = Assembly.GetExecutingAssembly().Location + @"..\..\..\..\..\LocalDB\FixedMeals.csv";

        public static void CreateFixedMealsList(RestDBInterface unit)
        {
            var customCulture = (CultureInfo)Thread.CurrentThread.CurrentCulture.Clone(); customCulture.NumberFormat.NumberDecimalSeparator = ".";
            Thread.CurrentThread.CurrentCulture = customCulture;
            unit.TestsRecipes.Empty();
            int curID = 0;

            var reader = new StreamReader(File.OpenRead(csvPath));

            while (!reader.EndOfStream)
            {
                var productsIndex = new Dictionary<int, string>();

                string name = "";
                var productsWeight = new Dictionary<string, double>();
                var types = new HashSet<MealType>();
                var categories = new HashSet<RecipeCategory>();
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
                            for (int idx = 1; idx < tokens.Length; idx++) {
                                var parts = tokens[idx].Split(':');
                                var weight = double.Parse(parts[0]);
                                if (parts.Length > 1) {
                                    var measureName = parts[1]; 
                                    if (Map.HasWord(Formulas.MeasuresWeights.Keys.ToList(),measureName)) {
                                        productsWeight[productsIndex[idx]] = weight * Formulas.MeasuresWeights[measureName];
                                    }
                                    else {
                                        var fullName = productsWeight.Keys.ToList().First(item => item == productsIndex[idx]);
                                        var product = Queries<Product>.GetMatchingProductsForIngredient(fullName)[0];
                                        var prodList = new List<Product>() { product };
                                        productsWeight[productsIndex[idx]] = AddProducts.TryParseRelativeWeight(measureName, weight, ref product, fullName, prodList);

                                    }
                                }
                                else 
                                    productsWeight[productsIndex[idx]] = weight;
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

                    unit.TestsRecipes.Add(meal);
                }
            }
            reader.Close();
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

        public static RecipeCategory GetCategory(string categoryStr)
        {
            foreach (RecipeCategory category in Enum.GetValues(typeof(RecipeCategory)))
            {
                if (category.ToString().ToLower() == categoryStr.ToLower())
                {
                    return category;
                }
            }

            System.Console.WriteLine("***ERROR*** No such category: " + categoryStr);
            Environment.Exit(1);

            return RecipeCategory.Chinese;
        }
    }
}
