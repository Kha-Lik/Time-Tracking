using System;
using System.Collections.Generic;
using TimeTracking.Common.Enums;
using TimeTracking.Entities;

namespace TimeTracking.Dal.Impl.Seeds.Data
{
    public static class ProjectsData
    {
      
        public static List<Project> Projects
            => new List<Project>()
            {
                new Project()
                {
                    Description =
                        "A program that allows the user to choose a time and date, and then prints out a message at given intervals (such as every second) that tells the user how much longer there is until the selected time.",
                    InitialRisk = DateTimeOffset.Now.AddYears(10),
                    Id = new Guid("df3cd5da-77d3-4972-b22c-306603578c1e"),
                    Name = "Countdown Clock",
                    Abbreviation = "APPA"
                },
                new Project()
                {
                    Description =
                        "Pomodoro Timer is a time management method. The technique uses a timer to break down work into intervals, traditionally 25 minutes in length, separated by short breaks. These intervals are named pomodoros, the plural in English of the Italian word pomodoro (tomato), after the tomato-shaped kitchen timer that Cirillo used as a university student",
                    InitialRisk = DateTimeOffset.Now.AddYears(10),
                    Id = new Guid("92a7c602-50c0-43f6-9d8d-5241378cfcc3"),
                    Name = "Pomodoro Timer",
                    Abbreviation = "POMODORO"
                },
                new Project()
                {
                    Description = "It's a game which allows you to play with english sentences.",
                    InitialRisk = DateTimeOffset.Now.AddYears(10),
                    Id = new Guid("609b11b0-cadf-49a4-8eba-a0dc01b992bd"),
                    Name = "GoogleCase",
                    Abbreviation = "GC"
                },
                new Project()
                {
                    Description = "Chess project",
                    InitialRisk = DateTimeOffset.Now.AddYears(10),
                    Id = new Guid("1c521438-8bac-4ca2-be5c-f2b9e96f4d2f"),
                    Name = "GoogleCaseData",
                    Abbreviation = "PCG"
                },
            };
    }

    public static class TeamsData
    {
        public static List<Team> Teams
            => new List<Team>()
            {
                new Team()
                {
                    ProjectId = ProjectsData.Projects[0].Id,
                    Id = new Guid("b28bde62-fc3d-4dfe-bd58-84f2685b7c95"),
                    Name = "Super team",
                    MembersCount = 4,
                },
                new Team()
                {
                    ProjectId = ProjectsData.Projects[1].Id,
                    Id = new Guid("7963567a-e4d2-478b-9020-b22c2f3d7d31"),
                    Name = "Dev team",
                    MembersCount = 4,
                },
                new Team()
                {
                    ProjectId = ProjectsData.Projects[2].Id,
                    Id = new Guid("b47a55fb-66bd-4c92-8a27-a48efab75adf"),
                    Name = "Arch team",
                    MembersCount = 4,
                },
                new Team()
                {
                    ProjectId = ProjectsData.Projects[3].Id,
                    Id =  new Guid("ed4345b5-71e3-4a12-bf76-1b22c4d9bd75"),
                    Name = "Simple team of proffessionals",
                    MembersCount = 4,
                },
            };
    }

    public static class TimeTrackingUsers
    {
        public static List<TimeTrackingUser> Users
            => new List<TimeTrackingUser>()
            {
                new TimeTrackingUser()
                {
                    Id = new Guid("45eb7068-bcfe-4b62-9b4a-474b453f96d2"),
                    FirstName = "userProjectManager",
                    LastName = "ProjectManager",
                    TeamId = TeamsData.Teams[0].Id,
                    Email = "user@secureapi.comProjectManager"
                },        
                new TimeTrackingUser()
                {
                    Id = new Guid("b0c7a3c8-33c6-4d0f-816d-318c3e49c664"),
                    FirstName = "userTeamLead",
                    LastName = "TeamLead",
                    TeamId = TeamsData.Teams[0].Id,
                    Email = "user@secureapi.comTeamLead"
                },   
                new TimeTrackingUser()
                {
                    Id = new Guid("3245fb7a-7d89-4eac-a564-c87fa40d85ad"),
                    FirstName = "userEngineer",
                    LastName = "Engineer",
                    TeamId = TeamsData.Teams[0].Id,
                    Email = "user@secureapi.comTeamLead"
                },
                new TimeTrackingUser()
                {
                    Id = new Guid("0765ED01-ABD9-4413-A99C-D0B76890DF57"),
                    FirstName = "Iryna",
                    LastName = "Tyshchenko",
                    TeamId = TeamsData.Teams[0].Id,
                    Email = "ityshchenko@example.com"
                },
                new TimeTrackingUser()
                {
                    Id = new Guid("250C17EF-9F8E-41E7-9534-933A800F7B8F"),
                    FirstName = "Vasil",
                    LastName = "Petrovich",
                    TeamId = TeamsData.Teams[0].Id,
                    Email = "vpetrovich@example.com"
                },
                new TimeTrackingUser()
                {
                    Id = new Guid("95BEE109-CBA5-41B2-BD5C-DEFD81482098"),
                    FirstName = "Ivan",
                    LastName = "Petrovich",
                    TeamId = TeamsData.Teams[0].Id,
                    Email = "ipetrovich@example.com"
                },
                new TimeTrackingUser()
                {
                    Id = new Guid("75EE0F98-9439-4378-B18F-0299A5DD57BC"),
                    FirstName = "Andiy",
                    LastName = "Petrovich",
                    TeamId = TeamsData.Teams[0].Id,
                    Email = "apetrovich@example.com"
                },
                new TimeTrackingUser()
                {
                    Id = new Guid("618BECE8-1538-4E54-BCE1-FDEBCC915BF3"),
                    FirstName = "Kyryl",
                    LastName = "Evenov",
                    TeamId = TeamsData.Teams[0].Id,
                    Email = "kevgenov@example.com"
                },
                new TimeTrackingUser()
                {
                    Id = new Guid("EDC48AF9-87F9-4EEC-A7CE-4BE99706FCE7"),
                    FirstName = "Vasil",
                    LastName = "Evenov",
                    TeamId = TeamsData.Teams[1].Id,
                    Email = "vevgenov@example.com"
                },
                new TimeTrackingUser()
                {
                    Id = new Guid("C954F213-493E-49BF-86C1-2264473B0684"),
                    FirstName = "Evgeniy",
                    LastName = "Krylov",
                    TeamId = TeamsData.Teams[1].Id,
                    Email = "eKrylov@example.com"
                },
                new TimeTrackingUser()
                {
                    Id = new Guid("FDDDF89D-A406-4342-AAD8-73A2C9432387"),
                    FirstName = "Hritz",
                    LastName = "Evenov",
                    TeamId = TeamsData.Teams[1].Id,
                    Email = "hevgenov@example.com"
                },
                new TimeTrackingUser()
                {
                    Id = new Guid("A68629A8-0685-4DC2-B122-615442EA9C8C"),
                    FirstName = "Alina",
                    LastName = "Amons",
                    TeamId = TeamsData.Teams[2].Id,
                    Email = "aamons@example.com"
                },
                new TimeTrackingUser()
                {
                    Id =new Guid("C7C3EEC1-3964-4BAB-86A1-8D9AC6FF17A6"),
                    FirstName = "Hritz",
                    LastName = "Amons",
                    TeamId = TeamsData.Teams[2].Id,
                    Email = "hamaons@example.com"
                },
                new TimeTrackingUser()
                {
                    Id = new Guid("3632B315-F353-425F-8768-4805C3A6E3DC"),
                    FirstName = "Vasil",
                    LastName = "Amons",
                    TeamId = TeamsData.Teams[2].Id,
                    Email = "vamons@example.com"
                },
                new TimeTrackingUser()
                {
                    Id = new Guid("7F5ECB88-7CFC-4231-B99D-A0E2B312BFFB"),
                    FirstName = "Denis",
                    LastName = "Amons",
                    TeamId = TeamsData.Teams[3].Id,
                    Email = "damaons@example.com"
                },
                new TimeTrackingUser()
                {
                    Id = new Guid("F057BD96-4D10-4355-B943-564B60C141AE"),
                    FirstName = "Alina",
                    LastName = "Kim",
                    TeamId = TeamsData.Teams[3].Id,
                    Email = "akim@example.com"
                },
                new TimeTrackingUser()
                {
                    Id = new Guid("ACD7F017-E8D9-486E-895C-89847FD30461"),
                    FirstName = "Alina",
                    LastName = "Antipina",
                    TeamId = TeamsData.Teams[3].Id,
                    Email = "aantipina@example.com"
                },
            };
    }

    public static class MilestonesData
    {
        public static List<Milestone> Milestones
            => new List<Milestone>()
            {

                new Milestone()
                {
                    ProjectId = ProjectsData.Projects[2].Id,
                    Id = new Guid("17201bf3-4617-4fdd-af23-f188a641a292"),
                    Description = $"Milestone {DateTimeOffset.UtcNow}",
                    DueDate = DateTimeOffset.UtcNow.AddDays(1000),
                    CreatedAt = DateTimeOffset.UtcNow,
                    UpdatedAt = DateTimeOffset.UtcNow.AddDays(1),
                    OpenedAt = DateTimeOffset.UtcNow,
                    Title = "Milestone1",
                    State = State.Opened,
                    CreatedByUserId = TimeTrackingUsers.Users[0].Id,
                },
                new Milestone()
                {
                    ProjectId = ProjectsData.Projects[2].Id,
                    Id = new Guid("c525d106-0be8-4c20-962e-4262224e2f97"),
                    Description = $"Milestone {DateTimeOffset.UtcNow.AddDays(1000)}",
                    DueDate = DateTimeOffset.UtcNow.AddDays(1000),
                    CreatedAt = DateTimeOffset.UtcNow,
                    UpdatedAt = DateTimeOffset.UtcNow.AddDays(1),
                    OpenedAt = DateTimeOffset.UtcNow.AddDays(1000),
                    Title = "Milestone2",
                    State = State.Opened,
                    CreatedByUserId = TimeTrackingUsers.Users[1].Id,
                },

                new Milestone()
                {
                    ProjectId = ProjectsData.Projects[2].Id,
                    Id = new Guid("45b7a147-91d2-4aa1-853d-e23005f17033"),
                    Description = $"Milestone {DateTimeOffset.UtcNow.AddDays(2)}",
                    DueDate = DateTimeOffset.UtcNow.AddDays(1000),
                    CreatedAt = default,
                    UpdatedAt = DateTimeOffset.UtcNow.AddDays(1),
                    OpenedAt = DateTimeOffset.UtcNow.AddDays(2),
                    Title = "Milestone3",
                    State = State.Opened,
                    CreatedByUserId = TimeTrackingUsers.Users[2].Id,
                },
                new Milestone()
                {
                    ProjectId = ProjectsData.Projects[2].Id,
                    Id = new Guid("191c0e71-f215-4a63-af13-d91db627bd04"),
                    Description = $"Milestone {DateTimeOffset.UtcNow.AddDays(-7)}",
                    DueDate = DateTimeOffset.UtcNow.AddDays(1000),
                    CreatedAt = default,
                    UpdatedAt = DateTimeOffset.UtcNow.AddDays(1),
                    OpenedAt = DateTimeOffset.UtcNow.AddDays(-7),
                    Title = "Milestone4",
                    State = State.Opened,
                    CreatedByUserId = TimeTrackingUsers.Users[3].Id,
                },
            };
    }

    public static class IssuesData
    {
        public static List<Issue>  Issues
            => new List<Issue>()
            {
                new Issue()
                {
                    AssignedToUserId = TimeTrackingUsers.Users[0].Id,
                    ReportedByUserId = TimeTrackingUsers.Users[1].Id,
                    Description = @"Subgoals:
                Program must then convert the given sentence in camel case.To know more about camel case click_here
                Sentence can be entered with any number of spaces.",
                    Id = new Guid("7b07dd7d-3e0b-486f-bd05-34fbc02c3cf3"),
                    MilestoneId = MilestonesData.Milestones[0].Id,
                    OpenedAt = DateTimeOffset.UtcNow.AddDays(-3),
                    ProjectId = ProjectsData.Projects[0].Id,
                    Status = Status.Open,
                    Title = "Convert the given sentence in camel case",
                    
                },
                new Issue()
                {
                    AssignedToUserId = TimeTrackingUsers.Users[1].Id,
                    ReportedByUserId = TimeTrackingUsers.Users[2].Id,
                    Description = @"Program must convert the given sentence in google case .
                    What is a google case style of sentence?know_about_it_here:.",
                    Id =new Guid("f91a29dd-d135-476b-935f-d0b2a9f6b9fb"),
                    MilestoneId = MilestonesData.Milestones[0].Id,
                    OpenedAt = DateTimeOffset.UtcNow.AddDays(-3),
                    ProjectId = ProjectsData.Projects[0].Id,
                    Status = Status.Open,
                    Title = "Convert the given sentence in google case"
                },
                new Issue()
                {
                    AssignedToUserId = TimeTrackingUsers.Users[1].Id,
                    ReportedByUserId = TimeTrackingUsers.Users[3].Id,
                    Description = @"User will enter a sentence in any format.(uppercase or lowercase or a mix of both)",
                    Id = new Guid("ff7783d3-5b92-445e-898c-64e694bc9cb3"),
                    MilestoneId = MilestonesData.Milestones[0].Id,
                    OpenedAt =DateTimeOffset.UtcNow.AddDays(-7),
                    ProjectId = ProjectsData.Projects[1].Id,
                    Status = Status.Open,
                    Title = "Uppercase or lowercase or a mix of both"
                },
                new Issue()
                {
                    AssignedToUserId = TimeTrackingUsers.Users[2].Id,
                    ReportedByUserId = TimeTrackingUsers.Users[3].Id,
                    Description =
                        @"Allow the user to input the amount of sides on a dice and how many times it should be rolled.",
                    Id = new Guid("9d4e086d-74b5-4da0-a0ae-ab0d21f451d1"),
                    MilestoneId = MilestonesData.Milestones[1].Id,
                    OpenedAt = DateTimeOffset.UtcNow.AddDays(-3),
                    ProjectId = ProjectsData.Projects[2].Id,
                    Status = Status.Open,
                    Title = "Allow the user to input the amount of sides"
                },
                new Issue()
                {
                    AssignedToUserId = TimeTrackingUsers.Users[4].Id,
                    ReportedByUserId = TimeTrackingUsers.Users[4].Id,
                    Description =
                        @"Your program should simulate dice rolls and keep track of how many times each number comes up (this does not have to be displayed).",
                    Id = new Guid("bdcd00d0-31de-4b43-8598-30f65f834b7b"),
                    MilestoneId = MilestonesData.Milestones[1].Id,
                    OpenedAt =  DateTimeOffset.UtcNow.AddDays(-1),
                    ProjectId = ProjectsData.Projects[2].Id,
                    Status = Status.Open,
                    Title = "Keep track of how many times each number comes up"
                },
                new Issue()
                {
                    AssignedToUserId = TimeTrackingUsers.Users[5].Id,
                    ReportedByUserId = TimeTrackingUsers.Users[4].Id,
                    Description = @"Print out how many times each number came up.",
                    Id = new Guid("32a0885d-19c2-425a-9b84-ac40a34997a6"),
                    MilestoneId = MilestonesData.Milestones[1].Id,
                    OpenedAt = DateTimeOffset.UtcNow.AddDays(-3),
                    ProjectId = ProjectsData.Projects[2].Id,
                    Status = Status.Closed,
                    Title = "Print out"
                },
                new Issue()
                {
                    AssignedToUserId = TimeTrackingUsers.Users[5].Id,
                    ReportedByUserId = TimeTrackingUsers.Users[4].Id,
                    Description = @"Print out how many times each number came up.",
                    Id = new Guid("6513f9a3-6e2a-4361-be31-eee93ea00c38"),
                    MilestoneId = MilestonesData.Milestones[2].Id,
                    OpenedAt =  DateTimeOffset.UtcNow.AddDays(-1),
                    ProjectId = ProjectsData.Projects[2].Id,
                    Status = Status.Closed,
                    Title = "Print out"
                },
                new Issue()
                {
                    AssignedToUserId = TimeTrackingUsers.Users[2].Id,
                    ReportedByUserId = TimeTrackingUsers.Users[6].Id,
                    Description =
                        @"Adjust your program so that if the user does not type in a number when they need to, the program will keep prompting them to type in a real number until they do so.",
                    Id =  new Guid("c85bff0c-dc12-4655-9870-3859792bd47d"),
                    MilestoneId = MilestonesData.Milestones[2].Id,
                    OpenedAt =  DateTimeOffset.UtcNow.AddDays(-8),
                    ProjectId = ProjectsData.Projects[2].Id,
                    Status = Status.Closed,
                    Title = "Adjust your program"
                },
                new Issue()
                {
                    AssignedToUserId = TimeTrackingUsers.Users[5].Id,
                    ReportedByUserId = TimeTrackingUsers.Users[6].Id,
                    Description =
                        @"Put the program into a loop so that the user can continue to simulate dice rolls without having to restart the entire program.",
                    Id = new Guid("68f8b3fe-f08f-4096-a259-c8f5c453fd54"),
                    MilestoneId = MilestonesData.Milestones[1].Id,
                    OpenedAt = DateTimeOffset.UtcNow.AddDays(-9),
                    ProjectId = ProjectsData.Projects[1].Id,
                    Status = Status.Open,
                    Title = "Put the program into a loop"
                },
                new Issue()
                {
                    AssignedToUserId = TimeTrackingUsers.Users[6].Id,
                    ReportedByUserId = TimeTrackingUsers.Users[7].Id,
                    Description =
                        @"In addition to printing out how many times each side appeared, also print out the percentage it appeared. If you can, round the percentage to 4 digits total OR two decimal places.",
                    Id = new Guid("ab22c4fb-2759-4dac-af57-804d73343777"),
                    MilestoneId = MilestonesData.Milestones[1].Id,
                    OpenedAt =  DateTimeOffset.UtcNow.AddDays(-10),
                    ProjectId = ProjectsData.Projects[1].Id,
                    Status = Status.Open,
                    Title = "Put the program into a loop"
                },
                new Issue()
                {
                    AssignedToUserId = TimeTrackingUsers.Users[6].Id,
                    ReportedByUserId = TimeTrackingUsers.Users[6].Id,
                    Description =
                        @"You are about to play a board game, but you realize you don't have any dice. Fortunately you have this program.",
                    Id = new Guid("45c663be-093e-4b72-b2fb-c0785f7941ab"),
                    MilestoneId = MilestonesData.Milestones[2].Id,
                    OpenedAt =  DateTimeOffset.UtcNow.AddDays(-3),
                    ProjectId = ProjectsData.Projects[1].Id,
                    Status = Status.Review,
                    Title = "Play a board game"
                },
                new Issue()
                {
                    AssignedToUserId = TimeTrackingUsers.Users[7].Id,
                    ReportedByUserId = TimeTrackingUsers.Users[7].Id,
                    Description = @"Create a program that opens a new window and draws 2 six-sided dice",
                    Id = new Guid("0c8fea9c-a48e-41e9-b651-4253fc4b33b9"),
                    MilestoneId = MilestonesData.Milestones[2].Id,
                    OpenedAt =DateTimeOffset.UtcNow.AddDays(-1),
                    ProjectId = ProjectsData.Projects[0].Id,
                    Status = Status.Review,
                    Title = "Opens a new window"
                },
                new Issue()
                {
                    AssignedToUserId = TimeTrackingUsers.Users[8].Id,
                    ReportedByUserId = TimeTrackingUsers.Users[1].Id,
                    Description =
                        @"Allow the user to select the number of dice to be drawn on screen(1-4) 2. Add up the total of the dice and display it",
                    Id = new Guid("87753e70-5ff6-4831-ba49-086ad4e720db"),
                    MilestoneId = MilestonesData.Milestones[1].Id,
                    OpenedAt = DateTimeOffset.UtcNow.AddDays(-3),
                    ProjectId = ProjectsData.Projects[0].Id,
                    Status = Status.Review,
                    Title = "Select the number of dice"
                },
            };
    }


    public static class WorkLogsData
    {
        public static List<WorkLog> WorkLogs
            => new List<WorkLog>()
            {

                new WorkLog()
                {
                    StartDate = DateTimeOffset.UtcNow,
                    Description = "Spent 2 days on this task",
                    TimeSpent = TimeSpan.FromDays(2).Ticks,
                    IsApproved = true,
                    Id = Guid.NewGuid(),
                    IssueId = IssuesData.Issues[0].Id,
                    UserId = TimeTrackingUsers.Users[0].Id,
                    CreatedAt = DateTimeOffset.UtcNow,
                    ActivityType = ActivityType.Coding,
                },
                new WorkLog()
                {
                    StartDate = DateTimeOffset.UtcNow,
                    Description = "Spent 1 days on this task",
                    TimeSpent = TimeSpan.FromDays(1).Ticks,
                    IsApproved = true,
                    Id = Guid.NewGuid(),
                    IssueId = IssuesData.Issues[1].Id,
                    UserId = TimeTrackingUsers.Users[1].Id,
                    CreatedAt = DateTimeOffset.UtcNow,
                    ActivityType = ActivityType.Coding,
                },
                new WorkLog()
                {
                    StartDate = DateTimeOffset.UtcNow,
                    Description = "Spent 5 days on this task",
                    TimeSpent = TimeSpan.FromDays(5).Ticks,
                    IsApproved = true,
                    Id = Guid.NewGuid(),
                    IssueId = IssuesData.Issues[0].Id,
                    UserId = TimeTrackingUsers.Users[4].Id,
                    CreatedAt = DateTimeOffset.UtcNow,
                    ActivityType = ActivityType.Coding,
                },
                new WorkLog()
                {
                    StartDate = DateTimeOffset.UtcNow,
                    Description = "Spent 5 days on this task",
                    TimeSpent = TimeSpan.FromDays(5).Ticks,
                    IsApproved = true,
                    Id = Guid.NewGuid(),
                    IssueId = IssuesData.Issues[2].Id,
                    UserId = TimeTrackingUsers.Users[4].Id,
                    CreatedAt = DateTimeOffset.UtcNow,
                    ActivityType = ActivityType.Coding,
                },
                new WorkLog()
                {
                    StartDate = DateTimeOffset.UtcNow,
                    Description = "Code review on this task",
                    TimeSpent = TimeSpan.FromDays(1).Ticks,
                    IsApproved = true,
                    Id = Guid.NewGuid(),
                    IssueId = IssuesData.Issues[2].Id,
                    UserId = TimeTrackingUsers.Users[4].Id,
                    CreatedAt = DateTimeOffset.UtcNow,
                    ActivityType = ActivityType.CodeReview,
                },
                new WorkLog()
                {
                    StartDate = DateTimeOffset.UtcNow,
                    Description = "Code review on this task",
                    TimeSpent = TimeSpan.FromDays(1).Ticks,
                    IsApproved = true,
                    Id = Guid.NewGuid(),
                    IssueId = IssuesData.Issues[8].Id,
                    UserId = TimeTrackingUsers.Users[6].Id,
                    CreatedAt = DateTimeOffset.UtcNow,
                    ActivityType = ActivityType.CodeReview,
                },
                new WorkLog()
                {
                    StartDate = DateTimeOffset.UtcNow,
                    Description = "Code review on this task",
                    TimeSpent = TimeSpan.FromDays(1).Ticks,
                    IsApproved = true,
                    Id = Guid.NewGuid(),
                    IssueId = IssuesData.Issues[9].Id,
                    UserId = TimeTrackingUsers.Users[5].Id,
                    CreatedAt = DateTimeOffset.UtcNow,
                    ActivityType = ActivityType.CodeReview,
                },
                new WorkLog()
                {
                    StartDate = DateTimeOffset.UtcNow,
                    Description = "Code review on this task",
                    TimeSpent = TimeSpan.FromDays(1).Ticks,
                    IsApproved = true,
                    Id = Guid.NewGuid(),
                    IssueId = IssuesData.Issues[5].Id,
                    UserId = TimeTrackingUsers.Users[8].Id,
                    CreatedAt = DateTimeOffset.UtcNow,
                    ActivityType = ActivityType.CodeReview,
                },
                new WorkLog()
                {
                    StartDate = DateTimeOffset.UtcNow,
                    Description = "Code review on this task",
                    TimeSpent = TimeSpan.FromDays(6).Ticks,
                    IsApproved = true,
                    Id = Guid.NewGuid(),
                    IssueId = IssuesData.Issues[6].Id,
                    UserId = TimeTrackingUsers.Users[4].Id,
                    CreatedAt = DateTimeOffset.UtcNow,
                    ActivityType = ActivityType.CodeReview,
                },
                new WorkLog()
                {
                    StartDate = DateTimeOffset.UtcNow,
                    Description = "Research on this task",
                    TimeSpent = TimeSpan.FromDays(1).Ticks,
                    IsApproved = true,
                    Id = Guid.NewGuid(),
                    IssueId = IssuesData.Issues[6].Id,
                    UserId = TimeTrackingUsers.Users[4].Id,
                    CreatedAt = DateTimeOffset.UtcNow,
                    ActivityType = ActivityType.Research,
                },
                new WorkLog()
                {
                    StartDate = DateTimeOffset.UtcNow,
                    Description = "Code review on this task",
                    TimeSpent = TimeSpan.FromDays(6).Ticks,
                    IsApproved = true,
                    Id = Guid.NewGuid(),
                    IssueId = IssuesData.Issues[9].Id,
                    UserId = TimeTrackingUsers.Users[9].Id,
                    CreatedAt = DateTimeOffset.UtcNow,
                    ActivityType = ActivityType.CodeReview,
                },
                new WorkLog()
                {
                    StartDate = DateTimeOffset.UtcNow,
                    Description = "Code review on this task",
                    TimeSpent = TimeSpan.FromDays(1).Ticks,
                    IsApproved = true,
                    Id = Guid.NewGuid(),
                    IssueId = IssuesData.Issues[10].Id,
                    UserId = TimeTrackingUsers.Users[10].Id,
                    CreatedAt = DateTimeOffset.UtcNow,
                    ActivityType = ActivityType.CodeReview,
                },
                new WorkLog()
                {
                    StartDate = DateTimeOffset.UtcNow,
                    Description = "Research on this task",
                    TimeSpent = TimeSpan.FromDays(1).Ticks,
                    IsApproved = true,
                    Id = Guid.NewGuid(),
                    IssueId = IssuesData.Issues[1].Id,
                    UserId = TimeTrackingUsers.Users[1].Id,
                    CreatedAt = DateTimeOffset.UtcNow,
                    ActivityType = ActivityType.Research,
                },
                new WorkLog()
                {
                    StartDate = DateTimeOffset.UtcNow,
                    Description = "Code review on this task",
                    TimeSpent = TimeSpan.FromDays(1).Ticks,
                    IsApproved = true,
                    Id = Guid.NewGuid(),
                    IssueId = IssuesData.Issues[1].Id,
                    UserId = TimeTrackingUsers.Users[10].Id,
                    CreatedAt = DateTimeOffset.UtcNow,
                    ActivityType = ActivityType.CodeReview,
                },
                new WorkLog()
                {
                    StartDate = DateTimeOffset.UtcNow,
                    Description = "Code review on this task",
                    TimeSpent = TimeSpan.FromDays(1).Ticks,
                    IsApproved = true,
                    Id = Guid.NewGuid(),
                    IssueId = IssuesData.Issues[3].Id,
                    UserId = TimeTrackingUsers.Users[10].Id,
                    CreatedAt = DateTimeOffset.UtcNow,
                    ActivityType = ActivityType.CodeReview,
                },
                new WorkLog()
                {
                    StartDate = DateTimeOffset.UtcNow,
                    Description = "Code review on this task",
                    TimeSpent = TimeSpan.FromMinutes(7).Ticks,
                    IsApproved = true,
                    Id = Guid.NewGuid(),
                    IssueId = IssuesData.Issues[3].Id,
                    UserId = TimeTrackingUsers.Users[6].Id,
                    CreatedAt = DateTimeOffset.UtcNow,
                    ActivityType = ActivityType.CodeReview,
                },
                new WorkLog()
                {
                    StartDate = DateTimeOffset.UtcNow,
                    Description = "Research on this task",
                    TimeSpent = TimeSpan.FromMinutes(45).Ticks,
                    IsApproved = true,
                    Id = Guid.NewGuid(),
                    IssueId = IssuesData.Issues[3].Id,
                    UserId = TimeTrackingUsers.Users[7].Id,
                    CreatedAt = DateTimeOffset.UtcNow,
                    ActivityType = ActivityType.Research,
                },
                new WorkLog()
                {
                    StartDate = DateTimeOffset.UtcNow,
                    Description = "Code review on this task",
                    TimeSpent = TimeSpan.FromDays(1).Ticks,
                    IsApproved = true,
                    Id = Guid.NewGuid(),
                    IssueId = IssuesData.Issues[2].Id,
                    UserId = TimeTrackingUsers.Users[10].Id,
                    CreatedAt = DateTimeOffset.UtcNow,
                    ActivityType = ActivityType.CodeReview,
                },
                new WorkLog()
                {
                    StartDate = DateTimeOffset.UtcNow,
                    Description = "Code review on this task",
                    TimeSpent = TimeSpan.FromMinutes(7).Ticks,
                    IsApproved = true,
                    Id = Guid.NewGuid(),
                    IssueId = IssuesData.Issues[10].Id,
                    UserId = TimeTrackingUsers.Users[9].Id,
                    CreatedAt = DateTimeOffset.UtcNow,
                    ActivityType = ActivityType.CodeReview,
                },
                new WorkLog()
                {
                    StartDate = DateTimeOffset.UtcNow,
                    Description = "Research on this task",
                    TimeSpent = TimeSpan.FromMinutes(45).Ticks,
                    IsApproved = true,
                    Id = Guid.NewGuid(),
                    IssueId = IssuesData.Issues[6].Id,
                    UserId = TimeTrackingUsers.Users[8].Id,
                    CreatedAt = DateTimeOffset.UtcNow,
                    ActivityType = ActivityType.Research,
                },
                new WorkLog()
                {
                    StartDate = DateTimeOffset.UtcNow,
                    Description = "Spent 15 days on this task",
                    TimeSpent = TimeSpan.FromDays(15).Ticks,
                    IsApproved = true,
                    Id = Guid.NewGuid(),
                    IssueId = IssuesData.Issues[0].Id,
                    UserId = TimeTrackingUsers.Users[9].Id,
                    CreatedAt = DateTimeOffset.UtcNow,
                    ActivityType = ActivityType.Coding,
                },
                new WorkLog()
                {
                    StartDate = DateTimeOffset.UtcNow,
                    Description = "Spent 15 days on this task",
                    TimeSpent = TimeSpan.FromDays(15).Ticks,
                    IsApproved = true,
                    Id = Guid.NewGuid(),
                    IssueId = IssuesData.Issues[0].Id,
                    UserId = TimeTrackingUsers.Users[2].Id,
                    CreatedAt = DateTimeOffset.UtcNow,
                    ActivityType = ActivityType.Coding,
                },
                new WorkLog()
                {
                    StartDate = DateTimeOffset.UtcNow,
                    Description = "Code review on this task",
                    TimeSpent = TimeSpan.FromDays(1).Ticks,
                    IsApproved = true,
                    Id = Guid.NewGuid(),
                    IssueId = IssuesData.Issues[0].Id,
                    UserId = TimeTrackingUsers.Users[1].Id,
                    CreatedAt = DateTimeOffset.UtcNow,
                    ActivityType = ActivityType.CodeReview,
                },
            };
    }
}

