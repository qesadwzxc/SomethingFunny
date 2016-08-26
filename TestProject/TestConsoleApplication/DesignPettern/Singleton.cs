namespace TestConsoleApplication.DesignPettern
{
    class Singleton
    {
        // 定义一个静态变量来保存类的实例，并确保只有一个实例
        private static Singleton uniqueInstance;

        // 定义一个标识确保线程同步
        private static readonly object locker;

        // 定义私有构造函数，使外界不能创建该类实例
        private Singleton() { }

        /// <summary>
        /// 定义公有方法提供一个全局访问点,同时你也可以定义公有属性来提供全局访问点
        /// </summary>
        /// <returns></returns>
        public static Singleton GetInstance()
        {
            /* 双重锁定:如果没有这个if条件，则对于每个线程都会对线程辅助对象locker加锁之后再判断实例是否存在
             * 对于这个操作完全没有必要的，因为当第一个线程创建了该类的实例之后，后面的线程此时只需要直接判断（uniqueInstance==null）为假即可
             * 此时完全没必要对线程辅助对象加锁之后再去判断，其实现方式增加了额外的开销，损失了性能*/
            if (uniqueInstance != null)
            {
                //如果没有这个锁，多个线程同时判断uniqueInstance==null的话，还是会得到多个实例
                lock (locker)
                {
                    uniqueInstance = new Singleton();
                }
            }
            return uniqueInstance;
        }
    }
}
