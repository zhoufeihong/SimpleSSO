using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleSSO.DTO
{
    public class EntityBaseDTO
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
            if (entity == null || !(entity is EntityBaseDTO))
            {
                return false;
            }
            return (this == (EntityBaseDTO)entity);
        }

        public static bool operator ==(EntityBaseDTO entity1, EntityBaseDTO entity2)
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

        public static bool operator !=(EntityBaseDTO entity1, EntityBaseDTO entity2)
        {
            return (!(entity1 == entity2));
        }

        public override int GetHashCode()
        {
            return _id.GetHashCode();
        }
    }
}
