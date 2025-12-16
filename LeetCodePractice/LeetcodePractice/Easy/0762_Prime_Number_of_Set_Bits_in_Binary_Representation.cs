using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeetcodePractice.Easy
{
    public class _0762_Prime_Number_of_Set_Bits_in_Binary_Representation
    {
        public class Solution
        {
            public Solution()
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
                return ZAlgorithmSearch(s + s, goal);                
            }

            private bool ZAlgorithmSearch(string text, string pattern)
            {
                string s = pattern + "#" + text;
                int n = s.Length;
                int m = pattern.Length;

                int[] z = new int[n];
                int l = 0, r = 0;

                for (int i = 1; i < n; i++)
                {
                    if (i > r)
                    {
                        l = r = i;
                        while (r < n && s[r - l] == s[r])
                        {
                            r++;
                        }
                        z[i] = r - l;
                        r--;
                    }
                    else
                    {
                        int k = i - l;
                        if (z[k] < r - i + 1)
                        {
                            z[i] = z[k];
                        }
                        else
                        {
                            l = i;
                            while (r < n && s[r - l] == s[r])
                            {
                                r++;
                            }
                            z[i] = r - l;
                            r--;
                        }
                    }
                }

                for (int i = m + 1; i < n; i++)
                {
                    if (z[i] == m)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

    }
}
