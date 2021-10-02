using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WordParser.Models.Database;

namespace WordParser.Models
{
    class DBAccessor
    {
        int _sessionID;
        public DBAccessor()
        {
            using (WordsContext context = new WordsContext())
            {
                context.Sessions.Load();
                _sessionID = context.Sessions.OrderByDescending(u => u.ID).FirstOrDefault().ID + 1;


                context.Sessions.Add(new Session
                {
                    ID = _sessionID,
                    Date = DateTime.Now.ToShortDateString()
                });
                context.SaveChanges();
            }
        }

        public void InsertWords(Dictionary<string, int> dictWords)
        {
            WordsContext context = new WordsContext();
            context.Words.Load();
            var globalWords = context.Words.Local;
            
            int maxID;
            maxID = context.Words.OrderByDescending(u => u.ID).FirstOrDefault().ID + 1;

            foreach (var word in dictWords)
            {
                if (globalWords.Where(x => x.SessionID == _sessionID & x.Text == word.Key).FirstOrDefault() == default)
                    globalWords.Add(new Word
                    {
                        ID = maxID,
                        SessionID = _sessionID,
                        Text = word.Key,
                        Count = word.Value
                    });
                else
                {
                    globalWords.Where(x => x.SessionID == _sessionID & x.Text == word.Key).First().Count += word.Value;
                }
            }
            context.SaveChanges();
        }
    }
}
