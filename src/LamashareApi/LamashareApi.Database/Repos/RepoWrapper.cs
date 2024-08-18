using LamashareApi.Database.DB;
using LamashareApi.Database.DB.Entities;
using Microsoft.AspNetCore.Http;
using File = LamashareApi.Database.DB.Entities.File;

namespace LamashareApi.Database.Repos;

public class RepoWrapper(LamashareContext context, IHttpContextAccessor hca) : IRepoWrapper
{
    private IRepo<SystemSetting> _systemSettingRepo = null!;
    private IRepo<File> _fileRepo = null!;
    private IRepo<Library> _libraryRepo = null!;
    private IRepo<User> _userRepo = null!;

    public LamashareContext DbContext => context;

    #region Repos
    public IRepo<SystemSetting> SystemSettingRepo
    {
        get { return _systemSettingRepo ??= new Repo<SystemSetting>(context, hca); }
    }

    public IRepo<User> UserRepo
    {
        get { return _userRepo ??= new Repo<User>(context, hca); }
    }

    public IRepo<File> FileRepo
    {
        get { return _fileRepo ??= new Repo<File>(context, hca); }
    }

    public IRepo<Library> LibraryRepo
    {
        get { return _libraryRepo ??= new Repo<Library>(context, hca); }
    }

    #endregion
}