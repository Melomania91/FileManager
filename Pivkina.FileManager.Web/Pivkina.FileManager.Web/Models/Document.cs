using Pivkina.FileManager.Web.Controllers;
using Pivkina.FileManager.Web.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Pivkina.FileManager.Web.Models
{
    public class Document : IEntity
    {
        public virtual int Id { get; set; }

        [Display(Name = "Название")]
        public virtual string Name { get; set; }

        [Display(Name = "Дата создания")]
        [ReadOnly(true)]
        public virtual DateTime CreationDate { get; set; }

        [Display(Name = "Автор создания")]
        [ReadOnly(true)]
        public virtual User CreationAuthor { get; set; }

        [Display(Name = "Файл")]
        public virtual string BinaryFile { get; set; }

    }
}