using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ZoDream.KeyboardSimulator.Models;

namespace ZoDream.KeyboardSimulator.ViewModels
{
    public class LanguageServer
    {


        public List<CodeCompletionData> SnippetItems { get; set; } = new();

        public IList<CodeCompletionData> Find(string text)
        {
            text = text.ToLower();
            var items = new List<CodeCompletionData>();
            foreach (var item in SnippetItems)
            {
                if (item.FilterFlag.StartsWith(text))
                {
                    items.Add(item);
                }
            }
            return items;
        }

        public async Task LoadAsync()
        {
            await LoadAsync(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "snippet.lua"));
        }
        public Task LoadAsync(string fileName)
        {
            return Task.Factory.StartNew(() =>
            {
                if (string.IsNullOrWhiteSpace(fileName) || !File.Exists(fileName))
                {
                    return;
                }
                using var reader = Language.Storage.File.Reader(fileName);
                var desc = new StringBuilder();
                var docRegex = new Regex(@"^-{2,}\s*");
                var funcRegex = new Regex(@"function\s+([^\(\)\s]+)\s*\(([^\(\)]+)\)");
                while (true)
                {
                    var line = reader.ReadLine();
                    if (line == null)
                    {
                        break;
                    }
                    line = line.Trim();
                    if (string.IsNullOrEmpty(line))
                    {
                        desc.Clear();
                        continue;
                    }
                    var match = funcRegex.Match(line);
                    if (match != null && match.Success)
                    {
                        SnippetItems.Add(new CodeCompletionData($"{match.Groups[1].Value}()")
                        {
                            Description = desc.ToString(),
                            Content = match.Groups[1].Value,
                            Priority = match.Groups[1].Value.Length + 1,
                            FilterFlag = match.Groups[1].Value.ToLower()
                        });
                        desc.Clear();
                        continue;
                    }
                    match = docRegex.Match(line);
                    if (match == null || !match.Success)
                    {
                        desc.Clear();
                        continue;
                    }
                    desc.AppendLine(line.Substring(match.Value.Length));
                }
            });
        }
    }
}
