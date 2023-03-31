namespace TestJeux.API.Services.TextInteractions
{
    public class SpeakerDto
    {
        public SpeakerDto(string icon, string name, string line)
        {
            Icon = icon;
            Name = name;
            Line = line;
        }

        public string Icon { get; set; }
        public string Name { get; set; }
        public string Line { get; set; }
    }
}