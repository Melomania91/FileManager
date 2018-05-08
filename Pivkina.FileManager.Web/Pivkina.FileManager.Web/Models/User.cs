using Pivkina.FileManager.Web.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pivkina.FileManager.Web.Models
{
    public class User : IEntity
    {
        public virtual int Id { get; set; }

        public virtual string Name { get; set; }

        public virtual string Login { get; set; }

        public virtual string Password { get; set; }
    }
}