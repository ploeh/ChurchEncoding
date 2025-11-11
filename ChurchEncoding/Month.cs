namespace Ploeh.Samples.ChurchEncoding
{
    internal sealed class Month : IPeriod
    {
        private readonly int year;
        private readonly int month;

        public Month(int year, int month)
        {
            this.year = year;
            this.month = month;
        }

        public T Accept<T>(IPeriodVisitor<T> visitor)
        {
            return visitor.VisitMonth(year, month);
        }

        public override bool Equals(object obj)
        {
            return obj is Month month &&
                   year == month.year &&
                   this.month == month.month;
        }

        public override int GetHashCode()
        {
            int hashCode = -1279022207;
            hashCode = hashCode * -1521134295 + year.GetHashCode();
            hashCode = hashCode * -1521134295 + month.GetHashCode();
            return hashCode;
        }
    }
}
