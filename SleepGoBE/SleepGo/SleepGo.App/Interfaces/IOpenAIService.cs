namespace SleepGo.App.Interfaces
{
    public interface IOpenAIService
    {
        Task<string> AskQuestionAboutReviewAsync(string question, List<string> reviewTexts);
        Task<string> AskQuestionAboutHotelAsync(string question, List<string> hotelInfos);
    }
}
