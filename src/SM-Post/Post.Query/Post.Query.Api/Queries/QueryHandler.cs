using Post.Query.Domain.Entities;
using Post.Query.Domain.Repositories;

namespace Post.Query.Api.Queries
{
    public class QueryHandler : IQueryHandler
    {
        private readonly IPostRepository _postRepository;

        public QueryHandler(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }
        public async Task<List<PostEntity>> HandleAsync(FindAllPostQuery query)
        {
            return await _postRepository.ListAllAsync();
        }

        public Task<List<PostEntity>> HandleAsync(FindPostByAuthorQuery query)
        {
            var posts = _postRepository.ListByAuthorAsync(query.Author);

            return posts;
        }

        public async Task<List<PostEntity>> HandleAsync(FindPostByIdQuery query)
        {
            var posts = await _postRepository.GetByIdAsync(query.Id);

            return new List<PostEntity> { posts };
        }

        public async Task<List<PostEntity>> HandleAsync(FindPostWithCommentQuery query)
        {
            return await _postRepository.ListWithCommentsAsysnc();
        }

        public async Task<List<PostEntity>> HandleAsync(FindPostWithLikesQuery query)
        {
            return await _postRepository.ListWithLikesAsysnc(query.NumberOfLikes);
        }
    }
}
