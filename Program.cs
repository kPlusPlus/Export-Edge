using System;
using System.IO;
using Newtonsoft.Json.Linq;

class Program
{
    static void Main(string[] args)
    {
        string edgeBookmarkFilePath = @"C:\Users\YourUsername\AppData\Local\Microsoft\Edge\User Data\Default\Favorites\bookmarks";
        edgeBookmarkFilePath = @"c:\Users\kresi\AppData\Local\Microsoft\Edge\User Data\Default\bookmarks";

        if (File.Exists(edgeBookmarkFilePath))
        {
            string bookmarksJson = File.ReadAllText(edgeBookmarkFilePath);
            if (bookmarksJson == null) { return; }
            dynamic bookmarks = JObject.Parse(bookmarksJson);

            if (bookmarks == null) 
            {
                return;
            }

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
    }
}
