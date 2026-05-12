namespace SistemPresensiMahasiswa
{
    partial class GenerateLaporan
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
            this.cbMatakuliah = new System.Windows.Forms.ComboBox();
            this.cbDosen = new System.Windows.Forms.ComboBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.dtpAwal = new System.Windows.Forms.DateTimePicker();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.dtpAkhir = new System.Windows.Forms.DateTimePicker();
            this.btnKembali = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // cbMatakuliah
            // 
            this.cbMatakuliah.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMatakuliah.FormattingEnabled = true;
            this.cbMatakuliah.Location = new System.Drawing.Point(37, 53);
            this.cbMatakuliah.Name = "cbMatakuliah";
            this.cbMatakuliah.Size = new System.Drawing.Size(184, 24);
            this.cbMatakuliah.TabIndex = 0;
            // 
            // cbDosen
            // 
            this.cbDosen.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDosen.FormattingEnabled = true;
            this.cbDosen.Location = new System.Drawing.Point(296, 53);
            this.cbDosen.Name = "cbDosen";
            this.cbDosen.Size = new System.Drawing.Size(222, 24);
            this.cbDosen.TabIndex = 1;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(594, 32);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(555, 525);
            this.dataGridView1.TabIndex = 2;
            // 
            // dtpAwal
            // 
            this.dtpAwal.Location = new System.Drawing.Point(37, 171);
            this.dtpAwal.Name = "dtpAwal";
            this.dtpAwal.Size = new System.Drawing.Size(200, 22);
            this.dtpAwal.TabIndex = 3;
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(37, 265);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(143, 53);
            this.btnGenerate.TabIndex = 5;
            this.btnGenerate.Text = "Generate";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // dtpAkhir
            // 
            this.dtpAkhir.Location = new System.Drawing.Point(318, 171);
            this.dtpAkhir.Name = "dtpAkhir";
            this.dtpAkhir.Size = new System.Drawing.Size(200, 22);
            this.dtpAkhir.TabIndex = 6;
            this.dtpAkhir.ValueChanged += new System.EventHandler(this.dtpAkhir_ValueChanged);
            // 
            // btnKembali
            // 
            this.btnKembali.BackColor = System.Drawing.Color.Red;
            this.btnKembali.Location = new System.Drawing.Point(381, 265);
            this.btnKembali.Name = "btnKembali";
            this.btnKembali.Size = new System.Drawing.Size(137, 53);
            this.btnKembali.TabIndex = 7;
            this.btnKembali.Text = "Kembali";
            this.btnKembali.UseVisualStyleBackColor = false;
            this.btnKembali.Click += new System.EventHandler(this.btnKembali_Click);
            // 
            // GenerateLaporan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1160, 592);
            this.Controls.Add(this.btnKembali);
            this.Controls.Add(this.dtpAkhir);
            this.Controls.Add(this.btnGenerate);
            this.Controls.Add(this.dtpAwal);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.cbDosen);
            this.Controls.Add(this.cbMatakuliah);
            this.Name = "GenerateLaporan";
            this.Text = "Generate Laporan Presensi";
            this.Load += new System.EventHandler(this.GenerateLaporan_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox cbMatakuliah;
        private System.Windows.Forms.ComboBox cbDosen;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DateTimePicker dtpAwal;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.DateTimePicker dtpAkhir;
        private System.Windows.Forms.Button btnKembali;
    }
}