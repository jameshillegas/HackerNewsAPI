using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using HackerNewsAPI.Models;
using System.Configuration;
using System.Net.Http;
using Newtonsoft.Json;
using System.Runtime.Caching;

namespace HackerNewsAPI.Utilities
{
    public class HackerNewsService : IHackerNewsService
    {
        private readonly string bestStoriesURI;
        private readonly string singleStoryURI;
        private MemoryCache cache;
        private static HttpClient client = new HttpClient();

        public HackerNewsService()
        {
            cache = MemoryCache.Default;

            if (ConfigurationManager.AppSettings["BestStoriesURI"] == null)
            {
                //hardcode if key is missing
                bestStoriesURI = "https://hacker-news.firebaseio.com/v0/beststories.json";
            }
            else
            {
                //allow for uri changes without rebuilding app.
                bestStoriesURI = ConfigurationManager.AppSettings["BestStoriesURI"];
            }

            if (ConfigurationManager.AppSettings["SingleStoryURI"] == null)
            {
                singleStoryURI = "https://hacker-news.firebaseio.com/v0/item/{0}.json";
            }
            else
            {
                singleStoryURI = ConfigurationManager.AppSettings["SingleStoryURI"];
            }
        }
        
        public async Task<IEnumerable<Story>> GetBestStories()
        {
            var stories = new List<Story>();
            var response = await client.GetStringAsync(bestStoriesURI);
            var result = JsonConvert.DeserializeObject<List<long>>(response);
            foreach (var item in result)
            {                
                stories.Add(await GetStory(item));
            }
            return stories;
        }

        private async Task<Story> GetStory(long id)
        {
            //check if story already in cache
            Story story = GetStoryFromCache(id.ToString());
            
            if (story == null)
            {
                var response = await client.GetStringAsync(string.Format(singleStoryURI, id));
                story = JsonConvert.DeserializeObject<Story>(response);
                AddStoryToCache(story);
            }
            return story;
        }
        
        private void AddStoryToCache(Story story)
        {
            CacheItemPolicy policy = new CacheItemPolicy
            {
                AbsoluteExpiration = new DateTimeOffset(DateTime.Now.AddHours(1)), //cache for 1 hour
                RemovedCallback = null,
                Priority = CacheItemPriority.Default
            };
            cache.Add(story.Id.ToString(), story, policy);
        }

        private Story GetStoryFromCache(string id)
        {
            if (cache[id] == null)
            {
                return null;
            }
            return (Story)cache[id];
        }

    }
}