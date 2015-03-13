using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadManSwitch.Data.TestRepository.Tables
{
    internal abstract class Table<TRow> : IList<TRow>
    {
        private readonly List<TRow> Rows;

        protected Table()
        {
            Rows = new List<TRow>();
        }

        protected Table(IEnumerable<TRow> initialRows)
            : this()
        {
            AddRange(initialRows);
        }

        public IEnumerator<TRow> GetEnumerator()
        {
            return ((IEnumerable<TRow>) Rows).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public virtual void Add(TRow item)
        {
            Rows.Add(item);
        }

        /// <summary>
        /// Call this method instead of Rows.AddRange to give subclasses
        /// a chance to run overridden Add method
        /// </summary>
        /// <param name="items"></param>
        protected void AddRange(IEnumerable<TRow> items)
        {
            foreach (var item in items)
            {
                Add(item);
            }
        }

        public void Clear()
        {
            Rows.Clear();
        }

        public bool Contains(TRow item)
        {
            return Rows.Contains(item);
        }

        public void CopyTo(TRow[] array, int arrayIndex)
        {
            Rows.CopyTo(array, arrayIndex);
        }

        public bool Remove(TRow item)
        {
            return Rows.Remove(item);
        }

        public int Count
        {
            get { return Rows.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public int IndexOf(TRow item)
        {
            return Rows.IndexOf(item);
        }

        public void Insert(int index, TRow item)
        {
            Rows.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            Rows.RemoveAt(index);
        }

        public TRow this[int index]
        {
            get { return Rows[index]; }
            set { Rows[index] = value; }
        }
    }
}
