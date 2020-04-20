using SSync.IO;
using System;
using System.IO;
using System.Text;

namespace Symbioz.Tools.IO {
    public unsafe class FastBigEndianReader : IDataReader, IDisposable {
        private long m_position;
        private readonly byte[] m_buffer;
        private long m_maxPosition = -1L;

        public byte[] Buffer {
            get { return this.m_buffer; }
        }

        public int Position {
            get { return (int) this.m_position; }
            set {
                if (this.m_maxPosition > 0L && value > this.m_maxPosition) {
                    throw new InvalidOperationException("Buffer overflow");
                }

                this.m_position = value;
            }
        }

        public long MaxPosition {
            get { return this.m_maxPosition; }
            set { this.m_maxPosition = value; }
        }

        public int BytesAvailable {
            get { return (int) (((this.m_maxPosition > 0L) ? this.m_maxPosition : this.m_buffer.Length) - this.Position); }
        }

        public FastBigEndianReader(byte[] buffer) {
            this.m_buffer = buffer;
        }


        public byte ReadByte() {
            fixed (byte* pbyte = &this.m_buffer[this.m_position++]) {
                return *pbyte;
            }
        }

        public sbyte ReadSByte() {
            fixed (byte* pbyte = &this.m_buffer[this.m_position++]) {
                return (sbyte) *pbyte;
            }
        }

        public short ReadShort() {
            var position = this.m_position;
            this.m_position += 2;
            fixed (byte* pbyte = &this.m_buffer[position]) {
                return (short) ((*pbyte << 8) | (*(pbyte + 1)));
            }
        }

        public int ReadInt() {
            var position = this.m_position;
            this.m_position += 4;
            fixed (byte* pbyte = &this.m_buffer[position]) {
                return (*pbyte << 24) | (*(pbyte + 1) << 16) | (*(pbyte + 2) << 8) | (*(pbyte + 3));
            }
        }

        public long ReadLong() {
            var position = this.m_position;
            this.m_position += 8;
            fixed (byte* pbyte = &this.m_buffer[position]) {
                int i1 = (*pbyte << 24) | (*(pbyte + 1) << 16) | (*(pbyte + 2) << 8) | (*(pbyte + 3));
                int i2 = (*(pbyte + 4) << 24) | (*(pbyte + 5) << 16) | (*(pbyte + 6) << 8) | (*(pbyte + 7));

                return (uint) i2 | ((long) i1 << 32);
            }
        }

        public ushort ReadUShort() {
            return (ushort) this.ReadShort();
        }

        public uint ReadUInt() {
            return (uint) this.ReadInt();
        }

        public ulong ReadULong() {
            return (ulong) this.ReadLong();
        }

        public byte[] ReadBytes(int n) {
            if (this.BytesAvailable < (long) n) {
                throw new InvalidOperationException("Buffer overflow");
            }

            var dst = new byte[n];
            fixed (byte* pSrc = &this.m_buffer[this.m_position], pDst = dst) {
                byte* ps = pSrc;
                byte* pd = pDst;

                // Loop over the count in blocks of 4 bytes, copying an integer (4 bytes) at a time:
                for (int i = 0; i < n / 4; i++) {
                    *((int*) pd) = *((int*) ps);
                    pd += 4;
                    ps += 4;
                }

                // Complete the copy by moving any bytes that weren't moved in blocks of 4:
                for (int i = 0; i < n % 4; i++) {
                    *pd = *ps;
                    pd++;
                    ps++;
                }
            }

            this.m_position += n;

            return dst;
        }

        public bool ReadBoolean() {
            return this.ReadByte() != 0;
        }

        public char ReadChar() {
            return (char) this.ReadShort();
        }

        public float ReadFloat() {
            int val = this.ReadInt();

            return *(float*) &val;
        }

        public double ReadDouble() {
            long val = this.ReadLong();

            return *(double*) &val;
        }

        public string ReadUTF() {
            ushort length = this.ReadUShort();

            byte[] bytes = this.ReadBytes(length);

            return Encoding.UTF8.GetString(bytes);
        }

        public string ReadUTFBytes(ushort len) {
            byte[] bytes = this.ReadBytes(len);

            return Encoding.UTF8.GetString(bytes);
        }

        public void Seek(int offset, SeekOrigin seekOrigin) {
            if (seekOrigin == SeekOrigin.Begin) {
                this.Position = offset;
            }
            else {
                if (seekOrigin == SeekOrigin.End) {
                    this.Position = this.m_buffer.Length + offset;
                }
                else {
                    if (seekOrigin == SeekOrigin.Current) {
                        this.Position += offset;
                    }
                }
            }
        }

        public void Seek(long offset, SeekOrigin seekOrigin) {
            if (seekOrigin == SeekOrigin.Begin) {
                this.Position = (int) offset;
            }
            else {
                if (seekOrigin == SeekOrigin.End) {
                    this.Position = (int) (this.m_buffer.Length + offset);
                }
                else {
                    if (seekOrigin == SeekOrigin.Current) {
                        this.Position += (int) offset;
                    }
                }
            }
        }

        public void SkipBytes(int n) {
            this.Position += n;
        }

        public void Dispose() { }

        public float ReadSingle() {
            return this.ReadFloat();
        }
    }
}