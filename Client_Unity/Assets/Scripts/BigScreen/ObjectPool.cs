public class ObjectPool<T> where T : new()
{
    private T[] packDataPool;
    private int curPos = 0;

    public ObjectPool(int pool_size, int data_size)
    {
        packDataPool = new T[pool_size];
        for (int i = 0; i < pool_size; i++)
        {
            T data = new T();
            if (data is IObjectPoolItem)
            {
                IObjectPoolItem item = data as IObjectPoolItem;
                item.init(data_size);
            }
            packDataPool[i] = data;
        }
    }

    public T Create()
    {
        T data = packDataPool[curPos];
        curPos++;
        if (curPos == packDataPool.Length)
        {
            curPos = 0;
        }
        return data;
    }
}