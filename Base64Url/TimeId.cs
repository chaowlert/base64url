using System;

namespace Base64Url
{
    public static class TimeId
    {
        [ThreadStatic]
        private static Random _local;
        static Random Random
        {
            get { return _local ?? (_local = new Random(Guid.NewGuid().GetHashCode())); }
        }

        public static string NewSortableId(bool ascending = false)
        {
            return NewSortableId(DateTime.UtcNow, ascending);
        }
        public static string NewSortableId(DateTime dateTime, bool ascending = false)
        {
            var writer = new Base64Writer(12);
            var tick = dateTime.Ticks;
            if (!ascending)
                tick = -tick;
            writer.Write(tick);
            writer.Write(Random.Next());
            return writer.ToString();
        }

        public static string GetTimeId(DateTime datetime, bool ascending = false)
        {
            var writer = new Base64Writer(8);
            var tick = datetime.Ticks;
            if (!ascending)
                tick = -tick;
            writer.Write(tick);
            return writer.ToString();
        }

        public static DateTime ToDateTime(string timeId)
        {
            var reader = new Base64Reader(timeId, 8);
            var tick = reader.ReadInt64();
            return new DateTime(Math.Abs(tick));
        }
    }
}