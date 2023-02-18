namespace ptmk_task.Models;

public class User
{
    public User(string name, DateTime dateOfBirth, int gender)
    {
        Name = name;
        DateOfBirth = dateOfBirth;
        Gender = gender;
    }
    public int Id { get; set; }
    public string? Name { get; set; }
    public DateTime DateOfBirth { get; set; }
    public int Gender { get; set; }
}