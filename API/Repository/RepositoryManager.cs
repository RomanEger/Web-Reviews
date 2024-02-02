using Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly WebReviewsContext _webReviewsContext;

        public RepositoryManager(WebReviewsContext webReviewsContext)
        {
            _webReviewsContext = webReviewsContext;
        }

        public async Task SaveAsync() =>        
            await _webReviewsContext.SaveChangesAsync();
    }
}
