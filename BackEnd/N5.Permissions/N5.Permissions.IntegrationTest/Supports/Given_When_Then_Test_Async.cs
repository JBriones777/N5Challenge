namespace N5.Permissions.IntegrationTest.Supports;

public abstract class Given_When_Then_Test_Async
{
    public async Task InitializeAsync()
    {
        await Given();
        await When();
    }
    protected abstract Task Given();
    protected abstract Task When();
}
