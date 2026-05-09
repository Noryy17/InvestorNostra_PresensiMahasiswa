namespace SistemPresensiMahasiswa
{
    partial class KelolaMahasiswa
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtJurusan = new System.Windows.Forms.TextBox();
            this.txtNIM = new System.Windows.Forms.TextBox();
            this.txtNama = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.ColNim = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColNamaMahasiswa = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColJurusan = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnLoad = new System.Windows.Forms.Button();
            this.btnHapus = new System.Windows.Forms.Button();
            this.btnUbah = new System.Windows.Forms.Button();
            this.btnTambah = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.btnKembali = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(412, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(235, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "SISTEM KELOLA DATA MAHASISWA";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.txtJurusan);
            this.panel1.Controls.Add(this.txtNIM);
            this.panel1.Controls.Add(this.txtNama);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Location = new System.Drawing.Point(24, 57);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(535, 218);
            this.panel1.TabIndex = 1;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // txtJurusan
            // 
            this.txtJurusan.Location = new System.Drawing.Point(142, 97);
            this.txtJurusan.Name = "txtJurusan";
            this.txtJurusan.Size = new System.Drawing.Size(197, 22);
            this.txtJurusan.TabIndex = 6;
            this.txtJurusan.TextChanged += new System.EventHandler(this.textBox3_TextChanged);
            // 
            // txtNIM
            // 
            this.txtNIM.Location = new System.Drawing.Point(142, 27);
            this.txtNIM.Name = "txtNIM";
            this.txtNIM.Size = new System.Drawing.Size(197, 22);
            this.txtNIM.TabIndex = 5;
            this.txtNIM.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtNIM_KeyPress);
            // 
            // txtNama
            // 
            this.txtNama.Location = new System.Drawing.Point(142, 62);
            this.txtNama.Name = "txtNama";
            this.txtNama.Size = new System.Drawing.Size(197, 22);
            this.txtNama.TabIndex = 4;
            this.txtNama.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtNama_KeyPress);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(33, 100);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(60, 16);
            this.label5.TabIndex = 2;
            this.label5.Text = "Jurusan :";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(33, 62);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(50, 16);
            this.label4.TabIndex = 1;
            this.label4.Text = "Nama :";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(33, 34);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(37, 16);
            this.label3.TabIndex = 0;
            this.label3.Text = "NIM :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(68, 45);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(107, 16);
            this.label2.TabIndex = 2;
            this.label2.Text = "Data Mahasiswa";
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColNim,
            this.ColNamaMahasiswa,
            this.ColJurusan});
            this.dataGridView1.Location = new System.Drawing.Point(585, 57);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(430, 511);
            this.dataGridView1.TabIndex = 3;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // ColNim
            // 
            this.ColNim.HeaderText = "Nim";
            this.ColNim.MinimumWidth = 6;
            this.ColNim.Name = "ColNim";
            this.ColNim.Width = 125;
            // 
            // ColNamaMahasiswa
            // 
            this.ColNamaMahasiswa.HeaderText = "Nama Mahasiswa";
            this.ColNamaMahasiswa.MinimumWidth = 6;
            this.ColNamaMahasiswa.Name = "ColNamaMahasiswa";
            this.ColNamaMahasiswa.Width = 125;
            // 
            // ColJurusan
            // 
            this.ColJurusan.HeaderText = "Jurusan";
            this.ColJurusan.MinimumWidth = 6;
            this.ColJurusan.Name = "ColJurusan";
            this.ColJurusan.Width = 125;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnLoad);
            this.panel2.Controls.Add(this.btnHapus);
            this.panel2.Controls.Add(this.btnUbah);
            this.panel2.Controls.Add(this.btnTambah);
            this.panel2.Location = new System.Drawing.Point(24, 315);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(535, 100);
            this.panel2.TabIndex = 4;
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(411, 29);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(97, 40);
            this.btnLoad.TabIndex = 3;
            this.btnLoad.Text = "Load";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // btnHapus
            // 
            this.btnHapus.Location = new System.Drawing.Point(292, 29);
            this.btnHapus.Name = "btnHapus";
            this.btnHapus.Size = new System.Drawing.Size(97, 40);
            this.btnHapus.TabIndex = 2;
            this.btnHapus.Text = "Hapus";
            this.btnHapus.UseVisualStyleBackColor = true;
            this.btnHapus.Click += new System.EventHandler(this.btnHapus_Click);
            // 
            // btnUbah
            // 
            this.btnUbah.Location = new System.Drawing.Point(162, 29);
            this.btnUbah.Name = "btnUbah";
            this.btnUbah.Size = new System.Drawing.Size(100, 40);
            this.btnUbah.TabIndex = 1;
            this.btnUbah.Text = "Ubah";
            this.btnUbah.UseVisualStyleBackColor = true;
            this.btnUbah.Click += new System.EventHandler(this.btnUbah_Click);
            // 
            // btnTambah
            // 
            this.btnTambah.Location = new System.Drawing.Point(36, 29);
            this.btnTambah.Name = "btnTambah";
            this.btnTambah.Size = new System.Drawing.Size(102, 40);
            this.btnTambah.TabIndex = 0;
            this.btnTambah.Text = "Tambah";
            this.btnTambah.UseVisualStyleBackColor = true;
            this.btnTambah.Click += new System.EventHandler(this.btnTambah_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(49, 300);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(33, 16);
            this.label6.TabIndex = 6;
            this.label6.Text = "Aksi";
            // 
            // btnKembali
            // 
            this.btnKembali.BackColor = System.Drawing.Color.Red;
            this.btnKembali.Location = new System.Drawing.Point(199, 491);
            this.btnKembali.Name = "btnKembali";
            this.btnKembali.Size = new System.Drawing.Size(153, 37);
            this.btnKembali.TabIndex = 7;
            this.btnKembali.Text = "Kembali";
            this.btnKembali.UseVisualStyleBackColor = false;
            this.btnKembali.Click += new System.EventHandler(this.btnKembali_Click);
            // 
            // KelolaMahasiswa
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1046, 580);
            this.Controls.Add(this.btnKembali);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label1);
            this.Name = "KelolaMahasiswa";
            this.Text = "Kelola Data Mahasiswa";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtNIM;
        private System.Windows.Forms.TextBox txtNama;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnHapus;
        private System.Windows.Forms.Button btnUbah;
        private System.Windows.Forms.Button btnTambah;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColNim;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColNamaMahasiswa;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColJurusan;
        private System.Windows.Forms.TextBox txtJurusan;
        private System.Windows.Forms.Button btnKembali;
        private System.Windows.Forms.Button btnLoad;
    }
}