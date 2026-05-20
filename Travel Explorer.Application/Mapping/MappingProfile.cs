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
using Travel_Explorer.Application.Features.ContactMessages.CreateContactMessage;
using Travel_Explorer.Application.DTOs.ContactMessage;
using Travel_Explorer.Application.DTOs.Profiles;
using Travel_Explorer.Application.Features.Profiles.Commands.UpdateUserProfile;
using Travel_Explorer.Application.DTOs.Users;

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
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));

            CreateMap<UpdateDestinationCommand, Destination>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // ─── DestinationBooking ─────────────────────────────────────────
            CreateMap<DestinationBooking, DestinationBookingDto>()
                .ForMember(dest => dest.UserFullName,
                    opt => opt.MapFrom(src => src.User != null ? src.User.FullName : string.Empty))
                .ForMember(dest => dest.DestinationName,
                    opt => opt.MapFrom(src => src.Destination != null ? src.Destination.Name : string.Empty));

            CreateMap<CreateBookingCommand, DestinationBooking>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));

            // ─── Activity ───────────────────────────────────────────────────
            CreateMap<Activity, ActivityDto>()
                .ForMember(dest => dest.DestinationName,
                    opt => opt.MapFrom(src => src.Destination != null ? src.Destination.Name : string.Empty));

            CreateMap<CreateActivityCommand, Activity>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));

            CreateMap<UpdateActivityCommand, Activity>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // ─── Review ─────────────────────────────────────────────────────
            CreateMap<Review, ReviewDto>()
                .ForMember(dest => dest.UserFullName,
                    opt => opt.MapFrom(src => src.User != null ? src.User.FullName : string.Empty));

            CreateMap<CreateReviewCommand, Review>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));

            CreateMap<UpdateReviewCommand, Review>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // ─── ContactMessage ─────────────────────────────────────────────
            CreateMap<ContactMessage, ContactMessageDto>();
            CreateMap<CreateContactMessageCommand, ContactMessage>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));

            // ─── UserProfile ────────────────────────────────────────────────
            CreateMap<UserProfile, UserProfileDto>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.User.FullName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.User.PhoneNumber));

            CreateMap<UpdateUserProfileCommand, UserProfile>()
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => src))
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<UpdateUserProfileCommand, ApplicationUser>()
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<RegisterDto, ApplicationUser>().ReverseMap();
            CreateMap<LoginDto, ApplicationUser>().ReverseMap();
        }

    }
}
