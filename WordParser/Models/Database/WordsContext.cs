using System.Data.Entity;

namespace WordParser.Models.Database
{
    class WordsContext : DbContext
    {
        public WordsContext() : base("DefaultConnection")
        {
        }
        public DbSet<Word> Words { get; set; }
        public DbSet<Session> Sessions {get;set;}
    }
}
