using System.Collections.Generic;
using System.Linq;
using DataGenerator.Repositories.Entities;

namespace DataGenerator.Nodes.Cources
{
    public sealed class PluralsightCourseNode : Node
    {
        private readonly PluralsightCourseEntity _entity;

        public PluralsightCourseNode(PluralsightCourseEntity entity)
        {
            _entity = entity;
            Level = 3;
            Id = entity.Id.ToString();
            Label = entity.Name;
            Tag = entity.Tags.FirstOrDefault();
        }

        public override string Id { get; }
        public override string Label { get; }
        public override int Level { get; }
        public override string Tag { get; }

        public static List<string> Captions()
        {
            var suffix = "PluralsightCourse";
            var items = new[] { "Name", "Tags", "Url" };
            return items.Select(x => $"{x}{suffix}").ToList();
        }

        public List<object> ToCsv()
        {
            List<object> result = ToCsvCommon();
            var node = new List<object>
            {
                _entity.Name,
                string.Join(",", _entity.Tags),
                _entity.Url
            };
            result.AddRange(node);
            return result;
        }

//        public override string ToString()
//        {
//            var items1 = new string(';', GithubRepositoryEntity.Captions().Count - 1);
//            var items2 = new string(';', StackOverflowUserEntity.Captions().Count - 1);
//
//            var result = new List<object>
//            {
//                Id,
//                Name,
//                Level,
//                Tags.FirstOrDefault(),
//                items1,
//                Name,
//                string.Join(",", Tags),
//                Url,
//                items2
//            };
//            return string.Join(";", result);
//        }
    }
}
