using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TaskExecutionSystem.DAL.Interfaces;
using TaskExecutionSystem.DAL.Entities.Identity;

namespace TaskExecutionSystem.DAL.Entities.Registration
{
    public class StudentRegisterRequest : RegisterRequestBase
    {
        public int GroupId { get; set; }
    }
}
