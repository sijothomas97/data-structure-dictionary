namespace TaskB
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
            this.label1 = new System.Windows.Forms.Label();
            this.tBID = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tBName = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rBNotCompleted = new System.Windows.Forms.RadioButton();
            this.rBCompleted = new System.Windows.Forms.RadioButton();
            this.btnSave = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.tBIDSearch = new System.Windows.Forms.TextBox();
            this.btnStatus = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.lBStudentList = new System.Windows.Forms.ListBox();
            this.label4 = new System.Windows.Forms.Label();
            this.lBEnrolCompleted = new System.Windows.Forms.ListBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tBNameSearch = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.btnDetails = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(60, 59);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(24, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "ID";
            // 
            // tBID
            // 
            this.tBID.Location = new System.Drawing.Point(60, 82);
            this.tBID.Name = "tBID";
            this.tBID.Size = new System.Drawing.Size(181, 27);
            this.tBID.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(60, 130);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 20);
            this.label2.TabIndex = 2;
            this.label2.Text = "Name";
            // 
            // tBName
            // 
            this.tBName.Location = new System.Drawing.Point(60, 153);
            this.tBName.Name = "tBName";
            this.tBName.Size = new System.Drawing.Size(181, 27);
            this.tBName.TabIndex = 3;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rBNotCompleted);
            this.groupBox1.Controls.Add(this.rBCompleted);
            this.groupBox1.Location = new System.Drawing.Point(60, 213);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(266, 67);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Enrolment status";
            // 
            // rBNotCompleted
            // 
            this.rBNotCompleted.AutoSize = true;
            this.rBNotCompleted.Location = new System.Drawing.Point(129, 26);
            this.rBNotCompleted.Name = "rBNotCompleted";
            this.rBNotCompleted.Size = new System.Drawing.Size(131, 24);
            this.rBNotCompleted.TabIndex = 1;
            this.rBNotCompleted.Text = "Not completed";
            this.rBNotCompleted.UseVisualStyleBackColor = true;
            // 
            // rBCompleted
            // 
            this.rBCompleted.AutoSize = true;
            this.rBCompleted.Checked = true;
            this.rBCompleted.Location = new System.Drawing.Point(6, 27);
            this.rBCompleted.Name = "rBCompleted";
            this.rBCompleted.Size = new System.Drawing.Size(104, 24);
            this.rBCompleted.TabIndex = 0;
            this.rBCompleted.TabStop = true;
            this.rBCompleted.Text = "Completed";
            this.rBCompleted.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(66, 321);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(94, 29);
            this.btnSave.TabIndex = 5;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(351, 59);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(90, 20);
            this.label3.TabIndex = 6;
            this.label3.Text = "ID to Search";
            // 
            // tBIDSearch
            // 
            this.tBIDSearch.Location = new System.Drawing.Point(351, 82);
            this.tBIDSearch.Name = "tBIDSearch";
            this.tBIDSearch.Size = new System.Drawing.Size(199, 27);
            this.tBIDSearch.TabIndex = 7;
            // 
            // btnStatus
            // 
            this.btnStatus.Location = new System.Drawing.Point(351, 149);
            this.btnStatus.Name = "btnStatus";
            this.btnStatus.Size = new System.Drawing.Size(94, 29);
            this.btnStatus.TabIndex = 8;
            this.btnStatus.Text = "Status";
            this.btnStatus.UseVisualStyleBackColor = true;
            this.btnStatus.Click += new System.EventHandler(this.btnStatus_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.Location = new System.Drawing.Point(456, 149);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(94, 29);
            this.btnEdit.TabIndex = 9;
            this.btnEdit.Text = "Edit";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(351, 184);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(199, 29);
            this.btnDelete.TabIndex = 10;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // lBStudentList
            // 
            this.lBStudentList.FormattingEnabled = true;
            this.lBStudentList.ItemHeight = 20;
            this.lBStudentList.Location = new System.Drawing.Point(587, 82);
            this.lBStudentList.Name = "lBStudentList";
            this.lBStudentList.Size = new System.Drawing.Size(199, 384);
            this.lBStudentList.TabIndex = 11;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(587, 59);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(50, 20);
            this.label4.TabIndex = 12;
            this.label4.Text = "ID List";
            // 
            // lBEnrolCompleted
            // 
            this.lBEnrolCompleted.FormattingEnabled = true;
            this.lBEnrolCompleted.ItemHeight = 20;
            this.lBEnrolCompleted.Location = new System.Drawing.Point(813, 87);
            this.lBEnrolCompleted.Name = "lBEnrolCompleted";
            this.lBEnrolCompleted.Size = new System.Drawing.Size(247, 364);
            this.lBEnrolCompleted.TabIndex = 13;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(813, 64);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(153, 20);
            this.label5.TabIndex = 14;
            this.label5.Text = "Enrolment completed";
            // 
            // tBNameSearch
            // 
            this.tBNameSearch.Location = new System.Drawing.Point(351, 277);
            this.tBNameSearch.Name = "tBNameSearch";
            this.tBNameSearch.Size = new System.Drawing.Size(199, 27);
            this.tBNameSearch.TabIndex = 15;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(351, 254);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(115, 20);
            this.label6.TabIndex = 16;
            this.label6.Text = "Name to Search";
            // 
            // btnDetails
            // 
            this.btnDetails.Location = new System.Drawing.Point(456, 321);
            this.btnDetails.Name = "btnDetails";
            this.btnDetails.Size = new System.Drawing.Size(94, 29);
            this.btnDetails.TabIndex = 17;
            this.btnDetails.Text = "Details";
            this.btnDetails.UseVisualStyleBackColor = true;
            this.btnDetails.Click += new System.EventHandler(this.btnDetails_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1092, 450);
            this.Controls.Add(this.btnDetails);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.tBNameSearch);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.lBEnrolCompleted);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lBStudentList);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.btnStatus);
            this.Controls.Add(this.tBIDSearch);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.tBName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tBID);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label label1;
        private TextBox tBID;
        private Label label2;
        private TextBox tBName;
        private GroupBox groupBox1;
        private RadioButton rBNotCompleted;
        private RadioButton rBCompleted;
        private Button btnSave;
        private Label label3;
        private TextBox tBIDSearch;
        private Button btnStatus;
        private Button btnEdit;
        private Button btnDelete;
        private ListBox lBStudentList;
        private Label label4;
        private ListBox lBEnrolCompleted;
        private Label label5;
        private TextBox tBNameSearch;
        private Label label6;
        private Button btnDetails;
    }
}