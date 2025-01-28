namespace AuthProject.Shared.DTOs;

public class ErrorDto
{
    public List<string> Errors { get; set; }

    public bool IsShow { get; private set; }

    public ErrorDto()
    {
        Errors = new List<string>();
    }

    public ErrorDto(string error, bool isShow)
    {
        Errors = [error];
        isShow = true;
    }

    public ErrorDto(List<string> errors, bool isShow)
    {
        Errors = errors;
        IsShow = isShow;
    }
}
