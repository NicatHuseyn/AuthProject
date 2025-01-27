using System.Net;
using System.Text.Json.Serialization;
using AuthProject.Shared.DTOs;

namespace AuthProject.Shared;

public class Result<T> where T : class
{
    public T? Data { get; private set; }
    public int StatusCode { get; private set; }

    [JsonIgnore]
    public bool IsSuccessful {  get; private set; }

    public ErrorDto Error { get; private set; }

    public static Result<T> Success(T data, int statusCode = (int)HttpStatusCode.OK)
    {
        return new Result<T> { Data = data, StatusCode = statusCode, IsSuccessful = true };
    }

    public static Result<T> Success(int statusCode = (int)HttpStatusCode.OK)
    {
        return new Result<T> { Data= default, StatusCode = statusCode, IsSuccessful = true };
    }

    public static Result<T> Fail(ErrorDto error, int statusCode)
    {
        return new Result<T> { Error = error, StatusCode = statusCode, IsSuccessful = false};
    }

    public static Result<T> Fail(string errorMessage, int statusCode, bool isShow)
    {
        var errorDto = new ErrorDto(errorMessage,isShow);

        return new Result<T> { Error = errorDto, StatusCode = statusCode, IsSuccessful = false};
    }
}

public class Result
{
    public int StatusCode { get; private set; }

    [JsonIgnore]
    public bool IsSuccessful { get; private set; }

    public ErrorDto Error { get; private set; }

    public static Result Success(int statusCode = (int)HttpStatusCode.OK)
    {
        return new Result {StatusCode = statusCode, IsSuccessful = true };
    }

    public static Result Fail(ErrorDto error, int statusCode)
    {
        return new Result { Error = error, StatusCode = statusCode, IsSuccessful = false };
    }

    public static Result Fail(string errorMessage, int statusCode, bool isShow)
    {
        var errorDto = new ErrorDto(errorMessage, isShow);

        return new Result { Error = errorDto, StatusCode = statusCode, IsSuccessful = false };
    }
}

