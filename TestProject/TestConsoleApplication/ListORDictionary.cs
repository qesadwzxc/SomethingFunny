using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestConsoleApplication
{
    /// <summary>
    /// 集合类效率测试
    /// </summary>
    public class ListORDictionary
    {
        static List<TestModel> todayList = InitTodayData();
        static List<TestModel> historyList = InitHisoryData();
        static Dictionary<int, List<string>> bDic = new Dictionary<int, List<string>>();

        public static void Run()
        {
            CodeTimer.Time("ListAnyOnlyStartTest", 1, ListAnyOnlyStartTest);
            CodeTimer.Time("ListAnyTest", 1, ListAnyTest);
            CodeTimer.Time("ListExceptTest", 1, ListExceptTest);
            CodeTimer.Time("DictionaryTest", 1, DictionaryTest);
            CodeTimer.Time("ListIntersectTest", 1, ListIntersectTest);
        }

        public static void ListTest()
        {
            List<TestModel> resultList = todayList.FindAll(re =>
            {
                if (historyList.Exists(m => m.UserID == re.UserID && m.BookID == re.BookID))
                {
                    return false;
                }
                return true;
            });
        }

        public static void ListAnyOnlyStartTest()
        {
            var query = from t in todayList
                        join h in historyList on new { t.BookID, t.UserID } equals new { h.BookID, h.UserID }
                        select t;
        }

        public static void ListAnyTest()
        {
            var query = from t in todayList
                        join h in historyList on new { t.BookID, t.UserID } equals new { h.BookID, h.UserID }
                        select t;

            var ret = todayList.Except(query).ToList();
        }

        public static void ListExceptTest()
        {
            var ret = todayList.Except(historyList).ToList();
        }

        public static void ListIntersectTest()
        {
            var ret = todayList.Intersect(historyList).ToList();
        }

        public static void DictionaryTest()
        {
            foreach (TestModel obj in historyList)
            {
                if (!bDic.ContainsKey(obj.UserID))
                {
                    bDic.Add(obj.UserID, new List<string>());
                }
                bDic[obj.UserID].Add(obj.BookID);
            }

            List<TestModel> resultList = todayList.FindAll(re =>
            {
                if (bDic.ContainsKey(re.UserID) && bDic[re.UserID].Contains(re.BookID))
                {
                    return false;
                }
                return true;
            });
        }

        /// <summary>
        /// 初始化数据（今日）
        /// </summary>
        /// <returns></returns>
        public static List<TestModel> InitTodayData()
        {
            List<TestModel> list = new List<TestModel>();
            for (int i = 0; i < 10000; i++)
            {
                list.Add(new TestModel() { UserID = i, BookID = i.ToString() });
            }
            return list;
        }

        /// <summary>
        /// 初始化数据（历史）
        /// </summary>
        /// <returns></returns>
        public static List<TestModel> InitHisoryData()
        {
            List<TestModel> list = new List<TestModel>();
            Random r = new Random();
            int loopTimes = 60000;
            for (int i = 0; i < loopTimes; i++)
            {
                list.Add(new TestModel() { UserID = r.Next(0, loopTimes), BookID = i.ToString() });
            }
            return list;
        }

        /// <summary>
        /// 测试实体
        /// </summary>
        public class TestModel
        {
            /// <summary>
            /// 用户ID
            /// </summary>
            public int UserID { get; set; }

            /// <summary>
            /// 书ID
            /// </summary>
            public string BookID { get; set; }
        }
    }
}
