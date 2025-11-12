using Domain.Interfaces;
using Moq;

namespace Application.Tests.TestHelpers;

internal static class UnitOfWorkMockBuilder
{
    public static Mock<IUnitOfWork> Build(
        Mock<ICategoryRepository>? category = null,
        Mock<ITransactionRepository>? transaction = null,
        Mock<IUserRepository>? user = null,
        Mock<IRefreshTokenRepository>? refreshToken = null,
        Mock<IUserVerificationRepository>? verification = null)
    {
        category ??= new Mock<ICategoryRepository>();
        transaction ??= new Mock<ITransactionRepository>();
        user ??= new Mock<IUserRepository>();
        refreshToken ??= new Mock<IRefreshTokenRepository>();
        verification ??= new Mock<IUserVerificationRepository>();

        var uow = new Mock<IUnitOfWork>();

        uow.SetupGet(x => x.Category).Returns(category.Object);
        uow.SetupGet(x => x.Transaction).Returns(transaction.Object);
        uow.SetupGet(x => x.User).Returns(user.Object);
        uow.SetupGet(x => x.RefreshToken).Returns(refreshToken.Object);
        uow.SetupGet(x => x.UserVerification).Returns(verification.Object);
        uow.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        return uow;
    }
}
