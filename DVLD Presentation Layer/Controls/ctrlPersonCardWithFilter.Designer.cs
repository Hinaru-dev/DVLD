namespace DVLD_Presentation_Layer.Controls
{
    partial class ctrlPersonCardWithFilter
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblFindBy = new System.Windows.Forms.Label();
            this.cbFindPersonFilter = new System.Windows.Forms.ComboBox();
            this.tbFindPerson = new System.Windows.Forms.TextBox();
            this.ctrlPersonCard1 = new DVLD_Presentation_Layer.Controls.ctrlPersonCard();
            this.btnFindPerson = new System.Windows.Forms.Button();
            this.btnAddNewPerson = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblFindBy
            // 
            this.lblFindBy.AutoSize = true;
            this.lblFindBy.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold);
            this.lblFindBy.Location = new System.Drawing.Point(28, 38);
            this.lblFindBy.Name = "lblFindBy";
            this.lblFindBy.Size = new System.Drawing.Size(67, 17);
            this.lblFindBy.TabIndex = 1;
            this.lblFindBy.Text = "Find By:";
            // 
            // cbFindPersonFilter
            // 
            this.cbFindPersonFilter.FormattingEnabled = true;
            this.cbFindPersonFilter.Items.AddRange(new object[] {
            "PersonID",
            "NationalNo"});
            this.cbFindPersonFilter.Location = new System.Drawing.Point(101, 34);
            this.cbFindPersonFilter.Name = "cbFindPersonFilter";
            this.cbFindPersonFilter.Size = new System.Drawing.Size(163, 24);
            this.cbFindPersonFilter.TabIndex = 2;
            // 
            // tbFindPerson
            // 
            this.tbFindPerson.Location = new System.Drawing.Point(270, 35);
            this.tbFindPerson.Name = "tbFindPerson";
            this.tbFindPerson.Size = new System.Drawing.Size(163, 22);
            this.tbFindPerson.TabIndex = 3;
            // 
            // ctrlPersonCard1
            // 
            this.ctrlPersonCard1.Location = new System.Drawing.Point(9, 60);
            this.ctrlPersonCard1.Name = "ctrlPersonCard1";
            this.ctrlPersonCard1.Size = new System.Drawing.Size(654, 219);
            this.ctrlPersonCard1.TabIndex = 0;
            // 
            // btnFindPerson
            // 
            this.btnFindPerson.BackgroundImage = global::DVLD_Presentation_Layer.Properties.Resources.manSearch;
            this.btnFindPerson.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnFindPerson.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFindPerson.Location = new System.Drawing.Point(439, 34);
            this.btnFindPerson.Name = "btnFindPerson";
            this.btnFindPerson.Size = new System.Drawing.Size(35, 24);
            this.btnFindPerson.TabIndex = 7;
            this.btnFindPerson.UseVisualStyleBackColor = true;
            this.btnFindPerson.Click += new System.EventHandler(this.btnFindPerson_Click);
            // 
            // btnAddNewPerson
            // 
            this.btnAddNewPerson.BackgroundImage = global::DVLD_Presentation_Layer.Properties.Resources.manPlus;
            this.btnAddNewPerson.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnAddNewPerson.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddNewPerson.Location = new System.Drawing.Point(480, 34);
            this.btnAddNewPerson.Name = "btnAddNewPerson";
            this.btnAddNewPerson.Size = new System.Drawing.Size(35, 24);
            this.btnAddNewPerson.TabIndex = 8;
            this.btnAddNewPerson.UseVisualStyleBackColor = true;
            this.btnAddNewPerson.Click += new System.EventHandler(this.btnAddNewPerson_Click);
            // 
            // ctrlPersonCardWithFilter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnAddNewPerson);
            this.Controls.Add(this.btnFindPerson);
            this.Controls.Add(this.tbFindPerson);
            this.Controls.Add(this.cbFindPersonFilter);
            this.Controls.Add(this.lblFindBy);
            this.Controls.Add(this.ctrlPersonCard1);
            this.Name = "ctrlPersonCardWithFilter";
            this.Size = new System.Drawing.Size(672, 289);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ctrlPersonCard ctrlPersonCard1;
        private System.Windows.Forms.Label lblFindBy;
        private System.Windows.Forms.ComboBox cbFindPersonFilter;
        private System.Windows.Forms.TextBox tbFindPerson;
        private System.Windows.Forms.Button btnFindPerson;
        private System.Windows.Forms.Button btnAddNewPerson;
    }
}
