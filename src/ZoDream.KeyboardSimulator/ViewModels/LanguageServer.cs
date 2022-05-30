using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using ZoDream.KeyboardSimulator.Models;
using ZoDream.Language.Storage;

namespace ZoDream.KeyboardSimulator.ViewModels
{
    public class LanguageServer
    {
        public LanguageServer()
        {
            Add("function", "function ()\n\nend\n", 10);
            Add("if", "if \nthen\n\nend\n", 3);
        }

        private readonly Regex docRegex = new(@"^-{2,}\s*");
        private readonly Regex funcRegex = new(@"function\s+([^\(\)\s]+)\s*\(([^\(\)]+)\)");

        public List<CodeCompletionData> SnippetItems { get; set; } = new();
        public List<CodeCompletionData> FuncItems { get; set; } = new();
        public List<CodeCompletionData> TempFuncItems { get; set; } = new();

        private bool IsLoading = false;

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
            foreach (var item in FuncItems)
            {
                if (item.FilterFlag.StartsWith(text))
                {
                    items.Add(item);
                }
            }
            foreach (var item in TempFuncItems)
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
                IsLoading = true;
                using var reader = LocationStorage.Reader(fileName);
                var desc = new StringBuilder();
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
                        Add(match.Groups[1].Value, desc.ToString(), false);
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
                IsLoading = false;
            });
        }

        public Task LoadTextAsync(string text)
        {
            if (IsLoading)
            {
                return Task.FromResult(0);
            }
            IsLoading = true;
            return Task.Factory.StartNew(() =>
            {
                if (string.IsNullOrWhiteSpace(text))
                {
                    TempFuncItems.Clear();
                    IsLoading = false;
                    return;
                }
                var items = new List<CodeCompletionData>();
                var lines = text.Split(new char[] {'\n'});
                var desc = new StringBuilder();
                foreach (var lineText in lines)
                {
                    var line = lineText.Trim();
                    if (string.IsNullOrEmpty(line))
                    {
                        desc.Clear();
                        continue;
                    }
                    var match = funcRegex.Match(line);
                    if (match != null && match.Success)
                    {
                        items.Add(Render(match.Groups[1].Value, desc.ToString()));
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
                TempFuncItems = items;
                Thread.Sleep(20000);
                IsLoading = false;
            });
        }


        /// <summary>
        /// 添加方法
        /// </summary>
        /// <param name="func"></param>
        /// <param name="desc"></param>
        /// <param name="isTemp"></param>
        public void Add(string func, string desc, bool isTemp)
        {
            var item = Render(func, desc);
            if (isTemp)
            {
                TempFuncItems.Add(item);
            } else
            {
                FuncItems.Add(item);
            }
        }

        public CodeCompletionData Render(string func, string desc)
        {
            return new CodeCompletionData($"{func}()")
            {
                Description = desc,
                Content = func,
                Offset = func.Length + 1,
                FilterFlag = func.ToLower()
            };
        }

        public CodeCompletionData Render(string name, string block, int offset)
        {
            return new CodeCompletionData(block)
            {
                Content = name,
                FilterFlag = name,
                Offset = offset
            };
        }

        /// <summary>
        /// 添加代码块
        /// </summary>
        /// <param name="name"></param>
        /// <param name="block"></param>
        /// <param name="offset"></param>
        public void Add(string name, string block, int offset)
        {
            SnippetItems.Add(Render(name, block, offset));
        }
    }
}
