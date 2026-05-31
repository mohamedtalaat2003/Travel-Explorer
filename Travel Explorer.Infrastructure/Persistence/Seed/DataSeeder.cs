using Microsoft.AspNetCore.Identity;
using Travel_Explorer.Domain.Entities;
using Travel_Explorer.Domain.Enums;
using Travel_Explorer.Infrastructure.Data;

namespace Travel_Explorer.Infrastructure.Persistence.Seed
{
    public class DataSeeder
    {
        public static async Task SeedDataAsync(ApplicationDbContext context)
        {
            // ===== 1. Seed Categories =====
            if (!context.Categories.Any())
            {
                var categories = new List<Category>
                {
                    new Category { Name = "Beach", Description = "Sun, sand, and sea — tropical beach destinations for ultimate relaxation.", IconUrl = "https://img.icons8.com/fluency/96/beach.png", CreatedAt = DateTime.UtcNow },
                    new Category { Name = "Mountain", Description = "Breathtaking mountain retreats with stunning views and hiking trails.", IconUrl = "https://img.icons8.com/fluency/96/mountain.png", CreatedAt = DateTime.UtcNow },
                    new Category { Name = "Historical", Description = "Explore ancient civilizations, monuments, and world heritage sites.", IconUrl = "https://img.icons8.com/fluency/96/historical.png", CreatedAt = DateTime.UtcNow },
                    new Category { Name = "Adventure", Description = "Thrilling outdoor experiences — desert safaris, diving, and extreme sports.", IconUrl = "https://img.icons8.com/fluency/96/adventure.png", CreatedAt = DateTime.UtcNow },
                    new Category { Name = "City", Description = "Vibrant urban destinations — nightlife, shopping, and cultural attractions.", IconUrl = "https://img.icons8.com/fluency/96/city.png", CreatedAt = DateTime.UtcNow },
                    new Category { Name = "Nature", Description = "Lush forests, national parks, and wildlife sanctuaries.", IconUrl = "https://img.icons8.com/fluency/96/forest.png", CreatedAt = DateTime.UtcNow },
                    new Category { Name = "Island", Description = "Secluded island getaways surrounded by crystal-clear waters.", IconUrl = "https://img.icons8.com/fluency/96/island-on-water.png", CreatedAt = DateTime.UtcNow },
                    new Category { Name = "Desert", Description = "Vast golden dunes, oasis retreats, and stargazing under clear skies.", IconUrl = "https://img.icons8.com/fluency/96/desert.png", CreatedAt = DateTime.UtcNow },
                    new Category { Name = "Cultural", Description = "Immerse yourself in local traditions, art, food, and heritage.", IconUrl = "https://img.icons8.com/fluency/96/museum.png", CreatedAt = DateTime.UtcNow },
                    new Category { Name = "Luxury", Description = "Premium five-star resorts, private villas, and exclusive experiences.", IconUrl = "https://img.icons8.com/fluency/96/5-star-hotel.png", CreatedAt = DateTime.UtcNow }
                };

                await context.Categories.AddRangeAsync(categories);
                await context.SaveChangesAsync();
            }

            // ===== 2. Seed Destinations =====
            if (!context.Destinations.Any())
            {
                var cats = context.Categories.ToList();
                int beach = cats.First(c => c.Name == "Beach").Id;
                int mountain = cats.First(c => c.Name == "Mountain").Id;
                int historical = cats.First(c => c.Name == "Historical").Id;
                int adventure = cats.First(c => c.Name == "Adventure").Id;
                int city = cats.First(c => c.Name == "City").Id;
                int nature = cats.First(c => c.Name == "Nature").Id;
                int island = cats.First(c => c.Name == "Island").Id;
                int desert = cats.First(c => c.Name == "Desert").Id;
                int cultural = cats.First(c => c.Name == "Cultural").Id;
                int luxury = cats.First(c => c.Name == "Luxury").Id;

                var destinations = new List<Destination>
                {
                    // ---- Beach ----
                    new Destination { Name = "Sharm El Sheikh Resort", Description = "A world-class beach resort on the Red Sea coast, offering crystal-clear waters, vibrant coral reefs, and luxury accommodations. Perfect for snorkeling, diving, and relaxation.", Location = "Sharm El Sheikh, South Sinai, Egypt", PricePerNight = 120m, AverageRating = 4.7, ReviewCount = 234, ImageUrls = new List<string>{ "https://images.unsplash.com/photo-1590523741831-ab7e8b8f9c7f?w=800", "https://images.unsplash.com/photo-1507525428034-b723cf961d3e?w=800" }, CategoryId = beach, CreatedAt = DateTime.UtcNow },
                    new Destination { Name = "Maldives Paradise Island", Description = "Overwater bungalows surrounded by turquoise lagoons. Experience world-famous snorkeling spots and pristine white sand beaches.", Location = "Malé, Maldives", PricePerNight = 450m, AverageRating = 4.9, ReviewCount = 512, ImageUrls = new List<string>{ "https://images.unsplash.com/photo-1514282401047-d79a71a590e8?w=800", "https://images.unsplash.com/photo-1573843981267-be1999ff37cd?w=800" }, CategoryId = beach, CreatedAt = DateTime.UtcNow },
                    new Destination { Name = "Hurghada Beach Resort", Description = "A popular Egyptian Red Sea destination known for warm weather, beautiful beaches, and affordable luxury. Great for families and water sports enthusiasts.", Location = "Hurghada, Red Sea, Egypt", PricePerNight = 85m, AverageRating = 4.3, ReviewCount = 189, ImageUrls = new List<string>{ "https://images.unsplash.com/photo-1520250497591-112f2f40a3f4?w=800" }, CategoryId = beach, CreatedAt = DateTime.UtcNow },
                    new Destination { Name = "Cancún Beach Paradise", Description = "Mexico's premier beach destination with turquoise Caribbean waters, ancient Mayan ruins nearby, and world-famous nightlife.", Location = "Cancún, Quintana Roo, Mexico", PricePerNight = 200m, AverageRating = 4.5, ReviewCount = 678, ImageUrls = new List<string>{ "https://images.unsplash.com/photo-1552074284-5e88ef1aef18?w=800" }, CategoryId = beach, CreatedAt = DateTime.UtcNow },
                    new Destination { Name = "Marsa Alam Eco Beach", Description = "An eco-friendly beach destination on Egypt's southern Red Sea coast. Pristine reefs, sea turtles, and dolphins in their natural habitat.", Location = "Marsa Alam, Red Sea, Egypt", PricePerNight = 70m, AverageRating = 4.4, ReviewCount = 112, ImageUrls = new List<string>{ "https://images.unsplash.com/photo-1519046904884-53103b34b206?w=800" }, CategoryId = beach, CreatedAt = DateTime.UtcNow },

                    // ---- Mountain ----
                    new Destination { Name = "Swiss Alps Chalet", Description = "A cozy mountain chalet nestled in the Swiss Alps with panoramic views. World-class skiing in winter and breathtaking hiking in summer.", Location = "Interlaken, Switzerland", PricePerNight = 320m, AverageRating = 4.8, ReviewCount = 178, ImageUrls = new List<string>{ "https://images.unsplash.com/photo-1531366936337-7c912a4589a7?w=800", "https://images.unsplash.com/photo-1476514525535-07fb3b4ae5f1?w=800" }, CategoryId = mountain, CreatedAt = DateTime.UtcNow },
                    new Destination { Name = "Mount Sinai Retreat", Description = "A spiritual experience at the foot of Mount Sinai. Hike to the summit for a legendary sunrise and explore St. Catherine's Monastery.", Location = "Saint Catherine, South Sinai, Egypt", PricePerNight = 65m, AverageRating = 4.5, ReviewCount = 98, ImageUrls = new List<string>{ "https://images.unsplash.com/photo-1464822759023-fed622ff2c3b?w=800" }, CategoryId = mountain, CreatedAt = DateTime.UtcNow },
                    new Destination { Name = "Himalayan Mountain Lodge", Description = "Wake up to the majestic Himalayan peaks. Trekking, meditation retreats, and authentic Nepali cuisine in a serene environment.", Location = "Pokhara, Nepal", PricePerNight = 90m, AverageRating = 4.6, ReviewCount = 145, ImageUrls = new List<string>{ "https://images.unsplash.com/photo-1585409677983-0f6c41ca9c3b?w=800" }, CategoryId = mountain, CreatedAt = DateTime.UtcNow },
                    new Destination { Name = "Aspen Mountain Resort", Description = "America's premier mountain destination for skiing, snowboarding, and luxury après-ski experiences in the heart of Colorado.", Location = "Aspen, Colorado, USA", PricePerNight = 400m, AverageRating = 4.7, ReviewCount = 267, ImageUrls = new List<string>{ "https://images.unsplash.com/photo-1551524559-8af4e6624178?w=800" }, CategoryId = mountain, CreatedAt = DateTime.UtcNow },

                    // ---- Historical ----
                    new Destination { Name = "Pyramids of Giza", Description = "Visit the last remaining wonder of the ancient world. The Great Pyramids and the Sphinx have stood for over 4,500 years.", Location = "Giza, Cairo, Egypt", PricePerNight = 95m, AverageRating = 4.6, ReviewCount = 876, ImageUrls = new List<string>{ "https://images.unsplash.com/photo-1539650116574-8efeb43e2750?w=800", "https://images.unsplash.com/photo-1503177119275-0aa32b3a9368?w=800" }, CategoryId = historical, CreatedAt = DateTime.UtcNow },
                    new Destination { Name = "Luxor Temple & Valley of Kings", Description = "Explore the open-air museum of Luxor — Valley of the Kings, Karnak Temple, and Hatshepsut's Mortuary Temple.", Location = "Luxor, Upper Egypt", PricePerNight = 75m, AverageRating = 4.7, ReviewCount = 543, ImageUrls = new List<string>{ "https://images.unsplash.com/photo-1568322445389-f64b0f5c7a28?w=800" }, CategoryId = historical, CreatedAt = DateTime.UtcNow },
                    new Destination { Name = "Colosseum & Roman Forum", Description = "Step back in time to the heart of the Roman Empire. The Colosseum, Roman Forum, and Palatine Hill await history enthusiasts.", Location = "Rome, Italy", PricePerNight = 180m, AverageRating = 4.8, ReviewCount = 1024, ImageUrls = new List<string>{ "https://images.unsplash.com/photo-1552832230-c0197dd311b5?w=800" }, CategoryId = historical, CreatedAt = DateTime.UtcNow },
                    new Destination { Name = "Petra - The Rose City", Description = "Discover Jordan's ancient Nabataean city carved into rose-red cliffs. A UNESCO World Heritage Site and one of the New Seven Wonders.", Location = "Petra, Ma'an, Jordan", PricePerNight = 110m, AverageRating = 4.9, ReviewCount = 432, ImageUrls = new List<string>{ "https://images.unsplash.com/photo-1579606032821-4e6161c81571?w=800" }, CategoryId = historical, CreatedAt = DateTime.UtcNow },
                    new Destination { Name = "Aswan & Abu Simbel", Description = "Cruise the Nile to Aswan and witness the colossal temples of Abu Simbel, Philae Temple, and the beautiful Nubian villages.", Location = "Aswan, Upper Egypt", PricePerNight = 80m, AverageRating = 4.5, ReviewCount = 312, ImageUrls = new List<string>{ "https://images.unsplash.com/photo-1572252009286-268acec5ca0a?w=800" }, CategoryId = historical, CreatedAt = DateTime.UtcNow },

                    // ---- Adventure ----
                    new Destination { Name = "Sahara Desert Safari Camp", Description = "Experience the Sahara — camel treks at sunset, stargazing in the dunes, and traditional Bedouin campfire nights.", Location = "Siwa Oasis, Western Desert, Egypt", PricePerNight = 55m, AverageRating = 4.4, ReviewCount = 167, ImageUrls = new List<string>{ "https://images.unsplash.com/photo-1509316785289-025f5b846b35?w=800" }, CategoryId = adventure, CreatedAt = DateTime.UtcNow },
                    new Destination { Name = "Queenstown Adventure Base", Description = "The adventure capital of the world! Bungee jumping, skydiving, jet boating, and mountain biking in stunning landscapes.", Location = "Queenstown, New Zealand", PricePerNight = 200m, AverageRating = 4.8, ReviewCount = 321, ImageUrls = new List<string>{ "https://images.unsplash.com/photo-1469521669194-babb45599def?w=800" }, CategoryId = adventure, CreatedAt = DateTime.UtcNow },
                    new Destination { Name = "Costa Rica Rainforest Lodge", Description = "Zip-lining through the canopy, white-water rafting, volcano hiking, and wildlife spotting in one of the most biodiverse places on Earth.", Location = "La Fortuna, Costa Rica", PricePerNight = 160m, AverageRating = 4.6, ReviewCount = 234, ImageUrls = new List<string>{ "https://images.unsplash.com/photo-1518709268805-4e9042af9f23?w=800" }, CategoryId = adventure, CreatedAt = DateTime.UtcNow },

                    // ---- City ----
                    new Destination { Name = "Dubai Marina Hotel", Description = "The glitz of Dubai — towering skyscrapers, luxury shopping, fine dining, and the iconic Burj Khalifa.", Location = "Dubai Marina, Dubai, UAE", PricePerNight = 280m, AverageRating = 4.6, ReviewCount = 445, ImageUrls = new List<string>{ "https://images.unsplash.com/photo-1512453979798-5ea266f8880c?w=800" }, CategoryId = city, CreatedAt = DateTime.UtcNow },
                    new Destination { Name = "Istanbul Old City Stay", Description = "Where East meets West — Blue Mosque, Hagia Sophia, Grand Bazaar, and Bosphorus cruises.", Location = "Sultanahmet, Istanbul, Turkey", PricePerNight = 110m, AverageRating = 4.5, ReviewCount = 398, ImageUrls = new List<string>{ "https://images.unsplash.com/photo-1524231757912-21f4fe3a7200?w=800" }, CategoryId = city, CreatedAt = DateTime.UtcNow },
                    new Destination { Name = "Paris City Center", Description = "The City of Light — Eiffel Tower, Louvre Museum, Champs-Élysées, and world-class French cuisine await you.", Location = "1st Arrondissement, Paris, France", PricePerNight = 250m, AverageRating = 4.7, ReviewCount = 890, ImageUrls = new List<string>{ "https://images.unsplash.com/photo-1502602898657-3e91760cbb34?w=800" }, CategoryId = city, CreatedAt = DateTime.UtcNow },
                    new Destination { Name = "Tokyo Urban Experience", Description = "Ancient temples alongside neon-lit streets. Experience sushi, anime culture, cherry blossoms, and cutting-edge technology.", Location = "Shibuya, Tokyo, Japan", PricePerNight = 180m, AverageRating = 4.8, ReviewCount = 567, ImageUrls = new List<string>{ "https://images.unsplash.com/photo-1540959733332-eab4deabeeaf?w=800" }, CategoryId = city, CreatedAt = DateTime.UtcNow },
                    new Destination { Name = "Barcelona Beach & City", Description = "Gaudí's masterpieces, La Rambla street life, tapas bars, and Mediterranean beaches all in one vibrant city.", Location = "Barcelona, Catalonia, Spain", PricePerNight = 170m, AverageRating = 4.6, ReviewCount = 712, ImageUrls = new List<string>{ "https://images.unsplash.com/photo-1583422409516-2895a77efded?w=800" }, CategoryId = city, CreatedAt = DateTime.UtcNow },

                    // ---- Nature ----
                    new Destination { Name = "Bali Rainforest Retreat", Description = "A serene escape in Bali's lush rainforest. Yoga, rice terrace walks, waterfall excursions, and Balinese spa treatments.", Location = "Ubud, Bali, Indonesia", PricePerNight = 150m, AverageRating = 4.7, ReviewCount = 289, ImageUrls = new List<string>{ "https://images.unsplash.com/photo-1537996194471-e657df975ab4?w=800" }, CategoryId = nature, CreatedAt = DateTime.UtcNow },
                    new Destination { Name = "Amazon Jungle Expedition", Description = "Navigate the Amazon River, spot exotic wildlife, and stay in eco-lodges deep in the world's largest rainforest.", Location = "Manaus, Amazonas, Brazil", PricePerNight = 130m, AverageRating = 4.5, ReviewCount = 156, ImageUrls = new List<string>{ "https://images.unsplash.com/photo-1516026672322-bc52d61a55d5?w=800" }, CategoryId = nature, CreatedAt = DateTime.UtcNow },
                    new Destination { Name = "Norwegian Fjords Cruise", Description = "Sail through spectacular fjords surrounded by towering cliffs, cascading waterfalls, and charming fishing villages.", Location = "Bergen, Norway", PricePerNight = 350m, AverageRating = 4.9, ReviewCount = 198, ImageUrls = new List<string>{ "https://images.unsplash.com/photo-1513519245088-0e12902e35a6?w=800" }, CategoryId = nature, CreatedAt = DateTime.UtcNow },

                    // ---- Island ----
                    new Destination { Name = "Santorini Caldera View", Description = "Iconic white-washed buildings with blue domes overlooking the Aegean Sea. Stunning sunsets and volcanic beaches.", Location = "Oia, Santorini, Greece", PricePerNight = 300m, AverageRating = 4.8, ReviewCount = 634, ImageUrls = new List<string>{ "https://images.unsplash.com/photo-1570077188670-e3a8d69ac5ff?w=800" }, CategoryId = island, CreatedAt = DateTime.UtcNow },
                    new Destination { Name = "Zanzibar Spice Island", Description = "Pristine beaches, historic Stone Town, spice plantations, and incredible seafood on Tanzania's exotic island.", Location = "Zanzibar, Tanzania", PricePerNight = 120m, AverageRating = 4.5, ReviewCount = 187, ImageUrls = new List<string>{ "https://images.unsplash.com/photo-1586500036706-41963de24d8b?w=800" }, CategoryId = island, CreatedAt = DateTime.UtcNow },

                    // ---- Desert ----
                    new Destination { Name = "Wadi Rum Mars Camp", Description = "Sleep under the stars in the Martian-like landscape of Wadi Rum. Jeep tours, rock climbing, and Bedouin culture.", Location = "Wadi Rum, Aqaba, Jordan", PricePerNight = 70m, AverageRating = 4.7, ReviewCount = 278, ImageUrls = new List<string>{ "https://images.unsplash.com/photo-1548820955-ac84e2b61be8?w=800" }, CategoryId = desert, CreatedAt = DateTime.UtcNow },
                    new Destination { Name = "White Desert Egypt", Description = "Surreal chalk-white rock formations in Egypt's Western Desert. Camping, stargazing, and a landscape from another planet.", Location = "Farafra, New Valley, Egypt", PricePerNight = 50m, AverageRating = 4.6, ReviewCount = 89, ImageUrls = new List<string>{ "https://images.unsplash.com/photo-1509316785289-025f5b846b35?w=800" }, CategoryId = desert, CreatedAt = DateTime.UtcNow },

                    // ---- Cultural ----
                    new Destination { Name = "Marrakech Medina Riad", Description = "Get lost in the vibrant souks, taste authentic Moroccan tagine, and relax in a traditional riad with a rooftop terrace.", Location = "Medina, Marrakech, Morocco", PricePerNight = 95m, AverageRating = 4.5, ReviewCount = 356, ImageUrls = new List<string>{ "https://images.unsplash.com/photo-1539020140153-e479b8c22e70?w=800" }, CategoryId = cultural, CreatedAt = DateTime.UtcNow },
                    new Destination { Name = "Kyoto Temple Trail", Description = "Walk through thousands of vermillion torii gates, visit zen gardens, and witness traditional geisha culture.", Location = "Kyoto, Kansai, Japan", PricePerNight = 160m, AverageRating = 4.8, ReviewCount = 423, ImageUrls = new List<string>{ "https://images.unsplash.com/photo-1493976040374-85c8e12f0c0e?w=800" }, CategoryId = cultural, CreatedAt = DateTime.UtcNow },

                    // ---- Luxury ----
                    new Destination { Name = "Burj Al Arab Suite", Description = "The world's most luxurious hotel. Private beach, helicopter transfers, gold-leaf interiors, and underwater restaurant.", Location = "Jumeirah, Dubai, UAE", PricePerNight = 1500m, AverageRating = 4.9, ReviewCount = 156, ImageUrls = new List<string>{ "https://images.unsplash.com/photo-1582719508461-905c673771fd?w=800" }, CategoryId = luxury, CreatedAt = DateTime.UtcNow },
                    new Destination { Name = "Four Seasons Bora Bora", Description = "Ultimate luxury overwater bungalows with glass floors, Mount Otemanu views, and a private lagoon sanctuary.", Location = "Bora Bora, French Polynesia", PricePerNight = 2000m, AverageRating = 5.0, ReviewCount = 89, ImageUrls = new List<string>{ "https://images.unsplash.com/photo-1559628233-100c798642d4?w=800" }, CategoryId = luxury, CreatedAt = DateTime.UtcNow }
                };

                await context.Destinations.AddRangeAsync(destinations);
                await context.SaveChangesAsync();
            }

            // ===== 3. Seed Activities =====
            if (!context.Activities.Any())
            {
                var dests = context.Destinations.ToList();
                var activities = new List<Activity>();

                void AddAct(string destKeyword, string name, string desc, string icon, string img)
                {
                    var d = dests.FirstOrDefault(x => x.Name.Contains(destKeyword));
                    if (d != null) activities.Add(new Activity { Name = name, Description = desc, Icon = icon, ImageUrls = new List<string>{ img }, DestinationId = d.Id, CreatedAt = DateTime.UtcNow });
                }

                // Sharm El Sheikh
                AddAct("Sharm", "Scuba Diving", "Explore vibrant coral reefs of Ras Mohammed and Tiran Island with certified diving instructors.", "🤿", "https://images.unsplash.com/photo-1544551763-46a013bb70d5?w=800");
                AddAct("Sharm", "Snorkeling Tour", "Swim with colorful fish and sea turtles in the crystal-clear waters of Naama Bay.", "🐠", "https://images.unsplash.com/photo-1560275619-4662e36fa65c?w=800");
                AddAct("Sharm", "Quad Biking in the Desert", "Ride through the Sinai desert on a quad bike and visit a Bedouin village for traditional tea.", "🏍️", "https://images.unsplash.com/photo-1558618666-fcd25c85f82e?w=800");
                AddAct("Sharm", "Glass Bottom Boat", "See the underwater world without getting wet! A family-friendly glass bottom boat tour.", "🚤", "https://images.unsplash.com/photo-1544551763-46a013bb70d5?w=800");

                // Maldives
                AddAct("Maldives", "Sunset Dolphin Cruise", "Sail into the sunset and watch dolphins playing in the Indian Ocean.", "🐬", "https://images.unsplash.com/photo-1514282401047-d79a71a590e8?w=800");
                AddAct("Maldives", "Private Island Picnic", "A secluded sandbank picnic with champagne and fresh seafood, just for you.", "🏝️", "https://images.unsplash.com/photo-1573843981267-be1999ff37cd?w=800");
                AddAct("Maldives", "Underwater Dining", "Dine 5 meters below sea level surrounded by vibrant marine life.", "🍽️", "https://images.unsplash.com/photo-1514282401047-d79a71a590e8?w=800");

                // Pyramids
                AddAct("Pyramids", "Camel Ride Around Pyramids", "Ride a camel across the Giza plateau with a panoramic pyramid view.", "🐪", "https://images.unsplash.com/photo-1553913861-c0fddf2619ee?w=800");
                AddAct("Pyramids", "Sound & Light Show", "Watch the Pyramids come alive at night with a spectacular sound and light show.", "🎆", "https://images.unsplash.com/photo-1539650116574-8efeb43e2750?w=800");
                AddAct("Pyramids", "Egyptian Museum Tour", "Explore King Tutankhamun's treasures and 5,000 years of Egyptian civilization.", "🏛️", "https://images.unsplash.com/photo-1539650116574-8efeb43e2750?w=800");

                // Dubai
                AddAct("Dubai Marina", "Burj Khalifa Observation Deck", "Visit the 148th floor of the world's tallest building for a 360° view of Dubai.", "🏙️", "https://images.unsplash.com/photo-1512453979798-5ea266f8880c?w=800");
                AddAct("Dubai Marina", "Desert Safari with BBQ", "Dune bashing, sandboarding, camel rides, and a BBQ dinner under the stars.", "🏜️", "https://images.unsplash.com/photo-1451337516015-6b6e9a44a8a3?w=800");
                AddAct("Dubai Marina", "Dhow Cruise Dinner", "A traditional Arabian dhow cruise along Dubai Marina with a buffet dinner.", "⛵", "https://images.unsplash.com/photo-1512453979798-5ea266f8880c?w=800");
                AddAct("Dubai Marina", "Dubai Mall & Aquarium", "Visit the world's largest mall and its incredible aquarium with 33,000 marine animals.", "🦈", "https://images.unsplash.com/photo-1512453979798-5ea266f8880c?w=800");

                // Bali
                AddAct("Bali", "Rice Terrace Trekking", "Walk through the stunning Tegallalang rice terraces — one of Bali's most iconic landscapes.", "🌾", "https://images.unsplash.com/photo-1537996194471-e657df975ab4?w=800");
                AddAct("Bali", "Waterfall Excursion", "Visit the breathtaking Tegenungan and Sekumpul waterfalls hidden in Bali's jungle.", "💧", "https://images.unsplash.com/photo-1432405972618-c6b0cfba5849?w=800");
                AddAct("Bali", "Monkey Forest Sanctuary", "Walk among hundreds of long-tailed macaques in the sacred Ubud Monkey Forest.", "🐒", "https://images.unsplash.com/photo-1537996194471-e657df975ab4?w=800");

                // Swiss Alps
                AddAct("Swiss Alps", "Paragliding Over Interlaken", "Soar above the Swiss Alps with a tandem paragliding flight over lakes and mountains.", "🪂", "https://images.unsplash.com/photo-1476514525535-07fb3b4ae5f1?w=800");
                AddAct("Swiss Alps", "Jungfraujoch Train", "Take the iconic train ride to the Top of Europe at 3,454 meters altitude.", "🚂", "https://images.unsplash.com/photo-1531366936337-7c912a4589a7?w=800");

                // Luxor
                AddAct("Luxor", "Hot Air Balloon at Sunrise", "Float over the Valley of the Kings at sunrise for a once-in-a-lifetime view.", "🎈", "https://images.unsplash.com/photo-1568322445389-f64b0f5c7a28?w=800");
                AddAct("Luxor", "Felucca Nile Sailing", "Sail the ancient Nile on a traditional Egyptian felucca at sunset.", "⛵", "https://images.unsplash.com/photo-1568322445389-f64b0f5c7a28?w=800");

                // Istanbul
                AddAct("Istanbul", "Bosphorus Cruise", "Cruise between Europe and Asia on a scenic Bosphorus boat tour.", "🚢", "https://images.unsplash.com/photo-1524231757912-21f4fe3a7200?w=800");
                AddAct("Istanbul", "Turkish Bath Experience", "Relax in a centuries-old Ottoman hammam with a full scrub and foam massage.", "🛁", "https://images.unsplash.com/photo-1524231757912-21f4fe3a7200?w=800");

                // Paris
                AddAct("Paris", "Eiffel Tower Summit", "Skip the line and ascend to the summit of the Eiffel Tower for breathtaking views.", "🗼", "https://images.unsplash.com/photo-1502602898657-3e91760cbb34?w=800");
                AddAct("Paris", "Louvre Museum Guided Tour", "Explore the world's largest art museum with an expert guide — see the Mona Lisa and more.", "🎨", "https://images.unsplash.com/photo-1502602898657-3e91760cbb34?w=800");
                AddAct("Paris", "Seine River Dinner Cruise", "A romantic dinner cruise along the Seine with views of illuminated Parisian landmarks.", "🥂", "https://images.unsplash.com/photo-1502602898657-3e91760cbb34?w=800");

                // Tokyo
                AddAct("Tokyo", "Tsukiji Fish Market Tour", "Experience the world's largest fish market and taste the freshest sushi for breakfast.", "🍣", "https://images.unsplash.com/photo-1540959733332-eab4deabeeaf?w=800");
                AddAct("Tokyo", "Shibuya Crossing & Akihabara", "Walk the world's busiest crossing and explore Tokyo's famous electronics and anime district.", "🎮", "https://images.unsplash.com/photo-1540959733332-eab4deabeeaf?w=800");

                // Queenstown
                AddAct("Queenstown", "Bungee Jumping", "Jump from the original AJ Hackett Kawarau Bridge — 43 meters of pure adrenaline!", "🤸", "https://images.unsplash.com/photo-1469521669194-babb45599def?w=800");
                AddAct("Queenstown", "Milford Sound Cruise", "Cruise through the stunning Milford Sound fiord with waterfalls and wildlife.", "🛥️", "https://images.unsplash.com/photo-1469521669194-babb45599def?w=800");

                // Santorini
                AddAct("Santorini", "Caldera Sunset Sail", "Sail along the Santorini caldera and watch the legendary Oia sunset from the water.", "🌅", "https://images.unsplash.com/photo-1570077188670-e3a8d69ac5ff?w=800");
                AddAct("Santorini", "Wine Tasting Tour", "Sample unique volcanic wines from Santorini's centuries-old vineyards.", "🍷", "https://images.unsplash.com/photo-1570077188670-e3a8d69ac5ff?w=800");

                // Petra
                AddAct("Petra", "Petra by Night", "Walk through the Siq to the Treasury illuminated by 1,500 candles — a magical experience.", "🕯️", "https://images.unsplash.com/photo-1579606032821-4e6161c81571?w=800");

                // Marrakech
                AddAct("Marrakech", "Cooking Class", "Learn to make authentic Moroccan tagine, couscous, and mint tea with a local chef.", "👨‍🍳", "https://images.unsplash.com/photo-1539020140153-e479b8c22e70?w=800");
                AddAct("Marrakech", "Souk Shopping Tour", "Navigate the maze-like souks with a local guide to find the best deals on spices and crafts.", "🛍️", "https://images.unsplash.com/photo-1539020140153-e479b8c22e70?w=800");

                if (activities.Any())
                {
                    await context.Activities.AddRangeAsync(activities);
                    await context.SaveChangesAsync();
                }
            }

            // ===== 4. Seed Flight Schedules =====
            if (!context.FlightSchedules.Any())
            {
                var flights = new List<FlightSchedule>
                {
                    new FlightSchedule { Airline = "EgyptAir", FlightNumber = "MS800", DepartureCity = "Cairo", ArrivalCity = "Dubai", DepartureTime = DateTime.UtcNow.AddDays(7).Date.AddHours(8), ArrivalTime = DateTime.UtcNow.AddDays(7).Date.AddHours(12), EconomyPrice = 350m, BusinessPrice = 750m, FirstClassPrice = 1500m, AvailableEconomySeats = 150, AvailableBusinessSeats = 30, AvailableFirstClassSeats = 10, CreatedAt = DateTime.UtcNow },
                    new FlightSchedule { Airline = "Emirates", FlightNumber = "EK924", DepartureCity = "Dubai", ArrivalCity = "Cairo", DepartureTime = DateTime.UtcNow.AddDays(7).Date.AddHours(14), ArrivalTime = DateTime.UtcNow.AddDays(7).Date.AddHours(16).AddMinutes(30), EconomyPrice = 380m, BusinessPrice = 800m, FirstClassPrice = 1600m, AvailableEconomySeats = 200, AvailableBusinessSeats = 40, AvailableFirstClassSeats = 12, CreatedAt = DateTime.UtcNow },
                    new FlightSchedule { Airline = "Turkish Airlines", FlightNumber = "TK694", DepartureCity = "Cairo", ArrivalCity = "Istanbul", DepartureTime = DateTime.UtcNow.AddDays(10).Date.AddHours(6), ArrivalTime = DateTime.UtcNow.AddDays(10).Date.AddHours(9).AddMinutes(45), EconomyPrice = 280m, BusinessPrice = 650m, FirstClassPrice = 1200m, AvailableEconomySeats = 180, AvailableBusinessSeats = 35, AvailableFirstClassSeats = 8, CreatedAt = DateTime.UtcNow },
                    new FlightSchedule { Airline = "EgyptAir", FlightNumber = "MS955", DepartureCity = "Cairo", ArrivalCity = "London", DepartureTime = DateTime.UtcNow.AddDays(14).Date.AddHours(22), ArrivalTime = DateTime.UtcNow.AddDays(15).Date.AddHours(2).AddMinutes(30), EconomyPrice = 520m, BusinessPrice = 1100m, FirstClassPrice = 2200m, AvailableEconomySeats = 160, AvailableBusinessSeats = 25, AvailableFirstClassSeats = 6, CreatedAt = DateTime.UtcNow },
                    new FlightSchedule { Airline = "Nile Air", FlightNumber = "NP420", DepartureCity = "Cairo", ArrivalCity = "Sharm El Sheikh", DepartureTime = DateTime.UtcNow.AddDays(3).Date.AddHours(7), ArrivalTime = DateTime.UtcNow.AddDays(3).Date.AddHours(8), EconomyPrice = 80m, BusinessPrice = 180m, FirstClassPrice = 350m, AvailableEconomySeats = 120, AvailableBusinessSeats = 20, AvailableFirstClassSeats = 0, CreatedAt = DateTime.UtcNow },
                    new FlightSchedule { Airline = "Qatar Airways", FlightNumber = "QR1302", DepartureCity = "Cairo", ArrivalCity = "Doha", DepartureTime = DateTime.UtcNow.AddDays(5).Date.AddHours(10), ArrivalTime = DateTime.UtcNow.AddDays(5).Date.AddHours(13).AddMinutes(15), EconomyPrice = 320m, BusinessPrice = 700m, FirstClassPrice = 1400m, AvailableEconomySeats = 190, AvailableBusinessSeats = 42, AvailableFirstClassSeats = 14, CreatedAt = DateTime.UtcNow },
                    new FlightSchedule { Airline = "EgyptAir", FlightNumber = "MS777", DepartureCity = "Cairo", ArrivalCity = "Luxor", DepartureTime = DateTime.UtcNow.AddDays(2).Date.AddHours(9), ArrivalTime = DateTime.UtcNow.AddDays(2).Date.AddHours(10).AddMinutes(15), EconomyPrice = 65m, BusinessPrice = 150m, FirstClassPrice = 300m, AvailableEconomySeats = 140, AvailableBusinessSeats = 18, AvailableFirstClassSeats = 4, CreatedAt = DateTime.UtcNow },
                    new FlightSchedule { Airline = "Etihad Airways", FlightNumber = "EY654", DepartureCity = "Cairo", ArrivalCity = "Abu Dhabi", DepartureTime = DateTime.UtcNow.AddDays(8).Date.AddHours(15), ArrivalTime = DateTime.UtcNow.AddDays(8).Date.AddHours(20).AddMinutes(30), EconomyPrice = 400m, BusinessPrice = 850m, FirstClassPrice = 1700m, AvailableEconomySeats = 170, AvailableBusinessSeats = 32, AvailableFirstClassSeats = 10, CreatedAt = DateTime.UtcNow },
                    new FlightSchedule { Airline = "Air France", FlightNumber = "AF502", DepartureCity = "Cairo", ArrivalCity = "Paris", DepartureTime = DateTime.UtcNow.AddDays(12).Date.AddHours(11), ArrivalTime = DateTime.UtcNow.AddDays(12).Date.AddHours(15).AddMinutes(45), EconomyPrice = 480m, BusinessPrice = 1050m, FirstClassPrice = 2100m, AvailableEconomySeats = 175, AvailableBusinessSeats = 28, AvailableFirstClassSeats = 8, CreatedAt = DateTime.UtcNow },
                    new FlightSchedule { Airline = "Lufthansa", FlightNumber = "LH581", DepartureCity = "Cairo", ArrivalCity = "Frankfurt", DepartureTime = DateTime.UtcNow.AddDays(9).Date.AddHours(13), ArrivalTime = DateTime.UtcNow.AddDays(9).Date.AddHours(17).AddMinutes(30), EconomyPrice = 460m, BusinessPrice = 980m, FirstClassPrice = 1950m, AvailableEconomySeats = 165, AvailableBusinessSeats = 30, AvailableFirstClassSeats = 7, CreatedAt = DateTime.UtcNow },
                    new FlightSchedule { Airline = "Royal Jordanian", FlightNumber = "RJ502", DepartureCity = "Cairo", ArrivalCity = "Amman", DepartureTime = DateTime.UtcNow.AddDays(4).Date.AddHours(16), ArrivalTime = DateTime.UtcNow.AddDays(4).Date.AddHours(17).AddMinutes(45), EconomyPrice = 180m, BusinessPrice = 420m, FirstClassPrice = 850m, AvailableEconomySeats = 130, AvailableBusinessSeats = 22, AvailableFirstClassSeats = 6, CreatedAt = DateTime.UtcNow },
                    new FlightSchedule { Airline = "Saudia", FlightNumber = "SV312", DepartureCity = "Cairo", ArrivalCity = "Jeddah", DepartureTime = DateTime.UtcNow.AddDays(6).Date.AddHours(5), ArrivalTime = DateTime.UtcNow.AddDays(6).Date.AddHours(7).AddMinutes(30), EconomyPrice = 250m, BusinessPrice = 580m, FirstClassPrice = 1100m, AvailableEconomySeats = 200, AvailableBusinessSeats = 36, AvailableFirstClassSeats = 12, CreatedAt = DateTime.UtcNow },
                    new FlightSchedule { Airline = "EgyptAir", FlightNumber = "MS903", DepartureCity = "Cairo", ArrivalCity = "Hurghada", DepartureTime = DateTime.UtcNow.AddDays(1).Date.AddHours(6).AddMinutes(30), ArrivalTime = DateTime.UtcNow.AddDays(1).Date.AddHours(7).AddMinutes(30), EconomyPrice = 70m, BusinessPrice = 160m, FirstClassPrice = 320m, AvailableEconomySeats = 135, AvailableBusinessSeats = 16, AvailableFirstClassSeats = 0, CreatedAt = DateTime.UtcNow },
                    new FlightSchedule { Airline = "FlyDubai", FlightNumber = "FZ8192", DepartureCity = "Dubai", ArrivalCity = "Sharm El Sheikh", DepartureTime = DateTime.UtcNow.AddDays(11).Date.AddHours(9), ArrivalTime = DateTime.UtcNow.AddDays(11).Date.AddHours(11).AddMinutes(30), EconomyPrice = 200m, BusinessPrice = 450m, FirstClassPrice = 0m, AvailableEconomySeats = 180, AvailableBusinessSeats = 24, AvailableFirstClassSeats = 0, CreatedAt = DateTime.UtcNow },
                    new FlightSchedule { Airline = "British Airways", FlightNumber = "BA155", DepartureCity = "London", ArrivalCity = "Cairo", DepartureTime = DateTime.UtcNow.AddDays(15).Date.AddHours(21), ArrivalTime = DateTime.UtcNow.AddDays(16).Date.AddHours(3).AddMinutes(15), EconomyPrice = 550m, BusinessPrice = 1200m, FirstClassPrice = 2400m, AvailableEconomySeats = 155, AvailableBusinessSeats = 28, AvailableFirstClassSeats = 10, CreatedAt = DateTime.UtcNow }
                };

                await context.FlightSchedules.AddRangeAsync(flights);
                await context.SaveChangesAsync();
            }

            // ===== 5. Seed Contact Messages =====
            if (!context.ContactMessages.Any())
            {
                var messages = new List<ContactMessage>
                {
                    new ContactMessage { FullName = "Sara Ahmed", Email = "sara.ahmed@gmail.com", Subject = "Booking Inquiry", Message = "Hi, I'm interested in booking the Sharm El Sheikh Resort for 5 nights in July. Are there any special offers?", IsRead = false, CreatedAt = DateTime.UtcNow.AddDays(-3) },
                    new ContactMessage { FullName = "Mohamed Hassan", Email = "m.hassan@outlook.com", Subject = "Partnership Opportunity", Message = "We are a travel agency based in Alexandria and we'd like to discuss a partnership opportunity with Travel Explorer.", IsRead = true, CreatedAt = DateTime.UtcNow.AddDays(-7) },
                    new ContactMessage { FullName = "Fatima Al-Zahra", Email = "fatima.z@yahoo.com", Subject = "Payment Issue", Message = "I tried to pay for my flight booking but the payment page keeps timing out. My booking reference is #FL-2024-0892.", IsRead = false, CreatedAt = DateTime.UtcNow.AddDays(-1) },
                    new ContactMessage { FullName = "John Smith", Email = "john.smith@email.com", Subject = "Group Booking", Message = "I want to book for a group of 20 people for a corporate retreat to the Swiss Alps. Can you provide a group discount?", IsRead = false, CreatedAt = DateTime.UtcNow.AddDays(-2) },
                    new ContactMessage { FullName = "Amira Khalil", Email = "amira.k@gmail.com", Subject = "Cancellation Request", Message = "I need to cancel my booking #DB-2024-1456 due to a family emergency. What is your refund policy?", IsRead = true, CreatedAt = DateTime.UtcNow.AddDays(-5) },
                    new ContactMessage { FullName = "Ahmed Mansour", Email = "ahmed.m@live.com", Subject = "Feedback", Message = "I just came back from the Maldives trip booked through your platform. It was absolutely amazing! Thank you so much.", IsRead = true, CreatedAt = DateTime.UtcNow.AddDays(-10) },
                    new ContactMessage { FullName = "Nour El-Din", Email = "nour.eldin@hotmail.com", Subject = "Flight Change Request", Message = "Can I change my flight from MS800 on June 7th to June 10th? Same route Cairo to Dubai.", IsRead = false, CreatedAt = DateTime.UtcNow.AddHours(-12) }
                };

                await context.ContactMessages.AddRangeAsync(messages);
                await context.SaveChangesAsync();
            }
        }

        // ===== 6. Seed Users (2 Admin, 5 Traveler, 3 Author) =====
        public static async Task SeedUsersAsync(UserManager<ApplicationUser> userManager)
        {
            var defaultPassword = "Pass@123";

            // ---------- 2 Admins ----------
            var admins = new List<(string Email, string FullName, Gender Gender)>
            {
                ("admin@travelexplorer.com", "Admin Mohamed", Gender.Male),
                ("admin2@travelexplorer.com", "Admin Sara", Gender.Female)
            };

            foreach (var (email, fullName, gender) in admins)
            {
                if (await userManager.FindByEmailAsync(email) == null)
                {
                    var user = new ApplicationUser
                    {
                        UserName = email,
                        Email = email,
                        FullName = fullName,
                        EmailConfirmed = true,
                        Gender = gender,
                        Role = "Admin",
                        Status = AccountStatus.Approved,
                        requestToBeAuthor = RequestToBeAuthor.Approved,
                        CreatedAt = DateTime.UtcNow
                    };
                    var result = await userManager.CreateAsync(user, defaultPassword);
                    if (result.Succeeded)
                        await userManager.AddToRoleAsync(user, "Admin");
                }
            }

            // ---------- 5 Travelers ----------
            var travelers = new List<(string Email, string FullName, Gender Gender)>
            {
                ("ahmed.traveler@gmail.com", "Ahmed Ali", Gender.Male),
                ("fatma.traveler@gmail.com", "Fatma Ibrahim", Gender.Female),
                ("omar.traveler@gmail.com", "Omar Khaled", Gender.Male),
                ("nada.traveler@gmail.com", "Nada Youssef", Gender.Female),
                ("hassan.traveler@gmail.com", "Hassan Mahmoud", Gender.Male)
            };

            foreach (var (email, fullName, gender) in travelers)
            {
                if (await userManager.FindByEmailAsync(email) == null)
                {
                    var user = new ApplicationUser
                    {
                        UserName = email,
                        Email = email,
                        FullName = fullName,
                        EmailConfirmed = true,
                        Gender = gender,
                        Role = "Traveler",
                        Status = AccountStatus.Approved,
                        requestToBeAuthor = RequestToBeAuthor.Pending,
                        CreatedAt = DateTime.UtcNow
                    };
                    var result = await userManager.CreateAsync(user, defaultPassword);
                    if (result.Succeeded)
                        await userManager.AddToRoleAsync(user, "Traveler");
                }
            }

            // ---------- 3 Authors ----------
            var authors = new List<(string Email, string FullName, Gender Gender)>
            {
                ("mona.author@gmail.com", "Mona Saeed", Gender.Female),
                ("karim.author@gmail.com", "Karim Adel", Gender.Male),
                ("layla.author@gmail.com", "Layla Nabil", Gender.Female)
            };

            foreach (var (email, fullName, gender) in authors)
            {
                if (await userManager.FindByEmailAsync(email) == null)
                {
                    var user = new ApplicationUser
                    {
                        UserName = email,
                        Email = email,
                        FullName = fullName,
                        EmailConfirmed = true,
                        Gender = gender,
                        Role = "Author",
                        Status = AccountStatus.Approved,
                        requestToBeAuthor = RequestToBeAuthor.Approved,
                        CreatedAt = DateTime.UtcNow
                    };
                    var result = await userManager.CreateAsync(user, defaultPassword);
                    if (result.Succeeded)
                        await userManager.AddToRoleAsync(user, "Author");
                }
            }
        }
    }
}
