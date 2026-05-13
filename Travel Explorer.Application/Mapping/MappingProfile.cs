using Travel_Explorer.Application.DTOs.Blogs;
using Travel_Explorer.Application.Features.Activities.Commands.CreateActivity;
using Travel_Explorer.Application.Features.Activities.Commands.UpdateActivity;
using Travel_Explorer.Application.Features.Blogs.Commands.Add;
using Travel_Explorer.Application.Features.Blogs.Commands.Delete;
using Travel_Explorer.Application.Features.Blogs.Commands.Update;
using Travel_Explorer.Application.Features.Categories.Commands.Create;
using Travel_Explorer.Application.Features.Categories.Commands.Update;
using Travel_Explorer.Application.Features.DestinationBookings.Commands.CreateBooking;
using Travel_Explorer.Application.Features.Destinations.Commands.CreateDestination;
using Travel_Explorer.Application.Features.Destinations.Commands.UpdateDestination;
using Travel_Explorer.Application.Features.Reviews.Commands.CreateReview;
using Travel_Explorer.Application.Features.Reviews.Commands.UpdateReview;

namespace Travel_Explorer.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // ─── Destination ────────────────────────────────────────────────
            CreateMap<Destination, DestinationDto>()
                .ForMember(dest => dest.CategoryName,
                    opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : string.Empty));

            CreateMap<CreateDestinationCommand, Destination>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()) 
                .ForMember(dest => dest.AverageRating, opt => opt.Ignore())
                .ForMember(dest => dest.ReviewCount, opt => opt.Ignore())
                .ForMember(dest => dest.Category, opt => opt.Ignore())
                .ForMember(dest => dest.Activities, opt => opt.Ignore())
                .ForMember(dest => dest.Bookings, opt => opt.Ignore())
                .ForMember(dest => dest.Reviews, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));

            CreateMap<UpdateDestinationCommand, Destination>()
                .ForMember(dest => dest.AverageRating, opt => opt.Ignore())
                .ForMember(dest => dest.ReviewCount, opt => opt.Ignore())
                .ForMember(dest => dest.Category, opt => opt.Ignore())
                .ForMember(dest => dest.Activities, opt => opt.Ignore())
                .ForMember(dest => dest.Bookings, opt => opt.Ignore())
                .ForMember(dest => dest.Reviews, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore()) // لا نغير تاريخ الإنشاء
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));

            // ─── DestinationBooking ─────────────────────────────────────────
            CreateMap<DestinationBooking, DestinationBookingDto>()
                .ForMember(dest => dest.UserFullName,
                    opt => opt.MapFrom(src => src.User != null ? src.User.FullName : string.Empty))
                .ForMember(dest => dest.DestinationName,
                    opt => opt.MapFrom(src => src.Destination != null ? src.Destination.Name : string.Empty));

            CreateMap<CreateBookingCommand, DestinationBooking>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.TotalPrice, opt => opt.Ignore()) 
                .ForMember(dest => dest.Status, opt => opt.Ignore()) 
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.Destination, opt => opt.Ignore());

            // ─── Activity ───────────────────────────────────────────────────
            CreateMap<Activity, ActivityDto>()
                .ForMember(dest => dest.DestinationName,
                    opt => opt.MapFrom(src => src.Destination != null ? src.Destination.Name : string.Empty));

            CreateMap<CreateActivityCommand, Activity>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Destination, opt => opt.Ignore());

            CreateMap<UpdateActivityCommand, Activity>()
                .ForMember(dest => dest.Destination, opt => opt.Ignore());

            // ─── Review ─────────────────────────────────────────────────────

           
            CreateMap<Review, ReviewDto>()
                .ForMember(dest => dest.UserFullName,
                    opt => opt.MapFrom(src => src.User != null ? src.User.FullName : string.Empty));

           
            CreateMap<CreateReviewCommand, Review>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.Destination, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));

            
            CreateMap<UpdateReviewCommand, Review>()
                .ForMember(dest => dest.UserId, opt => opt.Ignore()) 
                .ForMember(dest => dest.DestinationId, opt => opt.Ignore()) 
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.Destination, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));

            CreateMap<CreateBlogCommand, Blog>().ReverseMap();
            CreateMap<UpdateBlogCommand, Blog>().ReverseMap();
            CreateMap<BlogDto, Blog>().ReverseMap();
            CreateMap<CreateBlogDto, Blog>().ReverseMap();
            CreateMap<UpdateBlogDto, Blog>().ReverseMap();

            CreateMap<CreateCategoryCommand, Category>().ReverseMap();
            CreateMap<UpdateCategoryCommand, Category>().ReverseMap();
            CreateMap<CategoryDto, Category>().ReverseMap();

               
        }

    }
}
