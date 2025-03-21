using CsvHelper;
using System;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

namespace TransferMediaCsvToS3App
{
    public partial class ViewFileModalForm : Form
    {
        private string csvFilePath;
        public ViewFileModalForm(string csvFilePath)
        {
            InitializeComponent();

            this.StartPosition = FormStartPosition.CenterScreen;
            this.csvFilePath = csvFilePath;
        }

        private void ViewFileModalForm_Load(object sender, EventArgs e)
        {
            try
            {
                DataTable table = LoadCsvToDataTable(csvFilePath);

                dataGridView1.DataSource = table;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"CSV dosyasını okurken hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private DataTable LoadCsvToDataTable(string csvPath)
        {
            DataTable table = new DataTable();

            using (var stream = new FileStream(csvPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var reader = new StreamReader(stream))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                using (var dr = new CsvDataReader(csv))
                {
                    table.Load(dr);
                }
            }

            return table;
        }

        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            var grid = sender as DataGridView;
            var rowIndex = (e.RowIndex + 1).ToString();
            var centerFormat = new StringFormat()
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };

            var headerBounds = new Rectangle(
                e.RowBounds.Left,
                e.RowBounds.Top,
                grid.RowHeadersWidth,
                e.RowBounds.Height);

            e.Graphics.DrawString(rowIndex, this.Font, SystemBrushes.ControlText, headerBounds, centerFormat);
        }
    }
}
