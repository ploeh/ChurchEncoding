namespace Ploeh.Samples.ChurchEncoding
{
    internal sealed class Year : IPeriod
    {
        private readonly int year;

        public Year(int year)
        {
            this.year = year;
        }

        public T Accept<T>(IPeriodVisitor<T> visitor)
        {
            return visitor.VisitYear(year);
        }

        public override bool Equals(object obj)
        {
            return obj is Year year &&
                   this.year == year.year;
        }

        public override int GetHashCode()
        {
            return -582376068 + year.GetHashCode();
        }
    }
}
