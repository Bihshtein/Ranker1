﻿using InitRecipes;
using Logic;
using RecommendationBuilder;
using RecommendationBuilder.Graders;
using RestModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LogicRunner
{
    public partial class LogicRunnerForm : Form
    {
        public string FolderPath = Assembly.GetExecutingAssembly().Location + @"\..\..\..\..\LocalDB\";

        public RestDBInterface unit = null;
        bool alexiknow = false;

        private Boolean useDBRecipes = true;
        private Boolean useTestsRecipes = false;

        public LogicRunnerForm()
        {
            InitializeComponent();

            unit = new RestDBInterface();
     
            var dv = unit.DailyValues.GetAllList();

            this.bindingSource1.DataSource = dv;
            ageGender.DataSource = bindingSource1.DataSource;
            dataGridView1.AutoGenerateColumns = true;
            mealType.DataSource = Enum.GetNames(typeof(MealType));
            weekday.DataSource = Enum.GetNames(typeof(DayOfWeek));
            minMax.DataSource = new string[] { "min", "max" }; 
            comboBox4.DataSource = Enum.GetNames(typeof(RecipesSource));
            workMode.DataSource = new string[] {  "Recommend", "Debug" };
            var range = new List<int>();
            for (int i = 0; i < 100; i++) {
                range.Add(i);
            }

            calories.DataSource = new List<int>(range);
            cookTime.DataSource = new List<int>(range);
            minValues.DataSource = new List<int>(range);
            maxValues.DataSource = new List<int>(range);
            servings.DataSource = new List<int>(range);
            steps.DataSource = new List<int>(range);
            idealServings.DataSource = new List<int>(range);
            recommendationsNum.DataSource = new List<int>(range);
            activityLevel.DataSource = Enum.GetNames(typeof(PhysicalActivityLevel));
            mealType.SelectedIndex = 0;
            ageGender.SelectedIndex = 8;
            maxValues.SelectedIndex = 4;
            minValues.SelectedIndex =16;
            calories.SelectedIndex = 4;
            weekday.SelectedIndex = (int)DateTime.Now.DayOfWeek;
            recommendationsNum.SelectedIndex =40;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var startingTime = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

            var uProfile = new UserProfile()
            {
                ID = 777,
                Age = 27,
                Gender = GenderType.Male,
                Pregnant = false,
                Name = "Aleg"
            };

            uProfile.ActivityLevel = (PhysicalActivityLevel)Enum.Parse(typeof(PhysicalActivityLevel),activityLevel.SelectedItem.ToString());
            uProfile.Weight = int.Parse(txtWeight.Text);
            uProfile.Height = int.Parse(txtHeight.Text);
            uProfile.Age = (ageGender.SelectedValue as DailyValue).Age.MaxAge;
            uProfile.Gender = (ageGender.SelectedValue as DailyValue).Gender.Type;
          
            uProfile.Restrictions = new HashSet<UserRestrictions>();
            if (Meat.Checked)
                uProfile.Restrictions.Add(UserRestrictions.Meat);
            if (Fish.Checked)
                uProfile.Restrictions.Add(UserRestrictions.Fish);
            if (Dairy.Checked)
                uProfile.Restrictions.Add(UserRestrictions.Dairy);
            if (Seafood.Checked)
                uProfile.Restrictions.Add(UserRestrictions.Seafood);
            if (NoFish.Checked)
                uProfile.Restrictions.Add(UserRestrictions.NoFish);
            if (NoDairy.Checked)
                uProfile.Restrictions.Add(UserRestrictions.NoDairy);
            if (NoMeat.Checked)
                uProfile.Restrictions.Add(UserRestrictions.NoMeat);
            if (NoSeafood.Checked)
                uProfile.Restrictions.Add(UserRestrictions.NoSeafood);
            if (Vegetarian.Checked)
                uProfile.Restrictions.Add(UserRestrictions.Vegetarian);
            if (Pescetarian.Checked)
                uProfile.Restrictions.Add(UserRestrictions.Pescetarian);
            if (Vegan.Checked)
                uProfile.Restrictions.Add(UserRestrictions.Vegan);
            if (Kosher.Checked)
                uProfile.Restrictions.Add(UserRestrictions.Kosher);
            var recommendationDB = RecommendationDBGenerator.FromUserProfile(uProfile, unit);
            recommendationDB.idealServingsNum = int.Parse(idealServings.SelectedItem.ToString());
            recommendationDB.dailyValues = (ageGender.SelectedValue as DailyValue).DuplicateDictionary();

            recommendationDB.range = new MealSuggestionRange() { Length = int.Parse(recommendationsNum.SelectedItem.ToString()), MealType = (MealType)Enum.Parse(typeof(MealType), mealType.SelectedItem.ToString()) };
            txtCalories.Text = recommendationDB.dailyCaloriesNum.ToString();
              recommendationDB.GradersWeight = new Dictionary<GraderType, double>()
               {
                    // Meal graders
                    {GraderType.CaloriesCountMealGrader, int.Parse(calories.SelectedItem.ToString())},
                    {GraderType.MinNutValuesMealGrader,  int.Parse(minValues.SelectedItem.ToString())},
                    {GraderType.MaxNutValuesMealGrader,  int.Parse(maxValues.SelectedItem.ToString())},
                    {GraderType.ServingsNumMealGrader,  int.Parse(servings.SelectedItem.ToString())},
                    {GraderType.PrepTimeMealGrader, int.Parse(cookTime.SelectedItem.ToString()) },
                    {GraderType.StepsNumMealGrader, int.Parse(steps.SelectedItem.ToString()) }
                };
            var source = (RecipesSource)Enum.Parse(typeof(RecipesSource), comboBox4.SelectedItem.ToString());
            RecommendationGenerator generator = null;
            if (workMode.SelectedItem.ToString() == "Debug") {
                var recipes = new List<int>();
                var recipesCollection = new RestDBInterface().Recipes;
                
                var meal = (MealType)Enum.Parse(typeof(MealType), mealType.SelectedItem.ToString());
                if (source == RecipesSource.All)
                    recipesCollection.GetAllList().ForEach(a => recipes.Add(a.ID));
                else 
                    recipesCollection.Queries.GetBySource(source, meal).ForEach(a => recipes.Add(a.ID));

                generator = new RecommendationGenerator(unit, recommendationDB,  new HashSet<int>(recipes), int.Parse(txtCalories.Text));
            }
            else
                generator = new RecommendationGenerator(unit, recommendationDB, useDBRecipes, useTestsRecipes);

            IEnumerable<Meal> meals = null;
            if (workMode.SelectedItem.ToString() == "Debug")
            {
                meals = generator.GetMealsList();

            }
            else
            {
                meals = null;
                if (generator.GetRecommendation() != null)
                {
                    meals = generator.GetRecommendation().MealsSet;
                    if (source != RecipesSource.All)
                        meals = meals.Where(m => m.Recipe.Source == source).ToList();
                }
            }
            if (meals == null) {
                MessageBox.Show("I'm sorry, but I can't recommend on any " + mealType.SelectedItem.ToString() + "s");
            }
            else {
                var list = meals.Select(o => new MyViewModel(o) {
                    Source = o.Recipe.Source,
                    Name = o.Recipe.Name,
                    URL = o.Recipe.Urn + o.Recipe.OriginalID.ToString(),
                GradersResult = parseGradersResult(o.GradeInfo.GradersInfo)
                }).ToList();
           
                this.bindingSource2.DataSource = list;


                dataGridView1.DataSource = this.bindingSource2;
                if (!alexiknow)
                {
                    richTextBox1.DataBindings.Add("Text", bindingSource2, "GradersResult");
                    richTextBox2.DataBindings.Add("Text", bindingSource2, "Products");
                    richTextBox5.DataBindings.Add("Text", bindingSource2, "MaxScores");
                    richTextBox3.DataBindings.Add("Text", bindingSource2, "NutValues");
                }
                alexiknow = true;
            }
            if (meals != null) {
                total.Text = meals.Count().ToString();
              
            }
         
        }
       

        private string parseNutValues(Dictionary<string, double> let)
        {
            var str = "";

            foreach (var x in let)
            {
                str += string.Format("{0} = {1}\n", x.Key, x.Value.ToString("N3"));
            }
            return str;
        }

        private string parseGradersResult(Dictionary<GraderType,GraderInfo> gradersInfo)
        {
            var str = "";

            foreach (var x in gradersInfo)
            {
                str += string.Format("{0}: weight = {1}, grade = {2}\n", x.Key.ToString(), x.Value.Weight, x.Value.Grade.ToString("N3"));
            }
            return str;
        }

    


        private void LogicRunnerForm_Load(object sender, EventArgs e) {

        }

        private void label13_Click(object sender, EventArgs e) {

        }

        private void recommendationsNum_SelectedIndexChanged(object sender, EventArgs e) {

        }

        private void minMax_SelectedIndexChanged(object sender, EventArgs e) {
                if (minMax.SelectedItem.ToString() == "max") {
                    richTextBox3.Visible = false;
                    richTextBox5.Visible = true;
                }
                else {
                    richTextBox3.Visible = true;
                    richTextBox5.Visible = false;
                }
        }

        private void button4_Click(object sender, EventArgs e) {
            var dayOfWeek = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), weekday.SelectedItem.ToString());
            var debug = workMode.SelectedItem.ToString() == "Debug";
            foreach (MealType item in Enum.GetValues(typeof(MealType))) 
               Hack.RecommendToUsers(item, debug, dayOfWeek);

        }

        private void button2_Click(object sender, EventArgs e) {
            
            var recipe = unit.Recipes.Get(int.Parse(dataGridView1.SelectedCells[0].Value.ToString()));
            var origRecipe = unit.Recipes.Get(int.Parse(dataGridView1.SelectedCells[0].Value.ToString()));
            recipe.TotalNutValues = null;
            recipe.CalculateNutValuesAndCalories(textBox1.Text.Trim());
            var newInfo = MyViewModel.GetNutInfo2(origRecipe, recipe);
            richTextBox6.Text = newInfo;
            textBox2.Text = (recipe.TotalCaloriesNum / recipe.Servings).ToString();
        }

        private void button5_Click(object sender, EventArgs e) {
            var recipe = unit.Recipes.Get(int.Parse(dataGridView1.SelectedCells[0].Value.ToString()));
            recipe.TotalNutValues = null;
            recipe.ProductsWeight[textBox3.Text.Split(',')[0]] =  double.Parse(textBox3.Text.Split(',')[1]);
            recipe.TotalNutValues = null;
            recipe.CalculateNutValuesAndCalories(textBox1.Text.Trim());

            unit.Recipes.Update(s => s.ID, recipe.ID, recipe);
            button1_Click(sender, e);
        }
    }
}

