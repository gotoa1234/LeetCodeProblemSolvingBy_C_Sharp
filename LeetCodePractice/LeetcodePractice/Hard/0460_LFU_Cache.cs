namespace LeetcodePractice.Hard
{
    public class _0460_LFU_Cache
    {
        public _0460_LFU_Cache()
        {
            var lFUCache = new LFUCache(2);

            lFUCache.Put(1, 1);     // cache=[1,_], cnt(1)=1
            lFUCache.Put(2, 2);     // cache=[2,1], cnt(2)=1, cnt(1)=1
            var t = lFUCache.Get(1);// return 1
                                    // cache=[1,2], cnt(2)=1, cnt(1)=2
            lFUCache.Put(3, 3);     // 2 is the LFU key because cnt(2)=1 is the smallest, invalidate 2.
                                    // cache=[3,1], cnt(3)=1, cnt(1)=2
            t = lFUCache.Get(2);    // return -1 (not found)
            t = lFUCache.Get(3);    // returns 3 
                                    // cache=[3,1], cnt(3)=2, cnt(1)=2
            lFUCache.Put(4, 4);     // Both 1 and 3 have the same cnt, but 1 is LRU, invalidate 1.
                                    // cache=[4,3], cnt(4)=1, cnt(3)=2
            t = lFUCache.Get(1);    // return -1 (not found)
                                    // cache=[3,4], cnt(4)=1, cnt(3)=3
            t = lFUCache.Get(3);    // return 3
            t = lFUCache.Get(4);    // return 4
                                    //Output
                                    //[null, null, null, 1, null, -1, 3, null, -1, 3, 4]
        }

        public class LFUCache
        {
            private readonly int _capacity;// 設定快取空間上限
            private int _minFreq;// 當前最小頻率值，為了快速查找
            private readonly Dictionary<int, Node> _cache;// 主表：查找 key 對應的節點資訊
            private readonly Dictionary<int, LinkedList<int>> _freqCache;// 頻率鏈表：每個頻率對應一個 LinkedList，存儲該頻率的所有 key
            private readonly Dictionary<int, LinkedListNode<int>> _AllNodeCache;// 所有節點表：快速定位 key 在其頻率鏈表中的節點位置 ※ LFU 能 Get/Put O(1)的關鍵

            public LFUCache(int capacity)
            {
                _capacity = capacity;
                _minFreq = 0;
                _cache = new Dictionary<int, Node>();
                _freqCache = new Dictionary<int, LinkedList<int>>();
                _AllNodeCache = new Dictionary<int, LinkedListNode<int>>();
            }

            /// <summary>
            /// 1. 查詢鍵 並獲得 值
            /// </summary>
            public int Get(int key)
            {
                // 1-1. 無對應資料，依照需求返回 -1
                if (!_cache.ContainsKey(key))
                    return -1;
                
                var node = _cache[key];
                UpdateFrequency(key, node);
                return node.Value;
            }

            /// <summary>
            /// 2. 設定鍵、值
            /// </summary>            
            public void Put(int key, int value)
            {
                // 2-1. 無空間直接釋放
                if (_capacity == 0) 
                    return;

                // 2-2. 設定值時，若已存在 Key 的情況，增加頻率值
                if (_cache.ContainsKey(key))
                {
                    // 2-3-1. 一定要刷新 Key , Value 因為相同的Key有可能不同的 Value
                    var node = _cache[key];
                    node.Value = value;

                    // 2-3-2. 增加此 Key 的頻率
                    UpdateFrequency(key, node);
                }
                else
                {
                    // 2-4-1. 如果當前的 Key 已經達到上限，那要進行 LFU 頻率最差的移除
                    if (_cache.Count >= _capacity)
                    {
                        RemoveLFU();
                    }

                    // 2-4-3. 為新的 Key 添加
                    var newNode = new Node
                    {
                        Key = key,
                        Value = value,
                        Frequency = 1
                    };
                    _cache[key] = newNode;

                    // 2-4-4. 檢查 1 的頻率表(最小頻率)是否有值
                    if (!_freqCache.ContainsKey(1))// 2-4-5. 不存在要為 1 的頻率建 LinkedList
                        _freqCache[1] = new LinkedList<int>();

                    // 2-4-6. 新加入此頻率必定要設在此 LinkedList 的最優先，並且刷新頻率
                    var listNode = _freqCache[1].AddFirst(key);
                    _AllNodeCache[key] = listNode;
                    _minFreq = 1;
                }

                // 2-4-2. 從最小頻率的鏈表頭部移除（最久未使用）
                void RemoveLFU()
                {                    
                    var keysWithMinFreq = _freqCache[_minFreq];
                    var keyToRemove = keysWithMinFreq.Last.Value;

                    // 2-4-2-1. 刪除 Cache
                    _cache.Remove(keyToRemove);

                    // 2-4-2.2. 刪除 LinkList 
                    keysWithMinFreq.RemoveLast();
                    if (keysWithMinFreq.Count == 0)
                        _freqCache.Remove(_minFreq);

                    // 2-4-2.2. 刪除 LinkListNode 
                    _AllNodeCache.Remove(keyToRemove);
                }
            }

            /// <summary>
            /// 3. 更新頻率
            /// </summary>            
            private void UpdateFrequency(int key, Node node)
            {
                // 3-1. 新舊頻率
                int oldFreq = node.Frequency;
                int newFreq = oldFreq + 1;

                // 3-2. 從舊頻率的鏈表中移除
                var listNode = _AllNodeCache[key];
                _freqCache[oldFreq].Remove(listNode);

                // 3-3. 如果舊頻率的鏈表空了，並且是最小頻率，那麼必須直接刷新最小頻率
                if (_freqCache[oldFreq].Count == 0)
                {
                    _freqCache.Remove(oldFreq);
                    if (_minFreq == oldFreq)
                        _minFreq = newFreq;
                }

                // 3-4. 加入新頻率到鏈表中
                node.Frequency = newFreq;

                // 3-5. 首次加入，要給此 [頻率] 初始化
                if (!_freqCache.ContainsKey(newFreq))
                    _freqCache[newFreq] = new LinkedList<int>();

                // 3-6. LRU 原則，最近使用的在相同頻率下優先級最高 (放在最前面)
                var newListNode = _freqCache[newFreq].AddFirst(key);
                _AllNodeCache[key] = newListNode;
            }

            /// <summary>
            /// 資料結構
            /// </summary>
            private class Node
            {                
                public int Key { get; set; }
                public int Value { get; set; }
                public int Frequency { get; set; }
            }
        }
    }
}
