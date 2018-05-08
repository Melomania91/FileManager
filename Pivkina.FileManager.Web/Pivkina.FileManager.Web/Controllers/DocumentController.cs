using Pivkina.FileManager.Web.Models;
using Pivkina.FileManager.Web.Repositories;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pivkina.FileManager.Web.Controllers
{
    [Authorize]
    public class DocumentController : Controller
    {
        private NHDocumentRepository DocumentRepository { get; set; }
        private NHUserRepository UserRepository { get; set; }

        public DocumentController()
        {
            DocumentRepository = new NHDocumentRepository();
            UserRepository = new NHUserRepository();
        }

        [HttpGet]
        public ActionResult AddDocument()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddDocument(Document model, HttpPostedFileBase file)
        {
            model.CreationAuthor = UserRepository.LoadByLogin(User.Identity.Name);
            model.CreationDate = DateTime.Now;
            model.BinaryFile = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

            var path = ConfigurationManager.AppSettings["FilePath"];

            if (file != null && file.ContentLength > 0)
            {
                string fname = Path.GetFileName(file.FileName);
                file.SaveAs(Server.MapPath(Path.Combine(path, model.BinaryFile)));
            }

            DocumentRepository.Save(model);

            return RedirectToAction("DocumentList");
        }

        [HttpGet]
        public ActionResult DocumentList(string sortOrder)
        {
            var model = DocumentRepository.Find(null);
            ViewBag.NameSortParm = sortOrder == "name" ? "name_desc" : "name";
            ViewBag.DateSortParm = sortOrder == "date" ? "date_desc" : "date";
            ViewBag.AuthorSortParm = sortOrder == "author" ? "author_desc" : "author";

            switch (sortOrder)
            {
                case "name":
                    model = model.OrderByDescending(s => s.Name);
                    break;
                case "name_desc":
                    model = model.OrderByDescending(s => s.Name);
                    break;
                case "date":
                    model = model.OrderBy(s => s.CreationDate);
                    break;
                case "date_desc":
                    model = model.OrderByDescending(s => s.CreationDate);
                    break;
                case "author":
                    model = model.OrderBy(s => s.CreationAuthor.Name);
                    break;
                case "author_desc":
                    model = model.OrderByDescending(s => s.CreationAuthor.Name);
                    break;
                default:
                    model = model.OrderBy(s => s.Name);
                    break;
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult DocumentList(string sortOrder, string searchString)
        {
            var model = DocumentRepository.Find(null);
            ViewBag.NameSortParm = sortOrder == "name" ? "name_desc" : "name";
            ViewBag.DateSortParm = sortOrder == "date" ? "date_desc" : "date";
            ViewBag.AuthorSortParm = sortOrder == "author" ? "author_desc" : "author";

            if (!String.IsNullOrEmpty(searchString))
            {
                model = model.Where(s => s.Name.ToLower().Contains(searchString.ToLower()));
            }

            switch (sortOrder)
            {
                case "name":
                    model = model.OrderByDescending(s => s.Name);
                    break;
                case "name_desc":
                    model = model.OrderByDescending(s => s.Name);
                    break;
                case "date":
                    model = model.OrderBy(s => s.CreationDate);
                    break;
                case "date_desc":
                    model = model.OrderByDescending(s => s.CreationDate);
                    break;
                case "author":
                    model = model.OrderBy(s => s.CreationAuthor.Name);
                    break;
                case "author_desc":
                    model = model.OrderByDescending(s => s.CreationAuthor.Name);
                    break;
                default:
                    model = model.OrderBy(s => s.Name);
                    break;
            }
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            var doc = DocumentRepository.Load(id);
            return View(doc);
        }

        [HttpPost]
        public ActionResult Edit(Document model)
        {
            var doc = DocumentRepository.Load(model.Id);
            doc.Name = model.Name;
            if (DocumentRepository.Save(doc))
            {
                return RedirectToAction("DocumentList");
            }
            ModelState.AddModelError("", "Не удалось сохранить");

            return View(model);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var doc = DocumentRepository.Load(id);
            return View(doc);
        }

        [HttpPost]
        public ActionResult Delete(Document model)
        {
            var doc = DocumentRepository.Load(model.Id);
            DocumentRepository.Delete(doc);
            return RedirectToAction("DocumentList");
        }
    }
}