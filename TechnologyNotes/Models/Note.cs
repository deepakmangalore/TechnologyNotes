using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TechnologyNotes.Models
{
    public class Note
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Body { get; set; }
        public DateTime CreateDate { get; set; }
        public int Rating { get; set; }
        public List<Tag> Tags { get; set; } 
    }
    
}