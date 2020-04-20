using Symbioz.Tools.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.UIModules {
    public class DofusUIModule {
        public string ModuleName { get; private set; }
        public string DofusPath { get; private set; }

        private string ModulePath {
            get { return Editor.DOFUS_PATH + "/ui/" + this.ModuleName + "/"; }
        }

        private string D2UIPath {
            get { return this.ModulePath + this.ModuleName + ".d2ui"; }
        }

        private string DMPath {
            get { return this.ModulePath + this.ModuleName + ".dm"; }
        }

        private string UIPath {
            get { return this.ModulePath + "/ui/"; }
        }

        public DofusUIModule(string dofusPath, string moduleName) {
            this.ModuleName = moduleName;
            this.DofusPath = dofusPath;
            this.InitializeNewModule();
        }

        private void InitializeNewModule() {
            if (!Directory.Exists(this.ModulePath)) {
                Directory.CreateDirectory(this.ModulePath);
                Directory.CreateDirectory(this.UIPath);
            }

            D2UIFile file = new D2UIFile(this.D2UIPath);
        }
    }
}