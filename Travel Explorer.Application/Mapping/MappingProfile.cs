using Travel_Explorer.Application.DTOs.Blogs;
using Travel_Explorer.Application.Features.Activities.Commands.CreateActivity;
using Travel_Explorer.Application.Features.Activities.Commands.UpdateActivity;
using Travel_Explorer.Application.Features.Blogs.Commands.CreateBlog;
using Travel_Explorer.Application.Features.Blogs.Commands.UpdateBlog;
using Travel_Explorer.Application.Features.Categories.Commands.CreateCategory;
using Travel_Explorer.Application.Features.Categories.Commands.UpdateCategory;
using Travel_Explorer.Application.Features.DestinationBookings.Commands.CreateBooking;
using Travel_Explorer.Application.Features.Destinations.Commands.CreateDestination;
using Travel_Explorer.Application.Features.Destinations.Commands.UpdateDestination;
using Travel_Explorer.Application.Features.Reviews.Commands.CreateReview;
using Travel_Explorer.Application.Features.Reviews.Commands.UpdateReview;
using Travel_Explorer.Application.Features.ContactMessages.CreateContactMessage;
using Travel_Explorer.Application.DTOs.ContactMessage;
using Travel_Explorer.Application.DTOs.Profiles;
using Travel_Explorer.Application.Features.Profiles.Commands.UpdateUserProfile;
using Travel_Explorer.Application.DTOs.Flights.Schedules;
using Travel_Explorer.Application.DTOs.Flights.Bookings;
using Travel_Explorer.Application.Features.Flights.Commands.CreateFlightSchedule;
using Travel_Explorer.Application.Features.Flights.Commands.UpdateFlightSchedule;
using Travel_Explorer.Application.Features.FlightBookings.Commands.CreateFlightBooking;

namespace Travel_Explorer.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            
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

            
            CreateMap<DestinationBooking, DestinationBookingDto>()
                .ForMember(dest => dest.UserFullName,
                    opt => opt.MapFrom(src => src.User != null ? src.User.FullName : string.Empty))
                .ForMember(dest => dest.DestinationName,
                    opt => opt.MapFrom(src => src.Destination != null ? src.Destination.Name : string.Empty));

            CreateMap<CreateBookingCommand, DestinationBooking>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));

            
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

            
            CreateMap<Review, ReviewDto>()
                .ForMember(dest => dest.UserFullName,
                    opt => opt.MapFrom(src => src.User != null ? src.User.FullName : string.Empty));

            CreateMap<CreateReviewCommand, Review>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForMember(dest => dest.ImageUrls, opt => opt.MapFrom(src => src.ImageUrls ?? new List<string>()));

            CreateMap<UpdateReviewCommand, Review>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForMember(dest => dest.ImageUrls, opt => opt.MapFrom(src => src.ImageUrls ?? new List<string>()))
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            
            CreateMap<ContactMessage, ContactMessageDto>();
            CreateMap<CreateContactMessageCommand, ContactMessage>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));

            
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

            
            CreateMap<Category, CategoryDto>();
            CreateMap<CreateCategoryCommand, Category>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));
            CreateMap<UpdateCategoryCommand, Category>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));


            CreateMap<Blog, BlogDto>().ReverseMap();

            CreateMap<CreateBlogCommand, Blog>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));
            CreateMap<UpdateBlogCommand, Blog>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            
            CreateMap<FlightSchedule, FlightScheduleDto>();
            CreateMap<FlightScheduleDto, FlightSchedule>();
            CreateMap<CreateFlightScheduleCommand, FlightSchedule>();
            CreateMap<UpdateFlightScheduleCommand, FlightSchedule>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            
            CreateMap<FlightBooking, FlightBookingDto>()
                .ForMember(dest => dest.UserFullName,
                    opt => opt.MapFrom(src => src.User != null ? src.User.FullName : string.Empty))
                .ForMember(dest => dest.Airline,
                    opt => opt.MapFrom(src => src.FlightSchedule != null ? src.FlightSchedule.Airline : string.Empty))
                .ForMember(dest => dest.FlightNumber,
                    opt => opt.MapFrom(src => src.FlightSchedule != null ? src.FlightSchedule.FlightNumber : string.Empty))
                .ForMember(dest => dest.DepartureTime,
                    opt => opt.MapFrom(src => src.FlightSchedule != null ? src.FlightSchedule.DepartureTime : default));

            CreateMap<CreateFlightBookingCommand, FlightBooking>();
               
        }

    }
}
