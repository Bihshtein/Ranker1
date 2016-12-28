using RecommendationBuilder;
using RecommendationBuilder.Graders;
using RestModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LogicRunner
{
    public partial class LogicRunnerForm : Form
    {
        public RestDBInterface unit = null;
        bool alexiknow = false;
        public LogicRunnerForm()
        {
            InitializeComponent();


            unit = new RestDBInterface();
     
            var dv = unit.DailyValues.GetAllList();

            this.bindingSource1.DataSource = dv;
            comboBox1.DataSource = bindingSource1.DataSource;
            dataGridView1.AutoGenerateColumns = true;

            comboBox2.DataSource = Enum.GetNames(typeof(MealType));
            caloreis.DataSource = new List<int> { 500, 1000, 1500, 2000, 2500, 3000, 3500, 4000, 4500, 5000 };
            calories.DataSource = new List<int> { 1, 2, 3, 3, 4, 5, 6, 7, 8, 9,10 };
            cookTime.DataSource = new List<int> { 1, 2, 3, 3, 4, 5, 6, 7, 8, 9, 10 };
            minValues.DataSource = new List<int> { 1, 2, 3, 3, 4, 5, 6, 7, 8, 9, 10 };
            maxValues.DataSource = new List<int> { 1, 2, 3, 3, 4, 5, 6, 7, 8, 9, 10 };
            servings.DataSource = new List<int> { 1, 2, 3, 3, 4, 5, 6, 7, 8, 9, 10 };
            idealServings.DataSource = new List<int> { 1, 2, 3, 3, 4, 5, 6, 7, 8, 9, 10 };

            caloreis.SelectedIndex = 5;
            idealServings.SelectedIndex = 3;
            maxValues.SelectedIndex = 5;
            minValues.SelectedIndex =3;
            servings.SelectedIndex = 2;
            calories.SelectedIndex = 3;
          
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var uProfile = new UserProfile()
            {
                Id = 777,
                Age = 27,
                Gender = GenderType.Male,
                Pregnant = 0,
                Name = "Hen"
            };

            var recommendationDB = RecommendationDBGenerator.FromUserProfile(uProfile, unit);
            recommendationDB.idealServingsNum = int.Parse(idealServings.SelectedItem.ToString());
            recommendationDB.dailyValues = (comboBox1.SelectedValue as DailyValue).DuplicateDictionary();
            recommendationDB.range = new MealSuggestionRange() { Length = 1, MealType = (MealType)Enum.Parse(typeof(MealType), comboBox2.SelectedItem.ToString()) };
            recommendationDB.dailyCaloriesNum = int.Parse(caloreis.SelectedItem.ToString());
              recommendationDB.GradersWeight = new Dictionary<GraderType, double>()
               {
                    // Meal graders
                    {GraderType.CaloriesCountMealGrader, int.Parse(calories.SelectedItem.ToString())},
                    {GraderType.MinNutValuesMealGrader,  int.Parse(minValues.SelectedItem.ToString())},
                      {GraderType.MaxNutValuesMealGrader,  int.Parse(maxValues.SelectedItem.ToString())},
                      {GraderType.ServingsNumMealGrader,  int.Parse(servings.SelectedItem.ToString())},
                    {GraderType.PrepTimeMealGrader, int.Parse(cookTime.SelectedItem.ToString()) }
                };
            RecommendationGenerator generator = new RecommendationGenerator(unit, recommendationDB);
            this.bindingSource2.DataSource = generator.GetMealsList().Select(o => new MyViewModel(o)
            { Id = o.Meal.ID, Name = o.Meal.Name, 
                NutValues = parseNutValues(o.NutValues), GradersResult = parseGradersResult(o.GradeInfo.GradersInfo) }).ToList();
            
            dataGridView1.DataSource = this.bindingSource2;

            if (!alexiknow)
            {
                richTextBox1.DataBindings.Add("Text", bindingSource2, "GradersResult");
                richTextBox2.DataBindings.Add("Text", bindingSource2, "Products");
                richTextBox3.DataBindings.Add("Text", bindingSource2, "NutValues");
                alexiknow = true;
            }
        }

        private string parseNutValues(Dictionary<string, double> let)
        {
            var str = "";

            foreach (var x in let)
            {
                str += string.Format("{0} = {1}\n", x.Key, x.Value);
            }
            return str;
        }

        private string parseGradersResult(HashSet<GraderInfo> gradersInfo)
        {
            var str = "";

            foreach (var x in gradersInfo)
            {
                str += string.Format("{0}: weight = {1}, grade = {2}\n", x.Type.ToString(), x.Weight, x.Grade.ToString("N3"));
            }
            return str;
        }

        private void button2_Click(object sender, EventArgs e) {
               try { 
                MealsCSVReader.CreateFixedMealsList(unit);
                }
                catch  (Exception ex) {
                    MessageBox.Show(ex.Message);
                }
        }
    }
}
