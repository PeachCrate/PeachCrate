
using Bogus;
using Models.Models;

namespace ServiceLayer.Services;

public class DataGenerator
{
    private Faker<User> userFake;
    private Faker<Group> groupFake;
    private Faker<Location> locationFake;
    private Faker<Place> placeFake;

    public DataGenerator()
    {
        //Randomizer.Seed = new Random(228);


        userFake = new Faker<User>()
            .RuleFor(u => u.UserId, 0)
            .RuleFor(u => u.Login, f => f.Name.FirstName() + "." + f.Name.LastName())
            .RuleFor(u => u.Email, f => f.Internet.Email())
            .RuleFor(u => u.PasswordHash, f => f.Internet.Password(50, false))
            .RuleFor(u => u.PasswordSalt, f => f.Internet.Password(50, false))
            .RuleFor(u => u.PhoneNumber, f => f.Phone.PhoneNumber("+(380)##-###-####"))
            .RuleFor(u => u.RegistrationDate, f => f.Date.Between(new DateTime(2000, 01, 01), new DateTime(2023, 11, 11)));


        groupFake = new Faker<Group>()
            .RuleFor(u => u.Title, f => f.Name.FullName());
        //.RuleFor(u=>u.UserId, f=>f.Random.Int(1,10000))
        /*.RuleFor(u => u.Email, f => f.Internet.Email())
        .RuleFor(u => u.Password, f => f.Internet.Password(12, true))
        .RuleFor(u => u.PhoneNumber, f=> f.Phone.PhoneNumber("+(380)##-###-####"))
        .RuleFor(u => u.RegistrationDate, f => f.Date.Between(new DateTime(2000,01,01), new DateTime(2023,11,11)));
        */
        locationFake = new Faker<Location>()
            .RuleFor(l => l.Title, f => f.Name.Random.Word())
            .RuleFor(l => l.Description, f => f.Random.Words(5))
            .RuleFor(l => l.Address, f => f.Address.StreetAddress())
            .RuleFor(l => l.GroupId, f => f.Random.Int(1, 20));


        placeFake = new Faker<Place>()
            .RuleFor(l => l.Name, f => f.Name.Random.Word())
            .RuleFor(l => l.Description, f => f.Random.Words(5));
    }

    public User GeneratePerson() => userFake.Generate();
    public Group GenerateGroup() => groupFake.Generate();
    public Location GenerateLocation() => locationFake.Generate();
}