using LamashareApi.Database.DB;
using LamashareApi.Database.DB.Entities;
using Microsoft.AspNetCore.Http;

namespace LamashareApi.Database.Repos;

public class RepoWrapper(LamashareContext context, IHttpContextAccessor hca) : IRepoWrapper
{
    private IRepo<BlockEntity> _blockRepo = null!;
    private IRepo<FileEntity> _fileRepo = null!;
    private IRepo<FileTransactionEntity> _fileTransactionRepo = null!;
    private IRepo<LibraryEntity> _libraryRepo = null!;
    private IRepo<SystemSettingEntity> _systemSettingRepo = null!;
    private IRepo<UserEntity> _userRepo = null!;

    public LamashareContext DbContext => context;

    #region Repos

    public IRepo<SystemSettingEntity> SystemSettingRepo
    {
        get { return _systemSettingRepo ??= new Repo<SystemSettingEntity>(context, hca); }
    }

    public IRepo<UserEntity> UserRepo
    {
        get { return _userRepo ??= new Repo<UserEntity>(context, hca); }
    }

    public IRepo<FileEntity> FileRepo
    {
        get { return _fileRepo ??= new Repo<FileEntity>(context, hca); }
    }

    public IRepo<LibraryEntity> LibraryRepo
    {
        get { return _libraryRepo ??= new Repo<LibraryEntity>(context, hca); }
    }

    public IRepo<BlockEntity> BlockRepo
    {
        get { return _blockRepo ??= new Repo<BlockEntity>(context, hca); }
    }

    public IRepo<FileTransactionEntity> FileTransactionRepo
    {
        get { return _fileTransactionRepo ??= new Repo<FileTransactionEntity>(context, hca); }
    }

    #endregion
}