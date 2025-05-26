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
            ByteOrderCheckBox = new CheckBox();
            fileaddress = new Label();
            filename = new Label();
            AddDbcFile_Button = new Button();
            button1 = new Button();
            button2 = new Button();
            openFileDialog1 = new OpenFileDialog();
            saveFileDialog1 = new SaveFileDialog();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(ByteOrderCheckBox);
            panel1.Controls.Add(fileaddress);
            panel1.Controls.Add(filename);
            panel1.Controls.Add(AddDbcFile_Button);
            panel1.Location = new Point(1, -3);
            panel1.Name = "panel1";
            panel1.Size = new Size(273, 104);
            panel1.TabIndex = 0;
            // 
            // ByteOrderCheckBox
            // 
            ByteOrderCheckBox.AutoSize = true;
            ByteOrderCheckBox.Checked = true;
            ByteOrderCheckBox.CheckState = CheckState.Checked;
            ByteOrderCheckBox.ForeColor = SystemColors.ControlLightLight;
            ByteOrderCheckBox.Location = new Point(13, 78);
            ByteOrderCheckBox.Name = "ByteOrderCheckBox";
            ByteOrderCheckBox.Size = new Size(136, 19);
            ByteOrderCheckBox.TabIndex = 6;
            ByteOrderCheckBox.Text = "is Intel / Little Endian";
            ByteOrderCheckBox.UseVisualStyleBackColor = true;
            // 
            // fileaddress
            // 
            fileaddress.AutoSize = true;
            fileaddress.ForeColor = SystemColors.ControlLightLight;
            fileaddress.Location = new Point(13, 60);
            fileaddress.Name = "fileaddress";
            fileaddress.Size = new Size(66, 15);
            fileaddress.TabIndex = 5;
            fileaddress.Text = "Adres : null";
            // 
            // filename
            // 
            filename.AutoSize = true;
            filename.ForeColor = SystemColors.ControlLightLight;
            filename.Location = new Point(13, 40);
            filename.Name = "filename";
            filename.Size = new Size(66, 15);
            filename.TabIndex = 4;
            filename.Text = "File :     null";
            // 
            // AddDbcFile_Button
            // 
            AddDbcFile_Button.Location = new Point(11, 15);
            AddDbcFile_Button.Name = "AddDbcFile_Button";
            AddDbcFile_Button.Size = new Size(251, 23);
            AddDbcFile_Button.TabIndex = 3;
            AddDbcFile_Button.Text = "Add .dbc File";
            AddDbcFile_Button.UseVisualStyleBackColor = true;
            AddDbcFile_Button.Click += AddDbcFile_Button_Click;
            // 
            // button1
            // 
            button1.Location = new Point(12, 103);
            button1.Name = "button1";
            button1.Size = new Size(125, 23);
            button1.TabIndex = 6;
            button1.Text = "Save .h file";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Location = new Point(143, 103);
            button2.Name = "button2";
            button2.Size = new Size(120, 23);
            button2.TabIndex = 7;
            button2.Text = "Save .c file";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // openFileDialog1
            // 
            openFileDialog1.FileName = "openFileDialog1";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ActiveCaptionText;
            ClientSize = new Size(275, 136);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "D2CC";
            Load += Form1_Load;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
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
        private CheckBox ByteOrderCheckBox;
    }
}
