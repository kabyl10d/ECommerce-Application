public interface IReviewService
{
    void AddReview(Customer c, Product p, ReviewType reviewtype, string reviewtext);
    void ShowReview(Product p);
    void RemoveReview(Product p, int reviewid);
    void UpdateReview(Product p);

}