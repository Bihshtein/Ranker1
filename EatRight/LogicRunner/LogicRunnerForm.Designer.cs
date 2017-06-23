namespace LogicRunner
{
    partial class LogicRunnerForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LogicRunnerForm));
            this.button1 = new System.Windows.Forms.Button();
            this.ageGender = new System.Windows.Forms.ComboBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.bindingSource2 = new System.Windows.Forms.BindingSource(this.components);
            this.mealType = new System.Windows.Forms.ComboBox();
            this.comboBox4 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.calories = new System.Windows.Forms.ComboBox();
            this.cookTime = new System.Windows.Forms.ComboBox();
            this.minValues = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.richTextBox2 = new System.Windows.Forms.RichTextBox();
            this.richTextBox3 = new System.Windows.Forms.RichTextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.maxValues = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.servings = new System.Windows.Forms.ComboBox();
            this.label12 = new System.Windows.Forms.Label();
            this.idealServings = new System.Windows.Forms.ComboBox();
            this.label13 = new System.Windows.Forms.Label();
            this.recommendationsNum = new System.Windows.Forms.ComboBox();
            this.label15 = new System.Windows.Forms.Label();
            this.Meat = new System.Windows.Forms.CheckBox();
            this.NoFish = new System.Windows.Forms.CheckBox();
            this.NoMeat = new System.Windows.Forms.CheckBox();
            this.Dairy = new System.Windows.Forms.CheckBox();
            this.Fish = new System.Windows.Forms.CheckBox();
            this.NoDairy = new System.Windows.Forms.CheckBox();
            this.Seafood = new System.Windows.Forms.CheckBox();
            this.NoSeafood = new System.Windows.Forms.CheckBox();
            this.custom = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.richTextBox4 = new System.Windows.Forms.RichTextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.workMode = new System.Windows.Forms.ComboBox();
            this.Kosher = new System.Windows.Forms.CheckBox();
            this.Vegan = new System.Windows.Forms.CheckBox();
            this.Vegetarian = new System.Windows.Forms.CheckBox();
            this.Pescetarian = new System.Windows.Forms.CheckBox();
            this.txtCalories = new System.Windows.Forms.TextBox();
            this.activityLevel = new System.Windows.Forms.ComboBox();
            this.label20 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.txtHeight = new System.Windows.Forms.TextBox();
            this.txtWeight = new System.Windows.Forms.TextBox();
            this.minMax = new System.Windows.Forms.ComboBox();
            this.label19 = new System.Windows.Forms.Label();
            this.richTextBox5 = new System.Windows.Forms.RichTextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.total = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.steps = new System.Windows.Forms.ComboBox();
            this.button4 = new System.Windows.Forms.Button();
            this.label24 = new System.Windows.Forms.Label();
            this.weekday = new System.Windows.Forms.ComboBox();
            this.label25 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.richTextBox6 = new System.Windows.Forms.RichTextBox();
            this.label26 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.LimeGreen;
            this.button1.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.button1.Location = new System.Drawing.Point(1005, 86);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(128, 54);
            this.button1.TabIndex = 0;
            this.button1.Text = "Recommend on screen";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // ageGender
            // 
            this.ageGender.FormattingEnabled = true;
            this.ageGender.Location = new System.Drawing.Point(136, 30);
            this.ageGender.Name = "ageGender";
            this.ageGender.Size = new System.Drawing.Size(89, 21);
            this.ageGender.TabIndex = 1;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(11, 241);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(584, 758);
            this.dataGridView1.TabIndex = 2;
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(612, 876);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(263, 123);
            this.richTextBox1.TabIndex = 4;
            this.richTextBox1.Text = "";
            // 
            // mealType
            // 
            this.mealType.FormattingEnabled = true;
            this.mealType.Location = new System.Drawing.Point(398, 30);
            this.mealType.Name = "mealType";
            this.mealType.Size = new System.Drawing.Size(89, 21);
            this.mealType.TabIndex = 5;
            // 
            // comboBox4
            // 
            this.comboBox4.FormattingEnabled = true;
            this.comboBox4.Location = new System.Drawing.Point(596, 161);
            this.comboBox4.Name = "comboBox4";
            this.comboBox4.Size = new System.Drawing.Size(89, 21);
            this.comboBox4.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label1.Location = new System.Drawing.Point(279, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 16);
            this.label1.TabIndex = 7;
            this.label1.Text = "Meal Type";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label2.Location = new System.Drawing.Point(17, 31);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 16);
            this.label2.TabIndex = 10;
            this.label2.Text = "Age+Gender ";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label5.Location = new System.Drawing.Point(788, 73);
            this.label5.Name = "label5";
            this.label5.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label5.Size = new System.Drawing.Size(94, 16);
            this.label5.TabIndex = 13;
            this.label5.Text = "Min Nutrients";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label4.Location = new System.Drawing.Point(788, 41);
            this.label4.Name = "label4";
            this.label4.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label4.Size = new System.Drawing.Size(59, 16);
            this.label4.TabIndex = 12;
            this.label4.Text = "Calories";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label3.Location = new System.Drawing.Point(788, 9);
            this.label3.Name = "label3";
            this.label3.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label3.Size = new System.Drawing.Size(91, 16);
            this.label3.TabIndex = 11;
            this.label3.Text = "Cooking Time";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.BackColor = System.Drawing.Color.Transparent;
            this.label14.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label14.Location = new System.Drawing.Point(598, 12);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(81, 16);
            this.label14.TabIndex = 7;
            this.label14.Text = "Peferences";
            // 
            // calories
            // 
            this.calories.FormattingEnabled = true;
            this.calories.Location = new System.Drawing.Point(899, 38);
            this.calories.Name = "calories";
            this.calories.Size = new System.Drawing.Size(89, 21);
            this.calories.TabIndex = 17;
            // 
            // cookTime
            // 
            this.cookTime.FormattingEnabled = true;
            this.cookTime.Location = new System.Drawing.Point(899, 7);
            this.cookTime.Name = "cookTime";
            this.cookTime.Size = new System.Drawing.Size(89, 21);
            this.cookTime.TabIndex = 16;
            // 
            // minValues
            // 
            this.minValues.FormattingEnabled = true;
            this.minValues.Location = new System.Drawing.Point(899, 70);
            this.minValues.Name = "minValues";
            this.minValues.Size = new System.Drawing.Size(89, 21);
            this.minValues.TabIndex = 18;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label6.Location = new System.Drawing.Point(280, 72);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(59, 16);
            this.label6.TabIndex = 20;
            this.label6.Text = "Calories";
            // 
            // richTextBox2
            // 
            this.richTextBox2.Location = new System.Drawing.Point(612, 649);
            this.richTextBox2.Name = "richTextBox2";
            this.richTextBox2.Size = new System.Drawing.Size(263, 207);
            this.richTextBox2.TabIndex = 21;
            this.richTextBox2.Text = "";
            // 
            // richTextBox3
            // 
            this.richTextBox3.Location = new System.Drawing.Point(878, 242);
            this.richTextBox3.Name = "richTextBox3";
            this.richTextBox3.Size = new System.Drawing.Size(253, 206);
            this.richTextBox3.TabIndex = 22;
            this.richTextBox3.Text = "";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label7.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label7.Location = new System.Drawing.Point(715, 859);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(59, 16);
            this.label7.TabIndex = 23;
            this.label7.Text = "Graders";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label8.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label8.Location = new System.Drawing.Point(704, 630);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(66, 16);
            this.label8.TabIndex = 24;
            this.label8.Text = "Products";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label9.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label9.Location = new System.Drawing.Point(951, 223);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(121, 16);
            this.label9.TabIndex = 25;
            this.label9.Text = "Highest Nutrients";
            // 
            // maxValues
            // 
            this.maxValues.FormattingEnabled = true;
            this.maxValues.Location = new System.Drawing.Point(899, 103);
            this.maxValues.Name = "maxValues";
            this.maxValues.Size = new System.Drawing.Size(89, 21);
            this.maxValues.TabIndex = 27;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.BackColor = System.Drawing.Color.Transparent;
            this.label10.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label10.Location = new System.Drawing.Point(788, 107);
            this.label10.Name = "label10";
            this.label10.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label10.Size = new System.Drawing.Size(98, 16);
            this.label10.TabIndex = 28;
            this.label10.Text = "Max nutrients";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.BackColor = System.Drawing.Color.Transparent;
            this.label11.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label11.Location = new System.Drawing.Point(790, 136);
            this.label11.Name = "label11";
            this.label11.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label11.Size = new System.Drawing.Size(64, 16);
            this.label11.TabIndex = 30;
            this.label11.Text = "Servings";
            // 
            // servings
            // 
            this.servings.FormattingEnabled = true;
            this.servings.Location = new System.Drawing.Point(899, 135);
            this.servings.Name = "servings";
            this.servings.Size = new System.Drawing.Size(89, 21);
            this.servings.TabIndex = 29;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.BackColor = System.Drawing.Color.Transparent;
            this.label12.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label12.Location = new System.Drawing.Point(280, 110);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(104, 16);
            this.label12.TabIndex = 32;
            this.label12.Text = "Ideal Servings ";
            // 
            // idealServings
            // 
            this.idealServings.FormattingEnabled = true;
            this.idealServings.Location = new System.Drawing.Point(399, 106);
            this.idealServings.Name = "idealServings";
            this.idealServings.Size = new System.Drawing.Size(89, 21);
            this.idealServings.TabIndex = 31;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.BackColor = System.Drawing.Color.Transparent;
            this.label13.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label13.Location = new System.Drawing.Point(281, 147);
            this.label13.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(75, 16);
            this.label13.TabIndex = 35;
            this.label13.Text = "NumOffers";
            this.label13.Click += new System.EventHandler(this.label13_Click);
            // 
            // recommendationsNum
            // 
            this.recommendationsNum.FormattingEnabled = true;
            this.recommendationsNum.Location = new System.Drawing.Point(399, 147);
            this.recommendationsNum.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.recommendationsNum.Name = "recommendationsNum";
            this.recommendationsNum.Size = new System.Drawing.Size(89, 21);
            this.recommendationsNum.TabIndex = 34;
            this.recommendationsNum.SelectedIndexChanged += new System.EventHandler(this.recommendationsNum_SelectedIndexChanged);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.BackColor = System.Drawing.Color.Transparent;
            this.label15.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label15.Location = new System.Drawing.Point(595, 143);
            this.label15.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(92, 16);
            this.label15.TabIndex = 36;
            this.label15.Text = "Recipes from";
            // 
            // Meat
            // 
            this.Meat.AutoSize = true;
            this.Meat.Location = new System.Drawing.Point(541, 40);
            this.Meat.Name = "Meat";
            this.Meat.Size = new System.Drawing.Size(50, 17);
            this.Meat.TabIndex = 37;
            this.Meat.Text = "Meat";
            this.Meat.UseVisualStyleBackColor = true;
            // 
            // NoFish
            // 
            this.NoFish.AutoSize = true;
            this.NoFish.Location = new System.Drawing.Point(604, 66);
            this.NoFish.Name = "NoFish";
            this.NoFish.Size = new System.Drawing.Size(62, 17);
            this.NoFish.TabIndex = 38;
            this.NoFish.Text = "No Fish";
            this.NoFish.UseVisualStyleBackColor = true;
            // 
            // NoMeat
            // 
            this.NoMeat.AutoSize = true;
            this.NoMeat.Location = new System.Drawing.Point(604, 40);
            this.NoMeat.Name = "NoMeat";
            this.NoMeat.Size = new System.Drawing.Size(67, 17);
            this.NoMeat.TabIndex = 39;
            this.NoMeat.Text = "No Meat";
            this.NoMeat.UseVisualStyleBackColor = true;
            // 
            // Dairy
            // 
            this.Dairy.AutoSize = true;
            this.Dairy.Location = new System.Drawing.Point(541, 95);
            this.Dairy.Name = "Dairy";
            this.Dairy.Size = new System.Drawing.Size(50, 17);
            this.Dairy.TabIndex = 40;
            this.Dairy.Text = "Dairy";
            this.Dairy.UseVisualStyleBackColor = true;
            // 
            // Fish
            // 
            this.Fish.AutoSize = true;
            this.Fish.Location = new System.Drawing.Point(541, 66);
            this.Fish.Name = "Fish";
            this.Fish.Size = new System.Drawing.Size(45, 17);
            this.Fish.TabIndex = 41;
            this.Fish.Text = "Fish";
            this.Fish.UseVisualStyleBackColor = true;
            // 
            // NoDairy
            // 
            this.NoDairy.AutoSize = true;
            this.NoDairy.Location = new System.Drawing.Point(604, 95);
            this.NoDairy.Name = "NoDairy";
            this.NoDairy.Size = new System.Drawing.Size(67, 17);
            this.NoDairy.TabIndex = 42;
            this.NoDairy.Text = "No Dairy";
            this.NoDairy.UseVisualStyleBackColor = true;
            // 
            // Seafood
            // 
            this.Seafood.AutoSize = true;
            this.Seafood.Location = new System.Drawing.Point(541, 121);
            this.Seafood.Name = "Seafood";
            this.Seafood.Size = new System.Drawing.Size(66, 17);
            this.Seafood.TabIndex = 43;
            this.Seafood.Text = "Seafood";
            this.Seafood.UseVisualStyleBackColor = true;
            // 
            // NoSeafood
            // 
            this.NoSeafood.AutoSize = true;
            this.NoSeafood.Location = new System.Drawing.Point(604, 120);
            this.NoSeafood.Name = "NoSeafood";
            this.NoSeafood.Size = new System.Drawing.Size(83, 17);
            this.NoSeafood.TabIndex = 44;
            this.NoSeafood.Text = "No Seafood";
            this.NoSeafood.UseVisualStyleBackColor = true;
            // 
            // custom
            // 
            this.custom.Location = new System.Drawing.Point(137, 188);
            this.custom.Name = "custom";
            this.custom.Size = new System.Drawing.Size(851, 20);
            this.custom.TabIndex = 45;
            this.custom.Text = "if empty will take all the recipes in the file (used only in debug work mode)";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.BackColor = System.Drawing.Color.Transparent;
            this.label16.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label16.Location = new System.Drawing.Point(20, 190);
            this.label16.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(114, 16);
            this.label16.TabIndex = 46;
            this.label16.Text = "Custom Recipes ";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.BackColor = System.Drawing.Color.Transparent;
            this.label17.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label17.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label17.Location = new System.Drawing.Point(640, 222);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(116, 16);
            this.label17.TabIndex = 48;
            this.label17.Text = "Nutrients Scores";
            // 
            // richTextBox4
            // 
            this.richTextBox4.Location = new System.Drawing.Point(612, 241);
            this.richTextBox4.Name = "richTextBox4";
            this.richTextBox4.Size = new System.Drawing.Size(263, 384);
            this.richTextBox4.TabIndex = 47;
            this.richTextBox4.Text = "";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.BackColor = System.Drawing.Color.Transparent;
            this.label18.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label18.Location = new System.Drawing.Point(1027, 45);
            this.label18.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(77, 16);
            this.label18.TabIndex = 50;
            this.label18.Text = "WorkMode";
            // 
            // workMode
            // 
            this.workMode.FormattingEnabled = true;
            this.workMode.Location = new System.Drawing.Point(1020, 62);
            this.workMode.Name = "workMode";
            this.workMode.Size = new System.Drawing.Size(89, 21);
            this.workMode.TabIndex = 49;
            // 
            // Kosher
            // 
            this.Kosher.AutoSize = true;
            this.Kosher.Location = new System.Drawing.Point(684, 120);
            this.Kosher.Name = "Kosher";
            this.Kosher.Size = new System.Drawing.Size(59, 17);
            this.Kosher.TabIndex = 56;
            this.Kosher.Text = "Kosher";
            this.Kosher.UseVisualStyleBackColor = true;
            // 
            // Vegan
            // 
            this.Vegan.AutoSize = true;
            this.Vegan.Location = new System.Drawing.Point(684, 95);
            this.Vegan.Name = "Vegan";
            this.Vegan.Size = new System.Drawing.Size(57, 17);
            this.Vegan.TabIndex = 55;
            this.Vegan.Text = "Vegan";
            this.Vegan.UseVisualStyleBackColor = true;
            // 
            // Vegetarian
            // 
            this.Vegetarian.AutoSize = true;
            this.Vegetarian.Location = new System.Drawing.Point(684, 40);
            this.Vegetarian.Name = "Vegetarian";
            this.Vegetarian.Size = new System.Drawing.Size(77, 17);
            this.Vegetarian.TabIndex = 54;
            this.Vegetarian.Text = "Vegetarian";
            this.Vegetarian.UseVisualStyleBackColor = true;
            // 
            // Pescetarian
            // 
            this.Pescetarian.AutoSize = true;
            this.Pescetarian.Location = new System.Drawing.Point(684, 66);
            this.Pescetarian.Name = "Pescetarian";
            this.Pescetarian.Size = new System.Drawing.Size(82, 17);
            this.Pescetarian.TabIndex = 53;
            this.Pescetarian.Text = "Pescetarian";
            this.Pescetarian.UseVisualStyleBackColor = true;
            // 
            // txtCalories
            // 
            this.txtCalories.Location = new System.Drawing.Point(398, 69);
            this.txtCalories.Name = "txtCalories";
            this.txtCalories.ReadOnly = true;
            this.txtCalories.Size = new System.Drawing.Size(90, 20);
            this.txtCalories.TabIndex = 57;
            this.txtCalories.Text = "3000";
            // 
            // activityLevel
            // 
            this.activityLevel.FormattingEnabled = true;
            this.activityLevel.Location = new System.Drawing.Point(137, 146);
            this.activityLevel.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.activityLevel.Name = "activityLevel";
            this.activityLevel.Size = new System.Drawing.Size(89, 21);
            this.activityLevel.TabIndex = 58;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.BackColor = System.Drawing.Color.Transparent;
            this.label20.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label20.Location = new System.Drawing.Point(19, 145);
            this.label20.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(97, 16);
            this.label20.TabIndex = 59;
            this.label20.Text = "Activity Level";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.BackColor = System.Drawing.Color.Transparent;
            this.label21.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label21.Location = new System.Drawing.Point(18, 68);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(50, 16);
            this.label21.TabIndex = 61;
            this.label21.Text = "Height";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.BackColor = System.Drawing.Color.Transparent;
            this.label22.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label22.Location = new System.Drawing.Point(18, 106);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(54, 16);
            this.label22.TabIndex = 63;
            this.label22.Text = "Weight";
            // 
            // txtHeight
            // 
            this.txtHeight.Location = new System.Drawing.Point(136, 67);
            this.txtHeight.Name = "txtHeight";
            this.txtHeight.Size = new System.Drawing.Size(90, 20);
            this.txtHeight.TabIndex = 64;
            this.txtHeight.Text = "175";
            // 
            // txtWeight
            // 
            this.txtWeight.Location = new System.Drawing.Point(137, 106);
            this.txtWeight.Name = "txtWeight";
            this.txtWeight.Size = new System.Drawing.Size(90, 20);
            this.txtWeight.TabIndex = 65;
            this.txtWeight.Text = "70";
            // 
            // minMax
            // 
            this.minMax.FormattingEnabled = true;
            this.minMax.Location = new System.Drawing.Point(757, 220);
            this.minMax.Name = "minMax";
            this.minMax.Size = new System.Drawing.Size(89, 21);
            this.minMax.TabIndex = 66;
            this.minMax.SelectedIndexChanged += new System.EventHandler(this.minMax_SelectedIndexChanged);
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.BackColor = System.Drawing.Color.Transparent;
            this.label19.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label19.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label19.Location = new System.Drawing.Point(231, 221);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(105, 16);
            this.label19.TabIndex = 67;
            this.label19.Text = "Recipes scores";
            // 
            // richTextBox5
            // 
            this.richTextBox5.Location = new System.Drawing.Point(611, 241);
            this.richTextBox5.Name = "richTextBox5";
            this.richTextBox5.Size = new System.Drawing.Size(263, 384);
            this.richTextBox5.TabIndex = 68;
            this.richTextBox5.Text = "";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(0, 0);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 0;
            // 
            // total
            // 
            this.total.AutoSize = true;
            this.total.BackColor = System.Drawing.Color.Transparent;
            this.total.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.total.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.total.Location = new System.Drawing.Point(579, 223);
            this.total.Name = "total";
            this.total.Size = new System.Drawing.Size(16, 16);
            this.total.TabIndex = 70;
            this.total.Text = "0";
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.BackColor = System.Drawing.Color.Transparent;
            this.label23.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label23.Location = new System.Drawing.Point(790, 165);
            this.label23.Name = "label23";
            this.label23.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label23.Size = new System.Drawing.Size(45, 16);
            this.label23.TabIndex = 72;
            this.label23.Text = "Steps";
            // 
            // steps
            // 
            this.steps.FormattingEnabled = true;
            this.steps.Location = new System.Drawing.Point(899, 164);
            this.steps.Name = "steps";
            this.steps.Size = new System.Drawing.Size(89, 21);
            this.steps.TabIndex = 71;
            // 
            // button4
            // 
            this.button4.BackColor = System.Drawing.Color.Red;
            this.button4.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.button4.Location = new System.Drawing.Point(1005, 143);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(128, 63);
            this.button4.TabIndex = 73;
            this.button4.Text = "Recommend to users";
            this.button4.UseVisualStyleBackColor = false;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.BackColor = System.Drawing.Color.Transparent;
            this.label24.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label24.Location = new System.Drawing.Point(1030, 3);
            this.label24.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(68, 16);
            this.label24.TabIndex = 75;
            this.label24.Text = "Weekday";
            // 
            // weekday
            // 
            this.weekday.FormattingEnabled = true;
            this.weekday.Location = new System.Drawing.Point(1021, 20);
            this.weekday.Name = "weekday";
            this.weekday.Size = new System.Drawing.Size(89, 21);
            this.weekday.TabIndex = 74;
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.BackColor = System.Drawing.Color.Transparent;
            this.label25.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label25.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label25.Location = new System.Drawing.Point(928, 691);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(170, 16);
            this.label25.TabIndex = 76;
            this.label25.Text = "Simulate Exclude Product";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(883, 710);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(250, 20);
            this.textBox1.TabIndex = 77;
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.PapayaWhip;
            this.button2.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.button2.Location = new System.Drawing.Point(942, 736);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(130, 86);
            this.button2.TabIndex = 78;
            this.button2.Text = "Re-calcualte highest nutrients";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // richTextBox6
            // 
            this.richTextBox6.Location = new System.Drawing.Point(880, 470);
            this.richTextBox6.Name = "richTextBox6";
            this.richTextBox6.Size = new System.Drawing.Size(253, 206);
            this.richTextBox6.TabIndex = 79;
            this.richTextBox6.Text = "";
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.BackColor = System.Drawing.Color.Transparent;
            this.label26.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label26.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label26.Location = new System.Drawing.Point(928, 451);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(157, 16);
            this.label26.TabIndex = 80;
            this.label26.Text = "Highest after exclusion";
            // 
            // LogicRunnerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.ClientSize = new System.Drawing.Size(1160, 1019);
            this.Controls.Add(this.label26);
            this.Controls.Add(this.richTextBox6);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label25);
            this.Controls.Add(this.label24);
            this.Controls.Add(this.weekday);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.label23);
            this.Controls.Add(this.steps);
            this.Controls.Add(this.total);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.richTextBox5);
            this.Controls.Add(this.label19);
            this.Controls.Add(this.minMax);
            this.Controls.Add(this.txtWeight);
            this.Controls.Add(this.txtHeight);
            this.Controls.Add(this.label22);
            this.Controls.Add(this.label21);
            this.Controls.Add(this.label20);
            this.Controls.Add(this.activityLevel);
            this.Controls.Add(this.txtCalories);
            this.Controls.Add(this.Kosher);
            this.Controls.Add(this.Vegan);
            this.Controls.Add(this.Vegetarian);
            this.Controls.Add(this.Pescetarian);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.workMode);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.richTextBox4);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.custom);
            this.Controls.Add(this.NoSeafood);
            this.Controls.Add(this.Seafood);
            this.Controls.Add(this.NoDairy);
            this.Controls.Add(this.Fish);
            this.Controls.Add(this.Dairy);
            this.Controls.Add(this.NoMeat);
            this.Controls.Add(this.NoFish);
            this.Controls.Add(this.Meat);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.recommendationsNum);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.idealServings);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.servings);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.maxValues);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.richTextBox3);
            this.Controls.Add(this.richTextBox2);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.minValues);
            this.Controls.Add(this.calories);
            this.Controls.Add(this.cookTime);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.mealType);
            this.Controls.Add(this.comboBox4);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.ageGender);
            this.Controls.Add(this.button1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "LogicRunnerForm";
            this.Text = "Nutritionist Helper";
            this.Load += new System.EventHandler(this.LogicRunnerForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ComboBox ageGender;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.BindingSource bindingSource2;
        private System.Windows.Forms.ComboBox mealType;
        private System.Windows.Forms.ComboBox comboBox4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.ComboBox calories;
        private System.Windows.Forms.ComboBox cookTime;
        private System.Windows.Forms.ComboBox minValues;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.RichTextBox richTextBox2;
        private System.Windows.Forms.RichTextBox richTextBox3;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.BindingSource bindingSource1;
        private System.Windows.Forms.ComboBox maxValues;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox servings;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.ComboBox idealServings;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.ComboBox recommendationsNum;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.CheckBox Meat;
        private System.Windows.Forms.CheckBox NoFish;
        private System.Windows.Forms.CheckBox NoMeat;
        private System.Windows.Forms.CheckBox Dairy;
        private System.Windows.Forms.CheckBox Fish;
        private System.Windows.Forms.CheckBox NoDairy;
        private System.Windows.Forms.CheckBox Seafood;
        private System.Windows.Forms.CheckBox NoSeafood;
        private System.Windows.Forms.TextBox custom;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.RichTextBox richTextBox4;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.ComboBox workMode;
        private System.Windows.Forms.CheckBox Kosher;
        private System.Windows.Forms.CheckBox Vegan;
        private System.Windows.Forms.CheckBox Vegetarian;
        private System.Windows.Forms.CheckBox Pescetarian;
        private System.Windows.Forms.TextBox txtCalories;
        private System.Windows.Forms.ComboBox activityLevel;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.TextBox txtHeight;
        private System.Windows.Forms.TextBox txtWeight;
        private System.Windows.Forms.ComboBox minMax;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.RichTextBox richTextBox5;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label total;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.ComboBox steps;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.ComboBox weekday;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.RichTextBox richTextBox6;
        private System.Windows.Forms.Label label26;
    }
}