using Domain.Entities;
using Domain.ValueObjects;
using FluentAssertions;

namespace Domain.Tests.Entities;

public class UserTests
{
    [Fact]
    public void Constructor_ShouldSetDefaults()
    {
        var user = new User(
            id: Ulid.NewUlid(),
            name: "Ada",
            email: new Email("ada@example.com"),
            password: Password.CreateHashed("Secret123!")
        );

        user.IsVerified.Should().BeFalse();
        user.Currency.Code.Should().Be("DO");
        user.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void UpdateProfile_ShouldUpdateProvidedFields()
    {
        var user = BuildUser();

        user.UpdateProfile(
            name: "Grace",
            phone: new PhoneNumber("+18095550000"),
            currency: new Currency("USD"),
            language: "es",
            avatar: "avatar.png"
        );

        user.Name.Should().Be("Grace");
        user.Phone!.Value.Should().Be("+18095550000");
        user.Currency.Code.Should().Be("USD");
        user.Language.Should().Be("es");
        user.Avatar.Should().Be("avatar.png");
    }

    [Fact]
    public void ChangePassword_ShouldReplaceHash()
    {
        var user = BuildUser();
        var oldHash = user.PasswordHash.Hash;

        var newPassword = Password.CreateHashed("NewSecret#1");
        user.ChangePassword(newPassword);

        user.PasswordHash.Hash.Should().NotBe(oldHash);
        user.PasswordHash.Verify("NewSecret#1").Should().BeTrue();
    }

    private static User BuildUser() =>
        new User(
            id: Ulid.NewUlid(),
            name: "Test User",
            email: new Email("test@example.com"),
            password: Password.CreateHashed("Secret123!")
        );
}
