using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TaskExecutionSystem.BLL.DTO;
using TaskExecutionSystem.BLL.DTO.Auth;
using TaskExecutionSystem.BLL.DTO.Filters;
using TaskExecutionSystem.BLL.DTO.Studies;
using TaskExecutionSystem.BLL.DTO.Task;
using static TaskExecutionSystem.BLL.Infrastructure.Contracts.DirectoryContract;
using static TaskExecutionSystem.BLL.Infrastructure.Contracts.ErrorMessageContract;
using TaskExecutionSystem.BLL.Interfaces;
using TaskExecutionSystem.DAL.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using TaskExecutionSystem.DAL.Entities.Identity;
using TaskExecutionSystem.DAL.Entities.Studies;
using TaskExecutionSystem.DAL.Entities.Task;
using TaskExecutionSystem.BLL.Validation;
using TaskExecutionSystem.DAL.Entities.Relations;
using TaskExecutionSystem.BLL.DTO.Repository;
using TaskExecutionSystem.DAL.Entities.Repository;
using TaskExecutionSystem.DAL.Entities.File;

namespace TaskExecutionSystem.BLL.Services
{
    public class RepoService : IRepoService
    {
        private readonly DataContext _context;

        public RepoService(DataContext context)
        {
            _context = context;
        }

        public async Task<OperationDetailDTO> AddFileToRepoAsync(int id, string fileName)
        {
            var detail = new OperationDetailDTO();
            try
            {
                var repo = await _context.RepositoryModels.FindAsync(id);
                if (repo != null)
                {
                    var newFile = new RepoFile
                    {
                        RepositoryItem = repo,
                        FileName = fileName,
                        Path = RepoFilePath + fileName,
                        FileURI = RepoFileURI + fileName
                    };

                    await _context.RepoFiles.AddAsync(newFile);
                    await _context.SaveChangesAsync();
                    detail.Succeeded = true;
                }
                else
                {
                    detail.ErrorMessages.Add("Ошибка при добавлении файла: репозиторий не найден.");
                }
                return detail;
            }
            catch (Exception e)
            {
                detail.ErrorMessages.Add("Ошибка при добавлении файла к репозиторию " + e.Message);
                return detail;
            }



        }

        public Task<OperationDetailDTO> UpdateRepoAsync(RepositoryDTO dto)
        {
            throw new NotImplementedException();
        }
    }
}
