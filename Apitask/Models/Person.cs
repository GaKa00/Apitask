namespace Apitask.Models
{
    public class Person
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;

        public List<PersonInterest> PersonInterests { get; set; } = new();
        public List<Link> InterestLinks { get; set; } = new();
    }
}
