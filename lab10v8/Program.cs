
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

class Team
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string City { get; set; }

    public Team(int id, string name, string city)
    {
        Id = id;
        Name = name;
        City = city;
    }
}

class Player
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int TeamId { get; set; }
    public int Age { get; set; }

    public Player(int id, string name, int teamId, int age)
    {
        Id = id;
        Name = name;
        TeamId = teamId;
        Age = age;
    }
}

class Program
{
    static void Main()
    {
        List<Team> teams = new List<Team>
        {
            new Team(1, "Lions", "Kyiv"),
            new Team(2, "Tigers", "Lviv"),
            new Team(3, "Eagles", "Odesa"),
            new Team(4, "Sharks", "Rivne"),
            new Team(5, "Wolves", "Kharkiv"),
            new Team(6, "Bulls", "Dnipro"),
            new Team(7, "Panthers", "Lutsk"),
            new Team(8, "Falcons", "Poltava"),
            new Team(9, "Hawks", "Chernihiv"),
            new Team(10, "Bears", "Vinnytsia")
        };

        List<Player> players = new List<Player>
        {
            new Player(1, "Alex", 1, 22),
            new Player(2, "Daniel", 1, 24),
            new Player(3, "Mark", 2, 21),
            new Player(4, "John", 2, 27),
            new Player(5, "David", 3, 23),
            new Player(6, "Michael", 3, 29),
            new Player(7, "James", 4, 20),
            new Player(8, "Robert", 4, 26),
            new Player(9, "William", 5, 25),
            new Player(10, "Thomas", 5, 31),
            new Player(11, "Oliver", 6, 22),
            new Player(12, "Henry", 6, 28),
            new Player(13, "Lucas", 7, 19),
            new Player(14, "Benjamin", 7, 24),
            new Player(15, "Ethan", 8, 30),
            new Player(16, "Mason", 8, 21),
            new Player(17, "Logan", 9, 33),
            new Player(18, "Jack", 9, 27),
            new Player(19, "Noah", 10, 18),
            new Player(20, "Leo", 10, 35)
        };

        Console.WriteLine("=== LABORATORY WORK №10 ===");
        Console.WriteLine("Variant 8: Sports League");
        Console.WriteLine();

        // TASK 1: Find players under 25 from the Lions team.

        Console.WriteLine("TASK 1: Players under 25 from Lions");

        Measure("Query syntax", () =>
        {
            var result = (
                from player in players
                join team in teams on player.TeamId equals team.Id
                where team.Name == "Lions" && player.Age < 25
                select player
            ).ToList();

            foreach (var player in result)
                Console.WriteLine($"{player.Name}, age: {player.Age}");

            return result.Count;
        });

        Measure("Method syntax", () =>
        {
            var result = players
                .Join(teams,
                    player => player.TeamId,
                    team => team.Id,
                    (player, team) => new { player, team })
                .Where(x => x.team.Name == "Lions" && x.player.Age < 25)
                .Select(x => x.player)
                .ToList();

            foreach (var player in result)
                Console.WriteLine($"{player.Name}, age: {player.Age}");

            return result.Count;
        });

        // TASK 2: Join players with their teams.

        Console.WriteLine("\nTASK 2: Players and their teams");

        Measure("Query syntax", () =>
        {
            var result = (
                from player in players
                join team in teams on player.TeamId equals team.Id
                select new
                {
                    PlayerName = player.Name,
                    TeamName = team.Name
                }
            ).ToList();

            foreach (var item in result)
                Console.WriteLine($"{item.PlayerName} - {item.TeamName}");

            return result.Count;
        });

        Measure("Method syntax", () =>
        {
            var result = players
                .Join(teams,
                    player => player.TeamId,
                    team => team.Id,
                    (player, team) => new
                    {
                        PlayerName = player.Name,
                        TeamName = team.Name
                    })
                .ToList();

            foreach (var item in result)
                Console.WriteLine($"{item.PlayerName} - {item.TeamName}");

            return result.Count;
        });

        // TASK 3: Calculate average age of players in each team.

        Console.WriteLine("\nTASK 3: Average age in each team");

        Measure("Query syntax", () =>
        {
            var result = (
                from team in teams
                join player in players
                    on team.Id equals player.TeamId into teamPlayers
                select new
                {
                    TeamName = team.Name,
                    AverageAge = teamPlayers.Average(p => p.Age)
                }
            ).ToList();

            foreach (var item in result)
                Console.WriteLine($"{item.TeamName}: {item.AverageAge:F2}");

            return result.Count;
        });

        Measure("Method syntax", () =>
        {
            var result = teams
                .GroupJoin(players,
                    team => team.Id,
                    player => player.TeamId,
                    (team, teamPlayers) => new
                    {
                        TeamName = team.Name,
                        AverageAge = teamPlayers.Average(p => p.Age)
                    })
                .ToList();

            foreach (var item in result)
                Console.WriteLine($"{item.TeamName}: {item.AverageAge:F2}");

            return result.Count;
        });

        // TASK 4: Find the oldest player.

        Console.WriteLine("\nTASK 4: The oldest player");

        Measure("Query syntax", () =>
        {
            var result = (
                from player in players
                orderby player.Age descending
                select player
            ).First();

            Console.WriteLine($"{result.Name}, age: {result.Age}");

            return result;
        });

        Measure("Method syntax", () =>
        {
            var result = players
                .OrderByDescending(player => player.Age)
                .First();

            Console.WriteLine($"{result.Name}, age: {result.Age}");

            return result;
        });

        Console.WriteLine("\n=== END OF LABORATORY WORK ===");
    }

    static void Measure<T>(string name, Func<T> action)
    {
        Stopwatch stopwatch = Stopwatch.StartNew();

        T result = action();

        stopwatch.Stop();

        Console.WriteLine(
            $"{name}: {stopwatch.Elapsed.TotalMilliseconds:F4} ms");

        Console.WriteLine();
    }
}
