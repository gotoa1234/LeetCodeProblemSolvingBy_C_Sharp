namespace LeetcodePractice.Mediun
{
    /// <summary>
    ///         思路 ：查詢、新增、更新時權重放到最前面，必須利用 Dictionary + LinkedList + LinkNode 
    ///               來實現新增、更新的 O(1) 效能
    ///               要考慮空間上限與上限時刪除最後一筆，即可實現 LRU 
    ///      Runtime :   
    /// Memory Usage : 
    /// </summary>
    /// <returns></returns>  
    public class _0146_LRU_Cache
    {
        public _0146_LRU_Cache()
        {
            var lRUCache = new LRUCache(2);

            lRUCache.Put(1, 1); // cache is {1=1}
            lRUCache.Put(2, 2); // cache is {1=1, 2=2}
            var t = lRUCache.Get(1);    // return 1
            lRUCache.Put(3, 3); // LRU key was 2, evicts key 2, cache is {1=1, 3=3}
            t = lRUCache.Get(2);    // returns -1 (not found)
            lRUCache.Put(4, 4); // LRU key was 1, evicts key 1, cache is {4=4, 3=3}
            t = lRUCache.Get(1);    // return -1 (not found)
            t = lRUCache.Get(3);    // return 3
            t = lRUCache.Get(4);    // return 4
                                    //Output
                                    //[null, null, null, 1, null, -1, null, -1, 3, 4]
        }
        

        public class LRUCache
        {
            private readonly int _capacity;
            private readonly Dictionary<int, LinkedListNode<(int key, int value)>> _cache;
            private readonly LinkedList<(int key, int value)> _links;

            /// <summary>
            /// 1. 建構式 - 快取策略容量上限
            /// </summary>
            public LRUCache(int capacity)
            {
                _capacity = capacity;//1-2. 記錄 LRU 快取上限
                _links = new LinkedList<(int key, int value)>();
                _cache = new Dictionary<int, LinkedListNode<(int key, int value)>>();
            }

            /// <summary>
            /// 2. 取值的處理
            /// </summary>
            public int Get(int key)
            {
                if (!_cache.ContainsKey(key))
                {
                    //2-1. 不存在返回 -1
                    return -1;
                }
                var node = _cache[key];//2-2. 存在時，將權重放到最前面
                _links.Remove(node);// 2-3. 移除舊的位置
                _links.AddFirst(node);// 2-4. 放在最前面
                return _cache[key].Value.value;// 2-5. 返回結果
            }

            /// <summary>
            /// 3. 存值的處理
            /// </summary>
            public void Put(int key, int value)
            {
                // 3-1. 是否存在
                if (_cache.TryGetValue(key, out LinkedListNode<(int key, int value)>? node))
                {
                    // 3-2. 存在時，將權重放到最前面，同個 Key 可能 Value 已經不同
                    node.Value = (key, value);
                    _links.Remove(node);// 3-2. 權重往前
                    _links.AddFirst(node);                    
                }
                else
                {
                    //4. 不存在 Key 且 快取額度達到上限
                    if (_capacity == _cache.Count())
                    {
                        //4-1. 移除最後一個資料
                        var lastNode = _links.Last;                                                
                        _cache.Remove(lastNode.Value.key);
                        _links.RemoveLast();                        
                    }
                    // 4-2. 新增資料，設定權重最上面
                    var newNode = _links.AddFirst((key, value));
                    _cache[key] = newNode;
                }
    
            }
        }
    }
}
