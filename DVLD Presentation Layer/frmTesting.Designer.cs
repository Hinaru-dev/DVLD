namespace DVLD_Presentation_Layer
{
    partial class frmTesting
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
            this.ctrlUserCard1 = new DVLD_Presentation_Layer.Controls.ctrlUserCard();
            this.btnFind = new System.Windows.Forms.Button();
            this.rbUsername = new System.Windows.Forms.RadioButton();
            this.rbUserID = new System.Windows.Forms.RadioButton();
            this.tbInput = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ctrlUserCard1
            // 
            this.ctrlUserCard1.Location = new System.Drawing.Point(102, 29);
            this.ctrlUserCard1.Name = "ctrlUserCard1";
            this.ctrlUserCard1.Size = new System.Drawing.Size(660, 320);
            this.ctrlUserCard1.TabIndex = 0;
            // 
            // btnFind
            // 
            this.btnFind.Location = new System.Drawing.Point(17, 220);
            this.btnFind.Name = "btnFind";
            this.btnFind.Size = new System.Drawing.Size(75, 91);
            this.btnFind.TabIndex = 4;
            this.btnFind.Text = "find";
            this.btnFind.UseVisualStyleBackColor = true;
            this.btnFind.Click += new System.EventHandler(this.btnFind_Click);
            // 
            // rbUsername
            // 
            this.rbUsername.AutoSize = true;
            this.rbUsername.Location = new System.Drawing.Point(0, 115);
            this.rbUsername.Name = "rbUsername";
            this.rbUsername.Size = new System.Drawing.Size(92, 21);
            this.rbUsername.TabIndex = 2;
            this.rbUsername.Text = "username";
            this.rbUsername.UseVisualStyleBackColor = true;
            // 
            // rbUserID
            // 
            this.rbUserID.AutoSize = true;
            this.rbUserID.Checked = true;
            this.rbUserID.Location = new System.Drawing.Point(0, 88);
            this.rbUserID.Name = "rbUserID";
            this.rbUserID.Size = new System.Drawing.Size(72, 21);
            this.rbUserID.TabIndex = 1;
            this.rbUserID.TabStop = true;
            this.rbUserID.Text = "UserID";
            this.rbUserID.UseVisualStyleBackColor = true;
            // 
            // tbInput
            // 
            this.tbInput.Location = new System.Drawing.Point(0, 165);
            this.tbInput.Name = "tbInput";
            this.tbInput.Size = new System.Drawing.Size(100, 22);
            this.tbInput.TabIndex = 3;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 314);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 91);
            this.button1.TabIndex = 5;
            this.button1.Text = "Login";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // frmTesting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(774, 417);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.tbInput);
            this.Controls.Add(this.rbUserID);
            this.Controls.Add(this.rbUsername);
            this.Controls.Add(this.btnFind);
            this.Controls.Add(this.ctrlUserCard1);
            this.Name = "frmTesting";
            this.Text = "frmTesting";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Controls.ctrlUserCard ctrlUserCard1;
        private System.Windows.Forms.Button btnFind;
        private System.Windows.Forms.RadioButton rbUsername;
        private System.Windows.Forms.RadioButton rbUserID;
        private System.Windows.Forms.TextBox tbInput;
        private System.Windows.Forms.Button button1;

    }
}