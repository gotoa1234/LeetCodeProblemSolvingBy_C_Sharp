using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LeetcodePractice.Mediun._0146_LRU_Cache;

namespace LeetcodePractice.Hard
{
    public class _0460_LFU_Cache
    {
        public _0460_LFU_Cache()
        {
            var lRUCache = new LRUCache(2);

            lRUCache.Put(1, 1);     // cache=[1,_], cnt(1)=1
            lRUCache.Put(2, 2);     // cache=[2,1], cnt(2)=1, cnt(1)=1
            var t = lRUCache.Get(1);// return 1
                                    // cache=[1,2], cnt(2)=1, cnt(1)=2
            lRUCache.Put(3, 3);     // 2 is the LFU key because cnt(2)=1 is the smallest, invalidate 2.
                                    // cache=[3,1], cnt(3)=1, cnt(1)=2
            t = lRUCache.Get(2);    // return -1 (not found)
            t = lRUCache.Get(3);    // returns 3 
                                    // cache=[3,1], cnt(3)=2, cnt(1)=2
            lRUCache.Put(4, 4);     // Both 1 and 3 have the same cnt, but 1 is LRU, invalidate 1.
                                    // cache=[4,3], cnt(4)=1, cnt(3)=2
            t = lRUCache.Get(1);    // return -1 (not found)
                                    // cache=[3,4], cnt(4)=1, cnt(3)=3
            t = lRUCache.Get(3);    // return 3
            t = lRUCache.Get(4);    // return 4
                                    //Output
                                    //[null, null, null, 1, null, -1, 3, null, -1, 3, 4]
        }

        public class LFUCache
        {
            private readonly int _capacity;
            private readonly Dictionary<int, LinkedListNode<Node>> _LFUCache;
            private readonly LinkedList<Node> _Links;

            public LFUCache(int capacity)
            {
                _capacity = capacity;
                _LFUCache = new Dictionary<int, LinkedListNode<Node>>();
                _Links = new LinkedList<Node>();
            }

            public int Get(int key)
            {
                if(!_LFUCache.ContainsKey(key))
                    return -1;
                var temp = _LFUCache[key];
                return 0;
            }

            public void Put(int key, int value)
            {

            }
        }

        public class Node
        {
            public int Key;
            public int value;
            public int Frequency;
        }
    }
}
