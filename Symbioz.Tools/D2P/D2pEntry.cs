using System;
using System.ComponentModel;
using System.IO;
using SSync.IO;

namespace Symbioz.Tools.D2P {
    public class D2pEntry : INotifyPropertyChanged {
        private string _FullFileName;
        private byte[] _NewData;

        public event PropertyChangedEventHandler PropertyChanged;

        private D2pFile _Container;

        public D2pFile Container {
            get { return this._Container; }

            set {
                if (this._Container == value) {
                    return;
                }

                this._Container = value;
                this.OnPropertyChanged("Container");
            }
        }

        public string FileName {
            get { return Path.GetFileName(this._FullFileName); }
        }

        public string FullFileName {
            get { return this._FullFileName; }
            private set {
                if (string.Equals(this._FullFileName, value)) {
                    return;
                }

                this._FullFileName = value;
                this.OnPropertyChanged("FullFileName");
            }
        }

        private D2pDirectory _Directory;

        public D2pDirectory Directory {
            get { return this._Directory; }

            set {
                if (this._Directory == value) {
                    return;
                }

                this._Directory = value;
                this.OnPropertyChanged("Directory");
            }
        }

        private int _Index;

        public int Index {
            get { return this._Index; }

            set {
                if (this._Index == value) {
                    return;
                }

                this._Index = value;
                this.OnPropertyChanged("Index");
            }
        }

        private int _Size;

        public int Size {
            get { return this._Size; }

            set {
                if (this._Size == value) {
                    return;
                }

                this._Size = value;
                this.OnPropertyChanged("Size");
            }
        }

        private D2pEntryState _State;

        public D2pEntryState State {
            get { return this._State; }

            set {
                if (this._State == value) {
                    return;
                }

                this._State = value;
                this.OnPropertyChanged("State");
            }
        }

        private D2pEntry(D2pFile container) {
            this.Container = container;
            this.Index = -1;
        }

        public D2pEntry(D2pFile container, string fileName) {
            this.Container = container;
            this.FullFileName = fileName;
            this.Index = -1;
        }

        public D2pEntry(D2pFile container, string fileName, byte[] data) {
            this.Container = container;
            this.FullFileName = fileName;
            this._NewData = data;
            this.State = D2pEntryState.Added;
            this.Size = data.Length;
            this.Index = -1;
        }

        public static D2pEntry CreateEntryDefinition(D2pFile container, IDataReader reader) {
            D2pEntry entry = new D2pEntry(container);
            entry.ReadEntryDefinition(reader);

            return entry;
        }

        public void ReadEntryDefinition(IDataReader reader) {
            this.FullFileName = reader.ReadUTF();
            this.Index = reader.ReadInt();
            this.Size = reader.ReadInt();
        }

        public void WriteEntryDefinition(IDataWriter writer) {
            if (this.Index == -1) {
                throw new InvalidOperationException("Invalid entry, index = -1");
            }

            writer.WriteUTF(this.FullFileName);
            writer.WriteInt(this.Index);
            writer.WriteInt(this.Size);
        }

        public byte[] ReadEntry(IDataReader reader) {
            if (this.State == D2pEntryState.Removed) {
                throw new InvalidOperationException("Cannot read a deleted entry");
            }

            byte[] result;
            if (this.State == D2pEntryState.Dirty || this.State == D2pEntryState.Added) {
                result = this._NewData;
            }
            else {
                result = reader.ReadBytes(this.Size);
            }

            return result;
        }

        public void ModifyEntry(byte[] data) {
            this._NewData = data;
            this.Size = data.Length;
            this.State = D2pEntryState.Dirty;
        }

        public string[] GetDirectoriesName() {
            return Path
                     .GetDirectoryName(this.FullFileName)
                     ?.Split(new char[] {'/', '\\'}, StringSplitOptions.RemoveEmptyEntries);
        }

        public virtual void OnPropertyChanged(string propertyName) {
            PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if (propertyChanged != null) {
                propertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}