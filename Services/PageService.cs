namespace MorcuTool.Services;

public sealed class PageService : IPageService
{
    readonly IServiceProvider serviceProvider;

    public PageService(IServiceProvider serviceProvider)
        => this.serviceProvider = serviceProvider;

    /// <inheritdoc />
    public T? GetPage<T>()
        where T : class
    {
        if (!typeof(FrameworkElement).IsAssignableFrom(typeof(T)))
            throw new InvalidOperationException("The page should be a WPF control.");

        return (T?)serviceProvider.GetService(typeof(T));
    }

    /// <inheritdoc />
    public FrameworkElement? GetPage(Type pageType)
    {
        if (!typeof(FrameworkElement).IsAssignableFrom(pageType))
            throw new InvalidOperationException("The page should be a WPF control.");

        return serviceProvider.GetService(pageType) as FrameworkElement;
    }
}