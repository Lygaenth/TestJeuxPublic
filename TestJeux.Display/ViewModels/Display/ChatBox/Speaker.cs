using System.Windows.Media.Imaging;
using TestJeux.API.Services.TextInteractions;
using TestJeux.Display.Helper;

namespace TestJeux.Display.ViewModels.Display.ChatBox
{
    public class Speaker
    {
        public Speaker(SpeakerDto speakerDto)
        {
            Name = speakerDto.Name;
            Line = speakerDto.Line;
            Icon = ImageHelper.GetImage(speakerDto.Icon);
        }

        public string Name { get; set; }
        public string Line { get; set; }
        public CachedBitmap Icon { get; set; }
    }
}
