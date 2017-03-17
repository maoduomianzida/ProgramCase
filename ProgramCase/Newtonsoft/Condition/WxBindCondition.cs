using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sodao.Juketool.Share.Customer
{
    public enum CustomerWxBind
    {
        Bind = 1,
        UnBind = 2,
    }

    public class WxBindCondition : CustomerGroupCondition, ICollection<string>
    {
        private List<string> _selects;
        private static readonly string[] Optionals = new string[]
        {
            CustomerWxBind.Bind.ToString(),
            CustomerWxBind.UnBind.ToString()  
        };

        public WxBindCondition()
        {
            _selects = new List<string>();
        }

        public int Count
        {
            get { return _selects.Count; }
        }

        public bool IsReadOnly
        {
            get
            {
                return ((IList<string>)_selects).IsReadOnly;
            }
        }

        public void Add(string item)
        {
            if (!Optionals.Contains(item, StringComparer.OrdinalIgnoreCase))
                throw new Exception($"{item}不是可选项");

            _selects.Add(item.ToLower());
        }

        public void Clear()
        {
            _selects.Clear();
        }

        public bool Contains(string item)
        {
            return _selects.Contains(item, StringComparer.OrdinalIgnoreCase);
        }

        public void CopyTo(string[] array, int arrayIndex)
        {
            _selects.CopyTo(array, arrayIndex);
        }

        public bool Remove(string item)
        {
            return _selects.Remove(item.ToLower());
        }

        public IEnumerator<string> GetEnumerator()
        {
            return _selects.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _selects.GetEnumerator();
        }
    }
}