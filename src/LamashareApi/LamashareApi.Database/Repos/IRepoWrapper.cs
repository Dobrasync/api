using LamashareApi.Database.DB;
using LamashareApi.Database.DB.Entities;
using File = LamashareApi.Database.DB.Entities.File;

namespace LamashareApi.Database.Repos;

public interface IRepoWrapper
{
    LamashareContext DbContext { get; }
    IRepo<SystemSetting> SystemSettingRepo { get; }
    IRepo<FileTransaction> FileTransactionRepo { get; }
    IRepo<Block> BlockRepo { get; }
    IRepo<User> UserRepo { get; }
    IRepo<Library> LibraryRepo { get; }
    IRepo<File> FileRepo { get; }
}