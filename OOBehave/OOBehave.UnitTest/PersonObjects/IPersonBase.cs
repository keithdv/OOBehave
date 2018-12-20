﻿namespace OOBehave.UnitTest.PersonObjects
{
    public interface IPersonBase
    {
        string FirstName { get; set; }
        string FullName { get; set; }
        string LastName { get; set; }
        string ShortName { get; set; }
        string Title { get; set; }
    }
}