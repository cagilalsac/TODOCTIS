using APP.TODO.Domain;
using CORE.APP.Features;
using System.Globalization;

namespace APP.TODO.Features
{
    public abstract class TodoDbHandler : Handler
    {
        protected readonly TodoDb _db;

        protected TodoDbHandler(TodoDb db) : base(new CultureInfo("en-US")) // tr-TR: Turkish
        {
            _db = db;
        }
    }
}
