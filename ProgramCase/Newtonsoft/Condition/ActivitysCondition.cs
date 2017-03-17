using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sodao.Juketool.Share.Customer
{
    public class ActivitysCondition : CustomerGroupCondition, ICollection<ActivitySelect>
    {
        public List<ActivitySelect> _selects;

        public ActivitysCondition()
        {
            _selects = new List<ActivitySelect>();
        }

        public int Count
        {
            get { return _selects.Count; }
        }

        public bool IsReadOnly
        {
            get { return ((ICollection<TagFilter>)_selects).IsReadOnly; }
        }

        public void Add(ActivitySelect item)
        {
            _selects.Add(item);
        }

        public void Clear()
        {
            _selects.Clear();
        }

        public bool Contains(ActivitySelect item)
        {
            return _selects.Contains(item);
        }

        public void CopyTo(ActivitySelect[] array, int arrayIndex)
        {
            _selects.CopyTo(array, arrayIndex);
        }

        public IEnumerator<ActivitySelect> GetEnumerator()
        {
            return _selects.GetEnumerator();
        }

        public bool Remove(ActivitySelect item)
        {
            return _selects.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _selects.GetEnumerator();
        }
    }

    public class ActivitySelect
    {
        public long Id { get; set; }

        public string Name { get; set; }
    }
}
