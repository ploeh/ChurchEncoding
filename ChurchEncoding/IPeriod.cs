namespace Ploeh.Samples.ChurchEncoding
{
    public interface IPeriod
    {
        T Accept<T>(IPeriodVisitor<T> visitor);
    }
}
