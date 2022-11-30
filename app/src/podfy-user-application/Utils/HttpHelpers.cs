using Amazon.Lambda.APIGatewayEvents;

namespace podfy_user_application.Utils;

public class HttpHelpers
{
    public APIGatewayProxyResponse InternalServerError(object body)
    {
        return Response(body, HttpStatusCode.InternalServerError);
    }

    public APIGatewayProxyResponse BadRequest(object body)
    {
        return Response(body, HttpStatusCode.BadRequest);
    }

    public APIGatewayProxyResponse Ok(object body = null)
    {
        return Response(body, HttpStatusCode.OK);
    }

    public APIGatewayProxyResponse Unauthorized(object body = null)
    {
        return Response(body, HttpStatusCode.Unauthorized);
    }

    private APIGatewayProxyResponse Response(object body, HttpStatusCode httpStatusCode)
    {
        return new APIGatewayProxyResponse
        {
            StatusCode = (int)httpStatusCode,
            Body = JsonSerializer.Serialize(body),
            Headers = new Dictionary<string, string> { 
                { "Content-Type", "application/json" },
                { "Access-Control-Allow-Origin", "*"}
            }
        };
    }
}

