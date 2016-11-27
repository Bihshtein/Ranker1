﻿using RestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuBuilder
{
    public class DailyMenu : GradableObject
    {
        public DailyMenu()
        {
            ID = IDCounter++;
            Meals = new List<MenuMeal>();
        }

        public DailyMenu(List<MenuMeal> meals)
        {
            ID = IDCounter++;
            Meals = meals;
        }

        public override bool Equals(object obj)
        {
            // Check for null values and compare run-time types.
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            DailyMenu dm = (DailyMenu)obj;
            return ID.Equals(dm.ID);
        }

        public List<MenuMeal> Meals { get; private set; }
        public int ID { get; private set; }

        public List<string> GetAllProducts()
        {
            var resList = new List<string>();
            Meals.ForEach(x => resList.AddRange(x.Meal.Products.ToList()));
            return resList;
        }

        private static int IDCounter = 0;
    }
}
