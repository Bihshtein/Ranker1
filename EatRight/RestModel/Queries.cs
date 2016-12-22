﻿using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace RestModel {
    public class Queries<T> {
        public IMongoCollection<T> collection { get; private set; }
        public static RestDBInterface unit = new RestDBInterface();
        public Queries(IMongoCollection<T> collection) {
            this.collection = collection;
        }


        public List<Product> QueryByNameAndValue(string name, string group, string value, bool partial = false) {
            name = name.ToLower();
            group = group.ToLower();
            Expression<Func<Product, bool>> query;

            if (Product.Name2FoodGroups.Contains(group))
                query = x => x.Name2.Equals(name) && x.FoodGroup.Equals(group);
            else
                if (partial)
                query = x => x.Name1.Contains(name) && x.FoodGroup.Equals(group);
            else
                query = x => x.Name1.Equals(name) && x.FoodGroup.Equals(group);

            var res = collection.Find<T>(query as Expression<Func<T, bool>>).ToList();

            var newRes = res.Cast<Product>().ToList();
            newRes.RemoveAll(p => p.Nutrients()[value] == 0);
            newRes.Sort((a, b) => a.Nutrients()[value] > b.Nutrients()[value] ? 1 : -1);
            return newRes;
        }
        public static Dictionary<string, string> RecipeToNutrientDictionary = new Dictionary<string, string> {
            { "white sugar","granulated sugar"},
            { "bread flour","bread wheat flour"},
            { "all-purpose flour","all-purpose wheat flour"},
            { "whole wheat flour","whole-grain wheat flour"},
            { "kosher salt","table salt"},
            { "dry milk powder","dry milk"},
        };
        public static List<string> CutDetails = new List<string> {
            "melted","sifted", "sprig", "sprigs", "ground", "shredded", "cubed",
            "head", "heads", "sliced", "stalk", "stalks", "diced", "minced", "chopped",
            "grated","mashed"};
        public static List<string> ServeDetails = new List<string> { "warm", "cooked", "fresh" };
        public static List<string> PackDetails = new List<string> { "packed", "package", "packages" };

        

        public static List<Product> GetMatchingProductsForIngredient(string ingredient) {
            ingredient = ingredient.ToLower();
            CutDetails.ForEach(item => ingredient = ingredient.Replace(item + " ", ""));
            ServeDetails.ForEach(item => ingredient = ingredient.Replace(item + " ", ""));
            PackDetails.ForEach(item => ingredient = ingredient.Replace(item + " ", ""));
            ingredient = ingredient.Trim();
            if (RecipeToNutrientDictionary.ContainsKey(ingredient))
                ingredient = RecipeToNutrientDictionary[ingredient];
            var innerSplit = ingredient.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

            List<Product> res = null;
            if (innerSplit.Length == 1)
                res = unit.Products.Queries.TryMatchWholeProduct(innerSplit[0]);
            else if (innerSplit.Length == 2)
                res = unit.Products.Queries.TryMatchWholeProduct(innerSplit[0], innerSplit[1]);
            else if (innerSplit.Length == 3)
                res = unit.Products.Queries.TryMatchWholeProduct(innerSplit[0], innerSplit[1], innerSplit[2]);
           
            if (res == null || res.Count == 0)// special long ass names (manual recipe init from file)
                res = unit.Products.Queries.TryMatchWholeProduct(ingredient);
            return res;
        }



        public List<DailyValue> GetByGender(GenderType gender) {

            Expression<Func<DailyValue, bool>> query = x => x.Gender.Type.Equals(gender);
            var res = collection.Find(query as Expression<Func<T, bool>>).ToList();
            var newRes = res.Cast<DailyValue>().ToList();
            newRes.Sort((a, b) => a.Age.MinAge > b.Age.MinAge ? 1 : -1);
            return newRes;
        }

        public List<Product> TryMatchWholeProduct(string part) {
            Expression<Func<Product, bool>> query = x =>
              (x.Name1.Equals(part + "s") || x.Name1.Equals(part + "es") || x.Name1.Equals(part) ||
              x.FoodGroup.Equals(part) || x.Name2.Equals(part));
            var res = collection.Find(query as Expression<Func<T, bool>>).ToList();
            var newRes = res.Cast<Product>().ToList();
            return newRes;

        }

        public List<Product> TryMatchWholeProduct(string part1, string part2, string part3) {
            Expression<Func<Product, bool>> query = x =>
            (x.Name3.Equals(part1 + " " + part2) && x.Name2.Equals(part3)) ||
            (x.HealthData.Equals(part1)&& x.Name1.Equals(part2) && x.StorageMethod.Equals("dry " +part3)) ||
            (x.Name3.Equals(part1) && x.Name1.Equals(part2 + " " + part3)) ||
            (x.Name2.Equals(part1) && x.Name1.Equals(part2 + " " + part3)) ||
              (x.FoodGroup.Equals(part1) && x.Name1.Equals(part2)) ||
              (x.Name3.Equals(part1) && x.Name1.Equals(part3)) ||
              (x.Name2.Equals(part2) && x.Name1.Equals(part3));
            var res = collection.Find(query as Expression<Func<T, bool>>).ToList();
            var newRes = res.Cast<Product>().ToList();
            return newRes;

        }
        public List<Product> TryMatchWholeProduct(string part1, string part2) {
            Expression<Func<Product, bool>> query = x =>
            (x.Name1.Equals(part1 + " " + part2)) ||
            (x.Name2.Equals(part1 + " " + part2)) ||
            (x.Name2.Equals(part1 + " " + part2+"s")) ||


            (x.Name3.Equals(part2.Remove(part2.Length-1,1)) && x.Name1.Equals(part1)) || // remove the last 's'

                (x.Name3.Equals(part1) && x.Name1.Equals(part2)) ||
                (x.Name3.Contains("or") && x.Name3.Contains(part1) && x.Name2.Equals(part2)) ||
                (x.Name3.Equals(part1) && x.Name2.Equals(part2)) ||
                (x.StorageMethod.Equals(part1) && x.Name1.Equals(part2)) ||
                (x.StorageMethod.Equals(part1) && x.Name2.Equals(part2)) ||

                (x.Name3.Equals(part2) && x.Name1.Equals(part1)) ||

                (x.Name2.Equals(part1) && x.Name1.Equals(part2)) ||

                (x.Name2.Equals(part1) && x.Name1.Equals(part2 + "s")) ||
                (x.Name2.Equals(part1) && x.Name1.Equals(part2+"es")) ||

                (x.Name3.Equals(part1) && x.Name1.Equals(part2 + "s")) ||
                (x.Name3.Equals(part1) && x.Name1.Equals(part2 + "es")) ||


                (x.Name2.Equals(part2) && x.Name1.Equals(part1)) ||

                (x.FoodGroup.Equals(part1) && x.Name1.Equals(part2)) ||
                (x.FoodGroup.Equals(part2) && x.Name1.Equals(part1)) ||

                (x.Name2.Equals(part2) && x.Name2.Equals(part1)) ||
                (x.FoodGroup.Equals(part1) && x.Name2.Equals(part2));

            var res = collection.Find(query as Expression<Func<T, bool>>).ToList();
            var newRes = res.Cast<Product>().ToList();
            return newRes;
        }

        public List<Meal> GetByMealType(string mealType) {
            Expression<Func<Meal, bool>> query = x => x.MealType.Equals(mealType);
            var res = collection.Find(query as Expression<Func<T, bool>>).ToList();
            var newRes = res.Cast<Meal>().ToList();
            return newRes;
        }

    }
}
