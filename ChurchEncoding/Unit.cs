namespace Ploeh.Samples.ChurchEncoding
{
    public sealed class Unit
    {
        public readonly static Unit Instance = new Unit();

        private Unit() { }
    }
}
