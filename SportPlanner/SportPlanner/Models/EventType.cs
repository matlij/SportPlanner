using System.Collections.Generic;
using System.Linq;

namespace SportPlanner.Models
{
    //public enum EventType
    //{
    //    Undefined,
    //    Traning,
    //    Game,
    //    Social,
    //    Other
    //}

    public class EventType
    {
        public static EventType Undefined { get; } = new EventType(0, "Okänd", string.Empty);
        public static EventType Traning { get; } = new EventType(1, "Träning", "icon_traning.png");
        public static EventType Game { get; } = new EventType(2, "Match", "icon_game.png");
        public static EventType Social { get; } = new EventType(3, "Socialt", "icon_party.png");
        public static EventType Other { get; } = new EventType(4, "Övrigt", string.Empty);


        public int Val { get; }
        public string Name { get; }
        public string IconImage { get; }

        public EventType(int val, string name, string iconImage)
        {
            Val = val;
            Name = name;
            IconImage = iconImage;
        }

        public static IEnumerable<EventType> List()
        {
            // alternately, use a dictionary keyed by value
            return new[] { Undefined, Traning, Game, Social, Other };
        }

        public static EventType FromInt(int val)
        {
            return List().Single(e => e.Val == val);
        }

        public override string ToString() => Name;
    }
}
