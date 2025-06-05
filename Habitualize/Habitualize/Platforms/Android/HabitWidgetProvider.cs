using Android.App;
using Android.Appwidget;
using Android.Content;
using Android.Widget;

namespace Habitualize.Platforms.Android;

[BroadcastReceiver(Label = "Habitualize Widget", Exported = false)]
[IntentFilter(new[] { "android.appwidget.action.APPWIDGET_UPDATE" })]
[MetaData("android.appwidget.provider", Resource = "@xml/habit_widget_provider")]
public class HabitWidgetProvider : AppWidgetProvider
{
    public override void OnUpdate(Context context, AppWidgetManager appWidgetManager, int[] appWidgetIds)
    {
        foreach (var widgetId in appWidgetIds)
        {
            RemoteViews views = new RemoteViews(context.PackageName, Resource.Layout.habit_widget);
            views.SetTextViewText(Resource.Id.widgetTitle, "Habit calendar");
            appWidgetManager.UpdateAppWidget(widgetId, views);
        }
    }
}