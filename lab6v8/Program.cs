
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Lab10v8
{
    public interface IParser<T>
    {
        T Parse(string input);
    }

    public class DateParser : IParser<DateTime>
    {
        public DateTime Parse(string input)
        {
            return DateTime.ParseExact(
                input,
                "dd.MM.yyyy",
                CultureInfo.InvariantCulture);
        }
    }

    public class TimeParser : IParser<DateTime>
    {
        public DateTime Parse(string input)
        {
            DateTime time = DateTime.ParseExact(
                input,
                "HH:mm",
                CultureInfo.InvariantCulture);

            return DateTime.Today.Add(time.TimeOfDay);
        }
    }

    public abstract class Renderer
    {
        public abstract void RenderHeader();
        public abstract void RenderBody();

        public void RenderFooter()
        {
            Console.WriteLine("Кінець документа.");
        }

        public void Render()
        {
            RenderHeader();
            RenderBody();
            RenderFooter();
            Console.WriteLine();
        }
    }

    public class HtmlRenderer : Renderer
    {
        public override void RenderHeader()
        {
            Console.WriteLine("<h1>HTML-документ</h1>");
        }

        public override void RenderBody()
        {
            Console.WriteLine("<p>Основний вміст HTML-документа.</p>");
        }
    }

    public class MarkdownRenderer : Renderer
    {
        public override void RenderHeader()
        {
            Console.WriteLine("# Markdown-документ");
        }

        public override void RenderBody()
        {
            Console.WriteLine("Основний вміст Markdown-документа.");
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            List<IParser<DateTime>> parsers =
                new List<IParser<DateTime>>
                {
                    new DateParser(),
                    new TimeParser()
                };

            string[] inputs =
            {
                "11.10.2026",
                "15:30"
            };

            Console.WriteLine("РОБОТА З ІНТЕРФЕЙСОМ");

            for (int i = 0; i < parsers.Count; i++)
            {
                DateTime result = parsers[i].Parse(inputs[i]);
                Console.WriteLine(result.ToString("dd.MM.yyyy HH:mm"));
            }

            List<Renderer> renderers = new List<Renderer>
            {
                new HtmlRenderer(),
                new MarkdownRenderer()
            };

            Console.WriteLine();
            Console.WriteLine("РОБОТА З АБСТРАКТНИМ КЛАСОМ");

            foreach (Renderer renderer in renderers)
            {
                renderer.Render();
            }
        }
    }
}

