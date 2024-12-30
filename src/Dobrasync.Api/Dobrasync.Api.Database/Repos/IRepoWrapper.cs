using Dobrasync.Api.Database.DB;
using Dobrasync.Api.Database.DB.Entities;

namespace Dobrasync.Api.Database.Repos;

public interface IRepoWrapper
{
    DobrasyncContext DbContext { get; }
    IRepo<SystemSettingEntity> SystemSettingRepo { get; }
    IRepo<FileTransactionEntity> FileTransactionRepo { get; }
    IRepo<BlockEntity> BlockRepo { get; }
    IRepo<UserEntity> UserRepo { get; }
    IRepo<LibraryEntity> LibraryRepo { get; }
    IRepo<FileEntity> FileRepo { get; }
}