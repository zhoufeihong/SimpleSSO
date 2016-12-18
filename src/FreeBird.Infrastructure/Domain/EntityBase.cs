using System;

namespace FreeBird.Infrastructure.Domain
{
    /// <summary>
    /// 实体基类。
    /// </summary>
    public abstract class EntityBase
    {
        private Guid _id = Guid.Empty;

        public Guid ID
        {
            get { return _id; }
            set { _id = value; }
        }

        public DateTime? CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? LastUpdatedOn { get; set; }
        public string LastUpdatedBy { get; set; }

        public static Guid NewID()
        {
            return Guid.NewGuid();
        }

        public override bool Equals(object entity)
        {
            if (entity == null || !(entity is EntityBase))
            {
                return false;
            }
            return (this == (EntityBase)entity);
        }

        public static bool operator ==(EntityBase entity1, EntityBase entity2)
        {
            if ((object)entity1 == null && (object)entity2 == null)
            {
                return true;
            }

            if ((object)entity1 == null || (object)entity2 == null)
            {
                return false;
            }

            return entity1.ID == entity2.ID;
        }

        public static bool operator !=(EntityBase entity1, EntityBase entity2)
        {
            return (!(entity1 == entity2));
        }

        public override int GetHashCode()
        {
            return _id.GetHashCode();
        }

        public virtual void Init(string userName)
        {
            this.ID = NewID();
            this.CreatedOn = DateTime.Now;
            this.CreatedBy = userName;
        }

        public virtual void InitUpdate(string userName)
        {
            this.LastUpdatedOn = DateTime.Now;
            this.LastUpdatedBy = userName;
        }

    }
}
