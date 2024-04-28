namespace MorcuTool.Extensions;

public static class BindableTreeViewSelector
{
    public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.RegisterAttached(
        "SelectedItem",
        typeof(object),
        typeof(BindableTreeViewSelector),
        new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedItemChanged)
    );

    public static object GetSelectedItem(DependencyObject obj)
    {
        return obj.GetValue(SelectedItemProperty);
    }

    public static void SetSelectedItem(DependencyObject obj, object value)
    {
        obj.SetValue(SelectedItemProperty, value);
    }

    static void OnSelectedItemChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
    {
        if (obj is TreeView treeView)
        {
            treeView.SelectedItemChanged -= OnTreeViewSelectedItemChanged;
            treeView.SelectedItemChanged += OnTreeViewSelectedItemChanged;
        }
    }

    static void OnTreeViewSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
    {
        if (sender is TreeView treeView)
        {
            treeView.SetValue(SelectedItemProperty, e.NewValue);
        }
    }
}