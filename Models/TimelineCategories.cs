namespace Glimpse.Orchard.Models
{
    public static class TimelineCategories
    {
        public static PerfmonCategory Widgets = new PerfmonCategory("Widgets", "#00A0B0", "#00A0B0");
        public static PerfmonCategory Layers = new PerfmonCategory("Layers", "#CC333F", "#CC333F");
        public static PerfmonCategory Shapes = new PerfmonCategory("Shapes", "#8A9B0F", "#8A9B0F");
        public static PerfmonCategory Layouts = new PerfmonCategory("Layouts", "#00a33e", "#00a33e");
        public static PerfmonCategory Parts = new PerfmonCategory("Parts", "#6A4A3C", "#6A4A3C");
        public static PerfmonCategory ContentManagement = new PerfmonCategory("Content Manager", "#AA39AA", "#AA39AA");
        public static PerfmonCategory Authorization = new PerfmonCategory("Authorization", "#EB6841", "#EB6841");
        public static PerfmonCategory Cache = new PerfmonCategory("Cache", "#393B79", "#393B79");
    }
}