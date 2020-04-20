using Symbioz.Tools.D2P;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Symbioz.DofusMusic {
    public partial class Form1 : Form {
        public static string MUSIC_PATH = Environment.CurrentDirectory + "/Ouput/";

        private List<D2PEntryDescription> Values { get; set; }
        private D2pFile D2PFile { get; set; }

        public Form1() {
            this.Values = new List<D2PEntryDescription>();
            this.InitializeComponent();
            this.gridView.CellMouseDoubleClick += this.OnCellClicked;
            if (Directory.Exists(MUSIC_PATH) == false) {
                Directory.CreateDirectory(MUSIC_PATH);
            }
        }

        private void BindDataSource(Array value) {
            this.gridView.DataSource = value;
            this.gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
        }

        private void OnCellClicked(object sender, DataGridViewCellMouseEventArgs e) {
            if (e.ColumnIndex == 1) {
                try {
                    string containerName = (string) this.gridView.Rows[e.RowIndex].Cells[0].Value;
                    string entryName = (string) this.gridView.Rows[e.RowIndex].Cells[1].Value;
                    Options option = new Options(this.D2PFile, entryName, containerName);
                    option.Show();
                }
                catch { }
            }
        }

        private void button1_Click(object sender, EventArgs e) {
            this.openFileDialog1.ShowDialog();
            string path = this.openFileDialog1.FileName;

            if (path != string.Empty) {
                this.D2PFile = new D2pFile(path);
                foreach (var entry in this.D2PFile.Entries) {
                    int soundId = 0;
                    int.TryParse(Path.GetFileNameWithoutExtension(entry.FileName), out soundId);
                    this.Values.Add(new D2PEntryDescription(entry.FullFileName, entry.Container.FilePath, D2OConstants.GetRelativeSubarea(soundId)));
                }

                this.BindDataSource(this.Values.ToArray());
            }
        }

        private void button3_Click(object sender, EventArgs e) {
            if (this.searchContent.Text == string.Empty) {
                this.BindDataSource(this.Values.ToArray());

                return;
            }

            string search = this.searchContent.Text.ToLower();
            var results = Array.FindAll(this.Values.ToArray(), x => x.ContainerFileName.ToLower().Contains(search) || x.FileName.ToLower().Contains(search) || x.Informations.ToLower().Contains(search));
            this.BindDataSource(results);
        }
    }
}