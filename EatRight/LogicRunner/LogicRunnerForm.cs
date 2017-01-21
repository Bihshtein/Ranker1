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
            minMax.DataSource = new string[] { "min", "max" };
            comboBox4.DataSource = new string[] { "Fixed", "Internet", "Both" };
            workMode.DataSource = new string[] { "Recommend", "Debug" };
            var range = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21 };

            calories.DataSource = new List<int>(range);
            cookTime.DataSource = new List<int>(range);
            minValues.DataSource = new List<int>(range);
            maxValues.DataSource = new List<int>(range);
            servings.DataSource = new List<int>(range);
            idealServings.DataSource = new List<int>(range);
            recommendationsNum.DataSource = new List<int>(range);
            activityLevel.DataSource = Enum.GetNames(typeof(PhysicalActivityLevel));
            idealServings.SelectedIndex = 4;
            ageGender.SelectedIndex = 8;
            maxValues.SelectedIndex = 8;
            mealType.SelectedIndex = 1;
            minValues.SelectedIndex =8;
           // calories.SelectedIndex = 4;
            servings.SelectedIndex = 0;
           // cookTime.SelectedIndex = 2;
            comboBox4.SelectedIndex = 1;
            recommendationsNum.SelectedIndex = 4;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var startingTime = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

            var uProfile = new UserProfile()
            {
                Id = 777,
                Age = 27,
                Gender = GenderType.Male,
                Pregnant = 0,
                Name = "Hen"
            };

            var manager = GlobalProfilingManger.Instance.Manager;

            manager.Reset();
            manager.TakeTime("starting time");
            uProfile.ActivityLevel = (PhysicalActivityLevel)Enum.Parse(typeof(PhysicalActivityLevel),activityLevel.SelectedItem.ToString());
            uProfile.Weight = int.Parse(txtWeight.Text);
            uProfile.Height = int.Parse(txtHeight.Text);
            uProfile.Age = (ageGender.SelectedValue as DailyValue).Age.MaxAge;
            uProfile.Gender = (ageGender.SelectedValue as DailyValue).Gender.Type;
            

            manager.TakeTime("get recommendation db from user profile");

          
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
                    {GraderType.PrepTimeMealGrader, int.Parse(cookTime.SelectedItem.ToString()) }
                };

            manager.TakeTime("initialize recommendation db");
            
            RecommendationGenerator generator = null;
            if (workMode.SelectedItem.ToString() == "Debug") {
                HashSet<int> recipes = null;
                try {
                    recipes = new HashSet<int>(custom.Text.Split(',').ToList<string>().ConvertAll<int>(a => int.Parse(a)));
                }
                catch {
                    recipes = new HashSet<int>(File.ReadAllLines(FolderPath + "recipes_num.txt").ToList().ConvertAll<int>((a => int.Parse(a))));
                }
                generator = new RecommendationGenerator(unit, recommendationDB, recipes, int.Parse(txtCalories.Text));
            }
            else
                generator = new RecommendationGenerator(unit, recommendationDB, useDBRecipes, useTestsRecipes);

            manager.TakeTime("creating recommendation generator");
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
                }
            }
            if (meals == null) {
                manager.End();
                MessageBox.Show("I'm sorry, but I can't recommend on any " + mealType.SelectedItem.ToString() + "s");
            }
            else {
                var list = meals.Select(o => new MyViewModel(o) {
                    Id = o.Recipe.ID,
                    Name = o.Recipe.Name,
                    NutValues = parseNutValues(o.NutValues),
                    GradersResult = parseGradersResult(o.GradeInfo.GradersInfo)
                }).ToList();
                manager.TakeTime("getting meals list");
            /*    list.Sort((a, b) => {
                     if (a.Id > b.Id)
                         return 1;
                     else
                         return -1;
                 });*///usefull only when creating exel files manually
                this.bindingSource2.DataSource = list;


                dataGridView1.DataSource = this.bindingSource2;
                if (!alexiknow)
                {
                    richTextBox1.DataBindings.Add("Text", bindingSource2, "GradersResult");
                    richTextBox2.DataBindings.Add("Text", bindingSource2, "Products");
                    richTextBox3.DataBindings.Add("Text", bindingSource2, "NutValues");
                    richTextBox4.DataBindings.Add("Text", bindingSource2, "MinScores");
                    richTextBox5.DataBindings.Add("Text", bindingSource2, "MaxScores");
                }
                manager.TakeTime("setting data source and rich text data binding");
                alexiknow = true;
                manager.End();
            }
            
            this.labelConsole.Text = string.Format("[{0}] Process took: {1} ms", DateTime.Now.ToShortTimeString(), manager.TotalTime());

            //MessageBox.Show(manager.ToString());
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

        private void button2_Click(object sender, EventArgs e)
        {
            try
            { 
                MealsCSVReader.CreateFixedMealsList(unit);
            }
            catch  (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox4.Text)
            {
                case "Fixed":
                    useDBRecipes = false;
                    useTestsRecipes = true;
                    break;
                case "Internet":
                    useDBRecipes = true;
                    useTestsRecipes = false;
                    break;
                case "Both":
                    useDBRecipes = true;
                    useTestsRecipes = true;
                    break;
            }
        }

        private void LogicRunnerForm_Load(object sender, EventArgs e) {

        }

        private void label13_Click(object sender, EventArgs e) {

        }

        private void recommendationsNum_SelectedIndexChanged(object sender, EventArgs e) {

        }

        private void minMax_SelectedIndexChanged(object sender, EventArgs e) {
          //  if (this.alexiknow) {
                if (minMax.SelectedItem.ToString() == "max") {
                    richTextBox4.Visible = false;
                    richTextBox5.Visible = true;
                }
                else {
                    richTextBox4.Visible = true;
                    richTextBox5.Visible = false;
                }
            //    }
        }
    }
}

