using System;
using System.IO;
using Newtonsoft.Json.Linq;
using Microsoft.Data.Sqlite;

class Program
{
    static void Main(string[] args)
    {
        string edgeBookmarkFilePath = @"C:\Users\YourUsername\AppData\Local\Microsoft\Edge\User Data\Default\Favorites\bookmarks";
        edgeBookmarkFilePath = @"c:\Users\kresi\AppData\Local\Microsoft\Edge\User Data\Default\bookmarks";

        if (File.Exists(edgeBookmarkFilePath))
        {
            string bookmarksJson = File.ReadAllText(edgeBookmarkFilePath);
            if (bookmarksJson == null) return;
            dynamic bookmarks = JObject.Parse(bookmarksJson);
            if (bookmarks == null) return;
            File.WriteAllText("edge_bookmarks.json", bookmarksJson);

            // Traverse the bookmarks JSON and extract necessary data
            foreach (var bookmark in bookmarks["roots"]["bookmark_bar"]["children"])
            {
                string title = bookmark["name"];
                string url = bookmark["url"];

                Console.WriteLine($"Title: {title}, URL: {url}");
            }

            Console.WriteLine("Export complete.");
        }
        else
        {
            Console.WriteLine("Edge bookmarks file not found.");
            Console.ReadLine();
        }

        ExportFirefox();
    }


    public static void ExportFirefox()
    {
        string firefoxProfilePath = @"C:\Users\username\AppData\Roaming\Mozilla\Firefox\Profiles\your_profile.default";
        firefoxProfilePath = @"c:\Users\kresi\AppData\Roaming\Mozilla\Firefox\Profiles\nyk29heu.default-release-1697969231492\";

        string databasePath = System.IO.Path.Combine(firefoxProfilePath, "places.sqlite");

        // Connection string for SQLite
        string connectionString = $"Data Source={databasePath};";

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            // Query to select bookmarks
            string query = "SELECT url, title FROM moz_bookmarks b " +
                           "JOIN moz_places p ON b.fk = p.id " +
                           "WHERE b.type = 1 AND b.parent != 4;"; // Type 1 represents bookmarks, parent 4 is the Bookmarks Menu

            using (var command = new Microsoft.Data.Sqlite.SqliteCommand(query, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string url = reader["url"].ToString();
                        string title = reader["title"].ToString();

                        Console.WriteLine($"Title: {title}, URL: {url}");

                        // Here you can export the bookmarks to a desired format
                        // For example, you can write them to a JSON file
                    }
                }
            }
        }
    }

}
