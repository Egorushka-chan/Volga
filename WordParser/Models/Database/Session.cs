using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WordParser.Models.Database
{   
    [Table("session")]
    public class Session
    {
        [Key]
        [Column("ID")]
        public int ID {get;set;}
        [Column("Date")]
        public string Date {get; set;}
    }
}
