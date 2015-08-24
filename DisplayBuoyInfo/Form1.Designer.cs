namespace DisplayBuoyInfo
{
    partial class Form1
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
            this.getDataButton = new System.Windows.Forms.Button();
            this.AirTempBox = new System.Windows.Forms.TextBox();
            this.airTemperatureLabel = new System.Windows.Forms.Label();
            this.nextButton = new System.Windows.Forms.Button();
            this.dateBox = new System.Windows.Forms.TextBox();
            this.dateBoxLabel = new System.Windows.Forms.Label();
            this.windDirectionBox = new System.Windows.Forms.TextBox();
            this.windDirectionLabel = new System.Windows.Forms.Label();
            this.windSpeedBox = new System.Windows.Forms.TextBox();
            this.windSpeedLabel = new System.Windows.Forms.Label();
            this.gustLabel = new System.Windows.Forms.Label();
            this.gustBox = new System.Windows.Forms.TextBox();
            this.waveHeightBox = new System.Windows.Forms.TextBox();
            this.waveHeightLabel = new System.Windows.Forms.Label();
            this.seaSurfaceTempLabel = new System.Windows.Forms.Label();
            this.seaSurfaceTempBox = new System.Windows.Forms.TextBox();
            this.previousButton = new System.Windows.Forms.Button();
            this.timeZoneLabel = new System.Windows.Forms.Label();
            this.fahrenheitCheckBox = new System.Windows.Forms.CheckBox();
            this.knotsCheckBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // getDataButton
            // 
            this.getDataButton.Location = new System.Drawing.Point(49, 392);
            this.getDataButton.Name = "getDataButton";
            this.getDataButton.Size = new System.Drawing.Size(107, 49);
            this.getDataButton.TabIndex = 0;
            this.getDataButton.Text = "Get Data";
            this.getDataButton.UseVisualStyleBackColor = true;
            this.getDataButton.Click += new System.EventHandler(this.getDataButton_Click);
            // 
            // AirTempBox
            // 
            this.AirTempBox.Location = new System.Drawing.Point(155, 223);
            this.AirTempBox.Name = "AirTempBox";
            this.AirTempBox.Size = new System.Drawing.Size(116, 20);
            this.AirTempBox.TabIndex = 1;
            // 
            // airTemperatureLabel
            // 
            this.airTemperatureLabel.AutoSize = true;
            this.airTemperatureLabel.Location = new System.Drawing.Point(152, 246);
            this.airTemperatureLabel.Name = "airTemperatureLabel";
            this.airTemperatureLabel.Size = new System.Drawing.Size(82, 13);
            this.airTemperatureLabel.TabIndex = 2;
            this.airTemperatureLabel.Text = "Air Temperature";
            // 
            // nextButton
            // 
            this.nextButton.Location = new System.Drawing.Point(279, 392);
            this.nextButton.Name = "nextButton";
            this.nextButton.Size = new System.Drawing.Size(116, 49);
            this.nextButton.TabIndex = 3;
            this.nextButton.Text = "Next";
            this.nextButton.UseVisualStyleBackColor = true;
            this.nextButton.Click += new System.EventHandler(this.nextButton_Click);
            // 
            // dateBox
            // 
            this.dateBox.Location = new System.Drawing.Point(15, 51);
            this.dateBox.Name = "dateBox";
            this.dateBox.Size = new System.Drawing.Size(240, 20);
            this.dateBox.TabIndex = 4;
            // 
            // dateBoxLabel
            // 
            this.dateBoxLabel.AutoSize = true;
            this.dateBoxLabel.Location = new System.Drawing.Point(12, 74);
            this.dateBoxLabel.Name = "dateBoxLabel";
            this.dateBoxLabel.Size = new System.Drawing.Size(30, 13);
            this.dateBoxLabel.TabIndex = 5;
            this.dateBoxLabel.Text = "Date";
            // 
            // windDirectionBox
            // 
            this.windDirectionBox.Location = new System.Drawing.Point(15, 156);
            this.windDirectionBox.Name = "windDirectionBox";
            this.windDirectionBox.Size = new System.Drawing.Size(100, 20);
            this.windDirectionBox.TabIndex = 6;
            // 
            // windDirectionLabel
            // 
            this.windDirectionLabel.AutoSize = true;
            this.windDirectionLabel.Location = new System.Drawing.Point(12, 180);
            this.windDirectionLabel.Name = "windDirectionLabel";
            this.windDirectionLabel.Size = new System.Drawing.Size(77, 13);
            this.windDirectionLabel.TabIndex = 7;
            this.windDirectionLabel.Text = "Wind Direction";
            // 
            // windSpeedBox
            // 
            this.windSpeedBox.Location = new System.Drawing.Point(155, 156);
            this.windSpeedBox.Name = "windSpeedBox";
            this.windSpeedBox.Size = new System.Drawing.Size(100, 20);
            this.windSpeedBox.TabIndex = 10;
            // 
            // windSpeedLabel
            // 
            this.windSpeedLabel.AutoSize = true;
            this.windSpeedLabel.Location = new System.Drawing.Point(152, 179);
            this.windSpeedLabel.Name = "windSpeedLabel";
            this.windSpeedLabel.Size = new System.Drawing.Size(66, 13);
            this.windSpeedLabel.TabIndex = 11;
            this.windSpeedLabel.Text = "Wind Speed";
            // 
            // gustLabel
            // 
            this.gustLabel.AutoSize = true;
            this.gustLabel.Location = new System.Drawing.Point(292, 180);
            this.gustLabel.Name = "gustLabel";
            this.gustLabel.Size = new System.Drawing.Size(29, 13);
            this.gustLabel.TabIndex = 12;
            this.gustLabel.Text = "Gust";
            // 
            // gustBox
            // 
            this.gustBox.Location = new System.Drawing.Point(295, 155);
            this.gustBox.Name = "gustBox";
            this.gustBox.Size = new System.Drawing.Size(100, 20);
            this.gustBox.TabIndex = 13;
            // 
            // waveHeightBox
            // 
            this.waveHeightBox.Location = new System.Drawing.Point(15, 223);
            this.waveHeightBox.Name = "waveHeightBox";
            this.waveHeightBox.Size = new System.Drawing.Size(100, 20);
            this.waveHeightBox.TabIndex = 14;
            // 
            // waveHeightLabel
            // 
            this.waveHeightLabel.AutoSize = true;
            this.waveHeightLabel.Location = new System.Drawing.Point(12, 246);
            this.waveHeightLabel.Name = "waveHeightLabel";
            this.waveHeightLabel.Size = new System.Drawing.Size(70, 13);
            this.waveHeightLabel.TabIndex = 15;
            this.waveHeightLabel.Text = "Wave Height";
            // 
            // seaSurfaceTempLabel
            // 
            this.seaSurfaceTempLabel.AutoSize = true;
            this.seaSurfaceTempLabel.Location = new System.Drawing.Point(292, 246);
            this.seaSurfaceTempLabel.Name = "seaSurfaceTempLabel";
            this.seaSurfaceTempLabel.Size = new System.Drawing.Size(129, 13);
            this.seaSurfaceTempLabel.TabIndex = 16;
            this.seaSurfaceTempLabel.Text = "Sea Surface Temperature";
            // 
            // seaSurfaceTempBox
            // 
            this.seaSurfaceTempBox.Location = new System.Drawing.Point(295, 223);
            this.seaSurfaceTempBox.Name = "seaSurfaceTempBox";
            this.seaSurfaceTempBox.Size = new System.Drawing.Size(100, 20);
            this.seaSurfaceTempBox.TabIndex = 17;
            // 
            // previousButton
            // 
            this.previousButton.Location = new System.Drawing.Point(162, 392);
            this.previousButton.Name = "previousButton";
            this.previousButton.Size = new System.Drawing.Size(111, 49);
            this.previousButton.TabIndex = 18;
            this.previousButton.Text = "Previous";
            this.previousButton.UseVisualStyleBackColor = true;
            this.previousButton.Click += new System.EventHandler(this.previousButton_Click);
            // 
            // timeZoneLabel
            // 
            this.timeZoneLabel.AutoSize = true;
            this.timeZoneLabel.Location = new System.Drawing.Point(261, 58);
            this.timeZoneLabel.Name = "timeZoneLabel";
            this.timeZoneLabel.Size = new System.Drawing.Size(58, 13);
            this.timeZoneLabel.TabIndex = 19;
            this.timeZoneLabel.Text = "Time Zone";
            // 
            // fahrenheitCheckBox
            // 
            this.fahrenheitCheckBox.AutoSize = true;
            this.fahrenheitCheckBox.Location = new System.Drawing.Point(15, 295);
            this.fahrenheitCheckBox.Name = "fahrenheitCheckBox";
            this.fahrenheitCheckBox.Size = new System.Drawing.Size(76, 17);
            this.fahrenheitCheckBox.TabIndex = 20;
            this.fahrenheitCheckBox.Text = "Fahrenheit";
            this.fahrenheitCheckBox.UseVisualStyleBackColor = true;
            this.fahrenheitCheckBox.CheckedChanged += new System.EventHandler(this.fahrenheitCheckBox_CheckedChanged);
            // 
            // knotsCheckBox
            // 
            this.knotsCheckBox.AutoSize = true;
            this.knotsCheckBox.Location = new System.Drawing.Point(15, 319);
            this.knotsCheckBox.Name = "knotsCheckBox";
            this.knotsCheckBox.Size = new System.Drawing.Size(53, 17);
            this.knotsCheckBox.TabIndex = 21;
            this.knotsCheckBox.Text = "Knots";
            this.knotsCheckBox.UseVisualStyleBackColor = true;
            this.knotsCheckBox.CheckedChanged += new System.EventHandler(this.knotsCheckBox_CheckedChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(534, 453);
            this.Controls.Add(this.knotsCheckBox);
            this.Controls.Add(this.fahrenheitCheckBox);
            this.Controls.Add(this.timeZoneLabel);
            this.Controls.Add(this.previousButton);
            this.Controls.Add(this.seaSurfaceTempBox);
            this.Controls.Add(this.seaSurfaceTempLabel);
            this.Controls.Add(this.waveHeightLabel);
            this.Controls.Add(this.waveHeightBox);
            this.Controls.Add(this.gustBox);
            this.Controls.Add(this.gustLabel);
            this.Controls.Add(this.windSpeedLabel);
            this.Controls.Add(this.windSpeedBox);
            this.Controls.Add(this.windDirectionLabel);
            this.Controls.Add(this.windDirectionBox);
            this.Controls.Add(this.dateBoxLabel);
            this.Controls.Add(this.dateBox);
            this.Controls.Add(this.nextButton);
            this.Controls.Add(this.airTemperatureLabel);
            this.Controls.Add(this.AirTempBox);
            this.Controls.Add(this.getDataButton);
            this.Name = "Form1";
            this.Text = "Buoy Info";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button getDataButton;
        private System.Windows.Forms.TextBox AirTempBox;
        private System.Windows.Forms.Label airTemperatureLabel;
        private System.Windows.Forms.Button nextButton;
        private System.Windows.Forms.TextBox dateBox;
        private System.Windows.Forms.Label dateBoxLabel;
        private System.Windows.Forms.TextBox windDirectionBox;
        private System.Windows.Forms.Label windDirectionLabel;
        private System.Windows.Forms.TextBox windSpeedBox;
        private System.Windows.Forms.Label windSpeedLabel;
        private System.Windows.Forms.Label gustLabel;
        private System.Windows.Forms.TextBox gustBox;
        private System.Windows.Forms.TextBox waveHeightBox;
        private System.Windows.Forms.Label waveHeightLabel;
        private System.Windows.Forms.Label seaSurfaceTempLabel;
        private System.Windows.Forms.TextBox seaSurfaceTempBox;
        private System.Windows.Forms.Button previousButton;
        private System.Windows.Forms.Label timeZoneLabel;
        private System.Windows.Forms.CheckBox fahrenheitCheckBox;
        private System.Windows.Forms.CheckBox knotsCheckBox;
    }
}

