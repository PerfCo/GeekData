using System;
using System.Collections.Generic;
using System.Linq;
using Contracts.StackOverflow;
using Nelibur.Sword.DataStructures;
using Nelibur.Sword.Extensions;
using NLog;
using StackExchange.StacMan;
using StackExchange.StacMan.Tags;

namespace StackOverflow.Crawler
{
    public sealed class Worker
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly StacManClient _client = new StacManClient("tXK73MbOGYDFm1cSHZHbYg((", "2.2");

        public List<StackOverflowUser> GetTopUsers(string tag)
        {
            _logger.Debug("GetTopUsers -> Tag: {0}", tag);

            List<StackOverflowUser> result = GetTopAnswerers(tag)
                .Map(GetUsers)
                .Map(x => x.ConvertAll(Convert))
                .Do(x => x.Iter(y => y.Tags.Add(tag)))
                .MapOnEmpty(() => new List<StackOverflowUser>())
                .Value;

            _logger.Debug("GetTopUsers <- Tag: {0} UserCount: {1}", tag, result.Count);
            return result;
        }

        private static StackOverflowUser Convert(User user)
        {
            var result = new StackOverflowUser
            {
                AccountId = user.AccountId,
                DisplayName = user.DisplayName,
                ProfileImage = user.ProfileImage,
                ProfileUrl = user.Link,
                BadgeCounts = new BadgeCounts
                {
                    Bronze = user.BadgeCounts.Bronze,
                    Gold = user.BadgeCounts.Gold,
                    Silver = user.BadgeCounts.Silver
                }
            };
            return result;
        }

        private Option<List<TagScore>> GetTopAnswerers(string tag)
        {
            _logger.Debug("GetTopAnswerers -> Tag: {0}", tag);
            string encodedTag = Uri.EscapeDataString(tag.ToLower());
            StacManResponse<TagScore> response = _client.Tags.GetTopAnswerers("stackoverflow", encodedTag, Period.AllTime).Result;
            if (!response.Success)
            {
                _logger.Error(response.Error);
                return new Option<List<TagScore>>(new List<TagScore>());
            }

            _logger.Debug("GetTopAnswerers <- Tag: {0} UserCount: {1}", tag, response.Data.Items.Length);
            return response.Data.Items.ToList().ToOption();
        }

        private Option<List<User>> GetUsers(List<TagScore> tagScores)
        {
            List<int> requestUsers = tagScores.Where(x => x.User.UserId.HasValue)
                                              .Select(x => x.User.UserId.Value)
                                              .ToList();

            _logger.Debug("GetUsers -> Count: {0}", requestUsers.Count);
            StacManResponse<User> response = _client.Users.GetByIds("stackoverflow", requestUsers).Result;
            if (!response.Success)
            {
                _logger.Error(response.Error);
                return new Option<List<User>>(new List<User>());
            }
            Option<List<User>> result = response.Data.Items.ToList().ToOption();
            _logger.Debug("GetUsers <- Count: {0}", result.Value.Count);
            return result;
        }
    }
}
