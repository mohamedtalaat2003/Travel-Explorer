//using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.DependencyInjection;
//using Travel_Explorer.Domain.Entities;
//using Travel_Explorer.Domain.Enums;
//using Travel_Explorer.Infrastructure.Data;

//namespace Travel_Explorer.Infrastructure.Persistence.Seed
//{
    
    
    
    
    
    
//    public static class DataSeeder
//    {
//        private static string Img(string photoId) =>
//            $"https://images.unsplash.com/{photoId}?auto=format&fit=crop&w=1400&q=80";

//        private record ActivitySeed(string Name, string Icon, string Description);

//        private record DestinationSeed(
//            string Name,
//            string City,
//            string Country,
//            string Category,
//            decimal Price,
//            string PhotoId,
//            string Description,
//            ActivitySeed[] Activities);

//        private static readonly DestinationSeed[] Destinations =
//        [
//            new("Santorini Sunset", "Oia", "Greece", "Island", 260m, "photo-1761755889797-5e709dc9ce6e",
//                "Whitewashed houses tumble down volcanic cliffs, framing the Aegean's most iconic sunsets. Savor fresh seafood by the caldera and explore hidden beaches of black sand.",
//                [
//                    new("Sunset caldera cruise", "Ship", "Sail beneath the cliffs as the sky turns to fire."),
//                    new("Vineyard wine tasting", "Wine", "Sample volcanic-soil wines unique to the island."),
//                    new("Red Beach swim", "Waves", "Snorkel the dramatic red volcanic shoreline."),
//                ]),
//            new("Machu Picchu", "Aguas Calientes", "Peru", "Historical", 185m, "photo-1747607471502-5312316792c5",
//                "Perched high in the Andes, the ancient Inca citadel greets the dawn with mist-kissed stone terraces. Walk the legendary trail and feel the awe of a lost empire.",
//                [
//                    new("Sunrise citadel trek", "Mountain", "Reach the ruins as first light spills over the peaks."),
//                    new("Temple of the Sun tour", "Landmark", "Explore the astronomy-aligned sacred sanctuary."),
//                    new("Sun Gate hike", "Footprints", "Follow the final stretch of the classic Inca Trail."),
//                ]),
//            new("Great Barrier Reef", "Cairns", "Australia", "Beach", 310m, "photo-1638580591001-8c07e6f54097",
//                "The world's largest coral kingdom bursts with color beneath crystal-clear water. Snorkel and dive among vibrant marine life, then unwind in a tropical resort.",
//                [
//                    new("Reef snorkeling", "Waves", "Drift over living coral gardens teeming with fish."),
//                    new("Scuba diving", "Anchor", "Descend to explore the reef's deeper wonders."),
//                    new("Sunset reef dinner", "UtensilsCrossed", "Dine on the water as the reef glows at dusk."),
//                ]),
//            new("Sahara Dunes", "Merzouga", "Morocco", "Desert", 150m, "photo-1706804198725-fa51050a1047",
//                "Golden dunes stretch endlessly, a playground for explorers by day and a theater of stars by night. Trade city noise for the hush of the desert and a fireside sky.",
//                [
//                    new("Camel caravan trek", "Compass", "Cross the dunes on camelback at golden hour."),
//                    new("4x4 dune bashing", "Car", "Race over towering sand ridges in a 4x4."),
//                    new("Desert stargazing camp", "Stars", "Sleep under a blanket of Saharan stars."),
//                ]),
//            new("Kyoto Temples", "Kyoto", "Japan", "Historical", 210m, "photo-1746059256049-3610f0c2e1a0",
//                "Ancient temples and tea houses are framed by clouds of cherry blossom in spring. Soak in centuries of culture along the lantern-lit lanes of old Japan.",
//                [
//                    new("Golden Pavilion visit", "Landmark", "Marvel at Kinkaku-ji mirrored on its pond."),
//                    new("Tea ceremony", "Coffee", "Take part in a traditional matcha ritual."),
//                    new("Cherry blossom walk", "Flower", "Stroll the Philosopher's Path in full bloom."),
//                ]),
//            new("Patagonia Glaciers", "El Calafate", "Argentina", "Mountain", 225m, "photo-1561990975-380e6ab97f62",
//                "Towering walls of blue ice dominate the Patagonian skyline at Perito Moreno. Hike the glacier's edge and cruise among drifting icebergs in pristine silence.",
//                [
//                    new("Glacier trekking", "Mountain", "Strap on crampons and walk on ancient ice."),
//                    new("Iceberg boat tour", "Ship", "Cruise the glacial lake past floating blue ice."),
//                    new("Wildlife safari", "Binoculars", "Spot condors and guanacos in the national park."),
//                ]),
//            new("New York City", "New York", "USA", "City", 350m, "photo-1518235506717-e1ed3306a89b",
//                "The city that never sleeps dazzles with soaring skyscrapers and world-class culture. From Broadway lights to Central Park's calm, every block hums with energy.",
//                [
//                    new("Broadway show", "Drama", "Catch a world-class performance in the theater district."),
//                    new("Liberty Island ferry", "Ship", "Sail past the Statue of Liberty and the harbor."),
//                    new("Central Park bike ride", "Bike", "Pedal through the city's green heart."),
//                ]),
//            new("Table Mountain", "Cape Town", "South Africa", "Mountain", 190m, "photo-1759440038373-5c8a325c5c01",
//                "A flat-topped giant watches over a vibrant coastal city and two oceans. Pair mountaintop panoramas with world-renowned vineyards just beyond town.",
//                [
//                    new("Cableway to the summit", "MountainSnow", "Ride to the top for sweeping ocean views."),
//                    new("Platteklip Gorge hike", "Footprints", "Climb the classic trail straight up the face."),
//                    new("Winelands tasting", "Wine", "Sip award-winning wines in nearby Stellenbosch."),
//                ]),
//            new("Bali Paradise", "Ubud", "Indonesia", "Island", 130m, "photo-1559628233-eb1b1a45564b",
//                "Lush rice terraces cascade down volcanic hills while temples whisper ancient myths. Balance serene spirituality with surf-ready beaches and jungle hideaways.",
//                [
//                    new("Monkey Forest visit", "Trees", "Wander a sacred sanctuary alive with macaques."),
//                    new("Rice terrace trek", "Sprout", "Hike the emerald steps of Tegallalang."),
//                    new("Beach surf lesson", "Waves", "Catch your first wave on Bali's golden coast."),
//                ]),
//            new("Paris Romance", "Paris", "France", "City", 420m, "photo-1694716021376-d44da3f54e8a",
//                "Elegant boulevards and iconic landmarks set a timeless stage for love-filled wandering. From candlelit cafés to glittering river cruises, the city exudes romance.",
//                [
//                    new("Eiffel Tower summit", "Landmark", "Ascend for the city's most famous panorama."),
//                    new("Seine dinner cruise", "Ship", "Glide past lit monuments over a French dinner."),
//                    new("Louvre Museum", "Palette", "Stand before the world's most celebrated art."),
//                ]),
//            new("Dubai Luxury", "Dubai", "UAE", "Desert", 380m, "photo-1746731341047-76b2652ea843",
//                "Sleek skyscrapers rise beside endless dunes, blending ultramodern opulence with desert adventure. Soar over the sand at dawn, then shop beneath the world's tallest tower.",
//                [
//                    new("Desert safari", "Car", "Dune-bash and dine under the open desert sky."),
//                    new("Burj Khalifa deck", "Building2", "Take in the skyline from the world's tallest tower."),
//                    new("Dhow creek cruise", "Ship", "Sail an illuminated traditional boat at night."),
//                ]),
//            new("Rio de Janeiro", "Rio de Janeiro", "Brazil", "Beach", 260m, "photo-1649405409422-0f6128419bf9",
//                "Sun-kissed Copacabana meets the rhythm of samba in a city famed for its festivals. Sugarloaf Mountain frames the bay for unforgettable beachside days.",
//                [
//                    new("Copacabana beach day", "Waves", "Relax on the world's most famous beach."),
//                    new("Sugarloaf cable car", "MountainSnow", "Ride to the summit for breathtaking bay views."),
//                    new("Live samba night", "Music", "Feel the city's heartbeat at a samba show."),
//                ]),
//        ];

//        private static readonly (int Rating, string Text)[] ReviewPool =
//        [
//            (5, "Absolutely unforgettable. Every detail went beyond our expectations."),
//            (5, "One of the best trips of my life — I'd book it again tomorrow."),
//            (4, "Stunning views and warm service. A couple of small hiccups, still worth every penny."),
//            (5, "Booked in minutes and everything went perfectly. Highly recommend."),
//            (4, "Beautiful place and friendly people. We'll definitely be back."),
//            (5, "Photos don't do it justice — magical from start to finish."),
//            (4, "Great value for such an incredible experience."),
//            (5, "The kind of place you keep talking about long after you're home."),
//        ];

//        public static async Task SeedAsync(IServiceProvider services)
//        {
//            var db = services.GetRequiredService<ApplicationDbContext>();
//            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

            
//            if (await db.Destinations.AnyAsync())
//                return;

//            var author = await EnsureUserAsync(userManager, "maya.lawson", "maya@travelexplorer.com",
//                "Maya Lawson", "Author", RequestToBeAuthor.Approved);

//            var travelerSeeds = new[]
//            {
//                ("liam.carter", "liam@example.com", "Liam Carter"),
//                ("ava.rossi", "ava@example.com", "Ava Rossi"),
//                ("noah.kim", "noah@example.com", "Noah Kim"),
//                ("emma.silva", "emma@example.com", "Emma Silva"),
//                ("omar.hassan", "omar@example.com", "Omar Hassan"),
//                ("yuki.tanaka", "yuki@example.com", "Yuki Tanaka"),
//            };
//            var travelers = new List<ApplicationUser>();
//            foreach (var (userName, email, fullName) in travelerSeeds)
//                travelers.Add(await EnsureUserAsync(userManager, userName, email, fullName, "Traveler", RequestToBeAuthor.Pending));

//            var now = DateTime.UtcNow;

            
//            var categoryNames = new (string Name, string Description)[]
//            {
//                ("Beach", "Sun, sand, and crystal-clear water."),
//                ("Island", "Escapes wrapped in sea and serenity."),
//                ("Mountain", "Peaks, trails, and crisp mountain air."),
//                ("City", "Skylines, culture, and nonstop energy."),
//                ("Historical", "Ancient wonders and timeless heritage."),
//                ("Desert", "Golden dunes and endless horizons."),
//                ("Adventure", "Adrenaline and the great outdoors."),
//            };
//            var categories = categoryNames
//                .Select(c => new Category { Name = c.Name, Description = c.Description, CreatedAt = now })
//                .ToList();
//            db.Categories.AddRange(categories);
//            await db.SaveChangesAsync();
//            var categoryId = categories.ToDictionary(c => c.Name, c => c.Id);

            
//            var destinations = Destinations
//                .Select(d => new Destination
//                {
//                    Name = d.Name,
//                    Description = d.Description,
//                    Location = $"{d.City}, {d.Country}",
//                    PricePerNight = d.Price,
//                    CategoryId = categoryId[d.Category],
//                    ImageUrls = [Img(d.PhotoId)],
//                    CreatedAt = now,
//                })
//                .ToList();
//            db.Destinations.AddRange(destinations);
//            await db.SaveChangesAsync();

            
//            var activities = new List<Activity>();
//            var reviews = new List<Review>();
//            var random = new Random(42);

//            for (var i = 0; i < Destinations.Length; i++)
//            {
//                var seed = Destinations[i];
//                var dest = destinations[i];

//                foreach (var act in seed.Activities)
//                {
//                    activities.Add(new Activity
//                    {
//                        Name = act.Name,
//                        Description = act.Description,
//                        Icon = act.Icon,
//                        ImageUrls = [Img(seed.PhotoId)],
//                        DestinationId = dest.Id,
//                        CreatedAt = now,
//                    });
//                }

//                var reviewCount = 4 + (i % 3); 
//                var ratings = new List<int>();
//                for (var r = 0; r < reviewCount; r++)
//                {
//                    var pick = ReviewPool[(i + r) % ReviewPool.Length];
//                    ratings.Add(pick.Rating);
//                    reviews.Add(new Review
//                    {
//                        Rating = pick.Rating,
//                        Comment = pick.Text,
//                        UserId = travelers[(i + r) % travelers.Count].Id,
//                        DestinationId = dest.Id,
//                        CreatedAt = now.AddDays(-random.Next(1, 90)),
//                    });
//                }

//                dest.ReviewCount = reviewCount;
//                dest.AverageRating = Math.Round(ratings.Average(), 1);
//            }

//            db.Activities.AddRange(activities);
//            db.Reviews.AddRange(reviews);
//            db.Destinations.UpdateRange(destinations);
//            await db.SaveChangesAsync();

            
//            var flights = new List<FlightSchedule>
//            {
//                NewFlight("EgyptAir", "MS801", "Cairo", "Paris", now.AddDays(6).AddHours(2), 5, 240m, 720m, 1450m),
//                NewFlight("Emirates", "EK231", "Dubai", "New York", now.AddDays(8).AddHours(3), 14, 690m, 2100m, 4200m),
//                NewFlight("Qatar Airways", "QR920", "Doha", "Tokyo", now.AddDays(9).AddHours(1), 10, 540m, 1650m, 3300m),
//                NewFlight("Turkish Airlines", "TK45", "Istanbul", "Cape Town", now.AddDays(11).AddHours(4), 11, 470m, 1400m, 2800m),
//                NewFlight("Singapore Airlines", "SQ22", "Singapore", "New York", now.AddDays(13).AddHours(6), 18, 980m, 3200m, 6400m),
//                NewFlight("British Airways", "BA247", "London", "Rio de Janeiro", now.AddDays(15).AddHours(5), 12, 610m, 1850m, 3700m),
//                NewFlight("Air France", "AF459", "Paris", "Santiago", now.AddDays(17).AddHours(7), 14, 720m, 2150m, 4300m),
//                NewFlight("Qantas", "QF50", "Sydney", "Cairns", now.AddDays(5).AddHours(1), 3, 180m, 540m, 1080m),
//                NewFlight("LATAM", "LA2470", "Lima", "Cusco", now.AddDays(7).AddHours(1), 1, 130m, 390m, 780m),
//                NewFlight("Garuda Indonesia", "GA407", "Jakarta", "Denpasar", now.AddDays(10).AddHours(2), 2, 110m, 330m, 660m),
//            };
//            db.FlightSchedules.AddRange(flights);

            
//            var blogs = new List<Blog>
//            {
//                NewBlog(author.Id, categoryId["Island"], "10 Ways to Fall for Santorini Beyond the Sunset",
//                    "photo-1761755889797-5e709dc9ce6e", now.AddDays(-4),
//                    "Everyone comes to Oia for the sunset, and yes, it deserves the hype. But the island rewards travelers who linger.\n\nStart your mornings early, before the cruise crowds arrive, and wander the cliff paths with a coffee in hand. Spend an afternoon in Pyrgos, the quiet old capital, where blue-domed churches outnumber tourists. Then trade the postcard beaches for the volcanic black sand of Perissa.\n\nThe secret to Santorini isn't a single view — it's the slow rhythm of a place that has perfected the art of doing very little, beautifully."),
//                NewBlog(author.Id, categoryId["Historical"], "A First-Timer's Guide to Machu Picchu",
//                    "photo-1747607471502-5312316792c5", now.AddDays(-8),
//                    "Few places live up to their photographs. Machu Picchu somehow exceeds them.\n\nGo for the first entry slot and you may have the terraces nearly to yourself as the mist lifts. Acclimatize in the Sacred Valley for a day or two before you climb — the altitude is no joke. Hire a guide for at least the first hour; the stories turn piles of stone into a living city.\n\nBring layers, water, and patience. The Andes set the pace, and the reward is a morning you will never forget."),
//                NewBlog(author.Id, categoryId["Desert"], "How to Pack for a Night in the Sahara",
//                    "photo-1706804198725-fa51050a1047", now.AddDays(-12),
//                    "The desert is a study in extremes: scorching by day, surprisingly cold by night.\n\nPack light, breathable layers and a warm jacket for the evening. A scarf does double duty against sun and blowing sand. Closed shoes beat sandals once the dunes cool. Don't forget a power bank — there are no outlets under the stars.\n\nWhat you won't need is your phone. Once the campfire dies down and the Milky Way appears, you'll understand why people cross the world for a single Saharan night."),
//                NewBlog(author.Id, categoryId["Historical"], "Kyoto in Cherry Blossom Season",
//                    "photo-1746059256049-3610f0c2e1a0", now.AddDays(-16),
//                    "For two short weeks each spring, Kyoto turns pink and the whole city seems to exhale.\n\nBeat the crowds with a dawn visit to Fushimi Inari, then drift along the Philosopher's Path as petals fall into the canal. Reserve a tea ceremony in advance — it's the calmest hour you'll spend all trip.\n\nSeason is everything here. Book early, watch the bloom forecasts, and build your days around mornings. Kyoto rewards the patient and the punctual."),
//                NewBlog(author.Id, categoryId["Island"], "Why Bali Belongs on Your Next Itinerary",
//                    "photo-1559628233-eb1b1a45564b", now.AddDays(-21),
//                    "Bali manages to be many trips at once: a surf town, a jungle retreat, a temple pilgrimage, a wellness escape.\n\nBase yourself in Ubud for rice terraces and craft markets, then swap to the coast for sunsets and waves. Rent a scooter only if you're confident; otherwise drivers are cheap and full of recommendations.\n\nGo with a loose plan. The island has a way of rearranging your priorities toward slower mornings and longer dinners."),
//                NewBlog(author.Id, categoryId["City"], "48 Hours in Paris: A Local-Approved Itinerary",
//                    "photo-1694716021376-d44da3f54e8a", now.AddDays(-27),
//                    "Two days in Paris is never enough, but it's enough to fall in love.\n\nStart in the Marais with pastries and small museums, then cross to the Left Bank for booksellers and the Luxembourg Gardens. Save the Eiffel Tower for the evening, when it sparkles on the hour. Day two belongs to the Louvre — go early — and a slow Seine cruise to end.\n\nWalk more than you think you should. In Paris, the streets between the landmarks are the real attraction."),
//            };
//            db.Blogs.AddRange(blogs);

//            await db.SaveChangesAsync();
//        }

//        private static FlightSchedule NewFlight(
//            string airline, string flightNo, string from, string to,
//            DateTime departure, int durationHours, decimal economy, decimal business, decimal first) =>
//            new()
//            {
//                Airline = airline,
//                FlightNumber = flightNo,
//                DepartureCity = from,
//                ArrivalCity = to,
//                DepartureTime = departure,
//                ArrivalTime = departure.AddHours(durationHours),
//                EconomyPrice = economy,
//                BusinessPrice = business,
//                FirstClassPrice = first,
//                AvailableEconomySeats = 120,
//                AvailableBusinessSeats = 24,
//                AvailableFirstClassSeats = 8,
//                CreatedAt = DateTime.UtcNow,
//            };

//        private static Blog NewBlog(int authorId, int categoryId, string title, string photoId, DateTime createdAt, string content) =>
//            new()
//            {
//                Title = title,
//                Content = content,
//                ImageUrl = Img(photoId),
//                IsPublished = true,
//                AuthorId = authorId,
//                CategoryId = categoryId,
//                CreatedAt = createdAt,
//            };

//        private static async Task<ApplicationUser> EnsureUserAsync(
//            UserManager<ApplicationUser> userManager,
//            string userName, string email, string fullName, string role, RequestToBeAuthor authorStatus)
//        {
//            var existing = await userManager.FindByNameAsync(userName);
//            if (existing is not null)
//                return existing;

//            var user = new ApplicationUser
//            {
//                UserName = userName,
//                Email = email,
//                FullName = fullName,
//                Role = role,
//                Status = AccountStatus.Approved,
//                requestToBeAuthor = authorStatus,
//                EmailConfirmed = true,
//                CreatedAt = DateTime.UtcNow,
//            };

//            var result = await userManager.CreateAsync(user, "Password@123");
//            if (result.Succeeded)
//                await userManager.AddToRoleAsync(user, role);

//            return user;
//        }
//    }
//}
