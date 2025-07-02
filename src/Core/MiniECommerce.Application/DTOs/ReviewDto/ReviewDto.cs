using MiniECommerceApp.Domain.Entities;

public class ReviewDto
{
    public Guid Id { get; set; }
    public string Comment { get; set; } = null!;


    public ReviewDto(Review review)
    {
        Id = review.Id;
        Comment = review.Comment;

    }
}
