namespace SistemPresensiMahasiswa
{
    partial class DashboardDosen
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
            this.label1 = new System.Windows.Forms.Label();
            this.btnInputPresensi = new System.Windows.Forms.Button();
            this.btnRekapPresensi = new System.Windows.Forms.Button();
            this.btnLogout = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(629, 50);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(104, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Selamat Datang";
            // 
            // btnInputPresensi
            // 
            this.btnInputPresensi.Location = new System.Drawing.Point(555, 153);
            this.btnInputPresensi.Name = "btnInputPresensi";
            this.btnInputPresensi.Size = new System.Drawing.Size(267, 72);
            this.btnInputPresensi.TabIndex = 1;
            this.btnInputPresensi.Text = "Input Presensi";
            this.btnInputPresensi.UseVisualStyleBackColor = true;
            this.btnInputPresensi.Click += new System.EventHandler(this.btnInputPresensi_Click);
            // 
            // btnRekapPresensi
            // 
            this.btnRekapPresensi.Location = new System.Drawing.Point(555, 246);
            this.btnRekapPresensi.Name = "btnRekapPresensi";
            this.btnRekapPresensi.Size = new System.Drawing.Size(267, 71);
            this.btnRekapPresensi.TabIndex = 2;
            this.btnRekapPresensi.Text = "Lihat Rekap Presensi";
            this.btnRekapPresensi.UseVisualStyleBackColor = true;
            this.btnRekapPresensi.Click += new System.EventHandler(this.btnRekapPresensi_Click);
            // 
            // btnLogout
            // 
            this.btnLogout.Location = new System.Drawing.Point(555, 335);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(267, 76);
            this.btnLogout.TabIndex = 3;
            this.btnLogout.Text = "Logout";
            this.btnLogout.UseVisualStyleBackColor = true;
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);
            // 
            // DashboardDosen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1473, 526);
            this.Controls.Add(this.btnLogout);
            this.Controls.Add(this.btnRekapPresensi);
            this.Controls.Add(this.btnInputPresensi);
            this.Controls.Add(this.label1);
            this.Name = "DashboardDosen";
            this.Text = "DashboardDosen";
            this.Load += new System.EventHandler(this.DashboardDosen_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnInputPresensi;
        private System.Windows.Forms.Button btnRekapPresensi;
        private System.Windows.Forms.Button btnLogout;
    }
}