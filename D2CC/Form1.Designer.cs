namespace D2CC
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            panel1 = new Panel();
            fileaddress = new Label();
            filename = new Label();
            AddDbcFile_Button = new Button();
            button1 = new Button();
            button2 = new Button();
            openFileDialog1 = new OpenFileDialog();
            saveFileDialog1 = new SaveFileDialog();
            label1 = new Label();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(fileaddress);
            panel1.Controls.Add(filename);
            panel1.Controls.Add(AddDbcFile_Button);
            panel1.Location = new Point(12, 23);
            panel1.Name = "panel1";
            panel1.Size = new Size(257, 78);
            panel1.TabIndex = 0;
            // 
            // fileaddress
            // 
            fileaddress.AutoSize = true;
            fileaddress.ForeColor = SystemColors.ControlLightLight;
            fileaddress.Location = new Point(3, 53);
            fileaddress.Name = "fileaddress";
            fileaddress.Size = new Size(66, 15);
            fileaddress.TabIndex = 5;
            fileaddress.Text = "Adres : null";
            // 
            // filename
            // 
            filename.AutoSize = true;
            filename.ForeColor = SystemColors.ControlLightLight;
            filename.Location = new Point(3, 29);
            filename.Name = "filename";
            filename.Size = new Size(66, 15);
            filename.TabIndex = 4;
            filename.Text = "File :     null";
            // 
            // AddDbcFile_Button
            // 
            AddDbcFile_Button.Location = new Point(3, 0);
            AddDbcFile_Button.Name = "AddDbcFile_Button";
            AddDbcFile_Button.Size = new Size(251, 23);
            AddDbcFile_Button.TabIndex = 3;
            AddDbcFile_Button.Text = "Add .dbc File";
            AddDbcFile_Button.UseVisualStyleBackColor = true;
            AddDbcFile_Button.Click += AddDbcFile_Button_Click;
            // 
            // button1
            // 
            button1.Location = new Point(15, 107);
            button1.Name = "button1";
            button1.Size = new Size(115, 23);
            button1.TabIndex = 6;
            button1.Text = "Save .h file";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Location = new Point(156, 107);
            button2.Name = "button2";
            button2.Size = new Size(113, 23);
            button2.TabIndex = 7;
            button2.Text = "Save .c file";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // openFileDialog1
            // 
            openFileDialog1.FileName = "openFileDialog1";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.ForeColor = SystemColors.ControlLightLight;
            label1.Location = new Point(12, 5);
            label1.Name = "label1";
            label1.Size = new Size(148, 15);
            label1.TabIndex = 8;
            label1.Text = "Dbc To C Library Converter";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ActiveCaptionText;
            ClientSize = new Size(275, 140);
            Controls.Add(label1);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Form1";
            Text = "D2CC";
            Load += Form1_Load;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel panel1;
        private Label fileaddress;
        private Label filename;
        private Button AddDbcFile_Button;
        private Button button1;
        private Button button2;
        private OpenFileDialog openFileDialog1;
        private SaveFileDialog saveFileDialog1;
        private Label label1;
    }
}
