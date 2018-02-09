using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HackerNewsAPI.Models;

namespace HackerNewsAPI.Utilities
{
    interface IHackerNewsService
    {
        Task<IEnumerable<Story>> GetBestStories();
    }
}
