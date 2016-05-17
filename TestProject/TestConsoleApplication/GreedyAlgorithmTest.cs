using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestConsoleApplication
{
    public class GreedyAlgorithmTest
    {
        class Unit
        {
            public double Key { get; set; }
            public double Value { get; set; }
        }

        private List<Unit> itemDic = new List<Unit>();
        public GreedyAlgorithmTest()
        {
            itemDic.Add(new Unit() { Key = 100, Value = 0 });
            itemDic.Add(new Unit() { Key = 50, Value = 0 });
            itemDic.Add(new Unit() { Key = 20, Value = 0 });
            itemDic.Add(new Unit() { Key = 10, Value = 0 });
            itemDic.Add(new Unit() { Key = 5, Value = 0 });
            itemDic.Add(new Unit() { Key = 1, Value = 0 });
            itemDic.Add(new Unit() { Key = 0.5, Value = 0 });
            itemDic.Add(new Unit() { Key = 0.1, Value = 0 });
        }

        public void Run(double pacageVolume)
        {
            var dic = ItemCount(pacageVolume);
            foreach (var item in itemDic)
            {
                if (item.Value > 0)
                {
                    Console.WriteLine($"数值：{item.Key}，数量：{item.Value}");
                }
            }
        }

        private List<Unit> ItemCount(double pacageVolume)
        {
            if (pacageVolume > 0.05)
            {
                foreach (var item in itemDic)
                {
                    if (pacageVolume >= item.Key)
                    {
                        item.Value = (int)(pacageVolume / item.Key);
                        pacageVolume -= (int)(item.Key * item.Value);
                    }
                }
            }
            return itemDic;
        }
    }
}
