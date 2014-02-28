using System.Collections.Generic;
using TechnologyNotes.Models;

namespace TechnologyNotes.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<TechnologyNotes.Models.NotesContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(TechnologyNotes.Models.NotesContext context)
        {
            var r = new Random();
            var notes = Enumerable.Range(1, 1000).Select(o => new Note
                {

                  
                    Body = "Notes on: " + o.ToString(),
                    CreateDate = new DateTime(2014, r.Next(1, 12), r.Next(1, 28)),
                    Description = "A-" + o.ToString(),
                    Rating = r.Next(1,5)

                }).ToArray();
            context.Notes.AddOrUpdate(item => new {item.Description}, notes);



        }
    }
}
