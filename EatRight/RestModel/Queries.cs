using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using Logic;

namespace RestModel {
    public class Queries<T> {
        public IMongoCollection<T> collection { get; private set; }
        public static RestDBInterface unit = new RestDBInterface();
        public Queries(IMongoCollection<T> collection) {
            this.collection = collection;
        }

        public List<Product> QueryByNameAndValue(string name, string group,string nutGroup, string value, bool partial = false) {
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
            newRes.RemoveAll(p => !p.Nutrients[nutGroup].ContainsKey(value) || p.Nutrients[nutGroup][value] == 0);
            newRes.Sort((a, b) => a.Nutrients[nutGroup][value] > b.Nutrients[nutGroup][value] ? 1 : -1);
            return newRes;
        }

        public List<T> QueryByExpression(Expression<Func<Product, bool>> query)
        {
            return collection.Find(query as Expression<Func<T, bool>>).ToList();
        }

        public static List<Product> GetMatchingProductsForIngredient(string ingredient)
        {
            var  res = unit.Products.Queries.TryMatchWholeProduct(ingredient);
            if (res != null && res.Count> 0)
                return res;           
            if (ingredient == string.Empty)
                return null;
            res = SplitIngredient(ingredient);

            if (res == null || res.Count == 0) // Handle 'or'
            {
                if (ingredient.Contains(" or "))
                {
                    res = HandleOr(ingredient);
                }
            }
            if (res == null || res.Count == 0) // Special long ass names (manual recipe init from file)
            {
                res = unit.Products.Queries.TryMatchWholeProduct(ingredient);
            }
            return res;
        }

        private static List<Product> HandleOr(string ingredient)
        {
            /* We expect the following cases when handling or:
             * product1 or product2 (for example butter or margarine)
             * product1-prefix or product2-prefix mutual-suffix (for example green or red apples)
             */
            List<Product> res = null;

            var innerSplit = ingredient.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

            int orIndex = 0;
            foreach (var part in innerSplit)
            {
                if (part.Equals("or"))
                {
                    break;
                }
                orIndex++;
            }
            if (orIndex == innerSplit.Length)
            {
                // No or- we should never get here
                return res;
            }

            // Try match the left part of the or
            if (orIndex > 0)
            {
                var leftParts = innerSplit.Take(orIndex).ToList();
                res = unit.Products.Queries.TryMatchWholeProduct(leftParts);
                int origCount = leftParts.Count;
                int rightIdx = innerSplit.Length - 1;
                while (leftParts.Count < 4 && rightIdx > orIndex)
                {
                    leftParts.Insert(origCount, innerSplit[rightIdx]);
                    if (res == null)
                    {
                        res = unit.Products.Queries.TryMatchWholeProduct(leftParts);
                    }
                    else
                    {
                        res = res.Union(unit.Products.Queries.TryMatchWholeProduct(leftParts)).ToList();
                    }
                    rightIdx--;
                }

                if (res != null && res.Count > 0)
                {
                    return res;
                }
            }

            // Try match the right part of the or
            var rightParts = innerSplit.Skip(orIndex + 1).Take(innerSplit.Length - orIndex - 1).ToList();
            res = unit.Products.Queries.TryMatchWholeProduct(rightParts);

            return res;
        }

        private static List<Product> SplitIngredient(string ingredient)
        {
            List<Product> res = null;

            var innerSplit = ingredient.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

            if (innerSplit.Length <= 4)
            {
                res = unit.Products.Queries.TryMatchWholeProduct(innerSplit);
            }

            return res;
        }

        public List<DailyValue> GetByGender(GenderType gender) {

            Expression<Func<DailyValue, bool>> query = x => x.Gender.Type.Equals(gender);
            var res = collection.Find(query as Expression<Func<T, bool>>).ToList();
            var newRes = res.Cast<DailyValue>().ToList();
            newRes.Sort((a, b) => a.Age.MinAge > b.Age.MinAge ? 1 : -1);
            return newRes;
        }

        public List<Product> TryMatchWholeProduct(string[] parts)
        {
            if (parts == null || parts.Length == 0)
            {
                return null;
            }
            if (parts.Length == 1)
            {
                return TryMatchWholeProduct(parts[0]);
            }
            if (parts.Length == 2)
            {
                return TryMatchWholeProduct(parts[0], parts[1]);
            }
            if (parts.Length == 3)
            {
                return TryMatchWholeProduct(parts[0], parts[1], parts[2]);
            }
            else // NOTE: when parts is longer than 4, we'll take the first 4
            {
                return TryMatchWholeProduct(parts[0], parts[1], parts[2], parts[3]);
            }
        }

        public List<Product> TryMatchWholeProduct(List<string> parts)
        {
            if (parts == null || parts.Count == 0)
            {
                return null;
            }
            if (parts.Count == 1)
            {
                return TryMatchWholeProduct(parts[0]);
            }
            if (parts.Count == 2)
            {
                return TryMatchWholeProduct(parts[0], parts[1]);
            }
            if (parts.Count == 3)
            {
                return TryMatchWholeProduct(parts[0], parts[1], parts[2]);
            }
            else // NOTE: when parts is longer than 4, we'll take the first 4
            {
                return TryMatchWholeProduct(parts[0], parts[1], parts[2], parts[3]);
            }
        }

        public List<Product> TryMatchWholeProduct(string part)
        {
            Expression<Func<Product, bool>> query = x =>
              (x.Name1.Equals(part) || x.Name1.Equals(part + "s") || x.Name1.Equals(part + "es") || x.Name1.Equals(ParseHelpers.GetWithoutLast_ES_letters(part)) ||
              (x.FoodGroup.Equals(part)));
            var res = collection.Find(query as Expression<Func<T, bool>>).ToList();

            if (res.Count == 0) {
                 query = x => (x.Name2.Equals(ParseHelpers.GetWithoutLast_S_letter(part)) || x.Name2.Equals(part));
                 res = collection.Find(query as Expression<Func<T, bool>>).ToList();

            }
            var newRes = res.Cast<Product>().ToList();
            return newRes;
        }
             

        public List<Product> TryMatchWholeProduct(string part1, string part2, string part3) {
            Expression<Func<Product, bool>> query = x =>
               (x.StorageMethod.Equals(part1) && x.Name2.Equals(part2 + " " + ParseHelpers.GetWithoutLast_S_letter(part3))) ||
               (x.PreparationMethod.Equals(part1) && x.Name1.Equals(part2 + " " + part3)) ||
            (x.Name3.Equals(part1 + " " + part2) && x.Name2.Equals(part3)) ||
            (x.Name3.Equals(part1 + " " + part2) && x.Name1.Equals(part3)) ||
            (x.Name2.Equals(part2 + " or " + part1) && x.Name1.Equals(part3)) ||
            (x.StorageMethod.Equals(part1) && x.Name2.Equals(part2 + " " + part3)) ||
            (x.StorageMethod.Equals(part1) && x.Name1.Equals(part2 + " " + part3)) ||
            (x.Name1.Equals(part1) && x.StorageMethod.Equals(part2 + " " + part3)) ||
            (x.Name1.Equals(part1) && x.HealthData.Equals(part2 + " " + part3)) ||
            (x.Name1.Equals(part3) && x.HealthData.Equals(part1 + " " + part2)) ||
            (x.Name2.Equals(part1 + " " + part2) && x.Name3.Equals(part3)) ||
            (x.Name2.Equals(part1 + " " + part2) && x.Name1.Equals(part3 + "s")) ||
            (x.HealthData.Equals(part1) && x.Name1.Equals(part2) && x.StorageMethod.Equals("dry " +part3)) ||
            (x.HealthData.Equals(part1.Replace('-',' ')) && x.Name2.Equals(part2+ " " + part3))|| // low-sodium chicken broth
            (x.PreparationMethod.Equals(part1) && x.Name2.Equals(part2) && x.Name1.Equals(part3)) ||
            (x.PackDetails.Equals(part1) && x.Name2.Equals(part2) && x.Name1.Equals(part3)) ||
            (x.Name3.Equals(part1) && x.Name1.Equals(part2 + " " + part3)) ||
            (x.Name2.Equals(part1) && x.Name1.Equals(part2 + " " + part3)) ||
            (x.Name2.Equals(part1) && x.Name1.Equals(part2 + " " + part3+"s")) ||
            (x.Name1.Equals(part3) && x.Name2.Equals(part1 + " " + part2)) ||
            (x.Name1.Equals(part1) && x.Name3.Equals(part2 + " " + part3)) ||
            (x.FoodGroup.Equals(part1) && x.Name1.Equals(part2)) ||
            (x.FoodGroup.Equals(part1) && x.Name2.Equals(part2)) && x.Name3.Equals(part3) ||
            (x.Name2.Equals(part1) && x.Name3.Equals(part2)) && x.Name1.Equals(part3) ||
            (x.Name3.Equals(part1) && x.Name1.Equals(part3)) ||
            (x.Name2.Equals(part2) && x.Name1.Equals(part3)) ||
            (x.Name2.Equals(part1) && x.Name1.Equals(part2)) ||
            (x.StorageMethod.Contains(part1 + "ned") && x.Name2.Equals(part2 + " " + part3)) ||
            (x.Name2.Equals(part3) && x.Name3.Equals(part2));
            var res = collection.Find(query as Expression<Func<T, bool>>).ToList();
            var newRes = res.Cast<Product>().ToList();
            return newRes;

        }

        public List<Product> TryMatchWholeProduct(string part1, string part2, string part3, string part4) {
            Expression<Func<Product, bool>> query = x =>
            (x.PackDetails.Equals(part1) && x.Name2.Equals(part2 + " " + part3) && x.Name1.Equals(part4)) ||
            (x.StorageMethod.Equals(part1) && x.Name2.Equals(part2 + " " +part3) && x.Name1.Equals(part4)) ||
            (x.PeelDetails.Equals(part1 + " " + part2) && x.FoodGroup.Equals(part3) && x.Name2.Equals(part4)) ||
            (x.BoneDetails.Equals(part1) && x.FoodGroup.Equals(part2) && x.Name1.Equals(part3) && x.Name3.Equals(part4)) ||
            (x.BoneDetails.Equals(part1) && x.FoodGroup.Equals(part2) && x.Name1.Equals(part3) && x.Name3.Equals(part4+"s")) ||
            (x.Name3.Equals(part1 + " " + part2 + " " + part3) && x.Name1.Equals(part4)) ||
            (x.Name2.Equals(part1 + " " + part2 + " " + part3) && x.Name1.Equals(part4)) ||
            (x.BoneDetails.Equals(part1) && x.FoodGroup.Equals(part2) && x.Name1.Equals(part3) && x.Name3.Equals(part4)) ||
            (x.Name1.Equals(part2 + " " + part3) && x.Name3.Equals(part1 + " "  + part4)) ||
            (x.Name3.Equals(part1 + " " + part2) && x.Name2.Equals(part3) && x.Name1.Equals(part4)) ||
            (x.PeelDetails.Equals(part1 + " " + part2) && x.FoodGroup.Equals(part3) && x.Name2.Equals(ParseHelpers.GetWithoutLast_S_letter(part4))) ||
            (x.Name2.Equals(part3 + " " + part4)) ||
            (x.Name1.Equals(part2) && x.Name2.Equals(part1)) ||
            (x.Name1.Equals(part3 + " " + part4) && x.Name2.Equals(part2));
            var res = collection.Find(query as Expression<Func<T, bool>>).ToList();
            var newRes = res.Cast<Product>().ToList();
            return newRes;
        }

        public List<Product> TryMatchWholeProduct(string part1, string part2) {
            Expression<Func<Product, bool>> query = x =>
            (x.Name1.Equals(part1 + " " + part2)) ||
            (x.Name1.Equals(part1 +part2)) ||
            (x.Name2.Equals(part1 + " " + part2)) ||
            (x.Name3.Equals(part1 + " " + part2)) ||
            (x.Name2.Equals(part1 + " " + part2+"s")) ||
            (x.Name3.Equals(ParseHelpers.GetWithoutLast_S_letter(part2)) && x.Name1.Equals(part1)) || 
            (x.Name3.Equals(part1) && x.Name1.Equals(part2)) ||
            (x.Name3.Contains("or") && x.Name3.Contains(part1) && x.Name2.Equals(part2)) ||
            (x.Name3.Equals(part1) && x.Name2.Equals(part2)) ||
            (x.StorageMethod.Equals(part1) && x.Name1.Equals(part2)) ||
            (x.FoodGroup.Equals(part1) && x.Name3.Equals(part2)) ||
            (x.StorageMethod.Equals(part1) && x.Name2.Equals(part2)) ||
            (x.StorageMethod.Contains(part1+"ned") && x.Name1.Equals(part2)) ||
            (x.Name3.Equals(part2) && x.Name1.Equals(part1)) ||
            (x.FatDetails.Contains(part1 + "|") && x.Name1.Equals(part2)) ||
            (x.Name2.Equals(part1) && x.Name1.Equals(part2)) ||
            (x.Name1.Equals(part1) && x.HealthData.Equals(part2)) ||
            (x.Name2.Equals(part1) && x.Name1.Equals(part2 + "s")) ||
            (x.Name2.Equals(part1) && x.Name1.Equals(ParseHelpers.GetWithoutLast_S_letter(part2))) ||
            (x.Name2.Equals(part1) && x.Name1.Equals(part2 +"es")) ||
            (x.Name3.Equals(part1) && x.Name1.Equals(part2 + "s")) ||
            (x.Name3.Equals(part1) && x.Name1.Equals(ParseHelpers.GetWithoutLast_S_letter(part2))) ||
            (x.Name3.Equals(part1) && x.Name1.Equals(part2 + "es")) ||
            (x.Name2.Equals(part2) && x.Name1.Equals(part1)) ||
            (x.PreparationMethod.Equals(part1) && x.Name2.Equals(part2)) ||
            (x.FoodGroup.Equals(part1) && x.Name1.Equals(part2)) ||
            (x.FoodGroup.Equals(part1) && x.Name2.Equals(ParseHelpers.GetWithoutLast_S_letter(part2))) ||
            (x.FoodGroup.Equals(part2) && x.Name1.Equals(part1)) ||
            (x.Name2.Equals(part2) && x.Name2.Equals(part1)) ||
            (x.PreparationMethod.Equals(part1) && x.Name1.Equals(part2)) ||
            (x.Name2.Equals(part1) && x.Name3.Equals(part2)) ||
            (x.FoodGroup.Equals(part1) && x.Name2.Equals(part2))||
            (x.StorageMethod.Equals(part1) && x.FoodGroup.Equals(part2));
            var res = collection.Find(query as Expression<Func<T, bool>>).ToList();
            var newRes = res.Cast<Product>().ToList();
            return newRes;
        }

      
        public List<Recipe> GetByIngredient(string ingredient) {
            Expression<Func<Recipe, bool>> query = x => x.ProductsWeight.ContainsKey(ingredient);
            var res = collection.Find(query as Expression<Func<T, bool>>).ToList();
            var newRes = res.Cast<Recipe>().ToList();
            return newRes;
        }

    }
}
