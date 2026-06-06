using Microsoft.EntityFrameworkCore;
using Travel_Explorer.Application.Common.Parameters;

namespace Travel_Explorer.Application.Features.Destinations
{
    
    
    
    
    
    public class DestinationSpecification : BaseSpecification<Destination>
    {
        
        public DestinationSpecification(int id)
            : base(d => d.Id == id)
        {
            AddInclude(d => d.Category);
        }

        
        
        
        
        public DestinationSpecification(DestinationSpecParams p)
            : base()
        {
            if (!string.IsNullOrWhiteSpace(p.Keyword))
            {
                var pattern = $"%{p.Keyword}%";
                AddCriteria(d => EF.Functions.ILike(d.Name, pattern)
                              || EF.Functions.ILike(d.Description, pattern));
            }

            if (!string.IsNullOrWhiteSpace(p.Location))
                AddCriteria(d => EF.Functions.ILike(d.Location, $"%{p.Location}%"));

            if (p.MinPrice.HasValue)
                AddCriteria(d => d.PricePerNight >= p.MinPrice.Value);

            if (p.MaxPrice.HasValue)
                AddCriteria(d => d.PricePerNight <= p.MaxPrice.Value);

            if (p.CategoryId.HasValue)
                AddCriteria(d => d.CategoryId == p.CategoryId.Value);

            AddInclude(d => d.Category);
            AddOrderBy(d => d.Id);
            ApplyPaging((p.PageNumber - 1) * p.PageSize, p.PageSize);
        }

        
        public DestinationSpecification(int count, bool topRated)
            : base()
        {
            AddInclude(d => d.Category);
            if (topRated)
                AddOrderByDescending(d => d.AverageRating);
            ApplyPaging(0, count);
        }
    }
}
