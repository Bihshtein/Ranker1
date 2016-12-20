using MenuBuilder;
using MenuBuilder.Graders;
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
            MealsCSVReader.CreateFixedMealsList(unit);
            var dv = unit.DailyValues.GetAllList();

            this.bindingSource1.DataSource = dv;
            comboBox1.DataSource = bindingSource1.DataSource;
            // comboBox1.DisplayMember = "Age";
            //  comboBox1.ValueMember = "Age";
            dataGridView1.AutoGenerateColumns = true;

            comboBox2.DataSource = Enum.GetNames(typeof(MealType));
            comboBox5.DataSource = new List<int> { 500, 1000, 1500, 2000, 2500, 3000, 3500, 4000, 4500, 5000 };
            comboBox3.DataSource = new List<int> { 1, 2, 3, 3, 4, 5, 6, 7, 8, 9,10 };
            comboBox4.DataSource = new List<int> { 1, 2, 3, 3, 4, 5, 6, 7, 8, 9, 10 };
            comboBox6.DataSource = new List<int> { 1, 2, 3, 3, 4, 5, 6, 7, 8, 9, 10 };
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

            /*
            grid.DataSource = objects.Select(o => new MyViewModel(o)
            { Column1 = o.SomeValue, Column2 = o.SomeOtherValue }).ToList();
            */
            var graderDB = GraderDBGenerator.FromUserProfile(uProfile, unit);
            graderDB.dailyValues = (comboBox1.SelectedValue as DailyValue).DuplicateDictionary();
            graderDB.range = new MealSuggestionRange() { Length = 1, MealType = (MealType)Enum.Parse(typeof(MealType), comboBox2.SelectedItem.ToString()) };
            graderDB.dailyCaloriesNum = int.Parse(comboBox5.SelectedItem.ToString());
              graderDB.GradersWeight = new Dictionary<GraderType, double>()
               {
                    // Meal graders
                    {GraderType.CaloriesCountMealGrader, int.Parse(comboBox3.SelectedItem.ToString())},
                    {GraderType.NutValuesMealGrader,  int.Parse(comboBox6.SelectedItem.ToString())},
                    {GraderType.PrepTimeMealGrader, int.Parse(comboBox4.SelectedItem.ToString()) }
                };
            MenuGenerator generator = new MenuGenerator(unit, graderDB);


          
            /*  
            var bindingList = new BindingList<MenuMeal>(menu.Days[0].Meals.Values.ToList());
            var source = new BindingSource(bindingList, null);
            */
            //     grid.DataSource = source;

            this.bindingSource2.DataSource = generator.GetMealsList().Select(o => new MyViewModel(o)
            { Id = o.Meal.ID, Name = o.Meal.Name, Grade = o.Grade,
                NutValues = parseNutValues(o.NutValues), GradersResult = parseGradersResult(o.GradeInfo.GradersInfo) }).ToList();

            dataGridView1.DataSource = this.bindingSource2;

            if (!alexiknow)
            {
                richTextBox1.DataBindings.Add("Text", bindingSource2, "GradersResult");
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
                str += string.Format("{0}: weight = {1}, grade = {2}\n", x.Type.ToString(), x.Weight, x.Grade);
            }
            return str;
        }
    }
}
