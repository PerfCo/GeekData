using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Contracts.Pluralsight;
using HtmlAgilityPack;
using NLog;

namespace Pluralsight.Crawler
{
    public sealed class Worker
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public List<PluralsightCourse> GetCourses(string filePath)
        {
            _logger.Debug("-> GetCourses: File: '{0}'", filePath);
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentException(nameof(filePath));
            }
            if (File.Exists(filePath) == false)
            {
                throw new FileNotFoundException(nameof(filePath));
            }

            List<PluralsightCourse> courses = GetCources(filePath);
            _logger.Debug("<- GetCourses: Courses Count: {0}", courses.Count);
            return courses;
        }

        private static List<PluralsightCourse> GetCources(string filePath)
        {
            var document = new HtmlDocument();
            document.Load(filePath);

            HtmlNode root = document.DocumentNode;

            List<HtmlNode> rawCategories = root.Descendants("div")
                                               .Where(x => x.GetAttributeValue("class", string.Empty).Contains("categoryHeader"))
                                               .ToList();


            List<HtmlNode> rawCources = root.Descendants("div")
                                            .Where(x => x.GetAttributeValue("class", string.Empty).Contains("courseList"))
                                            .ToList();

            List<PluralsightCourse> result = GetCourses(rawCategories, rawCources);

            return result;
        }

        private static List<PluralsightCourse> GetCourses(List<HtmlNode> rawCategories, List<HtmlNode> rawCources)
        {
            if (rawCategories.Count != rawCources.Count)
            {
                return new List<PluralsightCourse>();
            }

            List<string> categories = rawCategories.Select(x => x.ChildNodes.First(y => y.GetAttributeValue("class", string.Empty).Contains("title")))
                                                   .Select(x => x.InnerText.Trim())
                                                   .ToList();

            var result = new List<PluralsightCourse>();
            for (var i = 0; i < rawCources.Count; i++)
            {
                string category = categories[i];
                List<PluralsightCourse> cources = GetCourses(rawCources[i]);
                cources.ForEach(x => x.Tags.Add(category));
                result.AddRange(cources);
            }
            return result;
        }

        private static List<PluralsightCourse> GetCourses(HtmlNode rawCources)
        {
            var result = new List<PluralsightCourse>();

            List<HtmlNode> rawCourses = rawCources.Descendants("td")
                                                  .Where(x => x.GetAttributeValue("class", string.Empty).Contains("title"))
                                                  .ToList();

            foreach (HtmlNode rawCourse in rawCourses)
            {
                HtmlNode courseAnchor = rawCourse.Descendants("a").FirstOrDefault();
                string courseName = courseAnchor?.InnerText;
                if (string.IsNullOrWhiteSpace(courseName))
                {
                    continue;
                }
                string courseUrl = courseAnchor.Attributes["href"]?.Value;
                if (string.IsNullOrWhiteSpace(courseUrl))
                {
                    continue;
                }

                var course = new PluralsightCourse
                {
                    Name = courseName,
                    Url = courseUrl
                };
                result.Add(course);
            }
            return result;
        }
    }
}