using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisitorBook.Core.Models
{
    public enum Gender
    {
        Man,
        Woman
    }

    public class Visitor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime BirthDate { get; set; }
        public Gender Gender { get; set; }

        public VisitorAddress? VisitorAddress { get; set; }

        public List<State> States { get; set; }
    }
}
