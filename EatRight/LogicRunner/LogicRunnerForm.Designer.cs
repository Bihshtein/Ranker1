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
            this.totalCalories = new System.Windows.Forms.ComboBox();
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
            this.labelConsole = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.recommendationsNum = new System.Windows.Forms.ComboBox();
            this.label15 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
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
            this.label19 = new System.Windows.Forms.Label();
            this.richTextBox5 = new System.Windows.Forms.RichTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.LimeGreen;
            this.button1.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.button1.Location = new System.Drawing.Point(753, 130);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(128, 30);
            this.button1.TabIndex = 0;
            this.button1.Text = "Recommend";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // ageGender
            // 
            this.ageGender.FormattingEnabled = true;
            this.ageGender.Location = new System.Drawing.Point(178, 16);
            this.ageGender.Name = "ageGender";
            this.ageGender.Size = new System.Drawing.Size(89, 21);
            this.ageGender.TabIndex = 1;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(11, 250);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(584, 692);
            this.dataGridView1.TabIndex = 2;
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(819, 632);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(263, 253);
            this.richTextBox1.TabIndex = 4;
            this.richTextBox1.Text = "";
            // 
            // mealType
            // 
            this.mealType.FormattingEnabled = true;
            this.mealType.Location = new System.Drawing.Point(178, 49);
            this.mealType.Name = "mealType";
            this.mealType.Size = new System.Drawing.Size(89, 21);
            this.mealType.TabIndex = 5;
            // 
            // comboBox4
            // 
            this.comboBox4.FormattingEnabled = true;
            this.comboBox4.Location = new System.Drawing.Point(769, 41);
            this.comboBox4.Name = "comboBox4";
            this.comboBox4.Size = new System.Drawing.Size(89, 21);
            this.comboBox4.TabIndex = 5;
            this.comboBox4.SelectedIndexChanged += new System.EventHandler(this.comboBox4_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label1.Location = new System.Drawing.Point(7, 51);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(93, 19);
            this.label1.TabIndex = 7;
            this.label1.Text = "Meal Type";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label2.Location = new System.Drawing.Point(8, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(118, 19);
            this.label2.TabIndex = 10;
            this.label2.Text = "Age+Gender ";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label5.Location = new System.Drawing.Point(486, 83);
            this.label5.Name = "label5";
            this.label5.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label5.Size = new System.Drawing.Size(119, 19);
            this.label5.TabIndex = 13;
            this.label5.Text = "Min Nutrients";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label4.Location = new System.Drawing.Point(486, 52);
            this.label4.Name = "label4";
            this.label4.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label4.Size = new System.Drawing.Size(75, 19);
            this.label4.TabIndex = 12;
            this.label4.Text = "Calories";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label3.Location = new System.Drawing.Point(487, 17);
            this.label3.Name = "label3";
            this.label3.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label3.Size = new System.Drawing.Size(121, 19);
            this.label3.TabIndex = 11;
            this.label3.Text = "Cooking Time";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.BackColor = System.Drawing.Color.Transparent;
            this.label14.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label14.Location = new System.Drawing.Point(329, 14);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(99, 19);
            this.label14.TabIndex = 7;
            this.label14.Text = "Peferences";
            // 
            // calories
            // 
            this.calories.FormattingEnabled = true;
            this.calories.Location = new System.Drawing.Point(616, 51);
            this.calories.Name = "calories";
            this.calories.Size = new System.Drawing.Size(89, 21);
            this.calories.TabIndex = 17;
            // 
            // cookTime
            // 
            this.cookTime.FormattingEnabled = true;
            this.cookTime.Location = new System.Drawing.Point(616, 18);
            this.cookTime.Name = "cookTime";
            this.cookTime.Size = new System.Drawing.Size(89, 21);
            this.cookTime.TabIndex = 16;
            // 
            // minValues
            // 
            this.minValues.FormattingEnabled = true;
            this.minValues.Location = new System.Drawing.Point(616, 83);
            this.minValues.Name = "minValues";
            this.minValues.Size = new System.Drawing.Size(89, 21);
            this.minValues.TabIndex = 18;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label6.Location = new System.Drawing.Point(8, 85);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(75, 19);
            this.label6.TabIndex = 20;
            this.label6.Text = "Calories";
            // 
            // totalCalories
            // 
            this.totalCalories.FormattingEnabled = true;
            this.totalCalories.Location = new System.Drawing.Point(179, 83);
            this.totalCalories.Name = "totalCalories";
            this.totalCalories.Size = new System.Drawing.Size(89, 21);
            this.totalCalories.TabIndex = 19;
            // 
            // richTextBox2
            // 
            this.richTextBox2.Location = new System.Drawing.Point(602, 632);
            this.richTextBox2.Name = "richTextBox2";
            this.richTextBox2.Size = new System.Drawing.Size(211, 253);
            this.richTextBox2.TabIndex = 21;
            this.richTextBox2.Text = "";
            // 
            // richTextBox3
            // 
            this.richTextBox3.Location = new System.Drawing.Point(914, 271);
            this.richTextBox3.Name = "richTextBox3";
            this.richTextBox3.Size = new System.Drawing.Size(151, 336);
            this.richTextBox3.TabIndex = 22;
            this.richTextBox3.Text = "";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label7.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label7.Location = new System.Drawing.Point(941, 610);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(66, 19);
            this.label7.TabIndex = 23;
            this.label7.Text = "Grades";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label8.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label8.Location = new System.Drawing.Point(667, 610);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(80, 19);
            this.label8.TabIndex = 24;
            this.label8.Text = "Products";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label9.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label9.Location = new System.Drawing.Point(923, 249);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(125, 19);
            this.label9.TabIndex = 25;
            this.label9.Text = "Nutrients Sum";
            // 
            // maxValues
            // 
            this.maxValues.FormattingEnabled = true;
            this.maxValues.Location = new System.Drawing.Point(616, 116);
            this.maxValues.Name = "maxValues";
            this.maxValues.Size = new System.Drawing.Size(89, 21);
            this.maxValues.TabIndex = 27;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.BackColor = System.Drawing.Color.Transparent;
            this.label10.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label10.Location = new System.Drawing.Point(486, 117);
            this.label10.Name = "label10";
            this.label10.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label10.Size = new System.Drawing.Size(122, 19);
            this.label10.TabIndex = 28;
            this.label10.Text = "Max nutrients";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.BackColor = System.Drawing.Color.Transparent;
            this.label11.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label11.Location = new System.Drawing.Point(487, 146);
            this.label11.Name = "label11";
            this.label11.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label11.Size = new System.Drawing.Size(78, 19);
            this.label11.TabIndex = 30;
            this.label11.Text = "Servings";
            // 
            // servings
            // 
            this.servings.FormattingEnabled = true;
            this.servings.Location = new System.Drawing.Point(616, 148);
            this.servings.Name = "servings";
            this.servings.Size = new System.Drawing.Size(89, 21);
            this.servings.TabIndex = 29;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.BackColor = System.Drawing.Color.Transparent;
            this.label12.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label12.Location = new System.Drawing.Point(8, 118);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(130, 19);
            this.label12.TabIndex = 32;
            this.label12.Text = "Ideal Servings ";
            // 
            // idealServings
            // 
            this.idealServings.FormattingEnabled = true;
            this.idealServings.Location = new System.Drawing.Point(179, 116);
            this.idealServings.Name = "idealServings";
            this.idealServings.Size = new System.Drawing.Size(89, 21);
            this.idealServings.TabIndex = 31;
            // 
            // labelConsole
            // 
            this.labelConsole.AutoSize = true;
            this.labelConsole.BackColor = System.Drawing.Color.Transparent;
            this.labelConsole.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.labelConsole.Location = new System.Drawing.Point(486, 193);
            this.labelConsole.Name = "labelConsole";
            this.labelConsole.Size = new System.Drawing.Size(53, 16);
            this.labelConsole.TabIndex = 33;
            this.labelConsole.Text = "---------";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.BackColor = System.Drawing.Color.Transparent;
            this.label13.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label13.Location = new System.Drawing.Point(9, 148);
            this.label13.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(159, 19);
            this.label13.TabIndex = 35;
            this.label13.Text = "Recommendations";
            // 
            // recommendationsNum
            // 
            this.recommendationsNum.FormattingEnabled = true;
            this.recommendationsNum.Location = new System.Drawing.Point(179, 150);
            this.recommendationsNum.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.recommendationsNum.Name = "recommendationsNum";
            this.recommendationsNum.Size = new System.Drawing.Size(89, 21);
            this.recommendationsNum.TabIndex = 34;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.BackColor = System.Drawing.Color.Transparent;
            this.label15.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label15.Location = new System.Drawing.Point(759, 19);
            this.label15.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(115, 19);
            this.label15.TabIndex = 36;
            this.label15.Text = "Recipes from";
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.DarkSeaGreen;
            this.button2.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.button2.Location = new System.Drawing.Point(754, 168);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(127, 30);
            this.button2.TabIndex = 26;
            this.button2.Text = "Refresh Manual";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // Meat
            // 
            this.Meat.AutoSize = true;
            this.Meat.Location = new System.Drawing.Point(342, 38);
            this.Meat.Name = "Meat";
            this.Meat.Size = new System.Drawing.Size(50, 17);
            this.Meat.TabIndex = 37;
            this.Meat.Text = "Meat";
            this.Meat.UseVisualStyleBackColor = true;
            // 
            // NoFish
            // 
            this.NoFish.AutoSize = true;
            this.NoFish.Location = new System.Drawing.Point(342, 134);
            this.NoFish.Name = "NoFish";
            this.NoFish.Size = new System.Drawing.Size(62, 17);
            this.NoFish.TabIndex = 38;
            this.NoFish.Text = "No Fish";
            this.NoFish.UseVisualStyleBackColor = true;
            // 
            // NoMeat
            // 
            this.NoMeat.AutoSize = true;
            this.NoMeat.Location = new System.Drawing.Point(342, 116);
            this.NoMeat.Name = "NoMeat";
            this.NoMeat.Size = new System.Drawing.Size(67, 17);
            this.NoMeat.TabIndex = 39;
            this.NoMeat.Text = "No Meat";
            this.NoMeat.UseVisualStyleBackColor = true;
            // 
            // Dairy
            // 
            this.Dairy.AutoSize = true;
            this.Dairy.Location = new System.Drawing.Point(342, 77);
            this.Dairy.Name = "Dairy";
            this.Dairy.Size = new System.Drawing.Size(50, 17);
            this.Dairy.TabIndex = 40;
            this.Dairy.Text = "Dairy";
            this.Dairy.UseVisualStyleBackColor = true;
            // 
            // Fish
            // 
            this.Fish.AutoSize = true;
            this.Fish.Location = new System.Drawing.Point(342, 56);
            this.Fish.Name = "Fish";
            this.Fish.Size = new System.Drawing.Size(45, 17);
            this.Fish.TabIndex = 41;
            this.Fish.Text = "Fish";
            this.Fish.UseVisualStyleBackColor = true;
            // 
            // NoDairy
            // 
            this.NoDairy.AutoSize = true;
            this.NoDairy.Location = new System.Drawing.Point(342, 155);
            this.NoDairy.Name = "NoDairy";
            this.NoDairy.Size = new System.Drawing.Size(67, 17);
            this.NoDairy.TabIndex = 42;
            this.NoDairy.Text = "No Dairy";
            this.NoDairy.UseVisualStyleBackColor = true;
            // 
            // Seafood
            // 
            this.Seafood.AutoSize = true;
            this.Seafood.Location = new System.Drawing.Point(342, 95);
            this.Seafood.Name = "Seafood";
            this.Seafood.Size = new System.Drawing.Size(66, 17);
            this.Seafood.TabIndex = 43;
            this.Seafood.Text = "Seafood";
            this.Seafood.UseVisualStyleBackColor = true;
            // 
            // NoSeafood
            // 
            this.NoSeafood.AutoSize = true;
            this.NoSeafood.Location = new System.Drawing.Point(342, 172);
            this.NoSeafood.Name = "NoSeafood";
            this.NoSeafood.Size = new System.Drawing.Size(83, 17);
            this.NoSeafood.TabIndex = 44;
            this.NoSeafood.Text = "No Seafood";
            this.NoSeafood.UseVisualStyleBackColor = true;
            // 
            // custom
            // 
            this.custom.Location = new System.Drawing.Point(155, 217);
            this.custom.Name = "custom";
            this.custom.Size = new System.Drawing.Size(752, 20);
            this.custom.TabIndex = 45;
            this.custom.Text = "if empty will take all the recipes in the file (used only in debug work mode)";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.BackColor = System.Drawing.Color.Transparent;
            this.label16.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label16.Location = new System.Drawing.Point(7, 215);
            this.label16.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(143, 19);
            this.label16.TabIndex = 46;
            this.label16.Text = "Custom Recipes ";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.BackColor = System.Drawing.Color.Transparent;
            this.label17.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label17.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label17.Location = new System.Drawing.Point(622, 249);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(100, 19);
            this.label17.TabIndex = 48;
            this.label17.Text = "Min Grades";
            // 
            // richTextBox4
            // 
            this.richTextBox4.Location = new System.Drawing.Point(602, 271);
            this.richTextBox4.Name = "richTextBox4";
            this.richTextBox4.Size = new System.Drawing.Size(147, 336);
            this.richTextBox4.TabIndex = 47;
            this.richTextBox4.Text = "";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.BackColor = System.Drawing.Color.Transparent;
            this.label18.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label18.Location = new System.Drawing.Point(767, 76);
            this.label18.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(96, 19);
            this.label18.TabIndex = 50;
            this.label18.Text = "WorkMode";
            // 
            // workMode
            // 
            this.workMode.FormattingEnabled = true;
            this.workMode.Location = new System.Drawing.Point(769, 98);
            this.workMode.Name = "workMode";
            this.workMode.Size = new System.Drawing.Size(89, 21);
            this.workMode.TabIndex = 49;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.BackColor = System.Drawing.Color.Transparent;
            this.label19.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label19.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label19.Location = new System.Drawing.Point(773, 249);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(105, 19);
            this.label19.TabIndex = 52;
            this.label19.Text = "Max Grades";
            // 
            // richTextBox5
            // 
            this.richTextBox5.Location = new System.Drawing.Point(755, 271);
            this.richTextBox5.Name = "richTextBox5";
            this.richTextBox5.Size = new System.Drawing.Size(147, 336);
            this.richTextBox5.TabIndex = 51;
            this.richTextBox5.Text = "";
            // 
            // LogicRunnerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.ClientSize = new System.Drawing.Size(1086, 948);
            this.Controls.Add(this.label19);
            this.Controls.Add(this.richTextBox5);
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
            this.Controls.Add(this.labelConsole);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.idealServings);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.servings);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.maxValues);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.richTextBox3);
            this.Controls.Add(this.richTextBox2);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.totalCalories);
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
        private System.Windows.Forms.ComboBox totalCalories;
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
        private System.Windows.Forms.Label labelConsole;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.ComboBox recommendationsNum;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Button button2;
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
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.RichTextBox richTextBox5;
    }
}