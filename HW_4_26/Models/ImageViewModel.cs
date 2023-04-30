using ImageSharingEntityFramework.Data;

namespace HW_4_26.Models
{
    public class ImageViewModel
    {
        public List<Image> Images = new List<Image>();
        public Image Image { get; set; }
        public bool Liked { get; set; }
        
       
    }
}
