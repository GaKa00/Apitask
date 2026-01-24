namespace Apitask.Models
{
    public class Interest
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;

        public List<PersonInterest> PersonInterests { get; set; } = new();
        public List<Link> Links { get; set; } = new();
    }
}

