using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sodao.Juketool.Share.Customer
{
    public class ProductsCondition : CustomerGroupCondition, ICollection<ProductSelect>
    {
        private List<ProductSelect> _selects;

        public ProductsCondition()
        {
            _selects = new List<ProductSelect>();
        }

        public int Count
        {
            get
            {
                return _selects.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return ((IList<ProductSelect>)_selects).IsReadOnly;
            }
        }

        public void Add(ProductSelect item)
        {
            _selects.Add(item);
        }

        public void Clear()
        {
            _selects.Clear();
        }

        public bool Contains(ProductSelect item)
        {
            return _selects.Contains(item);
        }

        public void CopyTo(ProductSelect[] array, int arrayIndex)
        {
            _selects.CopyTo(array,arrayIndex);
        }

        public IEnumerator<ProductSelect> GetEnumerator()
        {
            return _selects.GetEnumerator();
        }

        public bool Remove(ProductSelect item)
        {
            return _selects.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _selects.GetEnumerator();
        }
    }

    public class ProductSelect
    {
        public long Id { get; set; }

        public string Title { get; set; }
    }
}
