using System;

namespace FreeBird.Infrastructure.Domain.Uow
{
    /// <summary>
    /// 代表一个工作单元。
    /// </summary>
    public interface IUnitOfWork
    {
        void RegisterAdded(EntityBase entityBase, IUnitOfWorkRepository unitOfWorkRepository);
        void RegisterChangeded(EntityBase entityBase, IUnitOfWorkRepository unitOfWorkRepository);
        void RegisterRemoved(EntityBase entityBase, IUnitOfWorkRepository unitOfWorkRepository);
        void Commit();
    }
}
