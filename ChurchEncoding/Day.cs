namespace Ploeh.Samples.ChurchEncoding
{
    internal sealed class Day : IPeriod
    {
        private readonly int year;
        private readonly int month;
        private readonly int day;

        public Day(int year, int month, int day)
        {
            this.year = year;
            this.month = month;
            this.day = day;
        }

        public T Accept<T>(IPeriodVisitor<T> visitor)
        {
            return visitor.VisitDay(year, month, day);
        }

        public override bool Equals(object obj)
        {
            return obj is Day day &&
                   month == day.month;
        }

        public override int GetHashCode()
        {
            int hashCode = -1116967258;
            hashCode = hashCode * -1521134295 + year.GetHashCode();
            hashCode = hashCode * -1521134295 + month.GetHashCode();
            hashCode = hashCode * -1521134295 + day.GetHashCode();
            return hashCode;
        }
    }
}
