using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sodao.Juketool.Share.Customer
{
    public class TagFilterCondition : CustomerGroupCondition, ICollection<TagFilter>
    {
        private List<TagFilter> _selects;

        public TagFilterCondition()
        {
            _selects = new List<TagFilter>();
        }

        public int Count
        {
            get { return _selects.Count; }
        }

        public bool IsReadOnly
        {
            get { return ((ICollection<TagFilter>)_selects).IsReadOnly; }
        }

        public void Add(TagFilter item)
        {
            _selects.Add(item);
        }

        public void Clear()
        {
            _selects.Clear();
        }

        public bool Contains(TagFilter item)
        {
            return _selects.Contains(item);
        }

        public void CopyTo(TagFilter[] array, int arrayIndex)
        {
            _selects.CopyTo(array, arrayIndex);
        }

        public IEnumerator<TagFilter> GetEnumerator()
        {
            return _selects.GetEnumerator();
        }

        public bool Remove(TagFilter item)
        {
            return _selects.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _selects.GetEnumerator();
        }
    }

    public class TagFilter
    {
        public long Id { get; set; }

        public string Name { get; set; }
    }
}