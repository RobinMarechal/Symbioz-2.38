using Symbioz.Tools.D2I;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Symbioz.Core;
using System.Windows.Forms;
using SSync;

namespace Symbioz.D2I {
    public partial class View : Form {
        private D2IFile File { get; set; }
        private Dictionary<int, string> Values { get; set; }

        public View() {
            this.InitializeComponent();
            this.dataGridView1.CellMouseClick += this.OnCellClicked;
        }

        private void OnCellClicked(object sender, DataGridViewCellMouseEventArgs e) {
            int id = (int) this.dataGridView1.Rows[e.RowIndex].Cells[0].Value;
            string value = (string) this.dataGridView1.Rows[e.RowIndex].Cells[1].Value;

            Edit edit = new Edit(this, id, value);
            edit.Show();
        }

        public void Seti18n(int id, string contentText) {
            this.File.SetText(id, contentText);
            this.Values[id] = contentText;
            this.BindDataSource((from item in this.Values select new {Item = item.Key, Price = item.Value}).ToArray());
        }

        private void button1_Click(object sender, EventArgs e) {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.ShowDialog();
            this.File = new D2IFile(dialog.FileName);
            this.Values = this.File.GetAllText();
            this.BindDataSource((from item in this.Values select new {Item = item.Key, Price = item.Value}).ToArray());
        }

        private void BindDataSource(Array value) {
            this.dataGridView1.DataSource = value;
            this.dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
        }

        private void button3_Click(object sender, EventArgs e) {
            if (this.searchContent.Text == string.Empty) {
                this.BindDataSource((from item in this.Values select new {Item = item.Key, Price = item.Value}).ToArray());

                return;
            }

            string search = this.searchContent.Text.ToLower();
            var results = Array.FindAll(this.Values.ToArray(), x => x.Value.ToLower().Contains(search));
            this.BindDataSource(results);
        }

        private void button2_Click(object sender, EventArgs e) {
            if (new FileInfo(this.File.FilePath).IsFileLocked()) {
                var dir = Path.GetDirectoryName(this.File.FilePath) + "_2.d2i";
                this.File.Save(dir);
            }
            else {
                this.File.Save();
            }
        }
    }
}