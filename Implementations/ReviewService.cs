

using System.Diagnostics.Metrics;

public class ReviewService : IReviewService
{
    private static void WriteCentered(string text)
    {
        int windowWidth = Console.WindowWidth;
        int textLength = text.Length;
        int spaces = (windowWidth - textLength) / 2;
        Console.WriteLine(new string(' ', spaces) + text);
    }
    private static string ReadCentered(string prompt)
    {
        int windowWidth = Console.WindowWidth;
        int textLength = prompt.Length;
        int spaces = (windowWidth - textLength) / 2;
        Console.Write(new string(' ', spaces) + prompt);
        return Console.ReadLine();
    }
    public void AddReview(Customer c, Product p, ReviewType reviewtype, string reviewtext)
    {
        int rid = p.Reviews.Count + 1;

        Review review = new Review(rid,c, p.Name, reviewtype, reviewtext);

        p.Reviews.Add(review);

        WriteCentered($"Review for {p.Name} Added Successfully");
    }

    public void ShowReview(Product p)
    {
        if (p.Reviews.Count > 0)
        {
            foreach (var review in p.Reviews)
            {
                //Console.WriteLine($"Review ID: {review.reviewId}");
                WriteCentered($"Review By: {review.c.Username}");
                WriteCentered($"{review.reviewtype} ({(int)review.reviewtype}/5)");
                WriteCentered($"\n{review.ReviewText}");
                WriteCentered("");
            }
        }
        else
        {
            WriteCentered("No reviews found.");
        }
    }

    public void RemoveReview(Product p, int reviewid)
    {
        p.Reviews.RemoveAt(reviewid);
        WriteCentered("Review Removed Successfully");
    }

    public void UpdateReview(Product p)
    {
        ShowReview(p);
        //Console.Write(");
        int reviewid = Convert.ToInt32(ReadCentered("\nEnter the review id you want to update: "));
        Review review = p.Reviews.Find(r => r.reviewId == reviewid);

        //Console.Write("Select review type :\n1. Critical\n2. NotBad\n3. Good\n4. VeryGood\n5. Excellent\nEnter choice : ");

        WriteCentered("Selected review type : ");
        foreach (var r in Enum.GetValues(typeof(ReviewType)))
        {
            WriteCentered($"{(int)r}. {r}");
        }
        int rt = Convert.ToInt32(ReadCentered("Enter choice: "));

        string reviewtext = ReadCentered("Enter review text: ");

        review.reviewtype = (ReviewType)rt;
        review.ReviewText = reviewtext;

    }
}