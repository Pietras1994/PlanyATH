namespace PlanyATH_Project.Models
{
    public enum eType
    {
        ClassRoom,
        Lecturer
    }
    public class DataModel
    {
        public string Name { get; set; }
        public eType eType { get; set; }
        public string Link { get; set; }
    }
}