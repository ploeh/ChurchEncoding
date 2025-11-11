namespace Ploeh.Samples.ChurchEncoding
{
    internal interface IPeriodVisitor<T>
    {
        T VisitYear(int year);
        T VisitMonth(int year, int month);
        T VisitDay(int year, int month, int day);
    }
}
