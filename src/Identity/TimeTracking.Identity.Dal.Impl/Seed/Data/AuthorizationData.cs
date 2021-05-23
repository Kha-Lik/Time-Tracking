using System;
using System.Collections.Generic;
using TimeTracking.Identity.Entities;

namespace TimeTracking.Identity.Dal.Impl.Seed.Data
{
    public static class AuthorizationData
    {
        public enum Roles
        {
            Engineer,
            ProjectManager,
            TeamLead,
        }
        public const string DefaultUsername = "user";
        public const string DefaultEmail = "user@secureapi.com";
        public const string DefaultPassword = "Pa$$w0rd_";
        public const Roles DefaultRole = Roles.Engineer;
    }
    
    public static class UsersData
    {
        public static List<User> Users
            => new List<User>()
            {
                new User
                {
                    Id = new Guid("0765ED01-ABD9-4413-A99C-D0B76890DF57"),
                    FirstName = "Iryna",
                    LastName = "Tyshchenko",
                    UserName = "ityshchenko@example.com",
                    Email = "ityshchenko@example.com",
                },
                new User
                {
                    Id = new Guid("250C17EF-9F8E-41E7-9534-933A800F7B8F"),
                    FirstName = "Vasil",
                    LastName = "Petrovich",
                    Email = "vpetrovich@example.com",
                    UserName = "vpetrovich@example.com",
                },
                new User
                {
                    Id = new Guid("95BEE109-CBA5-41B2-BD5C-DEFD81482098"),
                    FirstName = "Ivan",
                    LastName = "Petrovich",
                    Email = "ipetrovich@example.com",
                    UserName = "ipetrovich@example.com",
                },
                new User
                {
                    Id = new Guid("75EE0F98-9439-4378-B18F-0299A5DD57BC"),
                    FirstName = "Andiy",
                    LastName = "Petrovich",
                    Email = "apetrovich2@example.com",
                    UserName = "apetrovich2@example.com",
                },
                new User
                {
                    Id = new Guid("618BECE8-1538-4E54-BCE1-FDEBCC915BF3"),
                    FirstName = "Kyryl",
                    LastName = "Evenov",
                    Email = "kevgenov@example.com",
                    UserName =  "kevgenov@example.com",
                },
                new User
                {
                    Id = new Guid("EDC48AF9-87F9-4EEC-A7CE-4BE99706FCE7"),
                    FirstName = "Vasil",
                    LastName = "Evenov",
                    Email = "vevgenov@example.com",
                    UserName =  "vevgenov@example.com",
                },
                new User
                {
                    Id = new Guid("C954F213-493E-49BF-86C1-2264473B0684"),
                    FirstName = "Evgeniy",
                    LastName = "Krylov",
                    Email = "eKrylov@example.com",
                    UserName =  "eKrylov@example.com",
                },
                new User
                {
                    Id = new Guid("FDDDF89D-A406-4342-AAD8-73A2C9432387"),
                    FirstName = "Hritz",
                    LastName = "Evenov",
                    Email = "hevgenov@example.com",
                    UserName = "hevgenov@example.com",
                },
                new User
                {
                    Id = new Guid("A68629A8-0685-4DC2-B122-615442EA9C8C"),
                    FirstName = "Alina",
                    LastName = "Amons",
                    Email = "aamons@example.com",
                    UserName ="aamons@example.com",
                },
                new User
                {
                    Id =new Guid("C7C3EEC1-3964-4BAB-86A1-8D9AC6FF17A6"),
                    FirstName = "Hritz",
                    LastName = "Amons",
                    Email = "hamaons@example.com",
                    UserName ="hamaons@example.com",
                },
                new User
                {
                    Id = new Guid("3632B315-F353-425F-8768-4805C3A6E3DC"),
                    FirstName = "Vasil",
                    LastName = "Amons",
                    Email = "vamons@example.com",
                    UserName ="vamons@example.com",
                },
                new User
                {
                    Id = new Guid("7F5ECB88-7CFC-4231-B99D-A0E2B312BFFB"),
                    FirstName = "Denis",
                    LastName = "Amons",
                    Email = "damaons@example.com",
                    UserName  ="damaons@example.com",
                },
                new User
                {
                    Id = new Guid("F057BD96-4D10-4355-B943-564B60C141AE"),
                    FirstName = "Alina",
                    LastName = "Kim",
                    Email = "akim@example.com",
                    UserName  ="akim@example.com",
                },
                new User
                {
                    Id = new Guid("ACD7F017-E8D9-486E-895C-89847FD30461"),
                    FirstName = "Alina",
                    LastName = "Antipina",
                    Email = "aantipina@example.com",
                    UserName  ="aantipina@example.com",
                },
            };
    }
}