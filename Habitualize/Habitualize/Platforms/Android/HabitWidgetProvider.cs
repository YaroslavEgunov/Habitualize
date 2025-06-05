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
    public const string ACTION_PREV_DAY = "com.melon.habitualize.widget.PREV_DAY";
    public const string ACTION_NEXT_DAY = "com.melon.habitualize.widget.NEXT_DAY";

    public override void OnUpdate(Context context, AppWidgetManager appWidgetManager, int[] appWidgetIds)
    {
        var prefs = context.GetSharedPreferences("habitualize", FileCreationMode.Private);
        long dateTicks = prefs.GetLong("widget_selected_date", DateTime.Today.Ticks);
        DateTime today = DateTime.Today;
        DateTime minDate = today;
        DateTime maxDate = today.AddDays(6);
        DateTime selectedDate = new DateTime(dateTicks);

        string habits = prefs.GetString($"habits_{selectedDate:yyyyMMdd}", "Nothing");

        var habitsList = habits.Split('\n').ToList();
        int maxHabits = 3;
        string displayHabits = string.Join("\n", habitsList.Take(maxHabits));
        if (habitsList.Count > maxHabits)
            displayHabits += $"\nand {habitsList.Count - maxHabits} more...";

        foreach (var widgetId in appWidgetIds)
        {
            RemoteViews views = new RemoteViews(context.PackageName, Resource.Layout.habit_widget);
            string titleAndDate = $"Schedule  {selectedDate:dd.MM.yy}";
            views.SetTextViewText(Resource.Id.widgetTitle, titleAndDate);
            views.SetTextViewText(Resource.Id.widgetHabits, displayHabits);

            Intent prevIntent = new Intent(context, typeof(HabitWidgetProvider));
            prevIntent.SetAction(ACTION_PREV_DAY);
            prevIntent.PutExtra(AppWidgetManager.ExtraAppwidgetIds, new int[] { widgetId });
            PendingIntent prevPendingIntent = PendingIntent.GetBroadcast(context, widgetId, prevIntent, PendingIntentFlags.UpdateCurrent | PendingIntentFlags.Immutable);
            views.SetOnClickPendingIntent(Resource.Id.btnPrev, prevPendingIntent);

            Intent nextIntent = new Intent(context, typeof(HabitWidgetProvider));
            nextIntent.SetAction(ACTION_NEXT_DAY);
            nextIntent.PutExtra(AppWidgetManager.ExtraAppwidgetIds, new int[] { widgetId });
            PendingIntent nextPendingIntent = PendingIntent.GetBroadcast(context, widgetId + 1000, nextIntent, PendingIntentFlags.UpdateCurrent | PendingIntentFlags.Immutable);
            views.SetOnClickPendingIntent(Resource.Id.btnNext, nextPendingIntent);

            views.SetBoolean(Resource.Id.btnPrev, "setEnabled", selectedDate > minDate);
            views.SetBoolean(Resource.Id.btnNext, "setEnabled", selectedDate < maxDate);

            appWidgetManager.UpdateAppWidget(widgetId, views);
        }
    }

    public override void OnReceive(Context context, Intent intent)
    {
        base.OnReceive(context, intent);

        var prefs = context.GetSharedPreferences("habitualize", FileCreationMode.Private);
        long dateTicks = prefs.GetLong("widget_selected_date", DateTime.Today.Ticks);
        DateTime today = DateTime.Today;
        DateTime minDate = today;
        DateTime maxDate = today.AddDays(6);
        DateTime selectedDate = new DateTime(dateTicks);

        if (intent.Action == ACTION_PREV_DAY)
            selectedDate = selectedDate.AddDays(-1);
        else if (intent.Action == ACTION_NEXT_DAY)
            selectedDate = selectedDate.AddDays(1);
        else
            return;

        if (selectedDate < minDate)
            selectedDate = minDate;
        if (selectedDate > maxDate)
            selectedDate = maxDate;

        var editor = prefs.Edit();
        editor.PutLong("widget_selected_date", selectedDate.Ticks);
        editor.Apply();

        AppWidgetManager appWidgetManager = AppWidgetManager.GetInstance(context);
        ComponentName thisWidget = new ComponentName(context, Java.Lang.Class.FromType(typeof(HabitWidgetProvider)));
        int[] appWidgetIds = appWidgetManager.GetAppWidgetIds(thisWidget);
        OnUpdate(context, appWidgetManager, appWidgetIds);
    }
}