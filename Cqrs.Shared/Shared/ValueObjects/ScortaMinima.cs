namespace FourSolid.Shared.ValueObjects
{
    public class ScortaMinima : ValueObjectDoubleGreaterThanZero<ScortaMinima>
    {
        public ScortaMinima(double value) : base(value)
        {
        }
    }
}