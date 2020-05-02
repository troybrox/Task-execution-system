using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace TaskExecutionSystem.BLL.DTO.Task
{
    public class TaskFileModelDTO
    {
        public TaskCreateModelDTO Task { get; set; }

        public IFormFile File { get; set; }
    }
}
