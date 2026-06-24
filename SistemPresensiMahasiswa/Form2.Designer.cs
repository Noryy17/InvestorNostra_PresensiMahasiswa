namespace SistemPresensiMahasiswa
{
    partial class DashboardAdmin
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DashboardAdmin));
            this.label1 = new System.Windows.Forms.Label();
            this.btnKelolaMahasiswa = new System.Windows.Forms.Button();
            this.btnKelolaMatakuliah = new System.Windows.Forms.Button();
            this.btnKelolaDosen = new System.Windows.Forms.Button();
            this.btnGenerateLaporan = new System.Windows.Forms.Button();
            this.btnLogout = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // btnKelolaMahasiswa
            // 
            resources.ApplyResources(this.btnKelolaMahasiswa, "btnKelolaMahasiswa");
            this.btnKelolaMahasiswa.Name = "btnKelolaMahasiswa";
            this.btnKelolaMahasiswa.UseVisualStyleBackColor = true;
            this.btnKelolaMahasiswa.Click += new System.EventHandler(this.btnKelolaMahasiswa_Click);
            // 
            // btnKelolaMatakuliah
            // 
            resources.ApplyResources(this.btnKelolaMatakuliah, "btnKelolaMatakuliah");
            this.btnKelolaMatakuliah.Name = "btnKelolaMatakuliah";
            this.btnKelolaMatakuliah.UseVisualStyleBackColor = true;
            this.btnKelolaMatakuliah.Click += new System.EventHandler(this.btnKelolaMatakuliah_Click);
            // 
            // btnKelolaDosen
            // 
            resources.ApplyResources(this.btnKelolaDosen, "btnKelolaDosen");
            this.btnKelolaDosen.Name = "btnKelolaDosen";
            this.btnKelolaDosen.UseVisualStyleBackColor = true;
            this.btnKelolaDosen.Click += new System.EventHandler(this.btnKelolaDosen_Click);
            // 
            // btnGenerateLaporan
            // 
            resources.ApplyResources(this.btnGenerateLaporan, "btnGenerateLaporan");
            this.btnGenerateLaporan.Name = "btnGenerateLaporan";
            this.btnGenerateLaporan.UseVisualStyleBackColor = true;
            this.btnGenerateLaporan.Click += new System.EventHandler(this.btnGenerateLaporan_Click);
            // 
            // btnLogout
            // 
            resources.ApplyResources(this.btnLogout, "btnLogout");
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.UseVisualStyleBackColor = true;
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);
            // 
            // DashboardAdmin
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnLogout);
            this.Controls.Add(this.btnGenerateLaporan);
            this.Controls.Add(this.btnKelolaDosen);
            this.Controls.Add(this.btnKelolaMatakuliah);
            this.Controls.Add(this.btnKelolaMahasiswa);
            this.Controls.Add(this.label1);
            this.Name = "DashboardAdmin";
            this.Load += new System.EventHandler(this.DashboardAdmin_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnKelolaMahasiswa;
        private System.Windows.Forms.Button btnKelolaMatakuliah;
        private System.Windows.Forms.Button btnKelolaDosen;
        private System.Windows.Forms.Button btnGenerateLaporan;
        private System.Windows.Forms.Button btnLogout;
    }
}