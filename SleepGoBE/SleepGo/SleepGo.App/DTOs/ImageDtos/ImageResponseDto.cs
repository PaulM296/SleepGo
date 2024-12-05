namespace SleepGo.App.DTOs.ImageDtos
{
    public class ImageResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public byte[] Data { get; set; }
    }
}
