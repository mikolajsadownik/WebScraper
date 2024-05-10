// See https://aka.ms/new-console-template for more information


using System.Net;
using System.Text.RegularExpressions;

namespace WebScraper;

public class Program
{
    
    
    
    public static async Task Main(string[] args)
    {
        
        
        if (args.Length == 0)
            throw new ArgumentNullException();
        

        string text = await downloadHTML(args[0]);
        
        Regex regex = new Regex(@"(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
                                + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
                                + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
                                + @"([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})", RegexOptions.IgnoreCase);
        
        MatchCollection matches = regex.Matches((text));
        
        if (matches.Count == 0)
            throw new Exception("No emails found");

        HashSet<string> set = new HashSet<string>();
        
        foreach (Match match in matches)
            set.Add(match.Value);
        
        
        foreach (string str in set)
            Console.WriteLine(str);
        
    }

    private static async Task<string> downloadHTML(string url)
    {
        if (!isValidUrl(url))
            throw new ArgumentException("Incorrect url");
        
        HttpClient client = new HttpClient(); 
        var response = await client.GetAsync(url);

        if (!response.IsSuccessStatusCode)
            throw new Exception("Error while downloading HTML");
        
        var htmlText = await response.Content.ReadAsStringAsync();
        return htmlText;
        
    }

    public static bool isValidUrl(string url)
    {
        return Uri.IsWellFormedUriString(url, UriKind.Absolute);
    }

    
}