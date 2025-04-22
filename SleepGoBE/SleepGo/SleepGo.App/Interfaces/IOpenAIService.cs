namespace SleepGo.App.Interfaces
{
    public interface IOpenAIService
    {
        Task<string> AskQuestionAboutReviewAsync(string question, List<string> reviewTexts);
    }
}
