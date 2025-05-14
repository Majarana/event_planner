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
            Console.WriteLine("Type one from following options.");
            Console.WriteLine("Should I enter the event? [Type \"EVENT;event name;event date\"]");
            Console.WriteLine("Should I list the events? [Type \"LIST\" for listing.]");
            Console.WriteLine("Should I list the statistics? [Type \"STATS\" for statistics.]");
            Console.WriteLine("Should I end? [Type \"END\" for end.]");

            string input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("Input cannot be empty. Please enter a valid input.");
                continue;
            }
            string[] parts = input.Split(';');

            if (input.Contains(';') && parts[0].ToUpper().Trim() == "EVENT")
            {
                try
                {
                    if (parts.Length != 3)
                    {
                        throw new FormatException("Invalid format. Use: [\"EVENT;event name;event date\"]");
                    }
                    string eventName = parts[1];
                    string eventDate = parts[2];
                    if (!DateTime.TryParseExact(eventDate, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out DateTime eventParsedDate))
                    {
                        throw new FormatException("Invalid date format. Use YYYY-MM-DD.");
                    }
                    events.Add(new Event(eventName, eventParsedDate));
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
            else
            {
                switch (input.ToUpper().Trim())
                {
                    case "LIST":
                        if (events.Count == 0)
                        {
                            Console.WriteLine($"No events to display.");
                        }
                        else
                        {
                            Console.WriteLine("List of events:");
                            foreach (var ev in events)
                            {
                                Console.WriteLine($"Event {ev.EventName} with date {ev.EventDate:yyyy-MM-dd} {ev.DaysUntilOrAfterEvent()}");
                            }
                        }
                        break;
                    case "STATS":
                        if (events.Count == 0)
                        {
                            Console.WriteLine($"No events to display.");
                        }
                        else
                        {
                            statistics = events
                           .GroupBy(ev => ev.EventDate.Date)
                           .ToDictionary(group => group.Key, group => group.Count());

                            Console.WriteLine("Statistics of events:");
                            var orderedStatisticsByDate = statistics.OrderBy(stat => stat.Key);
                            foreach (var stat in orderedStatisticsByDate)
                            {
                                Console.WriteLine($"Date: {stat.Key:yyyy-MM-dd}: events: {stat.Value}");
                            }
                        }
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
}
