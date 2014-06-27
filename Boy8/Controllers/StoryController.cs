using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Boy8.Models;
using Boy8.DAL;
using Boy8.ViewModels;

namespace Boy8.Controllers
{
    [Authorize(Roles = "parent")]
    public class StoryController : Controller
    {
        private Baby7DbContext db = new Baby7DbContext();

        // GET: /Story/
        public ActionResult Index()
        {
            return View(db.Stories.ToList());
        }

        // GET: /Story/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Story story = db.Stories.Find(id);
            if (story == null)
            {
                return HttpNotFound();
            }
            return View(story);
        }

        // GET: /Story/Create
        public ActionResult Create()
        {
            var story = new Story();
            story.Resources = new List<Resource>();
            PopulateAssignedResources(story);
            return View();
        }

        // POST: /Story/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Title,Body,Abstract,StoryCreatedOrSyncTime,SyncAccount,SyncComment, Rating")] Story story, Resource[] selectedResources)
        {
            if (selectedResources != null)
            {                
                foreach (var r in selectedResources)
                {                   
                    story.Resources.Add(r);
                }
            }

            if (ModelState.IsValid)
            {
                story.StoryCreatedOrSyncTime = DateTime.Now;
                db.Stories.Add(story);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            PopulateAssignedResources(story);
            return View(story);
        }

        // GET: /Story/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Story story = db.Stories.Find(id);
            if (story == null)
            {
                return HttpNotFound();
            }
            return View(story);
        }

        // POST: /Story/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Title,Body,Abstract,StoryCreatedOrSyncTime,SyncAccount,SyncComment, Rating")] Story story)
        {
            if (ModelState.IsValid)
            {
                db.Entry(story).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(story);
        }

        // GET: /Story/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Story story = db.Stories.Find(id);
            if (story == null)
            {
                return HttpNotFound();
            }
            return View(story);
        }

        // POST: /Story/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Story story = db.Stories.Find(id);
            db.Stories.Remove(story);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Helper
        private void PopulateAssignedResources(Story story)
        {
            var allStories = db.Stories;
            var linkedResources = story.Resources == null ? null : new HashSet<Resource>(story.Resources.Select(r => r));
            var viewModel = new List<StoryViewModel>();
            foreach (var r in allStories)
            {
                viewModel.Add(new StoryViewModel
                {
                    Story = r,
                    Resources = linkedResources,
                });
            }
            ViewBag.Stories = viewModel;
        }
        #endregion
    }
}
