namespace EventPlanner;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Event Planner");
        List<Event> events = new List<Event>();
        Dictionary<DateTime, int> statistics = new Dictionary<DateTime, int>();

        while (true)
        {
            DisplayMenu();

            string input = Console.ReadLine() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("Input cannot be empty. Please enter a valid input.");
                continue;
            }
            string[] parts = input.Split(';');

            if (input.Contains(';') && parts[0].ToUpper().Trim() == "EVENT")
            {
                HandleEventInput(input, events);
            }
            else
            {
                switch (input.ToUpper().Trim())
                {
                    case "LIST":
                        HandleListCommand(events);
                        break;
                    case "STATS":
                        HandleStatsCommand(events);
                        break;
                    case "END":
                        Console.WriteLine("You have closed the event planner!");
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Wrong option. Please try again and choose valid option.");
                        break;
                }
            }
        }
    }

    static void DisplayMenu()
    {
        Console.WriteLine("Type one from the following options:");
        Console.WriteLine("Should I enter the event? [Type \"EVENT;event name;event date\"]");
        Console.WriteLine("Should I list the events? [Type \"LIST\" for listing.]");
        Console.WriteLine("Should I list the statistics? [Type \"STATS\" for statistics.]");
        Console.WriteLine("Should I end? [Type \"END\" for end.]");
    }

    static void HandleEventInput(string input, List<Event> events)
    {
        try
        {
            string[] parts = input.Split(';');
            if (parts.Length != 3)
            {
                throw new FormatException("Invalid format. Use: EVENT;[event name];[event date]");
            }

            string eventName = parts[1];
            string eventDate = parts[2];

            if (!DateTime.TryParseExact(eventDate, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out DateTime eventParsedDate))
            {
                throw new FormatException("Invalid date format. Use YYYY-MM-DD.");
            }

            events.Add(new Event(eventName, eventParsedDate));
            Console.WriteLine($"Event added: {eventName} on {eventParsedDate:yyyy-MM-dd}");
        }
        catch (FormatException ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An unexpected error occurred: {ex.Message}");
        }
    }

    static void HandleListCommand(List<Event> events)
    {
        if (events.Count == 0)
        {
            Console.WriteLine("No events to display.");
        }
        else
        {
            Console.WriteLine("List of events:");
            foreach (var ev in events.OrderBy(e => e.EventDate))
            {
                Console.WriteLine($"Event {ev.EventName} with date {ev.EventDate:yyyy-MM-dd} {ev.DaysUntilOrAfterEvent()}");
            }
        }
    }
    static void HandleStatsCommand(List<Event> events)
    {
        if (events.Count == 0)
        {
            Console.WriteLine("No events to display.");
        }
        else
        {
            var statistics = events
                .GroupBy(ev => ev.EventDate.Date)
                .ToDictionary(group => group.Key, group => group.Count());

            Console.WriteLine("Statistics of events:");
            foreach (var stat in statistics.OrderBy(stat => stat.Key))
            {
                Console.WriteLine($"Date: {stat.Key:yyyy-MM-dd}: events: {stat.Value}");
            }
        }
    }
}
