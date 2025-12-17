namespace LeetcodePractice.Easy
{
    public class _0796_Prime_Number_of_Set_Bits_in_Binary_Representation
    {

        public _0796_Prime_Number_of_Set_Bits_in_Binary_Representation()
        {
            var data = GetData1();
            Console.WriteLine(this.RotateString(data.s, data.goal));

            data = GetData2();
            Console.WriteLine(this.RotateString(data.s, data.goal));

            // Closer Data
            (string s, string goal) GetData1()
            {
                return ("abcde", "cdeab");
            }

            (string s, string goal) GetData2()
            {
                return ("abcde", "abced");
            }
        }
        

        public bool RotateString(string s, string goal)
        {
            if (s.Length != goal.Length)
            {
                return false;
            }                      
            // C# - 標準解法
            return (s+s).Contains(goal);
            
            // 演算髮姐法 - z 字串演算法
            // 時間複雜度: O(M + N)
            // 空間複雜度: O(M + N)            
            // M = 變數(模式長度可變) LeetCode 的變數 goal   
            // N = 變數(文本長度可變) LeetCode 的變數 s
            //return ZAlgorithmSearch(s + s, goal);
        }

        private bool ZAlgorithmSearch(string text, string pattern)
        {
            // 1. 將模式、分隔符、文本組合成一個字串 (分隔符這邊用 # 要避免與匹配字串影響)
            var searchStr = $"{pattern}#{text}";

            var searchLength = searchStr.Length;// 組合字串的總長度
            var modelLength = pattern.Length;   // 模式字串的長度（用於後續判斷是否完整匹配）

            // 2. 初始化 Z 陣列 
            // 補充 : zBox[index]表示：從位置 index 開始的子字串與整個字串 s 的最長公共前綴長度
            // 例如 : s="ababab", zBox[2]=4 表示從位置2開始的 "abab" 與開頭的 "abab" 有 4 個字元匹配
            int[] zBox = new int[searchLength];

            // 3. 初始化 zBox 陣列
            // 目的： Z 字串演算法要將所有陣列的匹配長度找出
            int zBoxLeft = 0, zBoxRight = 0;

            // 4. 計算 zBox 陣列 從位置 1 開始（位置 0 可跳過，自己跟自己）
            for (int index = modelLength + 1; index < searchLength; index++)
            {
                // 5-1. Index 在 zBox 之外 (index > zBoxRight)
                if (index > zBoxRight)
                {
                    // 5-1-1. 當前位置在已知匹配區間之外，需要重新暴力比對
                    zBoxLeft = zBoxRight = index;  // 將新的匹配區間起點設為 i

                    // 5-1-2. 進行逐一匹配
                    MatchAndSetArray(ref zBox, ref index, ref zBoxLeft, ref zBoxRight);

                }
                // 5-2. Index 在 Z-box 之內
                else
                {
                    // 5-2-1. 當前位置在已知匹配區間內，可以利用之前計算出的 zBox 信息
                    int offsetIndex = index - zBoxLeft;

                    // 5-2-2. 可重用 : zBox 紀錄 (會進入此表示前一次有找到至少1個匹配)
                    if (zBox[offsetIndex] < zBoxRight - index + 1)
                    {
                        zBox[index] = zBox[offsetIndex];
                    }
                    else// 5-2-3. 不可重用的狀況
                    {
                        zBoxLeft = index;

                        // 5-2-4. 進入匹配
                        MatchAndSetArray(ref zBox,ref index, ref zBoxLeft, ref zBoxRight);
                    }
                }
            }

            // 6-1. 檢查是否完全匹配 - 有目標數量表示有完全匹配
            for (int index = modelLength + 1; index < searchLength; index++)
            {
                if (zBox[index] == modelLength)
                {
                    return true;
                }
            }
            return false;//6-2. 沒找到

            // 找出匹配量，並更新 zBox 陣列
            void MatchAndSetArray(ref int[] array, ref int zBoxIndex,
                ref int zBoxLeft, ref int zBoxRight)
            {
                // 1. 匹配
                while (zBoxRight < searchLength &&
                       searchStr[zBoxRight - zBoxLeft] == searchStr[zBoxRight])
                {
                    zBoxRight++;
                }

                // 2. 記錄匹配長度
                array[zBoxIndex] = zBoxRight - zBoxLeft;

                // 3. 因為 while 迴圈會讓 zBoxRight 多加 1，因此要減 1 校正
                zBoxRight--;
            }
        }

    }
}
