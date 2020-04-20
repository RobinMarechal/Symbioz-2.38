using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text;
using SSync.IO;

namespace Symbioz.Tools.DLM {
    /// <summary>
    ///  From Stump
    /// </summary>
    public class DlmMap : INotifyPropertyChanged {
        public const int CELL_COUNT = 560;

        public event PropertyChangedEventHandler PropertyChanged;

        private byte _Version;


        public List<short> TopArrowCells = new List<short>();

        public List<short> BottomArrowCells = new List<short>();

        public List<short> RightArrowCells = new List<short>();

        public List<short> LeftArrowCells = new List<short>();

        public byte Version {
            get { return this._Version; }

            set {
                this._Version = value;
                this.OnPropertyChanged("Version");
            }
        }

        private int _Id;

        public int Id {
            get { return this._Id; }

            set {
                if (this._Id == value) {
                    return;
                }

                this._Id = value;
                this.OnPropertyChanged("Id");
            }
        }

        private bool _Encrypted;

        public bool Encrypted {
            get => this._Encrypted;

            set {
                if (this._Encrypted == value) {
                    return;
                }

                this._Encrypted = value;
                this.OnPropertyChanged("Encrypted");
            }
        }

        private byte _EncryptionVersion;

        public byte EncryptionVersion {
            get { return this._EncryptionVersion; }

            set {
                this._EncryptionVersion = value;
                this.OnPropertyChanged("EncryptionVersion");
            }
        }

        private uint _RelativeId;

        public uint RelativeId {
            get => this._RelativeId;

            set {
                if (this._RelativeId == value) {
                    return;
                }

                this._RelativeId = value;
                this.OnPropertyChanged("RelativeId");
            }
        }

        private byte _MapType;

        public byte MapType {
            get => this._MapType;

            set {
                this._MapType = value;
                this.OnPropertyChanged("MapType");
            }
        }

        private int _SubAreaId;

        public int SubAreaId {
            get => this._SubAreaId;

            set {
                if (this._SubAreaId == value) {
                    return;
                }

                this._SubAreaId = value;
                this.OnPropertyChanged("SubAreaId");
            }
        }

        private int _BottomNeighbourId;

        public int BottomNeighbourId {
            get => this._BottomNeighbourId;

            set {
                if (this._BottomNeighbourId == value) {
                    return;
                }

                this._BottomNeighbourId = value;
                this.OnPropertyChanged("BottomNeighbourId");
            }
        }

        private int _LeftNeighbourId;

        public int LeftNeighbourId {
            get => this._LeftNeighbourId;

            set {
                if (this._LeftNeighbourId == value) {
                    return;
                }

                this._LeftNeighbourId = value;
                this.OnPropertyChanged("LeftNeighbourId");
            }
        }

        private int _RightNeighbourId;

        public int RightNeighbourId {
            get => this._RightNeighbourId;

            set {
                if (this._RightNeighbourId == value) {
                    return;
                }

                this._RightNeighbourId = value;
                this.OnPropertyChanged("RightNeighbourId");
            }
        }

        private int _ShadowBonusOnEntities;

        public int ShadowBonusOnEntities {
            get => this._ShadowBonusOnEntities;

            set {
                if (this._ShadowBonusOnEntities == value) {
                    return;
                }

                this._ShadowBonusOnEntities = value;
                this.OnPropertyChanged("ShadowBonusOnEntities");
            }
        }

        private Color _BackgroundColor;

        public Color BackgroundColor {
            get => this._BackgroundColor;

            set {
                if (this._BackgroundColor == value) {
                    return;
                }

                this._BackgroundColor = value;
                this.OnPropertyChanged("BackgroundColor");
            }
        }

        private ushort _ZoomScale;

        public ushort ZoomScale {
            get => this._ZoomScale;

            set {
                this._ZoomScale = value;
                this.OnPropertyChanged("ZoomScale");
            }
        }

        private short _ZoomOffsetX;

        public short ZoomOffsetX {
            get => this._ZoomOffsetX;

            set {
                this._ZoomOffsetX = value;
                this.OnPropertyChanged("ZoomOffsetX");
            }
        }

        private short _ZoomOffsetY;

        public short ZoomOffsetY {
            get => this._ZoomOffsetY;

            set {
                this._ZoomOffsetY = value;
                this.OnPropertyChanged("ZoomOffsetY");
            }
        }

        private bool _UseLowPassFilter;

        public bool UseLowPassFilter {
            get => this._UseLowPassFilter;

            set {
                if (this._UseLowPassFilter == value) {
                    return;
                }

                this._UseLowPassFilter = value;
                this.OnPropertyChanged("UseLowPassFilter");
            }
        }

        private bool _UseReverb;

        public bool UseReverb {
            get => this._UseReverb;

            set {
                if (this._UseReverb == value) {
                    return;
                }

                this._UseReverb = value;
                this.OnPropertyChanged("UseReverb");
            }
        }

        private int _PresetId;

        public int PresetId {
            get => this._PresetId;

            set {
                if (this._PresetId == value) {
                    return;
                }

                this._PresetId = value;
                this.OnPropertyChanged("PresetId");
            }
        }

        private DlmFixture[] _BackgroudFixtures;

        public DlmFixture[] BackgroudFixtures {
            get => this._BackgroudFixtures;

            set {
                if (this._BackgroudFixtures == value) {
                    return;
                }

                this._BackgroudFixtures = value;
                this.OnPropertyChanged("BackgroudFixtures");
            }
        }

        private int _TopNeighbourId;

        public int TopNeighbourId {
            get => this._TopNeighbourId;

            set {
                if (this._TopNeighbourId == value) {
                    return;
                }

                this._TopNeighbourId = value;
                this.OnPropertyChanged("TopNeighbourId");
            }
        }

        private DlmFixture[] _ForegroundFixtures;

        public DlmFixture[] ForegroundFixtures {
            get => this._ForegroundFixtures;

            set {
                if (this._ForegroundFixtures == value) {
                    return;
                }

                this._ForegroundFixtures = value;
                this.OnPropertyChanged("ForegroundFixtures");
            }
        }

        private DlmCellData[] _Cells;

        public DlmCellData[] Cells {
            get => this._Cells;

            set {
                if (this._Cells == value) {
                    return;
                }

                this._Cells = value;
                this.OnPropertyChanged("Cells");
            }
        }

        private int _GroundCrc;

        public int GroundCrc {
            get => this._GroundCrc;

            set {
                if (this._GroundCrc == value) {
                    return;
                }

                this._GroundCrc = value;
                this.OnPropertyChanged("GroundCRC");
            }
        }

        private DlmLayer[] _Layers;

        public DlmLayer[] Layers {
            get => this._Layers;

            set {
                if (this._Layers == value) {
                    return;
                }

                this._Layers = value;
                this.OnPropertyChanged("Layers");
            }
        }

        public static DlmMap ReadFromStream(BigEndianReader givenReader, DlmReader dlmReader) {
            BigEndianReader reader = givenReader;
            DlmMap map = new DlmMap { Version = reader.ReadByte(), Id = reader.ReadInt() };
            if (map.Version >= 7) {
                map.Encrypted = reader.ReadBoolean();
                map.EncryptionVersion = reader.ReadByte();
                int len = reader.ReadInt();
                if (map.Encrypted) {
                    string key = dlmReader.DecryptionKey;
                    if (key == null && dlmReader.DecryptionKeyProvider != null) {
                        key = dlmReader.DecryptionKeyProvider(map.Id);
                    }

                    if (key == null) {
                        throw new InvalidOperationException($"Cannot decrypt the map {map.Id} without decryption key");
                    }

                    byte[] data = reader.ReadBytes(len);
                    byte[] encodedKey = Encoding.Default.GetBytes(key);
                    if (key.Length > 0) {
                        for (int i = 0; i < data.Length; i++) {
                            data[i] ^= encodedKey[i % key.Length];
                        }

                        reader = new BigEndianReader(new MemoryStream(data));
                    }
                }
            }

            map.RelativeId = reader.ReadUInt();
            map.MapType = reader.ReadByte();
            map.SubAreaId = reader.ReadInt();
            map.TopNeighbourId = reader.ReadInt();
            map.BottomNeighbourId = reader.ReadInt();
            map.LeftNeighbourId = reader.ReadInt();
            map.RightNeighbourId = reader.ReadInt();
            map.ShadowBonusOnEntities = (int) reader.ReadUInt();

            // ReSharper disable UnusedVariable
            if (map.Version >= 9) {
                var readColor = reader.ReadInt();
                var backgroundAlpha = (readColor & 4278190080) >> 32;
                var backgroundRed = (readColor & 16711680) >> 16;
                var backgroundGreen = (readColor & 65280) >> 8;
                var backgroundBlue = readColor & 255;
                readColor = (int) reader.ReadUInt();
                var gridAlpha = (readColor & 4278190080) >> 32;
                var gridRed = (readColor & 16711680) >> 16;
                var gridGreen = (readColor & 65280) >> 8;
                var gridBlue = readColor & 255;
                var gridColor = (gridAlpha & 255) << 32 | (gridRed & 255) << 16 | (gridGreen & 255) << 8 | gridBlue & 255;
            }
            // ReSharper restore UnusedVariable
            else if (map.Version >= 3) {
                map.BackgroundColor = Color.FromArgb(reader.ReadByte(), reader.ReadByte(), reader.ReadByte());
            }

            if (map.Version >= 4) {
                map.ZoomScale = reader.ReadUShort();
                map.ZoomOffsetX = reader.ReadShort();
                map.ZoomOffsetY = reader.ReadShort();
            }

            map.UseLowPassFilter = (reader.ReadByte() == 1);
            map.UseReverb = (reader.ReadByte() == 1);
            if (map.UseReverb) {
                map.PresetId = reader.ReadInt();
            }
            else
                map.PresetId = -1;

            map.BackgroudFixtures = new DlmFixture[reader.ReadByte()];
            for (int i = 0; i < map.BackgroudFixtures.Length; i++) {
                map.BackgroudFixtures[i] = DlmFixture.ReadFromStream(map, reader);
            }

            map.ForegroundFixtures = new DlmFixture[reader.ReadByte()];
            for (int i = 0; i < map.ForegroundFixtures.Length; i++) {
                map.ForegroundFixtures[i] = DlmFixture.ReadFromStream(map, reader);
            }

            reader.ReadInt();
            map.GroundCrc = reader.ReadInt();
            map.Layers = new DlmLayer[reader.ReadByte()];
            for (int i = 0; i < map.Layers.Length; i++) {
                map.Layers[i] = DlmLayer.ReadFromStream(map, reader);
            }

            map.Cells = new DlmCellData[560];
            short j = 0;
            while (j < map.Cells.Length) {
                map.Cells[j] = DlmCellData.ReadFromStream(map, j, reader);
                j += 1;
            }

            return map;
        }

        public virtual void OnPropertyChanged(string propertyName) {
            PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            propertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}