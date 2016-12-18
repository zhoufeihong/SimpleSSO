using System;

namespace FreeBird.Infrastructure.Domain.Uow
{
    /// <summary>
    /// 支持工作单元的资源库，负责持久化对象。
    /// </summary>
    public interface IUnitOfWorkRepository
    {
        void PersistNewItem(EntityBase entityBase);
        void PersistUpdatedItem(EntityBase entityBase);
        void PersistDeletedItem(EntityBase entityBase);
    }
}
