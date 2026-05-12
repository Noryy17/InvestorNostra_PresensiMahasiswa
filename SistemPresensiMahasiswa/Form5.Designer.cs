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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(KelolaMahasiswa));
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblTotal = new System.Windows.Forms.Label();
            this.txtJurusan = new System.Windows.Forms.TextBox();
            this.mahasiswaBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.sistemPresensiDBDataSet = new SistemPresensiMahasiswa.SistemPresensiDBDataSet();
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
            this.btnReset = new System.Windows.Forms.Button();
            this.btnInject = new System.Windows.Forms.Button();
            this.bindingNavigator1 = new System.Windows.Forms.BindingNavigator(this.components);
            this.bindingNavigatorAddNewItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorCountItem = new System.Windows.Forms.ToolStripLabel();
            this.bindingNavigatorDeleteItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorMoveFirstItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorMovePreviousItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.bindingNavigatorPositionItem = new System.Windows.Forms.ToolStripTextBox();
            this.bindingNavigatorSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.bindingNavigatorMoveNextItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorMoveLastItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.mahasiswaTableAdapter = new SistemPresensiMahasiswa.SistemPresensiDBDataSetTableAdapters.MahasiswaTableAdapter();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mahasiswaBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sistemPresensiDBDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator1)).BeginInit();
            this.bindingNavigator1.SuspendLayout();
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
            this.panel1.Controls.Add(this.lblTotal);
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
            // lblTotal
            // 
            this.lblTotal.AutoSize = true;
            this.lblTotal.Location = new System.Drawing.Point(175, 138);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(125, 16);
            this.lblTotal.TabIndex = 7;
            this.lblTotal.Text = "Total Mahasiswa : 0";
            this.lblTotal.Click += new System.EventHandler(this.lblTotal_Click);
            // 
            // txtJurusan
            // 
            this.txtJurusan.DataBindings.Add(new System.Windows.Forms.Binding("Tag", this.mahasiswaBindingSource, "jurusan", true));
            this.txtJurusan.Location = new System.Drawing.Point(142, 97);
            this.txtJurusan.Name = "txtJurusan";
            this.txtJurusan.Size = new System.Drawing.Size(197, 22);
            this.txtJurusan.TabIndex = 6;
            // 
            // mahasiswaBindingSource
            // 
            this.mahasiswaBindingSource.DataMember = "Mahasiswa";
            this.mahasiswaBindingSource.DataSource = this.sistemPresensiDBDataSet;
            // 
            // sistemPresensiDBDataSet
            // 
            this.sistemPresensiDBDataSet.DataSetName = "SistemPresensiDBDataSet";
            this.sistemPresensiDBDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // txtNIM
            // 
            this.txtNIM.DataBindings.Add(new System.Windows.Forms.Binding("Tag", this.mahasiswaBindingSource, "nim", true));
            this.txtNIM.Location = new System.Drawing.Point(142, 27);
            this.txtNIM.Name = "txtNIM";
            this.txtNIM.Size = new System.Drawing.Size(197, 22);
            this.txtNIM.TabIndex = 5;
            // 
            // txtNama
            // 
            this.txtNama.DataBindings.Add(new System.Windows.Forms.Binding("Tag", this.mahasiswaBindingSource, "nama", true));
            this.txtNama.Location = new System.Drawing.Point(142, 62);
            this.txtNama.Name = "txtNama";
            this.txtNama.Size = new System.Drawing.Size(197, 22);
            this.txtNama.TabIndex = 4;
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
            // btnReset
            // 
            this.btnReset.Location = new System.Drawing.Point(146, 445);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(75, 23);
            this.btnReset.TabIndex = 8;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // btnInject
            // 
            this.btnInject.Location = new System.Drawing.Point(387, 445);
            this.btnInject.Name = "btnInject";
            this.btnInject.Size = new System.Drawing.Size(75, 23);
            this.btnInject.TabIndex = 9;
            this.btnInject.Text = "Inject";
            this.btnInject.UseVisualStyleBackColor = true;
            this.btnInject.Click += new System.EventHandler(this.btnInject_Click);
            // 
            // bindingNavigator1
            // 
            this.bindingNavigator1.AddNewItem = this.bindingNavigatorAddNewItem;
            this.bindingNavigator1.BindingSource = this.mahasiswaBindingSource;
            this.bindingNavigator1.CountItem = this.bindingNavigatorCountItem;
            this.bindingNavigator1.DeleteItem = this.bindingNavigatorDeleteItem;
            this.bindingNavigator1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.bindingNavigator1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bindingNavigatorMoveFirstItem,
            this.bindingNavigatorMovePreviousItem,
            this.bindingNavigatorSeparator,
            this.bindingNavigatorPositionItem,
            this.bindingNavigatorCountItem,
            this.bindingNavigatorSeparator1,
            this.bindingNavigatorMoveNextItem,
            this.bindingNavigatorMoveLastItem,
            this.bindingNavigatorSeparator2,
            this.bindingNavigatorAddNewItem,
            this.bindingNavigatorDeleteItem});
            this.bindingNavigator1.Location = new System.Drawing.Point(0, 0);
            this.bindingNavigator1.MoveFirstItem = this.bindingNavigatorMoveFirstItem;
            this.bindingNavigator1.MoveLastItem = this.bindingNavigatorMoveLastItem;
            this.bindingNavigator1.MoveNextItem = this.bindingNavigatorMoveNextItem;
            this.bindingNavigator1.MovePreviousItem = this.bindingNavigatorMovePreviousItem;
            this.bindingNavigator1.Name = "bindingNavigator1";
            this.bindingNavigator1.PositionItem = this.bindingNavigatorPositionItem;
            this.bindingNavigator1.Size = new System.Drawing.Size(1046, 31);
            this.bindingNavigator1.TabIndex = 10;
            this.bindingNavigator1.Text = "bindingNavigator1";
            // 
            // bindingNavigatorAddNewItem
            // 
            this.bindingNavigatorAddNewItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorAddNewItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorAddNewItem.Image")));
            this.bindingNavigatorAddNewItem.Name = "bindingNavigatorAddNewItem";
            this.bindingNavigatorAddNewItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorAddNewItem.Size = new System.Drawing.Size(29, 28);
            this.bindingNavigatorAddNewItem.Text = "Add new";
            // 
            // bindingNavigatorCountItem
            // 
            this.bindingNavigatorCountItem.Name = "bindingNavigatorCountItem";
            this.bindingNavigatorCountItem.Size = new System.Drawing.Size(45, 24);
            this.bindingNavigatorCountItem.Text = "of {0}";
            this.bindingNavigatorCountItem.ToolTipText = "Total number of items";
            // 
            // bindingNavigatorDeleteItem
            // 
            this.bindingNavigatorDeleteItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorDeleteItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorDeleteItem.Image")));
            this.bindingNavigatorDeleteItem.Name = "bindingNavigatorDeleteItem";
            this.bindingNavigatorDeleteItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorDeleteItem.Size = new System.Drawing.Size(29, 24);
            this.bindingNavigatorDeleteItem.Text = "Delete";
            // 
            // bindingNavigatorMoveFirstItem
            // 
            this.bindingNavigatorMoveFirstItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveFirstItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveFirstItem.Image")));
            this.bindingNavigatorMoveFirstItem.Name = "bindingNavigatorMoveFirstItem";
            this.bindingNavigatorMoveFirstItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveFirstItem.Size = new System.Drawing.Size(29, 24);
            this.bindingNavigatorMoveFirstItem.Text = "Move first";
            // 
            // bindingNavigatorMovePreviousItem
            // 
            this.bindingNavigatorMovePreviousItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMovePreviousItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMovePreviousItem.Image")));
            this.bindingNavigatorMovePreviousItem.Name = "bindingNavigatorMovePreviousItem";
            this.bindingNavigatorMovePreviousItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMovePreviousItem.Size = new System.Drawing.Size(29, 24);
            this.bindingNavigatorMovePreviousItem.Text = "Move previous";
            // 
            // bindingNavigatorSeparator
            // 
            this.bindingNavigatorSeparator.Name = "bindingNavigatorSeparator";
            this.bindingNavigatorSeparator.Size = new System.Drawing.Size(6, 27);
            // 
            // bindingNavigatorPositionItem
            // 
            this.bindingNavigatorPositionItem.AccessibleName = "Position";
            this.bindingNavigatorPositionItem.AutoSize = false;
            this.bindingNavigatorPositionItem.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.bindingNavigatorPositionItem.Name = "bindingNavigatorPositionItem";
            this.bindingNavigatorPositionItem.Size = new System.Drawing.Size(50, 27);
            this.bindingNavigatorPositionItem.Text = "0";
            this.bindingNavigatorPositionItem.ToolTipText = "Current position";
            // 
            // bindingNavigatorSeparator1
            // 
            this.bindingNavigatorSeparator1.Name = "bindingNavigatorSeparator1";
            this.bindingNavigatorSeparator1.Size = new System.Drawing.Size(6, 27);
            // 
            // bindingNavigatorMoveNextItem
            // 
            this.bindingNavigatorMoveNextItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveNextItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveNextItem.Image")));
            this.bindingNavigatorMoveNextItem.Name = "bindingNavigatorMoveNextItem";
            this.bindingNavigatorMoveNextItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveNextItem.Size = new System.Drawing.Size(29, 24);
            this.bindingNavigatorMoveNextItem.Text = "Move next";
            // 
            // bindingNavigatorMoveLastItem
            // 
            this.bindingNavigatorMoveLastItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveLastItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveLastItem.Image")));
            this.bindingNavigatorMoveLastItem.Name = "bindingNavigatorMoveLastItem";
            this.bindingNavigatorMoveLastItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveLastItem.Size = new System.Drawing.Size(29, 24);
            this.bindingNavigatorMoveLastItem.Text = "Move last";
            // 
            // bindingNavigatorSeparator2
            // 
            this.bindingNavigatorSeparator2.Name = "bindingNavigatorSeparator2";
            this.bindingNavigatorSeparator2.Size = new System.Drawing.Size(6, 27);
            // 
            // mahasiswaTableAdapter
            // 
            this.mahasiswaTableAdapter.ClearBeforeFill = true;
            // 
            // KelolaMahasiswa
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1046, 580);
            this.Controls.Add(this.bindingNavigator1);
            this.Controls.Add(this.btnInject);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.btnKembali);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label1);
            this.Name = "KelolaMahasiswa";
            this.Text = "Kelola Data Mahasiswa";
            this.Load += new System.EventHandler(this.KelolaMahasiswa_Load_1);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mahasiswaBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sistemPresensiDBDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator1)).EndInit();
            this.bindingNavigator1.ResumeLayout(false);
            this.bindingNavigator1.PerformLayout();
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
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Button btnInject;
        private System.Windows.Forms.BindingNavigator bindingNavigator1;
        private System.Windows.Forms.ToolStripButton bindingNavigatorAddNewItem;
        private System.Windows.Forms.ToolStripLabel bindingNavigatorCountItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorDeleteItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveFirstItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMovePreviousItem;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator;
        private System.Windows.Forms.ToolStripTextBox bindingNavigatorPositionItem;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator1;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveNextItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveLastItem;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator2;
        private SistemPresensiDBDataSet sistemPresensiDBDataSet;
        private System.Windows.Forms.BindingSource mahasiswaBindingSource;
        private SistemPresensiDBDataSetTableAdapters.MahasiswaTableAdapter mahasiswaTableAdapter;
        private System.Windows.Forms.Label lblTotal;
    }
}