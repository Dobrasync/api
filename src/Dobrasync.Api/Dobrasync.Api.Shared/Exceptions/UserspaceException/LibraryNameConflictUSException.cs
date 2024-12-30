using Dobrasync.Api.Shared.Localization;

namespace Dobrasync.Api.Shared.Exceptions.UserspaceException;

public class LibraryNameConflictUSException() : UserspaceException(409,
    LocKeys.ExceptionLibraryNameConflict.KeyTemplate, "A library with same name already exists.");