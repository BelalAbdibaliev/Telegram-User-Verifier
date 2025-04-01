namespace AntiBotVerifier.Entities;

public class Question
{
    public int Id { get; set; }
    public string QuestionText { get; set; }
    public string TrueAnswer { get; set; }
    public string FalseAnswer { get; set; }
}