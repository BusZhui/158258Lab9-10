using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication3.Models
{
    public class Campus
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        

        public virtual ICollection<Student> Students { get; set; }

    }
}