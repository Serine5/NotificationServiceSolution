# Notification Service

A .NET 6 Web API showcasing:
- Multiple channels : Email, SMS, Push (all mocked)
- Dependency Injection: Uses .NET's built-in DI to inject services and senders
- Mock vs. SMTP Email integration
- Retry logic : Retries sending notifications on failure
- In-memory DB (EF Core)
- In-memory caching
- FluentValidation :Validates input models before processing requests
- Unit tests (xUnit + Moq)
- Environment-based integration selection

## Getting Started

1. **Clone** this repo or download the ZIP. https://github.com/Serine5/NotificationServiceSolution.git
2. Open `NotificationServiceSolution.sln` in Visual Studio / VS Code.
3. Run `dotnet restore`.
4. Run the project:  
   ```bash
   dotnet run --project ./src/NotificationServiceSolution/NotificationServiceSolution.csproj
   
## Usage

- **Swagger**: Navigate to `https://localhost:7055/swagger` to see the available endpoints and test the `POST /api/notifications/send` endpoint.
- **Endpoint**: `POST /api/notifications/send`
  ```json
  {
    "channel": 0,        // 0=Email, 1=SMS, 2=Push
    "recipient": "john@example.com",
    "message": "Hello from the Notification Service!",
    "retryCount": 3
  }
