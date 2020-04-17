using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using TaskExecutionSystem.DAL.Data;
using TaskExecutionSystem.DAL.Entities.Studies;

namespace TaskExecutionSystem.Application.Initialization
{
    public partial class StudyDataInitializer : DataInitializerBase
    {
        public StudyDataInitializer(IServiceProvider serviceProvider)
            : base(serviceProvider) { }

        protected override async Task InitializeAsync(DataContext context)
        {
            await InitializeFaculties(context);
            await InitializeGroups(context);
            await InitializeDepartments(context);
            await InitializeSubjects(context);
        }

        private async Task InitializeFaculties(DataContext context)
        {
            var existingFaculties = context
                .Faculties
                .ToArray();

            if(_faculties.Length != existingFaculties.Length)
            {
                var newFaculties = _faculties
                    .Where(faculty => existingFaculties.All(x => x.Name != faculty))
                    .Select(x => new Faculty(x))
                    .ToList();

                await context.AddRangeAsync(newFaculties);
                await context.SaveChangesAsync();
            }
        }

        private async Task InitializeGroups(DataContext context)
        {
            foreach(var facName in _faculties)
            {
                var groupNames = new List<string>();
                switch (facName)
                {
                    case "Информатики":
                        groupNames = _groupsIT;
                        break;
                    case "Авиационной техники":
                        groupNames = _groupsAT;
                        break;
                    case "Электроники":
                        groupNames = _groupsEl;
                        break;
                }

                var faculty = await GetFacultyByNameAsync(facName, context);
                var existingGroups = faculty.Groups;
                
                if (groupNames.Count != existingGroups.Count)
                {
                    var newGroups = groupNames
                        .Where(groupNumberName => existingGroups.All(x => x.NumberName != groupNumberName))
                        .Select(x => new Group(x, faculty.Id))
                        .ToList();

                    await context.AddRangeAsync(newGroups);
                    await context.SaveChangesAsync();
                }
            }
        }

        private async Task InitializeDepartments(DataContext context)
        {
            foreach (var facName in _faculties)
            {
                var departmentNames = new List<string>();
                switch (facName)
                {
                    case "Информатики":
                        departmentNames = _departmensIT;
                        break;
                    case "Авиационной техники":
                        departmentNames = _departmensAT;
                        break;
                    case "Электроники":
                        departmentNames = _departmensEl;
                        break;
                }

                var faculty = await GetFacultyByNameAsync(facName, context);
                var existingDepartments = faculty.Departments;

                if (departmentNames.Count != existingDepartments.Count)
                {
                    var newDepartments = departmentNames
                        .Where(departmentName => existingDepartments.All(x => x.Name != departmentName))
                        .Select(x => new Department(x, faculty.Id))
                        .ToList();

                    await context.AddRangeAsync(newDepartments);
                    await context.SaveChangesAsync();
                }
            }
        }

        private async Task InitializeSubjects(DataContext context)
        {
            var existingSubjects = await context
                .Subjects
                .AsNoTracking()
                .ToListAsync();

            if (_subjects.Count != existingSubjects.Count)
            {
                var newSubjects = _subjects
                    .Where(subject => existingSubjects.All(x => x.Name != subject))
                    .Select(x => new Subject(x))
                    .ToList();

                await context.AddRangeAsync(newSubjects);
                await context.SaveChangesAsync();
            }
        }

        private async Task<Faculty> GetFacultyByNameAsync(string facultyName, DataContext context)
        {
            return await context.Faculties
                .Include(f => f.Groups)
                .Include(f => f.Departments)
                .Where(f => f.Name == facultyName)
                .FirstOrDefaultAsync();
        }
    }


    public partial class StudyDataInitializer
    {
        private string[] _faculties =
        {
            "Информатики",
            "Авиационной техники",
            "Электроники"
        };

        private List<string> _groupsIT = new List<string>
        {
            "6108-010403D",
            "6214-020302D"
        };

        private List<string> _groupsAT = new List<string>
        {
            "2315-040806E"
        };

        private List<string> _groupsEl = new List<string>
        {
            "5204-030407D"
        };

        private List<string> _departmensIT = new List<string>
        {
            "Программных систем",
            "Высокоуровневого программирования"
        };

        private List<string> _departmensAT = new List<string>
        {
            "Авиационных двигателей",
            "Аэродинамики"
        };

        private List<string> _departmensEl = new List<string>
        {
            "Наноэлектроники"
        };

        private List<string> _subjects = new List<string>
        {
            "Математический анализ",
            "Линейная алгебра",
            "Физика",
            "Основы программирования",
            "Инженерная графика"
        };
    }
}
