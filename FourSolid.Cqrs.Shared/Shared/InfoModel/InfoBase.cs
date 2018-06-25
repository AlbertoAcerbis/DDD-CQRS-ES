namespace FourSolid.Shared.InfoModel
{
    public abstract class InfoBase
    {
        public override bool Equals(object obj)
        {
            return this.Equals(obj as InfoBase);
        }

        public virtual bool Equals(InfoBase other)
        {
            return (null != other) && this.GetType() == other.GetType();
        }

        public static bool operator ==(InfoBase entity1, InfoBase entity2)
        {
            if ((object)entity1 == null && (object)entity2 == null)
                return true;

            if ((object)entity1 == null || (object)entity2 == null)
                return false;

            return entity1.GetType() == entity2.GetType();
        }

        public static bool operator !=(InfoBase entity1, InfoBase entity2)
        {
            return (!(entity1 == entity2));
        }
    }
}