using LamashareApi.Database.DB;
using LamashareApi.Database.DB.Entities;

namespace LamashareApi.Database.Repos;

public interface IRepoWrapper
{
    LamashareContext DbContext { get; }
    IRepo<SystemSettingEntity> SystemSettingRepo { get; }
    IRepo<FileTransactionEntity> FileTransactionRepo { get; }
    IRepo<BlockEntity> BlockRepo { get; }
    IRepo<UserEntity> UserRepo { get; }
    IRepo<LibraryEntity> LibraryRepo { get; }
    IRepo<FileEntity> FileRepo { get; }
}