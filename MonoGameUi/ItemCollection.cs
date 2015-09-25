using System;
using System.Collections;
using System.Collections.Generic;

namespace MonoGameUi
{
    /// <summary>
    /// Erweiterte Liste für Controls
    /// </summary>
    public class ItemCollection<T> : IList<T> where T : class
    {
        private List<T> items = new List<T>();

        public ItemCollection() { }

        public T this[int index]
        {
            get
            {
                lock (items)
                {
                    return items[index];
                }
            }

            set
            {
                throw new NotSupportedException();
            }
        }

        public int Count
        {
            get
            {
                lock (items)
                {
                    return items.Count;
                }
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public virtual void Add(T item)
        {
            lock (items)
            {
                if (item == null)
                    throw new ArgumentNullException("Item cant be null");

                if (items.Contains(item))
                    throw new ArgumentException("Item is already part of this collection");

                // Control einfügen
                items.Add(item);

                // Event werfen
                if (OnInsert != null)
                    OnInsert(item, items.IndexOf(item));
            }
        }

        public virtual void Clear()
        {
            lock (items)
            {
                var temp = items.ToArray();

                items.Clear();

                for (int i = 0; i < temp.Length; i++)
                    if (OnRemove != null)
                        OnRemove(temp[i], i);
            }
        }

        public bool Contains(T item)
        {
            lock (items)
            {
                return items.Contains(item);
            }
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            lock (items)
            {
                items.CopyTo(array, arrayIndex);
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            lock (items)
            {
                return items.GetEnumerator();
            }
        }

        public int IndexOf(T item)
        {
            lock (items)
            {
                return items.IndexOf(item);
            }
        }

        public virtual void Insert(int index, T item)
        {
            lock (items)
            {
                if (item == null)
                    throw new ArgumentNullException("Item cant be null");

                if (items.Contains(item))
                    throw new ArgumentException("Item is already part of this collection");

                // Control einfügen
                items.Insert(index, item);

                // Event werfen
                if (OnInsert != null)
                    OnInsert(item, items.IndexOf(item));
            }
        }

        public virtual bool Remove(T item)
        {
            lock (items)
            {
                if (item == null)
                    throw new ArgumentNullException("Item cant be null");

                if (!items.Contains(item))
                    return false;

                // Control entfernen
                int index = items.IndexOf(item);
                items.Remove(item);

                // Event
                if (OnRemove != null)
                    OnRemove(item, index);

                return true;
            }
        }

        public virtual void RemoveAt(int index)
        {
            lock (items)
            {
                if (index < 0 && index >= items.Count)
                    throw new ArgumentOutOfRangeException("index");

                // Control entfernen
                T c = items[index];
                items.RemoveAt(index);

                // Event werfen
                if (OnRemove != null)
                    OnRemove(c, index);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            lock (items)
            {
                return items.GetEnumerator();
            }
        }

        public event ItemCollectionDelegate<T> OnInsert;

        public event ItemCollectionDelegate<T> OnRemove;
    }

    public delegate void ItemCollectionDelegate<T>(T item, int index);
}
