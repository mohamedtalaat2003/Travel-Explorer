namespace Travel_Explorer.Application.Features.Blogs.Commands.DeleteBlog
{
    public record DeleteBlogCommand(int Id) : IRequest<bool>;
}
