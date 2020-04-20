using System;
using System.Diagnostics;
using System.IO;

namespace SSync.Sockets {
    /// <summary>
    /// Similar to MemoryStream, but with an underlying BufferSegment.
    /// Will automatically free the old and get a new segment if its length was exceeded.
    /// </summary>
    public class SegmentStream : Stream {
        private int m_Position;
        private BufferSegment _segment;
        private int __length;

        private int _length {
            get { return this.__length; }
            set { this.__length = value; }
        }

        public SegmentStream(BufferSegment segment) {
            this._segment = segment;
            this.m_Position = this._segment.Offset;
        }

        public BufferSegment Segment {
            get { return this._segment; }
        }

        public override void Flush() { }

        public override long Seek(long offset, SeekOrigin origin) {
            switch (origin) {
                case SeekOrigin.Begin:
                    this.m_Position = (int) offset;

                    break;
                case SeekOrigin.Current:
                    this.m_Position += (int) offset;

                    break;
                case SeekOrigin.End:
                    this.m_Position = this._segment.Offset + this._segment.Length - (int) offset;

                    break;
            }

            if (this.m_Position > this._segment.Length) {
                this.m_Position = this._segment.Length;
            }

            return this.m_Position;
        }

        public override void SetLength(long value) {
            this._length = (int) value;
            if (this.m_Position > this._length) {
                this.m_Position = this._length + this._segment.Offset;
            }

            if (this._length > this._segment.Length) {
                this.EnsureCapacity(this._length);
            }
        }

        public override int Read(byte[] buffer, int offset, int count) {
            count = Math.Min(count, this._segment.Offset + this._segment.Length - this.m_Position);
            Buffer.BlockCopy(this._segment.Buffer.Array, this.m_Position, buffer, offset, count);
            this.m_Position += count;

            return count;
        }

        public override void Write(byte[] buffer, int offset, int count) {
            if (this.m_Position + count >= this._segment.Offset + this._segment.Length) {
                this.EnsureCapacity(this.m_Position - this._segment.Offset + count);
            }

            Buffer.BlockCopy(buffer, offset, this._segment.Buffer.Array, this.m_Position, count);
            this.m_Position += count;
            this._length = Math.Max(this._length, this.m_Position - this._segment.Offset);
#if DEBUG
            //CheckOpcode(count);
#endif
        }

        public override int ReadByte() {
            return this._segment.Buffer.Array[this.m_Position++];
        }

        public override void WriteByte(byte value) {
            if (this.m_Position + 1 >= this._segment.Offset + this._segment.Length) {
                this.EnsureCapacity(this.m_Position - this._segment.Offset + 1);
            }

            this._segment.Buffer.Array[this.m_Position++] = value;
            this._length = Math.Max(this._length, this.m_Position - this._segment.Offset);
#if DEBUG
            //CheckOpcode(1);
#endif
        }

        void CheckOpcode(int count) {
            //var opcode = (RealmServerOpCode)(_segment.Buffer.Array[_segment.Offset + 2] | (_segment.Buffer.Array[_segment.Offset + 3] << 8));
            //if (!Enum.IsDefined(typeof(RealmServerOpCode), opcode))
            //{
            //    string extra;
            //    if (count > 0)
            //    {
            //        extra = string.Format(" when writing bytes {0} to {1}", Position - count, count);
            //    }  
            //    else
            //    {
            //        extra = " when writing no bytes at all.";
            //    }

            //    lock (Console.Out)
            //    {
            //        Console.WriteLine("Invalid opcode {0} in stream" + extra, opcode);
            //    }
            //}
        }

        private void EnsureCapacity(int size) {
            // return the old segment and get a new, bigger one
            var newSegment = BufferManager.GetSegment(size);
            this._segment.CopyTo(newSegment, this._length);
            this.m_Position = this.m_Position - this._segment.Offset + newSegment.Offset;

            this._segment.DecrementUsage();
            this._segment = newSegment;
        }

        public override bool CanRead {
            get { return true; }
        }

        public override bool CanSeek {
            get { return true; }
        }

        public override bool CanWrite {
            get { return true; }
        }

        public override long Length {
            get { return this._length; }
        }

        public override long Position {
            get { return this.m_Position - this._segment.Offset; }
            set { this.m_Position = (int) value + this._segment.Offset; }
        }

        protected override void Dispose(bool disposing) {
            base.Dispose(disposing);
            this._segment.DecrementUsage();
        }
    }
}