using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WordParser.Models.Database
{
    [Table("word")]
    public class Word
    {
        [Key]
        [Column("ID")]
        public int ID {get; set;}
        [Column("Session_ID")]
        public int SessionID {get;set;}
        [Column("Text")]
        public string Text {get; set;}
        [Column("Count")]
        public int? Count {get; set;}
    }
}
