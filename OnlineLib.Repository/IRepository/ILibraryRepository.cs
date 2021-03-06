﻿using System;
using System.Collections.Generic;
using OnlineLib.Models;
using OnlineLib.Repository.ViewModels;

namespace OnlineLib.Repository.IRepository
{
    public interface ILibraryRepository
    {
        ICollection<Library> GetLibraryList { get; }
        LibUser GetUserByGuid(Guid id);
        Library GetLibraryById(int id);
        bool RemoveUserFromLibrary(int idlib, Guid iduser);
        bool AddLibrary(Library library, LibUser id);
        ProfiEditViewModel GetUserEditViewModel(Guid user);
        bool UpdateUserEditVieModel(ProfiEditViewModel model);
        bool IsUserWithThisEMail(string email);
        Guid GetUserGuidFromEmail(string email);
        string GetUserCode(Guid user);
        string GetUserFirstAndSecondName(Guid user);
        bool UpdateLibrary(Library library);
        bool Subscribe(int lib, Guid id);
        void Dispose();
        bool UserSubscibeLibrary(int lib, Guid user);
        bool IsWorker(int lib, Guid user);
        bool IsLibOwner( Guid user, int lib);
        bool IsWorker(string email, int lib);
    }
}