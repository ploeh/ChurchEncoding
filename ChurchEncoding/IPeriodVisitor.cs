namespace Ploeh.Samples.ChurchEncoding
{
    public interface IPeriodVisitor<T>
    {
        T VisitYear(int year);
        T VisitMonth(int year, int month);
        T VisitDay(int year, int month, int day);
    }
}
