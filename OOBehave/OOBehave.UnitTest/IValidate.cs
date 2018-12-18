namespace OOBehave.UnitTest
{
    public interface IValidate : IValidateBase
    {
        string FirstName { get; set; }
        string LastName { get; set; }
        string ShortName { get; set; }
        string FullName { get; set; }
        string Title { get; set; }
    }
}
