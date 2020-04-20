using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Symbioz.UIModules {
    /// <summary>
    /// Logique d'interaction pour Editor.xaml
    /// </summary>
    public partial class Editor : Window {
        public static string DOFUS_PATH;

        public static string MODULE_NAME;


        private DofusUIModule Module { get; set; }

        public Editor(string dofusPath, string moduleName) {
            DOFUS_PATH = dofusPath;
            MODULE_NAME = moduleName;
            this.Module = new DofusUIModule(DOFUS_PATH, MODULE_NAME);
            this.InitializeComponent();
        }


        private void OnMouseMove(object sender, MouseEventArgs e) {
            var pos = Mouse.GetPosition(this.moduleBack);
            this.rect.Margin = new Thickness(pos.X - this.rect.Width / 2, pos.Y - this.rect.Height / 2, 0, 0);
        }
    }
}