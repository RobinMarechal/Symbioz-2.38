/*************************************************************************
 *
 *   file		: SynchronizedDictionary.cs
 *   copyright		: (C) The WCell Team
 *   email		: info@wcell.org
 *   last changed	: $LastChangedDate: 2008-08-17 02:08:03 +0200 (sø, 17 aug 2008) $
 
 *   revision		: $Rev: 598 $
 *
 *   This program is free software; you can redistribute it and/or modify
 *   it under the terms of the GNU General Public License as published by
 *   the Free Software Foundation; either version 2 of the License, or
 *   (at your option) any later version.
 *
 *************************************************************************/

using System;
using System.Collections.Generic;
using System.Threading;

namespace Symbioz.Core.Pool {
    public class SynchronizedDictionary<TKey, TValue> : Dictionary<TKey, TValue> {
        private readonly object _syncLock = new object();

        public SynchronizedDictionary() { }
        public SynchronizedDictionary(IEqualityComparer<TKey> comparer) : base(comparer) { }
        public SynchronizedDictionary(int capacity) : base(capacity) { }
        public SynchronizedDictionary(IDictionary<TKey, TValue> dictionary) : base(dictionary) { }

        public object SyncLock {
            get { return this._syncLock; }
        }

        public virtual new TValue this[TKey key] {
            get {
                Monitor.Enter(this._syncLock);

                try {
                    if (!base.ContainsKey(key)) {
                        throw new KeyNotFoundException();
                    }

                    return base[key];
                }
                finally {
                    Monitor.Exit(this._syncLock);
                }
            }
            set {
                Monitor.Enter(this._syncLock);

                try {
                    if (base.ContainsKey(key)) {
                        base[key] = value;
                    }
                    else {
                        base.Add(key, value);
                    }
                }
                finally {
                    Monitor.Exit(this._syncLock);
                }
            }
        }

        public virtual new void Add(TKey key, TValue value) {
            Monitor.Enter(this._syncLock);

            try {
                base.Add(key, value);
            }
            finally {
                Monitor.Exit(this._syncLock);
            }
        }

        public virtual new void Clear() {
            Monitor.Enter(this._syncLock);

            try {
                base.Clear();
            }
            finally {
                Monitor.Exit(this._syncLock);
            }
        }

        public new bool ContainsKey(TKey key) {
            Monitor.Enter(this._syncLock);

            try {
                return base.ContainsKey(key);
            }
            finally {
                Monitor.Exit(this._syncLock);
            }
        }

        public virtual new bool Remove(TKey key) {
            Monitor.Enter(this._syncLock);

            try {
                if (!base.ContainsKey(key)) {
                    return false;
                }

                base.Remove(key);
            }
            finally {
                Monitor.Exit(this._syncLock);
            }

            return true;
        }

        public void Lock() {
            Monitor.Enter(this._syncLock);
        }

        public void Unlock() {
            Monitor.Exit(this._syncLock);
        }
    }
}