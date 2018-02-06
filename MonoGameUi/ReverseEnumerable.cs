using System;
using System.Collections;
using System.Collections.Generic;

namespace MonoGameUi
{
    public class ReverseEnumerable<T> : IEnumerable<T>
    {
        
        public List<T> BaseList { private get; set; }
        [Serializable]
        public struct ReverseEnumerator : IEnumerator<T>, IEnumerator
        {
            private readonly List<T> _list;
            private int _index;
            private T _current;

            internal ReverseEnumerator(List<T> list)
            {
                _list = list;
                _index = list.Count-1;
                _current = default(T);
            }

            public void Dispose()
            {
            }

            public bool MoveNext()
            {
                var localList = _list;

                if (_index >= 0)
                {
                    _current = localList[_index];
                    _index--;
                    return true;
                }

                return MoveNextRare();
            }

            private bool MoveNextRare()
            {
                _index = -1;
                _current = default(T);
                return false;
            }

            public T Current => _current;

            object IEnumerator.Current
            {
                get
                {
                    if (_index == _list.Count || _index == -1)
                    {
                        throw new InvalidOperationException("enumeration operation not possible");
                    }

                    return Current;
                }
            }

            void IEnumerator.Reset()
            {
                _index = 0;
                _current = default(T);
            }
        }

        public ReverseEnumerator GetEnumerator()
        {
            return new ReverseEnumerator(BaseList);
        }
        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}