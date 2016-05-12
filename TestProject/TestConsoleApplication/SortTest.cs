using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;

namespace TestConsoleApplication
{
    public class SortTest
    {
        #region 冒泡排序
        public static List<int> BubbleSort(List<int> list)
        {
            int temp = 0;
            for (int i = 0; i < list.Count - 1; i++)
            {
                for (int j = i; j > 0; j--)
                {
                    if (list[j] < list[j - 1])
                    {
                        temp = list[j - 1];
                        list[j - 1] = list[j];
                        list[j] = temp;
                    }
                }
            }
            return list;
        }

        //第二个方法的性能优于第一个，为什么？
        public static List<int> BubbleSort2(List<int> list)
        {
            int temp = 0;
            for (int i = 0; i < list.Count - 1; i++)
            {
                for (int j = list.Count - 1; j > i; j--)
                {
                    if (list[j - 1] > list[j])
                    {
                        temp = list[j - 1];
                        list[j - 1] = list[j];
                        list[j] = temp;
                    }
                }
            }
            return list;
        }
        #endregion

        #region 快速排序
        ///<summary>
        /// 分割函数
        ///</summary>
        ///<param name="list">待排序的数组</param>
        ///<param name="left">数组的左下标</param>
        ///<param name="right"></param>
        ///<returns></returns>
        public static int Division(List<int> list, int left, int right)
        {
            //首先挑选一个基准元素
            int baseNum = list[left];

            while (left < right)
            {
                //从数组的右端开始向前找，一直找到比base小的数字为止(包括base同等数)
                while (left < right && list[right] >= baseNum)
                { right = right - 1; }

                //最终找到了比baseNum小的元素，要做的事情就是此元素放到base的位置
                list[left] = list[right];

                //从数组的左端开始向后找，一直找到比base大的数字为止（包括base同等数）
                while (left < right && list[left] <= baseNum)
                { left = left + 1; }

                //最终找到了比baseNum大的元素，要做的事情就是将此元素放到最后的位置
                list[right] = list[left];
            }
            //最后就是把baseNum放到该left的位置
            list[left] = baseNum;

            //最终，我们发现left位置的左侧数值部分比left小，left位置右侧数值比left大
            //至此，我们完成了第一篇排序
            return left;
        }

        public static void QuickSort(List<int> list, int left, int right)
        {
            //左下标一定小于右下标，否则就超越了
            if (left < right)
            {
                //对数组进行分割，取出下次分割的基准标号
                int i = Division(list, left, right);

                //对“基准标号“左侧的一组数值进行递归的切割，以至于将这些数值完整的排序
                QuickSort(list, left, i - 1);

                //对“基准标号“右侧的一组数值进行递归的切割，以至于将这些数值完整的排序
                QuickSort(list, i + 1, right);
            }
        }

        public static void Sort(List<int> list,int low,int high)
        {
            if (low >= high)
            {
                int i = SortUnit(list, low, high);
                Sort(list, low, i - 1);
                Sort(list, i + 1, high);
            }
        }

        public static int SortUnit(List<int> list, int low, int high)
        {
            int key = list[low];
            while (low < high)
            {
                while (low < high && list[high] >= key)
                {
                    high -= 1;
                }
                list[low] = list[high];

                while (low < high && list[low] <= key)
                {
                    low += 1;
                }
                list[high] = list[low];
            }
            list[low] = key;
            return 0;
        }
        #endregion

        #region 直接选择排序
        #endregion

        #region 堆排序
        #endregion

        #region 直接插入排序
        #endregion

        #region 希尔排序
        #endregion

        #region 合并排序
        #endregion
    }
}
