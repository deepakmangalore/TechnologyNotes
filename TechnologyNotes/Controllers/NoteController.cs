using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using TechnologyNotes.Models;

namespace TechnologyNotes.Controllers
{
    public class NoteController : ApiController
    {
        private NotesContext db = new NotesContext();

        public IEnumerable<Note> GetNotes(string q = null, string sort = null, bool desc = false,
                                                        int? limit = null, int offset = 0)
        {
            var list = ((IObjectContextAdapter)db).ObjectContext.CreateObjectSet<Note>();

            IQueryable<Note> items = string.IsNullOrEmpty(sort) 
                ? list.OrderBy(o => o.CreateDate)
                : list.OrderBy(String.Format("it.{0} {1}", sort, desc ? "DESC" : "ASC"));

            if (!string.IsNullOrEmpty(q) && q != "undefined") 
                items = items.Where(t => t.Description.Contains(q));

            if (offset > 0) items = items.Skip(offset);
            if (limit.HasValue) items = items.Take(limit.Value);
            return items;
        }

        // GET api/Note/5
        public Note GetNote(int id)
        {
            Note note = db.Notes.Find(id);
            if (note == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return note;
        }

        // PUT api/Note/5
        public HttpResponseMessage PutNote(int id, Note note)
        {
            if (ModelState.IsValid && id == note.Id)
            {
                db.Entry(note).State = EntityState.Modified;

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // POST api/Note
        public HttpResponseMessage PostNote(Note note)
        {
            if (ModelState.IsValid)
            {
                note.CreateDate = DateTime.Now;
                db.Notes.Add(note);

                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, note);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = note.Id }));
                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // DELETE api/Note/5
        public HttpResponseMessage DeleteNote(int id)
        {
            Note note = db.Notes.Find(id);
            if (note == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.Notes.Remove(note);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, note);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}