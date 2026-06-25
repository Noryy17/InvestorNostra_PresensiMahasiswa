namespace SistemPresensiMahasiswa
{
    partial class RekapPresensi
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.cbMatakuliah = new System.Windows.Forms.ComboBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.dtpAwal = new System.Windows.Forms.DateTimePicker();
            this.dtpAkhir = new System.Windows.Forms.DateTimePicker();
            this.btnRekap = new System.Windows.Forms.Button();
            this.btnKembali = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.chartPresensi = new System.Windows.Forms.DataVisualization.Charting.Chart();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartPresensi)).BeginInit();
            this.SuspendLayout();
            // 
            // cbMatakuliah
            // 
            this.cbMatakuliah.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMatakuliah.FormattingEnabled = true;
            this.cbMatakuliah.Location = new System.Drawing.Point(95, 78);
            this.cbMatakuliah.Name = "cbMatakuliah";
            this.cbMatakuliah.Size = new System.Drawing.Size(216, 24);
            this.cbMatakuliah.TabIndex = 0;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(359, 34);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(555, 365);
            this.dataGridView1.TabIndex = 1;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // dtpAwal
            // 
            this.dtpAwal.Location = new System.Drawing.Point(111, 135);
            this.dtpAwal.Name = "dtpAwal";
            this.dtpAwal.Size = new System.Drawing.Size(200, 22);
            this.dtpAwal.TabIndex = 2;
            // 
            // dtpAkhir
            // 
            this.dtpAkhir.Location = new System.Drawing.Point(111, 188);
            this.dtpAkhir.Name = "dtpAkhir";
            this.dtpAkhir.Size = new System.Drawing.Size(200, 22);
            this.dtpAkhir.TabIndex = 3;
            // 
            // btnRekap
            // 
            this.btnRekap.BackColor = System.Drawing.Color.Lime;
            this.btnRekap.Location = new System.Drawing.Point(12, 246);
            this.btnRekap.Name = "btnRekap";
            this.btnRekap.Size = new System.Drawing.Size(146, 57);
            this.btnRekap.TabIndex = 4;
            this.btnRekap.Text = "Rekap Presensi";
            this.btnRekap.UseVisualStyleBackColor = false;
            this.btnRekap.Click += new System.EventHandler(this.btnRekap_Click);
            // 
            // btnKembali
            // 
            this.btnKembali.BackColor = System.Drawing.Color.Red;
            this.btnKembali.Location = new System.Drawing.Point(111, 349);
            this.btnKembali.Name = "btnKembali";
            this.btnKembali.Size = new System.Drawing.Size(115, 31);
            this.btnKembali.TabIndex = 5;
            this.btnKembali.Text = "Kembali";
            this.btnKembali.UseVisualStyleBackColor = false;
            this.btnKembali.Click += new System.EventHandler(this.btnKembali_Click);
            // 
            // btnClear
            // 
            this.btnClear.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.btnClear.Location = new System.Drawing.Point(181, 246);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(146, 57);
            this.btnClear.TabIndex = 6;
            this.btnClear.Text = "ClearForm";
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(95, 59);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(112, 16);
            this.label1.TabIndex = 7;
            this.label1.Text = "Nama Matakuliah";
            // 
            // chartPresensi
            // 
            chartArea1.Name = "ChartArea1";
            this.chartPresensi.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chartPresensi.Legends.Add(legend1);
            this.chartPresensi.Location = new System.Drawing.Point(957, 34);
            this.chartPresensi.Name = "chartPresensi";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chartPresensi.Series.Add(series1);
            this.chartPresensi.Size = new System.Drawing.Size(494, 365);
            this.chartPresensi.TabIndex = 8;
            this.chartPresensi.Text = "chart1";
            // 
            // RekapPresensi
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1479, 424);
            this.Controls.Add(this.chartPresensi);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnKembali);
            this.Controls.Add(this.btnRekap);
            this.Controls.Add(this.dtpAkhir);
            this.Controls.Add(this.dtpAwal);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.cbMatakuliah);
            this.Name = "RekapPresensi";
            this.Text = "FormRekapPresensi";
            this.Load += new System.EventHandler(this.FormRekapPresensi_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartPresensi)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cbMatakuliah;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DateTimePicker dtpAwal;
        private System.Windows.Forms.DateTimePicker dtpAkhir;
        private System.Windows.Forms.Button btnRekap;
        private System.Windows.Forms.Button btnKembali;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartPresensi;
    }
}