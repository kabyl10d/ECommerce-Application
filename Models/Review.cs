public class Review
{
    public int reviewId;
    public Customer c;

    public string productname;

    public string ReviewText;

    public ReviewType reviewtype;
    
    public Review(int rid,Customer cname,string pname,ReviewType rtype,string rtext)
    {
        this.reviewId = rid;
        this.c = cname;
        this.productname = pname;
        this.reviewtype = rtype;
        this.ReviewText = rtext;
    }

}