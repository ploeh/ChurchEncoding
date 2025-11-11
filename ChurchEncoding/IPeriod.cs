namespace Ploeh.Samples.ChurchEncoding
{
    internal interface IPeriod
    {
        T Accept<T>(IPeriodVisitor<T> visitor);
    }
}
