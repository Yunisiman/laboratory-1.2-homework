using System.Security.Cryptography;
using System.Security.Policy;
using System;
using System.Windows.Controls;
using System.Windows.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using HtmlAgilityPack;

class Program
{
    static async System.Threading.Tasks.Task Main(string[] args)
    {
        var url = "https://www.labirint.ru/books/";
        var books = await GetBooks(url);

        Console.WriteLine("Книги до сортировки:");
        PrintBooks(books);

        // Сортировка по названию в алфавитном порядке
        var sortedBooks = SortBooks(books);
        Console.WriteLine("\nКниги после сортировки по названию (в алфавитном порядке):");
        PrintBooks(sortedBooks);

        // Сортировка по названию в обратном порядке
        var sortedBooksReverse = SortBooks(books, true);
        Console.WriteLine("\nКниги после сортировки по названию (в обратном порядке):");

        PrintBooks(sortedBooksReverse);
    }

    static async System.Threading.Tasks.Task<List<Book>> GetBooks(string url)
    {
        var httpClient = new HttpClient();
        var html = await httpClient.GetStringAsync(url);
        var htmlDocument = new HtmlDocument();
        htmlDocument.LoadHtml(html);

        var books = new List<Book>();
        var productNodes = htmlDocument.DocumentNode.SelectNodes("//div[contains(@class, 'product')]");

        foreach (var node in productNodes)
        {
            var titleNode = node.SelectSingleNode(".//a[contains(@class, 'product-title')]");
            var authorNode = node.SelectSingleNode(".//span[contains(@class, 'product-author')]");

            var title = titleNode?.InnerText.Trim() ?? "Неизвестное название";
            var author = authorNode?.InnerText.Trim() ?? "Неизвестный автор";

            books.Add(new Book { Title = title, Author = author });
        }

        return books;
    }

    static List<Book> SortBooks(List<Book> books, bool reverse = false)
    {
        return reverse ? books.OrderByDescending(b => b.Title).ToList() : books.OrderBy(b => b.Title).ToList();
    }

    static void PrintBooks(List<Book> books)
    {
        foreach (var book in books)
        {
            Console.WriteLine($"{book.Title} - {book.Author}");
        }
    }
}

class Book
{
    public string Title { get; set; }
    public string Author { get; set; }
}

