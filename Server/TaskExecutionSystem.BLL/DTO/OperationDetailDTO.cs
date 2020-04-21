using System.Collections.Generic;

namespace TaskExecutionSystem.BLL.DTO
{
    public class OperationDetailDTO
    {
        public bool Succeeded { get; set; }

        public List<string> ErrorMessages { get; set; }
    }

    public class OperationDetailDTO<T> : OperationDetailDTO where T : class
    {
        public T Data { get; set; }
    }
}
