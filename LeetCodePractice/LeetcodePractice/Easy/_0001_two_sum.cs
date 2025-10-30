using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeetcodePractice.Easy
{
    public class _0001_two_sum
    {
        public _0001_two_sum()
        {
            Console.WriteLine("========" + this.GetType().Name + " Start ========");

            var result = new Solution().TwoSum(GetData1().nums, GetData1().target);
            Console.WriteLine(string.Join(",", result));

            var result2 = new Solution().TwoSum(GetData2().nums, GetData2().target);
            Console.WriteLine(string.Join(",", result2));

            var result3 = new Solution().TwoSum(GetData3().nums, GetData3().target);
            Console.WriteLine(string.Join(",", result3));

            Console.WriteLine("========" + this.GetType().Name + " End ========");


            // Closer Data
            (int[] nums, int target) GetData1()
            {
                return (new int[] { 2, 7, 11, 15 }, 9);
            }
      
            (int[] nums, int target) GetData2()
            {
                return (new int[] { 3, 2, 4 }, 6);
            }

            (int[] nums, int target) GetData3()
            {
                return (new int[] { 2, 3 }, 6);
            }
        }


        public class Solution
        {
            public int[] TwoSum(int[] nums, int target)
            {
                var dic = new Dictionary<int, int>();

                for (int index = 0; index < nums.Count(); index++)
                {
                    if (dic.ContainsKey(target - nums[index]))
                    {
                        return new int[]
                        { dic[target - nums[index]] ,index};
                    }
                    dic[nums[index]] = index;
                }
                return new int[] { };
            }
        }

    }
}
