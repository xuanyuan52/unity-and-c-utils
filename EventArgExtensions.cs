public static class EventArgExtensions
{
    public static void Raise<T>(this T e, Object sender, ref EventHandler<T> eventDelegate) where T : EventArgs
    {
        //出于线程安全的考虑，将委托字段的引用赋值到一个临时字段中
        EventHandler<T> temp = Interlocked.CompareExchange(ref eventDelegate, null, null);

        if (temp != null) temp(sender, e);
    }
}
