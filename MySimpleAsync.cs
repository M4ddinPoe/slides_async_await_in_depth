public class MySimpleAsync
{
    public async Task<int> DoSomethingAsync(int parameter)
    {
        var result = this.SomeSynchronousMethod(parameter);

        result = await this.MyMethodAsync(result);

        return result;
    }

    private async Task<int> MyMethodAsync(int parameter)
    {
        return await Task.FromResult(parameter + 1);
    }

    private int SomeSynchronousMethod(int parameter)
    {
        return parameter + 1;
    }
}