using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEAsg1
{
    public interface Collectable
    {
        public Iterator CreateIterator();
        public object? Get(int index);
        public void Remove(object obj);
        public void Add(object obj);
    }
}
