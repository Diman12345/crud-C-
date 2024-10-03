using Microsoft.AspNetCore.Mvc;

public class ApiResponse
{
    public static IActionResult Success(object data, string message = "Request was successful", object? pagination = null)
    {
        if(pagination == null){
            var response = new
            {
                message = message,
                success = true,
                data = data,
            };

            return new OkObjectResult(response);
        } else {
            var response = new
            {
                message = message,
                success = true,
                data = data,
                pagination = pagination,
            };

            return new OkObjectResult(response);
        }
    }

    public static IActionResult Failure(string errorMessage, int statusCode = 400)
    {
        var response = new
        {
            message = errorMessage,
            success = false
        };

        return new ObjectResult(response) { StatusCode = statusCode };
    }
}
